using System;

namespace ItAcademy.Models
{
    public class Cost
    {

        public int Id { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.UtcNow.AddHours(4);
        public string By { get; set; }
    }
}
