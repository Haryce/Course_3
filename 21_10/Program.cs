using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 50000);
            var listener = new TcpListener(ep);
            listener.Start();
            Console.WriteLine("Server started...");

            while (true)
            {
                try
                {
                    var client = await listener.AcceptTcpClientAsync();
                    _ = Task.Run(() => HandleClient(client));
                }
                catch (SocketException ex) { Console.WriteLine(ex.ToString()); }
                catch (ObjectDisposedException ex) { Console.WriteLine(ex.ToString()); }
            }
        }

        public static async Task HandleClient(TcpClient client)
        {
            try {
                using var _ = client;
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

                await writer.WriteLineAsync("Enter number of ages:");

                while (true)
                {
                    var countStr = await reader.ReadLineAsync();
                    if (countStr is null) break;

                    if (int.TryParse(countStr, out int count) && count > 0)
                    {
                        await writer.WriteLineAsync("Enter ages separated by space:");
                        
                        var agesStr = await reader.ReadLineAsync();
                        if (agesStr is null) break;

                        string[] ages = agesStr.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        
                        if (ages.Length == count)
                        {
                            int sum = 0;
                            bool valid = true;

                            foreach (string ageStr in ages)
                            {
                                if (int.TryParse(ageStr, out int age))
                                {
                                    sum += age;
                                }
                                else
                                {
                                    await writer.WriteLineAsync("Invalid age format");
                                    valid = false;
                                    break;
                                }
                            }

                            if (valid)
                            {
                                double average = (double)sum / count;
                                await writer.WriteLineAsync($"Average age: {average:F2}");
                            }
                        }
                        else
                        {
                            await writer.WriteLineAsync("Number of ages doesn't match");
                        }
                    }
                    else
                    {
                        await writer.WriteLineAsync("Invalid count");
                    }
                }
            }
            catch (SocketException ex) { Console.WriteLine(ex.ToString()); }
            catch (IOException ex) { Console.WriteLine(ex.ToString()); }
        }
    }
}