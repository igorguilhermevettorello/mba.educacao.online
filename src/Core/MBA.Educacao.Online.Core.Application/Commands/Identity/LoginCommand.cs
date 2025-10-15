using MBA.Educacao.Online.Core.Application.Models;
using MBA.Educacao.Online.Core.Application.Validators.Identity;
using FluentValidation.Results;
using MediatR;

namespace MBA.Educacao.Online.Core.Application.Commands.Identity
{
    public class LoginCommand : IRequest<LoginResult>
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public LoginResult Result { get; private set; }
        public ValidationResult ValidationResult { get; set; }

        public LoginCommand(string email, string senha)
        {
            Email = email;
            Senha = senha;
        }

        public void SetResult(LoginResult result)
        {
            Result = result;
        }

        public bool IsValid()
        {
            ValidationResult = new LoginCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

