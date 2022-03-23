namespace Scpi;

public interface IScpi
{
    public bool IsConnected { get; }
    public bool Connect(string name);
    public void Disconnect();
    public double GetVoltage(int channel);
    public void SetVoltage(int channel, double value);
    public bool GetOutput(int channel);
    public void SetOutput(int channel, bool value);
    public double GetCurrent(int channel);
    public void SetCurrent(int channel, double value);
    public double GetCurrentSetpoint(int channel);
    public double GetVoltageSetpoint(int channel);
}