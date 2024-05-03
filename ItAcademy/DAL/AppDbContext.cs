using ItAcademy.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ItAcademy.DAL
{
    public class AppDbContext: IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        { 
        }
       
        public DbSet<Students> Students { get; set; }
        public DbSet<GroupStudent> GroupStudent { get; set; }
        public DbSet<Teachers> Teachers { get; set; }
        public DbSet<Employers> Employers { get; set; }
        public DbSet<Groups> Groups { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<Positions> Positions { get; set; }
        public DbSet<Budget> Budgets { get; set; } 
        public DbSet<Benefit> Benefits { get; set; }
        public DbSet<Cost> Costs { get; set; }
       
       
    }
}
