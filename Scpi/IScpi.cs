namespace Scpi;

public interface IScpi
{
    public bool Connect(string name);
    public void Disconnect();
    public decimal GetVoltage(int channel);
    public void SetVoltage(int channel,double value);
    public bool GetOutput(int channel);
    public void SetOutput(int channel,bool value);
    public decimal GetCurrent(int channel);
    public void SetCurrent(int channel,double value);
    public string Identification();
}