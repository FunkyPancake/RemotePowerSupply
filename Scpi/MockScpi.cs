namespace Scpi;

public class MockScpi : IScpi
{
    public bool Connect(string name)
    {
        return true;
    }

    public void Disconnect()
    {
        throw new NotImplementedException();
    }

    public decimal GetVoltage(int channel)
    {
        return 0;
    }

    public void SetVoltage(int channel, double value)
    {
    }

    public bool GetOutput(int channel)
    {
        
        return true;
    }

    public void SetOutput(int channel, bool value)
    {
    }

    public decimal GetCurrent(int channel)
    {

        return 0;    }

    public void SetCurrent(int channel, double value)
    {
    }

    public string Identification()
    {
        return string.Empty;
    }
}