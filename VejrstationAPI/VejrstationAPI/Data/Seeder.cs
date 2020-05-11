using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using VejrstationAPI.Models;

namespace VejrstationAPI.Data
{
    public class Seeder
    {
        public static void Seed(IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var userManager = scope.ServiceProvider.GetService<UserManager<VejrstationAPIUser>>();

            const string seededUser = "SeededUser@seed.dk";
            const string seededPass = "SeededPassWord123!";

            if (userManager.FindByNameAsync(seededUser).Result == null)
            {
                var user = new VejrstationAPIUser
                {
                    UserName = seededUser,
                    Email = seededUser,
                    EmailConfirmed = true
                };
                
                IdentityResult result = userManager.CreateAsync(user, seededPass).Result;
            }

            scope.Dispose();
        }

        public static void UnSeed(IApplicationBuilder app)
        {

            var scope = app.ApplicationServices.CreateScope();
            var userManager = scope.ServiceProvider.GetService<UserManager<VejrstationAPIUser>>();

            const string seededUser = "SeededUser@seed.dk";

            var findUserTask = userManager.FindByNameAsync(seededUser);

            if (findUserTask.Result != null)
            {
                var user = findUserTask.Result;

                userManager.DeleteAsync(user);
            }

            scope.Dispose();
        }
    }
}
