using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] sym = { "y", "p", "l", "k", "z", "h", "rr" };
            int[] mult = { 2, 3, 5, 10, 12, 15, 21 };
            while (true)
            {
                Console.Write("Enter bet, min bet 100: ");
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int bet)||bet < 100)
                {
                    Console.WriteLine("can not bet\n");
                    continue;
                }
                Random random = new Random();
                string[] res = new string[3];
                for (int i = 0; i < 3; i++)
                {
                    res[i] = sym[random.Next(sym.Length)];
                }
                Console.WriteLine($"[{res[0]}] [{res[1]}] [{res[2]}]\n");
                if (res[0] == res[1] && res[1] == res[2])
                {
                    int multpl = Array.IndexOf(sym, res[0]);
                    int winshow = bet * mult[multpl];
                    Console.WriteLine($"you won {winshow}!");
                }
                else
                {
                    Console.WriteLine("unluck");
                }
            }
            }
    }
}
