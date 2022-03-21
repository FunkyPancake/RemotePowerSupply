namespace Scpi;

public abstract class ScpiGeneric : IScpi
{
    public abstract bool Connect(string name);

    public abstract void Disconnect();

    public decimal GetVoltage(int channel)
    {
        return (decimal) double.Parse(RequestResponse($"V{channel}O?").Trim('V'));
    }

    public void SetVoltage(int channel, double value)
    {
        Request($"V{channel} {value}");
    }

    public bool GetOutput(int channel)
    {
        return int.Parse(RequestResponse($"OP{channel}?")) == 1;
    }

    public void SetOutput(int channel, bool value)
    {
        Request($"OP{channel} {(value ? 1 : 0)}");
    }

    public decimal GetCurrent(int channel)
    {
        return (decimal) double.Parse(RequestResponse($"I{channel}O?").Trim('A'));
    }

    public void SetCurrent(int channel, double value)
    {
        Request($"I{channel} {value}");
    }

    public string Identification()
    {
        return RequestResponse("IDN?");
    }

    protected abstract string RequestResponse(string request);
    protected abstract void Request(string request);
}