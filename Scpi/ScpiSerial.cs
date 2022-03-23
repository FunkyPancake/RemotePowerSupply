using System.IO.Ports;
using System.Text;
using Serilog;

namespace Scpi;

public class ScpiSerial : ScpiGeneric, IDisposable
{
    private SerialPort? _serialPort;
    private readonly ILogger _logger;
    private string _comPort;

    public string ComPort => _comPort;

    public ScpiSerial(ILogger logger, string comPort)
    {
        _logger = logger;
        _comPort = comPort;
    }

    public void Dispose()
    {
        _serialPort?.Dispose();
    }

    protected override bool IsConnectedGetter() => _serialPort is not null && _serialPort.IsOpen;

    public override bool Connect(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            _logger.Error("Device name is empty.");
            return false;
        }
        _logger.Information("Try to connect.");
        var portNames = SerialPort.GetPortNames();
        if (portNames.Contains(_comPort))
        {
            _logger.Debug("Try to connect to the last used port: {port}.", _comPort);
            if (CheckPort(_comPort, name))
            {
                _serialPort = InitSerialPort(_comPort);
                _logger.Information("Connection successful.");
                return true;
            }
        }

        foreach (var port in portNames)
        {
            if (!CheckPort(port, name))
                continue;
            _serialPort = InitSerialPort(port);
            _comPort = port;
            _logger.Information("Connection successful on port {port}.", port);
            return true;
        }

        _logger.Information("Connection failed.");
        _comPort = "";
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
        catch (Exception e)
        {
            _logger.Error("{exception}", e);
            return false;
        }

        var response = RequestResponse(serialPort, "*IDN?");
        _logger.Debug("Response for *IDN? {response}.", response);

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
            var response = serial.ReadLine().Trim();
            serial.ReadExisting();
            return response;
        }
        catch (TimeoutException)
        {
            _logger.Error("Request: {request} - Timeout", request);
            return string.Empty;
        }
    }
}