namespace MBA.Educacao.Online.Core.Domain.Extensions
{
    public static class GuidExtensions
    {
        /// <summary>
        /// Normaliza um GUID para garantir consistência entre diferentes sistemas
        /// </summary>
        /// <param name="guid">O GUID a ser normalizado</param>
        /// <returns>GUID normalizado em formato lowercase</returns>
        public static Guid Normalize(this Guid guid)
        {
            return Guid.Parse(guid.ToString("D").ToLowerInvariant());
        }
        
        /// <summary>
        /// Normaliza uma string GUID para garantir consistência
        /// </summary>
        /// <param name="guidString">A string GUID a ser normalizada</param>
        /// <returns>GUID normalizado</returns>
        public static Guid NormalizeGuid(this string guidString)
        {
            if (string.IsNullOrWhiteSpace(guidString))
                throw new ArgumentException("A string GUID não pode ser nula ou vazia", nameof(guidString));
                
            return Guid.Parse(guidString).Normalize();
        }
        
        /// <summary>
        /// Compara dois GUIDs de forma normalizada
        /// </summary>
        /// <param name="guid1">Primeiro GUID</param>
        /// <param name="guid2">Segundo GUID</param>
        /// <returns>True se os GUIDs são iguais após normalização</returns>
        public static bool EqualsNormalized(this Guid guid1, Guid guid2)
        {
            return guid1.Normalize().Equals(guid2.Normalize());
        }
    }
}

