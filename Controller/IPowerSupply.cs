namespace Controller;

public interface IPowerSupply
{
    string Name { get; }
    List<IChannel> Channels { get; }
    void Init(List<int>? channelDefaults = null);
    bool Connect();
    void Disconnect();
    bool IsConnected { get; }
    
    void RefreshData();
}