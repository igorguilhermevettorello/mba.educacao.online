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
    }
}
