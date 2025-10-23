namespace MBA.Educacao.Online.Core.Application.DTOs
{
    public class LoginResultDto
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public Guid? UsuarioId { get; set; }

        public LoginResultDto()
        {
        }

        public LoginResultDto(bool success, string token = null, string email = null, string nome = null, Guid? usuarioId = null)
        {
            Success = success;
            Token = token;
            Email = email;
            Nome = nome;
            UsuarioId = usuarioId;
        }

        public static LoginResultDto Sucesso(string token, string email, string nome, Guid usuarioId)
        {
            return new LoginResultDto(true, token, email, nome, usuarioId);
        }

        public static LoginResultDto Falha()
        {
            return new LoginResultDto(false);
        }
    }
}
