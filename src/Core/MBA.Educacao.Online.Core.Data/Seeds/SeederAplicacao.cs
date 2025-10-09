using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBA.Educacao.Online.Core.Data.Seeds
{
    public static class SeederAplicacao
    {
        public static async Task SeedData(IServiceProvider serviceProvider, string env)
        {
            await EnsureSeedData(serviceProvider, env);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider, string env)
        {
            try
            {
                //using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
                //var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                //var contextIdentity = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
                //var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                //if (env.Equals("Development") || env.Equals("Docker") || env.Equals("Staging"))
                //{
                //    await context.Database.MigrateAsync();
                //    await contextIdentity.Database.MigrateAsync();

                //    await EnsureSeedRoles(contextIdentity);
                //    await EnsureSeedApplication(userManager, context, contextIdentity);
                //}
            }
            catch (Exception ex)
            {
                var _ = ex.Message;
            }
        }
    }
}
