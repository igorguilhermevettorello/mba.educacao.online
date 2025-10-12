using MBA.Educacao.Online.Core.Data.Context;
using MBA.Educacao.Online.Core.Domain.Enums;
using MBA.Educacao.Online.Core.Domain.Extensions;
using Microsoft.AspNetCore.Identity;

namespace MBA.Educacao.Online.Core.Data.Seeds
{
    public static class SeederAplicacao
    {
        public static async Task EnsureSeedRoles(IdentityDbContext contextIdentity)
        {
            try
            {
                // Verifica se já existem roles criadas
                if (contextIdentity.Roles.Any())
                    return;

                // Obtém todos os valores do enum TipoUsuario
                var tiposUsuario = Enum.GetValues(typeof(TipoUsuario)).Cast<TipoUsuario>();

                foreach (var tipoUsuario in tiposUsuario)
                {
                    var roleName = tipoUsuario.GetDescription();
                    var normalizedRoleName = roleName.ToUpperInvariant();
                    if (!contextIdentity.Roles.Any(r => r.NormalizedName == normalizedRoleName))
                    {
                        await contextIdentity.Roles.AddAsync(new IdentityRole
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = roleName,
                            NormalizedName = normalizedRoleName,
                            ConcurrencyStamp = Guid.NewGuid().ToString()
                        });
                    }
                }

                await contextIdentity.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var _ = ex.Message;
            }
        }
        public static async Task EnsureSeedApplication(UserManager<IdentityUser> userManager, IdentityDbContext contextIdentity)
        {
            var emailAdmin = "administrador@educacao.com.br";
            if (await userManager.FindByEmailAsync(emailAdmin) == null)
            {
                var user = new IdentityUser
                {
                    UserName = "Administrador",
                    Email = emailAdmin,
                    NormalizedUserName = "Administrador",
                    NormalizedEmail = emailAdmin,
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                };

                var result = await userManager.CreateAsync(user, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, TipoUsuario.Administrador.ToString().ToUpper());
                }

                await contextIdentity.SaveChangesAsync();
            }
        }
    }
}
