using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            var ep = new IPEndPoint(IPAddress.Loopback, 50000);
            var client = new TcpClient();

            await client.ConnectAsync(ep);
            client.NoDelay = true;

            var encoding = new UTF8Encoding(false);

            var stream = client.GetStream();

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

            Console.WriteLine("connected...");

            string? response = await reader.ReadLineAsync();
            if (response != null) Console.WriteLine(response);
            Console.WriteLine("Enter number of people in group: ");
            string? countInput = Console.ReadLine();
            
            if (int.TryParse(countInput, out int count) && count > 0)
            {
                int[] ages = new int[count];
                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine($"Enter age of person {i + 1}: ");
                    string? ageInput = Console.ReadLine();
                    
                    if (int.TryParse(ageInput, out int age) && age > 0 && age < 150)
                    {
                        ages[i] = age;
                    }
                    else
                    {
                        Console.WriteLine("Invalid age, using 0");
                        ages[i] = 0;
                    }
                }

                string agesString = string.Join(" ", ages);
                await writer.WriteLineAsync(agesString);
            }
            else
            {
                await writer.WriteLineAsync("0");
            }
            response = await reader.ReadLineAsync();
            if (response != null) Console.WriteLine(response);

            Console.ReadKey();

            return 0;
        }
    }
}