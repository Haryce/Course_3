using System;

namespace ConsoleApp4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] symbols = { "y", "p", "l", "k", "z", "h", "rr" };
            int[] multipliers = { 2, 3, 5, 10, 12, 15, 21 };     
            int balance = 1000;      
            Console.WriteLine($"баланс: {balance}");
            while (balance >= 100)
            {
                Console.Write($"\nвведите ставку,'q' для выхода\n баланс: {balance}: ");
                string input = Console.ReadLine();       
                if (input.ToLower() == "q")
                {
                    break;
                }

                if (!int.TryParse(input, out int bet) || bet < 100 || bet > balance)
                {
                    Console.WriteLine("err");
                    continue;
                }
                balance -= bet;

                Random random = new Random();
                string[] result = new string[3];
                for (int i = 0; i < 3; i++)
                {
                    result[i] = symbols[random.Next(symbols.Length)];
                }

                Console.WriteLine($"[{result[0]}] [{result[1]}] [{result[2]}]");

                int winMultiplier = CheckCombinations(result, symbols, multipliers);
                
                if (winMultiplier > 0)
                {
                    int winAmount = bet * winMultiplier;
                    balance += winAmount;
                    Console.WriteLine($"{winAmount} x{winMultiplier}");
                }
                else
                {
                    Console.WriteLine("unluck");
                }
            }
            if (balance < 100)
            {
                Console.WriteLine("\nНедостаточно средств");
            }     
            Console.WriteLine($"баланс: {balance}");
            Console.WriteLine("q для выхода");
            Console.ReadKey();
        }
        static int CheckCombinations(string[] result, string[] symbols, int[] multipliers)
        {
            if (result[0] == result[1] && result[1] == result[2])
            {
                int index = Array.IndexOf(symbols, result[0]);
                return multipliers[index];
            }        
            if (result[0] == result[1] || result[1] == result[2] || result[0] == result[2])
            {
                string matchingSymbol = result[0] == result[1] ? result[0] : 
                                      result[1] == result[2] ? result[1] : result[0];
                
                int index = Array.IndexOf(symbols, matchingSymbol);
                return multipliers[index] / 2;
            }
            if ((result[0] == "rr" && result[1] == "rr") ||
                (result[1] == "rr" && result[2] == "rr"))
            {
                return multipliers[Array.IndexOf(symbols, "rr")] / 3;
            }

            return 0;
        }
    }
}