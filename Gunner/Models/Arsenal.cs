using System;
using System.Data.Entity;

namespace Gunner.Models
{
    public class Fixtures
    {
        public int Id { get; set; }
        public string Opponent { get; set; }
        public DateTime Date { get; set; }
        public string Result { get; set; }
        public string Ground { get; set; }
        public string Stadium { get; set; }
        public int Attendance { get; set; }
        public string Referee { get; set; }
    }

    public class ArsenalDbContext : DbContext
    {
        public DbSet<Fixtures> Fixtures { get; set; }
    }

}