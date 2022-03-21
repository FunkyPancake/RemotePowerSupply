namespace Controller;

public interface IChannel
{
    public int Id { get; }
    decimal Voltage { set; get; }
    decimal Current { set; get; }
    bool OutputEnable { set; get; }
}