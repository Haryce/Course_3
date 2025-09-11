using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Clock
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public Clock(int hours, int minutes, int seconds)
        {
            SetTime(hours, minutes, seconds);
        }
        public void Tick()
        {
            Seconds++;
            if (Seconds >= 60)
            {
                Seconds= 0;
                Minutes++;
                if (Minutes >= 60)
                {
                    Minutes= 0;
                    Hours++;
                    if (Hours >= 24)
                    {
                        Hours= 0;
                    }
                }
            }
        }
        public string DisplayTime()
        {
            return $"{Hours:00}:{Minutes:00}:{Seconds:00}";
        }
        public void SetTime(int hours, int minutes, int seconds)
        {
            Hours= (hours >= 0 && hours < 24) ? hours : 0;
            Minutes= (minutes >= 0 && minutes < 60) ? minutes : 0;
            Seconds= (seconds >= 0 && seconds < 60) ? seconds : 0;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Clock cl = new Clock(11, 43, 22);
            Console.WriteLine("Time: "+cl.DisplayTime());
            cl.Tick();
            Console.WriteLine("1 tick: "+cl.DisplayTime());
            cl.Tick();
            Console.WriteLine("2 tick: "+cl.DisplayTime());
            cl.SetTime(14, 5, 9);
            Console.WriteLine(cl.DisplayTime());
        }
    }
}
