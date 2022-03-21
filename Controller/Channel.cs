using Scpi;

namespace Controller;

public class Channel : IChannel
{
    private readonly IScpi _scpi;
    private decimal _voltage;
    private decimal _current;
    private bool _output;
    public int Id { get; }

    public decimal Voltage
    {
        get => _voltage;
        set => _scpi.SetVoltage(Id, (double) value);
    }

    public decimal Current
    {
        get => _current;
        set => _scpi.SetCurrent(Id, (double) value);
    }

    public bool OutputEnable
    {
        get => _output;
        set => _scpi.SetOutput(Id, value);
    }

    internal Channel(int id, ref IScpi scpi)
    {
        Id = id;
        _scpi = scpi;
    }

    internal void RefreshChannel()
    {
        _voltage = _scpi.GetVoltage(Id);
        _current = _scpi.GetCurrent(Id);
        _output = _scpi.GetOutput(Id);
    }
}