using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            var ep = new IPEndPoint(IPAddress.Loopback, 50000);
            var listener = new TcpListener(ep);
            listener.Start();

            Console.WriteLine("Waiting for clients...");

            using TcpClient client = await listener.AcceptTcpClientAsync();

            NetworkStream stream = client.GetStream();
            var encoding = new UTF8Encoding(false);

            using var reader = new StreamReader(
                stream,
                encoding,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 4096,
                leaveOpen: false
                );

            using var writer = new StreamWriter(
                stream,
                encoding,
                bufferSize: 4096,
                leaveOpen: false
                )
            { AutoFlush = true };

            await writer.WriteLineAsync("Hello client");

            string? response = await reader.ReadLineAsync();

            if (response != null)
            {
                string[] ages = response.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                
                if (ages.Length > 0)
                {
                    int sum = 0;
                    int validAges = 0;
                    
                    foreach (string ageStr in ages)
                    {
                        if (int.TryParse(ageStr, out int age) && age > 0 && age < 150)
                        {
                            sum += age;
                            validAges++;
                        }
                    }
                    
                    if (validAges > 0)
                    {
                        double averageAge = (double)sum / validAges;
                        await writer.WriteLineAsync($"Average age in group: {averageAge:F2}");
                    }
                    else
                    {
                        await writer.WriteLineAsync("No valid ages provided");
                    }
                }
                else
                {
                    await writer.WriteLineAsync("No ages provided");
                }
            }

            Console.ReadKey();

            return 0;
        }
    }
}