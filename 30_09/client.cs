using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace client_tcp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int port = 50000;

            using Socket server = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, port);
            server.Connect(ep);
            byte[] buffer = new byte[4096];
            int received = server.Receive(buffer);
            string message = Encoding.ASCII.GetString(buffer, 0, received);
            Console.WriteLine("Сервер: " + message);
            Console.Write("Введите имя: ");
            string name = Console.ReadLine();
            Console.Write("Введите должность: ");
            string position = Console.ReadLine();
            Console.Write("Введите стаж работы (целое число): ");
            string experience = Console.ReadLine();

            string sendMessage = $"{name} {position} {experience}";
            byte[] sendBuffer = Encoding.ASCII.GetBytes(sendMessage);
            server.Send(sendBuffer);

            received = server.Receive(buffer);
            message = Encoding.ASCII.GetString(buffer, 0, received);
            Console.WriteLine("Сервер: " + message);
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }
    }
}
