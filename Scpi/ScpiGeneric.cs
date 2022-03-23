namespace Scpi;

public abstract class ScpiGeneric : IScpi
{
    public abstract bool Connect(string name);

    public abstract void Disconnect();

    public double GetVoltage(int channel)
    {
        double.TryParse(RequestResponse($"V{channel}O?").Trim('V'), out var value);
        return value;
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

    public double GetCurrent(int channel)
    {
        double.TryParse(RequestResponse($"I{channel}O?").Trim('A'), out var value);
        return value;
    }

    public void SetCurrent(int channel, double value)
    {
        Request($"I{channel} {value}");
    }

    public double GetCurrentSetpoint(int channel)
    {
        double.TryParse(RequestResponse($"I{channel}?").Trim('A'), out var value);
        return value;
    }

    public double GetVoltageSetpoint(int channel)
    {
        double.TryParse(RequestResponse($"V{channel}?").Trim('V'), out var value);
        return value;
    }

    public bool IsConnected => IsConnectedGetter();

    protected abstract bool IsConnectedGetter();

    protected abstract string RequestResponse(string request);
    protected abstract void Request(string request);
}