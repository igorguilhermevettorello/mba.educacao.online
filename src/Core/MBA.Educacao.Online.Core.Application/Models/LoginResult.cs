namespace MBA.Educacao.Online.Core.Application.Models
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public Guid? UsuarioId { get; set; }

        public LoginResult()
        {
        }

        public LoginResult(bool success, string token = null, string email = null, string nome = null, Guid? usuarioId = null)
        {
            Success = success;
            Token = token;
            Email = email;
            Nome = nome;
            UsuarioId = usuarioId;
        }

        public static LoginResult Sucesso(string token, string email, string nome, Guid usuarioId)
        {
            return new LoginResult(true, token, email, nome, usuarioId);
        }

        public static LoginResult Falha()
        {
            return new LoginResult(false);
        }
    }
}

