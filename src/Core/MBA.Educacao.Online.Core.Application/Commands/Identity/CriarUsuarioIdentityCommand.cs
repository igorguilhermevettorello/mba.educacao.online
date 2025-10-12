using MBA.Educacao.Online.Core.Application.Validators.Identity;
using MBA.Educacao.Online.Core.Domain.Messages;

namespace MBA.Educacao.Online.Core.Application.Commands.Identity
{
    public class CriarUsuarioIdentityCommand : Command
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string ConfirmacaoSenha { get; set; }
        public Guid? UsuarioId { get; private set; }

        public CriarUsuarioIdentityCommand(string nome, string email, string senha, string confirmacaoSenha)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
            ConfirmacaoSenha = confirmacaoSenha;
        }

        public void SetUsuarioId(Guid usuarioId)
        {
            UsuarioId = usuarioId;
        }

        public override bool IsValid()
        {
            ValidationResult = new CriarUsuarioIdentityCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

