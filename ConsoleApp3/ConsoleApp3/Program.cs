using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("enter size N: ");
            int n = Convert.ToInt32(Console.ReadLine());
            int[] arr = new int[n];
            Console.WriteLine("enter el:");
            for (int i = 0; i < n; i++)
            {
                Console.Write($"el [{i}]: ");
                arr[i] = Convert.ToInt32(Console.ReadLine());
            }
            List<int> zxc = new List<int>();
            for (int i = 1; i < n; i++)
            {
                if (arr[i] > arr[i - 1])
                {
                    zxc.Add(i);
                }
            }
            for (int i = 0; i < zxc.Count - 1; i++)
            {
                for (int j = i + 1; j < zxc.Count; j++)
                {
                    if (zxc[i] < zxc[j])
                    {
                        int temp = zxc[i];
                        zxc[i] = zxc[j];
                        zxc[j] = temp;
                    }
                }
            }
            Console.WriteLine($"\ntotal el: {zxc.Count}");
            if (zxc.Count > 0)
            {
                Console.WriteLine("Num el v ubuvania:");
                foreach (int index in zxc)
                {
                    Console.WriteLine(index);
                }
            }
            else
            {
                Console.WriteLine("none el");
            }
        }
    }
}
