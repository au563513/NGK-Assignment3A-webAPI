using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using VejrstationAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace VejrstationAPI.Data
{
    public interface IVejrstationAPIContext
    {
        DbSet<Vejrobservation> Vejrobservationer { get; set; }
        public DbSet<Sted> Steder { get; set; }
        public DbSet<VejrstationAPIUser> VejrstationAPIUsers { get; set; }
    }
}