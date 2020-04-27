using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VejrstationAPI.Models;

namespace VejrstationAPI.Data
{
    public class VejrstationAPIContext : IdentityDbContext<VejrstationAPIUser>
    {
        public VejrstationAPIContext(DbContextOptions<VejrstationAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Vejrobservation> Vejrobservationer { get; set; }
        public DbSet<Sted> Steder { get; set; }
        public DbSet<VejrstationAPIUser> VejrstationAPIUsers { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);


            builder.Entity<Vejrobservation>()
                .HasOne(v => v.Sted)
                .WithMany(s => s.Vejrobservationer)
                .HasForeignKey(v => v.StedNavn);
        }
    }
}
