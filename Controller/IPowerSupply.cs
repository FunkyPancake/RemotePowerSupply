namespace Controller;

public interface IPowerSupply
{
    string Name { get; }
    List<IChannel> Channels { get; }
    bool IsConnected { get; }
    bool Connect();
    void Disconnect();
    void RefreshData();
}