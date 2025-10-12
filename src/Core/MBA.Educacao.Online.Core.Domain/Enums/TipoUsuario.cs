using System.ComponentModel;

namespace MBA.Educacao.Online.Core.Domain.Enums
{
    public enum TipoUsuario
    {
        [Description("Administrador")]
        Administrador,
        [Description("Vendedor")]
        Vendedor,
        [Description("Cliente")]
        Cliente
    }
}
