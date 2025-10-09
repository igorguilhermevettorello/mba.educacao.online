namespace MBA.Educacao.Online.API.Extensions
{
    public class IdentityErrorDescriberPtBr : IdentityErrorDescriber
    {
        public override IdentityError PasswordTooShort(int length)
        => new() { Code = nameof(PasswordTooShort), Description = $"A senha deve ter no mínimo {length} caracteres." };

        public override IdentityError PasswordRequiresNonAlphanumeric()
            => new() { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "A senha deve conter pelo menos um caractere especial (!@#$%)." };

        public override IdentityError PasswordRequiresLower()
            => new() { Code = nameof(PasswordRequiresLower), Description = "A senha deve conter pelo menos uma letra minúscula." };

        public override IdentityError PasswordRequiresUpper()
            => new() { Code = nameof(PasswordRequiresUpper), Description = "A senha deve conter pelo menos uma letra maiúscula." };

        public override IdentityError PasswordRequiresDigit()
            => new() { Code = nameof(PasswordRequiresDigit), Description = "A senha deve conter pelo menos um número." };

        public override IdentityError DuplicateUserName(string userName)
            => new() { Code = nameof(DuplicateUserName), Description = $"O usuário '{userName}' já está em uso." };

        public override IdentityError DuplicateEmail(string email)
            => new() { Code = nameof(DuplicateEmail), Description = $"O email '{email}' já está em uso." };

        public override IdentityError InvalidEmail(string email)
            => new() { Code = nameof(InvalidEmail), Description = $"O email '{email}' não é válido." };

        public override IdentityError InvalidUserName(string userName)
            => new() { Code = nameof(InvalidUserName), Description = $"O nome de usuário '{userName}' não é válido." };

        public override IdentityError PasswordMismatch()
            => new() { Code = nameof(PasswordMismatch), Description = "Senha incorreta." };

        public override IdentityError UserAlreadyHasPassword()
            => new() { Code = nameof(UserAlreadyHasPassword), Description = "O usuário já possui uma senha definida." };

        public override IdentityError UserLockoutNotEnabled()
            => new() { Code = nameof(UserLockoutNotEnabled), Description = "O bloqueio de usuário não está habilitado." };

        public override IdentityError UserAlreadyInRole(string role)
            => new() { Code = nameof(UserAlreadyInRole), Description = $"O usuário já está na função '{role}'." };

        public override IdentityError UserNotInRole(string role)
            => new() { Code = nameof(UserNotInRole), Description = $"O usuário não está na função '{role}'." };

        public override IdentityError InvalidToken()
            => new() { Code = nameof(InvalidToken), Description = "Token inválido." };

        public override IdentityError RecoveryCodeRedemptionFailed()
            => new() { Code = nameof(RecoveryCodeRedemptionFailed), Description = "Falha na recuperação do código." };

        public override IdentityError LoginAlreadyAssociated()
            => new() { Code = nameof(LoginAlreadyAssociated), Description = "Um usuário com este login já existe." };

        public override IdentityError InvalidRoleName(string role)
            => new() { Code = nameof(InvalidRoleName), Description = $"O nome da função '{role}' não é válido." };

        public override IdentityError DuplicateRoleName(string role)
            => new() { Code = nameof(DuplicateRoleName), Description = $"A função '{role}' já existe." };

        public override IdentityError ConcurrencyFailure()
            => new() { Code = nameof(ConcurrencyFailure), Description = "Falha de concorrência otimista, o objeto foi modificado." };
    }
}

