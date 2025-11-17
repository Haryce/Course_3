using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Console_app322
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int port = 50000;
            Dictionary<string, int> positions = new Dictionary<string, int>
            {
                { "Инженер", 3 },
                { "Менеджер", 5 },
                { "Аналитик", 2 }
            };
            using Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, port);
            listener.Bind(ep);
            listener.Listen(1);
            Console.WriteLine("Ожидание подключения клиента...");
            using Socket clientSocket = listener.Accept();
            Console.WriteLine($"Клиент подключен: {clientSocket.RemoteEndPoint}");


            string requestMessage = "Пожалуйста, отправьте ваше имя, должность и стаж через пробел (например: Иван Инженер 4):\r\n";
            byte[] requestBuffer = Encoding.ASCII.GetBytes(requestMessage);
            clientSocket.Send(requestBuffer);

            byte[] buffer = new byte[4096];
            int received = clientSocket.Receive(buffer);
            string clientData = Encoding.ASCII.GetString(buffer, 0, received).Trim();

            Console.WriteLine($"Получены данные от клиента: {clientData}");
            string[] parts = clientData.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);
            string name = parts.Length > 0 ? parts[0] : "";
            string position = parts.Length > 1 ? parts[1] : "";
            int experience = (parts.Length > 2 && int.TryParse(parts[2], out int exp)) ? exp : -1;
            string response;
            if (positions.ContainsKey(position))
            {
                int requiredExp = positions[position];
                if (experience >= requiredExp)
                {
                    response = $"{name}, вы приняты на работу.";
                }
                else
                {
                    response = $"{name}, вы не приняты на работу. Недостаточный стаж.";
                }
            }
            else
            {
                response = $"{name}, вы не приняты на работу. Должность \"{position}\" не найдена.";
            }
            byte[] responseBuffer = Encoding.ASCII.GetBytes(response + "\r\n");
            clientSocket.Send(responseBuffer);

            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            listener.Close();
        }
    }
}

