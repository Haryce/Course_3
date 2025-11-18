using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    static void Main()
    {
        using Socket listener = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp
        );

        listener.Bind(new IPEndPoint(IPAddress.Loopback, 50000));
        listener.Listen(backlog: 10);

        bool isRunning = true;

        void Stop()
        {
            if (!isRunning) return;
            isRunning = false;
            try { listener.Close(); } catch { }
            Console.WriteLine("Server stopping");
        }

        Console.WriteLine("Server started on port 50000");

        while (isRunning)
        {
            Socket clientSocket;
            try
            {
                clientSocket = listener.Accept();
                Console.WriteLine("Client connected");
            }
            catch (ObjectDisposedException)
            {
                break;
            }
            catch (SocketException)
            {
                break;
            }

            ThreadPool.QueueUserWorkItem(_ => HandleClient(clientSocket, Stop));
        }
    }

    public static void HandleClient(Socket clientSocket, Action stop)
    {
        string welcomeMessage = "Available commands: average, max, min, stop\r\nEnter command: ";
        
        try
        {
            Send(clientSocket, welcomeMessage);
            byte[] bufferReceive = new byte[4096];

            while (true)
            {
                int read = clientSocket.Receive(bufferReceive);
                if (read == 0)
                {
                    Console.WriteLine("Client disconnected");
                    break;
                }

                string message = Encoding.UTF8.GetString(bufferReceive, 0, read).TrimEnd('\r', '\n');

                if (message.Equals("stop", StringComparison.OrdinalIgnoreCase))
                {
                    stop();
                    break;
                }

                ProcessCommand(clientSocket, message);
            }
        }
        catch (SocketException se)
        {
            Console.WriteLine($"Socket Error: {se.SocketErrorCode}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            try { clientSocket.Shutdown(SocketShutdown.Both); } catch { }
            clientSocket.Close();
            Console.WriteLine("Client connection closed");
        }
    }

    public static void ProcessCommand(Socket clientSocket, string command)
    {
        switch (command.ToLower())
        {
            case "average":
            case "max":
            case "min":
                Send(clientSocket, $"Enter numbers separated by spaces for {command}: ");
                ProcessNumbers(clientSocket, command);
                break;
            default:
                Send(clientSocket, $"Unknown command: {command}\r\nAvailable commands: average, max, min, stop\r\nEnter command: ");
                break;
        }
    }

    public static void ProcessNumbers(Socket clientSocket, string command)
    {
        byte[] buffer = new byte[4096];
        int read = clientSocket.Receive(buffer);
        if (read == 0) return;

        string numbersInput = Encoding.UTF8.GetString(buffer, 0, read).TrimEnd('\r', '\n');
        string[] numberStrings = numbersInput.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

        List<double> numbers = new List<double>();
        foreach (string numStr in numberStrings)
        {
            if (double.TryParse(numStr, out double num))
            {
                numbers.Add(num);
            }
        }

        if (numbers.Count == 0)
        {
            Send(clientSocket, "No valid numbers provided. Please enter valid numbers.\r\nEnter command: ");
            return;
        }

        double result = 0;
        string operation = command.ToLower();

        switch (operation)
        {
            case "average":
                result = numbers.Average();
                break;
            case "max":
                result = numbers.Max();
                break;
            case "min":
                result = numbers.Min();
                break;
        }

        string response = $"{operation} of {numbers.Count} numbers: {result:F2}\r\nEnter command: ";
        Send(clientSocket, response);
    }

    public static void Send(Socket s, string message) =>
        s.Send(Encoding.UTF8.GetBytes(message));
}