using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VejrstationAPI.Data;
using VejrstationAPI.Models;

[assembly: HostingStartup(typeof(VejrstationAPI.Areas.Identity.IdentityHostingStartup))]
namespace VejrstationAPI.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<VejrstationAPIContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("VejrstationAPIContextConnection")));

                services.AddDefaultIdentity<VejrstationAPIUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<VejrstationAPIContext>();
            });
        }
    }
}