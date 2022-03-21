using System.IO.Ports;

namespace Scpi;

public class ScpiSerial : IDisposable
{
    private readonly string _deviceName;
    private SerialPort? _serialPort;

    public ScpiSerial(string deviceName)
    {
        _deviceName = deviceName;
    }

    public void Init()
    {
        var portNames = SerialPort.GetPortNames();
        foreach (var port in portNames)
        {
            if (!CheckPort(port))
                continue;
            _serialPort = new SerialPort(port);
            return;
        }
    }

    private bool CheckPort(string port)
    {
        using var sp = new SerialPort(port);
        sp.Open();

        return true;
    }

    public void Dispose()
    {
        _serialPort?.Dispose();
    }

    private void Request(string request)
    {
    }
    private string RequestResponse(string request)
    {
        return RequestResponse(_serialPort, request);
    }
    private string RequestResponse(SerialPort? serial, string request)
    {
        return string.Empty;
    }
}