using Scpi;
using Serilog;

namespace Controller;

public class PowerSupplySerial : IPowerSupply
{
    private readonly List<Action> _refreshList;
    private readonly IScpi _scpi;
    private bool _isConnected;
    private readonly string _comPort;

    public string ComPort => _comPort;

    public PowerSupplySerial(ILogger logger,string name, int channelCount, string comPort = "")
    {
        _comPort = comPort;
        _scpi = new ScpiSerial(logger,ref _comPort);
        var channels = new List<IChannel>();
        _refreshList = new List<Action>();
        for (var i = 1; i <= channelCount; i++)
        {
            var channel = new Channel(logger, i, ref _scpi);
            _refreshList.Add(channel.RefreshChannel);
            channels.Add(channel);
        }

        Name = name;
        Channels = channels;
    }

    public string Name { get; }
    public List<IChannel> Channels { get; }

    public bool Connect()
    {
        _isConnected = _scpi.Connect(Name);
        return _isConnected;
    }

    public void Disconnect()
    {
        _scpi.Disconnect();
        _isConnected = false;
    }

    public bool IsConnected => _isConnected;

    public void RefreshData()
    {
        foreach (var action in _refreshList)
        {
            action.Invoke();
        }
    }
}