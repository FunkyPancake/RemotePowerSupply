using System.IO.Ports;
using System.Text;

namespace Scpi;

public class ScpiSerial : ScpiGeneric, IDisposable
{
    private SerialPort? _serialPort;

    public new bool Connect(string name)
    {
        var portNames = SerialPort.GetPortNames();
        foreach (var port in portNames)
        {
            if (!CheckPort(port,name))
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

    public new void Disconnect()
    {
        _serialPort?.Close();
    }

    private bool CheckPort(string port, string name)
    {
        var serialPort = InitSerialPort(port);
        var response = RequestResponse(serialPort, "*IDN?");
        serialPort.Close();
        return response.Contains(name);
    }

    public void Dispose()
    {
        _serialPort?.Dispose();
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
        var requestBytes = Encoding.ASCII.GetBytes(request + "\n");
        serial.Write(requestBytes, 0, requestBytes.Length);
    }

    private string RequestResponse(SerialPort serial, string request)
    {
        Request(serial, request);
        try
        {
            var response = serial.ReadLine();
            serial.ReadExisting();
            return response;
        }
        catch (TimeoutException)
        {
            return string.Empty;
        }
    }
}