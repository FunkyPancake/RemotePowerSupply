using System.IO.Ports;
using System.Text;

namespace Scpi;

public class ScpiSerial : ScpiGeneric, IDisposable
{
    private SerialPort? _serialPort;

    public void Dispose()
    {
        _serialPort?.Dispose();
    }

    protected override bool IsConnectedGetter() => _serialPort is not null && _serialPort.IsOpen;

    public override bool Connect(string name)
    {
        var portNames = SerialPort.GetPortNames();
        foreach (var port in portNames)
        {
            if (!CheckPort(port, name))
                continue;
            _serialPort = InitSerialPort(port);
            return true;
        }

        return false;
    }

    private SerialPort InitSerialPort(string port)
    {
        var serialPort = new SerialPort(port)
        {
            ReadTimeout = 1000,
            NewLine = "\n",
            Encoding = Encoding.UTF8
        };
        serialPort.Open();
        serialPort.DiscardInBuffer();
        serialPort.DiscardOutBuffer();
        return serialPort;
    }

    public override void Disconnect()
    {
        _serialPort?.Close();
    }

    private bool CheckPort(string port, string name)
    {
        SerialPort serialPort;
        try
        {
            serialPort = InitSerialPort(port);
        }
        catch (Exception)
        {
            return false;
        }

        var response = RequestResponse(serialPort, "*IDN?");
        serialPort.Close();
        return response.Contains(name);
    }

    protected override string RequestResponse(string request)
    {
        return RequestResponse(_serialPort ?? throw new InvalidOperationException(), request);
    }

    protected override void Request(string request)
    {
        Request(_serialPort ?? throw new InvalidOperationException(), request);
    }

    private void Request(SerialPort serial, string request)
    {
        if (!serial.IsOpen)
            return;
        var requestBytes = Encoding.ASCII.GetBytes(request + "\n");
        serial.Write(requestBytes, 0, requestBytes.Length);
    }

    private string RequestResponse(SerialPort serial, string request)
    {
        Request(serial, request);
        try
        {
            if (!serial.IsOpen)
                return string.Empty;
            var response = serial.ReadLine();
            serial.ReadExisting();
            return response.Trim();
        }
        catch (TimeoutException)
        {
            return string.Empty;
        }
    }
}