using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace commonFDP
{
    class EventoContexto : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Origen> Origenes { get; set; }
        public DbSet<Evento> Eventos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string path = Directory.GetCurrentDirectory();
            optionsBuilder.UseSqlCe(@"Data Source=" + path + "\\Kairos-db.sdf");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Origen>()
                .HasMany(b => b.eventos)
                .WithOne(p => p.origen)
                .HasForeignKey(p => p.idOrigen);
        }
    }
}
