using System.Net;
using System.Net.Sockets;
using System.Text;

class Client
{
    static void Main()
    {
        try
        {
            using Socket serverSocket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            Console.WriteLine("Connecting to server...");
            serverSocket.Connect(new IPEndPoint(IPAddress.Loopback, 50000));

            byte[] buffer = new byte[4096];
            int read = serverSocket.Receive(buffer);

            if (read == 0)
            {
                Console.WriteLine("Server closed connection");
                return;
            }

            string message = Encoding.UTF8.GetString(buffer, 0, read);
            Console.Write(message);

            bool isRunning = true;

            while (isRunning)
            {
                message = Console.ReadLine();

                if (message.Equals("stop", StringComparison.OrdinalIgnoreCase))
                {
                    isRunning = false;
                }

                Send(serverSocket, message + "\r\n");

                read = serverSocket.Receive(buffer);
                if (read == 0)
                {
                    Console.WriteLine("Server closed connection");
                    break;
                }
                
                message = Encoding.UTF8.GetString(buffer, 0, read);
                Console.Write(message);
            }

            try { serverSocket.Shutdown(SocketShutdown.Both); } catch { }
            serverSocket.Close();
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"Socket Error: {ex.SocketErrorCode}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    public static void Send(Socket s, string message) =>
        s.Send(Encoding.UTF8.GetBytes(message));
}