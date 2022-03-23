using Scpi;
using Serilog;

namespace Controller;

public class Channel : IChannel
{
    private readonly UniqueQueue<Action> _queue = new();
    private readonly ILogger _logger;
    private readonly IScpi _scpi;
    private double _current;
    private bool _output;
    private double _voltage;

    internal Channel(ILogger logger, int id, ref IScpi scpi)
    {
        Id = id;
        _logger = logger;
        _scpi = scpi;
    }

    public int Id { get; }

    public double Voltage
    {
        get => _voltage;
        set
        {
            if (_scpi.IsConnected)
            {
                _logger.Debug("Channel {id}, set voltage to {value} V", Id, value);
                _queue.Enqueue(() => _scpi.SetVoltage(Id, value));
            }
        }
    }

    public double Current
    {
        get => _current;
        set
        {
            if (_scpi.IsConnected)
            {
                _logger.Debug("Channel {id}, set current limit to {value} A", Id, value);
                _queue.Enqueue(() => _scpi.SetCurrent(Id, value));
            }
        }
    }

    public bool OutputEnable
    {
        get => _output;
        set
        {
            if (_scpi.IsConnected)
            {
                _logger.Debug("Channel {id}, set output to {value} V", Id, value ? "on" : "off");
                _queue.Enqueue(() => _scpi.SetOutput(Id, value));
            }
        }
    }

    public (double, double) GetSetpoints()
    {
        _output = _scpi.GetOutput(Id);
        var voltage = _scpi.GetVoltageSetpoint(Id);
        var current = _scpi.GetCurrentSetpoint(Id);
        _logger.Debug("Channel {id}, setpoints set to: voltage = {voltage} V, current = {current} A",
            Id, voltage, current);
        return (voltage, current);
    }

    internal void RefreshChannel()
    {
        ProcessCommandQueue();
        _voltage = _scpi.GetVoltage(Id);
        _current = _scpi.GetCurrent(Id);
        _output = _scpi.GetOutput(Id);
    }

    private void ProcessCommandQueue()
    {
        while (_queue.Count > 0)
        {
            _queue.Dequeue().Invoke();
        }
    }
}