using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VejrstationAPI.Models;

namespace VejrstationAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Vejrobservation> Vejrobservationer { get; set; }
        public DbSet<Sted> Steder { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vejrobservation>()
                .HasOne(v => v.Sted)
                .WithMany(s => s.Vejrobservationer)
                .HasForeignKey(v => v.StedNavn);
        }
    }
}