using System;

namespace ItAcademy.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public double LastModifiedAmount { get; set; }
        public string LastModifiedDescription { get; set; }
        public DateTime LastModifiedDate { get; set; }= DateTime.UtcNow.AddHours(4);
        public Double TotalBudget { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
