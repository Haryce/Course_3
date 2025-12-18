using System;

namespace StoreG1G3.Models
{
    public class Shift
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime ShiftDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string ShiftType { get; set; } // Утренняя, дневная, ночная

        // Навигационное свойство
        public virtual Employee Employee { get; set; }

        public TimeSpan Duration => EndTime - StartTime;
    }
}