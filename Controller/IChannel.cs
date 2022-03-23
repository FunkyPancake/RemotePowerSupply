namespace Controller;

public interface IChannel
{
    public int Id { get; }
    double Voltage { set; get; }
    double Current { set; get; }
    bool OutputEnable { set; get; }
    (double voltage, double current) GetSetpoints();
}