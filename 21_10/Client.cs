using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var client = new TcpClient();
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 50000);
            await client.ConnectAsync(ep);

            using var net = client.GetStream();

            var encoding = new UTF8Encoding(false);
            var reader = new StreamReader(
                net,
                encoding,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 4096,
                leaveOpen: false
                );
            var writer = new StreamWriter(
                net,
                encoding,
                bufferSize: 4096,
                leaveOpen: false
                )
            { AutoFlush = true };

            var response = await reader.ReadLineAsync();
            if (response is null) return;
            Console.WriteLine(response);

            while (true)
            {
                string countInput = Console.ReadLine();
                await writer.WriteLineAsync(countInput);

                response = await reader.ReadLineAsync();
                if (response is null) continue;
                Console.WriteLine(response);

                if (response == "Enter ages separated by space:")
                {
                    string agesInput = Console.ReadLine();
                    await writer.WriteLineAsync(agesInput);

                    response = await reader.ReadLineAsync();
                    if (response is null) continue;
                    Console.WriteLine(response);
                }
            }
        }
    }
}