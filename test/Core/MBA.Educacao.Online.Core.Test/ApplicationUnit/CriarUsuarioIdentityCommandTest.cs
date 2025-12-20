using FluentAssertions;
using MBA.Educacao.Online.Core.Application.Commands.Identity;

namespace MBA.Educacao.Online.Core.Test.ApplicationUnit
{
    public class CriarUsuarioIdentityCommandTest
    {
        #region Cenários de Sucesso

        [Fact(DisplayName = "CriarUsuarioIdentityCommand - Dados Válidos - Deve Passar na Validação")]
        [Trait("Core", "Commands - Identity - CriarUsuarioIdentity")]
        public void CriarUsuarioIdentityCommand_DadosValidos_DevePassarNaValidacao()
        {
            // Arrange
            var nome = "João Silva";
            var email = "joao.silva@example.com";
            var senha = "Senha123";
            var confirmacaoSenha = "Senha123";

            var command = new CriarUsuarioIdentityCommand(nome, email, senha, confirmacaoSenha);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeTrue();
            command.ValidationResult.Errors.Should().BeEmpty();
            command.Nome.Should().Be(nome);
            command.Email.Should().Be(email);
            command.Senha.Should().Be(senha);
            command.ConfirmacaoSenha.Should().Be(confirmacaoSenha);
        }

        [Fact(DisplayName = "CriarUsuarioIdentityCommand - Nome com Tamanho Mínimo - Deve Passar na Validação")]
        [Trait("Core", "Commands - Identity - CriarUsuarioIdentity")]
        public void CriarUsuarioIdentityCommand_NomeComTamanhoMinimo_DevePassarNaValidacao()
        {
            // Arrange
            var nome = "Ana"; // 3 caracteres - tamanho mínimo
            var email = "ana@example.com";
            var senha = "Senha123";
            var confirmacaoSenha = "Senha123";

            var command = new CriarUsuarioIdentityCommand(nome, email, senha, confirmacaoSenha);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeTrue();
            command.ValidationResult.Errors.Should().BeEmpty();
        }

        [Fact(DisplayName = "CriarUsuarioIdentityCommand - Nome com Tamanho Máximo - Deve Passar na Validação")]
        [Trait("Core", "Commands - Identity - CriarUsuarioIdentity")]
        public void CriarUsuarioIdentityCommand_NomeComTamanhoMaximo_DevePassarNaValidacao()
        {
            // Arrange
            var nome = new string('A', 100); // 100 caracteres - tamanho máximo
            var email = "usuario@example.com";
            var senha = "Senha123";
            var confirmacaoSenha = "Senha123";

            var command = new CriarUsuarioIdentityCommand(nome, email, senha, confirmacaoSenha);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeTrue();
            command.ValidationResult.Errors.Should().BeEmpty();
        }

        [Fact(DisplayName = "CriarUsuarioIdentityCommand - Email com Tamanho Máximo - Deve Passar na Validação")]
        [Trait("Core", "Commands - Identity - CriarUsuarioIdentity")]
        public void CriarUsuarioIdentityCommand_EmailComTamanhoMaximo_DevePassarNaValidacao()
        {
            // Arrange
            var nome = "João Silva";
            var email = new string('a', 240) + "@example.com"; // 256 caracteres - tamanho máximo
            var senha = "Senha123";
            var confirmacaoSenha = "Senha123";

            var command = new CriarUsuarioIdentityCommand(nome, email, senha, confirmacaoSenha);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeTrue();
            command.ValidationResult.Errors.Should().BeEmpty();
        }

        [Fact(DisplayName = "CriarUsuarioIdentityCommand - Senha com Tamanho Mínimo - Deve Passar na Validação")]
        [Trait("Core", "Commands - Identity - CriarUsuarioIdentity")]
        public void CriarUsuarioIdentityCommand_SenhaComTamanhoMinimo_DevePassarNaValidacao()
        {
            // Arrange
            var nome = "João Silva";
            var email = "joao@example.com";
            var senha = "123456"; // 6 caracteres - tamanho mínimo
            var confirmacaoSenha = "123456";

            var command = new CriarUsuarioIdentityCommand(nome, email, senha, confirmacaoSenha);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeTrue();
            command.ValidationResult.Errors.Should().BeEmpty();
        }

        [Fact(DisplayName = "CriarUsuarioIdentityCommand - SetUsuarioId - Deve Definir AggregateId")]
        [Trait("Core", "Commands - Identity - CriarUsuarioIdentity")]
        public void CriarUsuarioIdentityCommand_SetUsuarioId_DeveDefinirAggregateId()
        {
            // Arrange
            var nome = "João Silva";
            var email = "joao@example.com";
            var senha = "Senha123";
            var confirmacaoSenha = "Senha123";
            var usuarioId = Guid.NewGuid();

            var command = new CriarUsuarioIdentityCommand(nome, email, senha, confirmacaoSenha);

            // Act
            command.SetUsuarioId(usuarioId);

            // Assert
            command.AggregateId.Should().Be(usuarioId);
        }

        #endregion

        #region Cenários de Falha - Nome

        [Fact(DisplayName = "CriarUsuarioIdentityCommand - Nome Vazio - Deve Falhar na Validação")]
        [Trait("Core", "Commands - Identity - CriarUsuarioIdentity")]
        public void CriarUsuarioIdentityCommand_NomeVazio_DeveFalharNaValidacao()
        {
            // Arrange
            var nome = string.Empty;
            var email = "joao@example.com";
            var senha = "Senha123";
            var confirmacaoSenha = "Senha123";

            var command = new CriarUsuarioIdentityCommand(nome, email, senha, confirmacaoSenha);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Nome");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("O nome é obrigatório"));
        }

        [Fact(DisplayName = "CriarUsuarioIdentityCommand - Nome Nulo - Deve Falhar na Validação")]
        [Trait("Core", "Commands - Identity - CriarUsuarioIdentity")]
        public void CriarUsuarioIdentityCommand_NomeNulo_DeveFalharNaValidacao()
        {
            // Arrange
            string nome = null!;
            var email = "joao@example.com";
            var senha = "Senha123";
            var confirmacaoSenha = "Senha123";

            var command = new CriarUsuarioIdentityCommand(nome, email, senha, confirmacaoSenha);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Nome");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("O nome é obrigatório"));
        }

        [Fact(DisplayName = "CriarUsuarioIdentityCommand - Nome com Menos de 3 Caracteres - Deve Falhar na Validação")]
        [Trait("Core", "Commands - Identity - CriarUsuarioIdentity")]
        public void CriarUsuarioIdentityCommand_NomeComMenosDe3Caracteres_DeveFalharNaValidacao()
        {
            // Arrange
            var nome = "Jo"; // 2 caracteres - menor que o mínimo
            var email = "joao@example.com";
            var senha = "Senha123";
            var confirmacaoSenha = "Senha123";

            var command = new CriarUsuarioIdentityCommand(nome, email, senha, confirmacaoSenha);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Nome");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("O nome deve ter no mínimo 3 caracteres"));
        }

        [Fact(DisplayName = "CriarUsuarioIdentityCommand - Nome com Mais de 100 Caracteres - Deve Falhar na Validação")]
        [Trait("Core", "Commands - Identity - CriarUsuarioIdentity")]
        public void CriarUsuarioIdentityCommand_NomeComMaisDe100Caracteres_DeveFalharNaValidacao()
        {
            // Arrange
            var nome = new string('A', 101); // 101 caracteres - maior que o máximo
            var email = "joao@example.com";
            var senha = "Senha123";
            var confirmacaoSenha = "Senha123";

            var command = new CriarUsuarioIdentityCommand(nome, email, senha, confirmacaoSenha);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Nome");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("O nome deve ter no máximo 100 caracteres"));
        }

        #endregion

        #region Cenários de Falha - Email

        [Fact(DisplayName = "CriarUsuarioIdentityCommand - Email Vazio - Deve Falhar na Validação")]
        [Trait("Core", "Commands - Identity - CriarUsuarioIdentity")]
        public void CriarUsuarioIdentityCommand_EmailVazio_DeveFalharNaValidacao()
        {
            // Arrange
            var nome = "João Silva";
            var email = string.Empty;
            var senha = "Senha123";
            var confirmacaoSenha = "Senha123";

            var command = new CriarUsuarioIdentityCommand(nome, email, senha, confirmacaoSenha);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Email");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("O e-mail é obrigatório"));
        }

        [Fact(DisplayName = "CriarUsuarioIdentityCommand - Email Nulo - Deve Falhar na Validação")]
        [Trait("Core", "Commands - Identity - CriarUsuarioIdentity")]
        public void CriarUsuarioIdentityCommand_EmailNulo_DeveFalharNaValidacao()
        {
            // Arrange
            var nome = "João Silva";
            string email = null!;
            var senha = "Senha123";
            var confirmacaoSenha = "Senha123";

            var command = new CriarUsuarioIdentityCommand(nome, email, senha, confirmacaoSenha);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Email");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("O e-mail é obrigatório"));
        }

        [Fact(DisplayName = "CriarUsuarioIdentityCommand - Email Inválido - Deve Falhar na Validação")]
        [Trait("Core", "Commands - Identity - CriarUsuarioIdentity")]
        public void CriarUsuarioIdentityCommand_EmailInvalido_DeveFalharNaValidacao()
        {
            // Arrange
            var nome = "João Silva";
            var email = "emailinvalido"; // Email sem @ e domínio
            var senha = "Senha123";
            var confirmacaoSenha = "Senha123";

            var command = new CriarUsuarioIdentityCommand(nome, email, senha, confirmacaoSenha);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Email");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("O e-mail informado é inválido"));
        }

        [Fact(DisplayName = "CriarUsuarioIdentityCommand - Email com Mais de 256 Caracteres - Deve Falhar na Validação")]
        [Trait("Core", "Commands - Identity - CriarUsuarioIdentity")]
        public void CriarUsuarioIdentityCommand_EmailComMaisDe256Caracteres_DeveFalharNaValidacao()
        {
            // Arrange
            var nome = "João Silva";
            var email = new string('a', 250) + "@example.com"; // Mais de 256 caracteres
            var senha = "Senha123";
            var confirmacaoSenha = "Senha123";

            var command = new CriarUsuarioIdentityCommand(nome, email, senha, confirmacaoSenha);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Email");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("O e-mail deve ter no máximo 256 caracteres"));
        }

        #endregion

        #region Cenários de Falha - Senha

        [Fact(DisplayName = "CriarUsuarioIdentityCommand - Senha Vazia - Deve Falhar na Validação")]
        [Trait("Core", "Commands - Identity - CriarUsuarioIdentity")]
        public void CriarUsuarioIdentityCommand_SenhaVazia_DeveFalharNaValidacao()
        {
            // Arrange
            var nome = "João Silva";
            var email = "joao@example.com";
            var senha = string.Empty;
            var confirmacaoSenha = string.Empty;

            var command = new CriarUsuarioIdentityCommand(nome, email, senha, confirmacaoSenha);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Senha");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("A senha é obrigatória"));
        }

        [Fact(DisplayName = "CriarUsuarioIdentityCommand - Senha Nula - Deve Falhar na Validação")]
        [Trait("Core", "Commands - Identity - CriarUsuarioIdentity")]
        public void CriarUsuarioIdentityCommand_SenhaNula_DeveFalharNaValidacao()
        {
            // Arrange
            var nome = "João Silva";
            var email = "joao@example.com";
            string senha = null!;
            string confirmacaoSenha = null!;

            var command = new CriarUsuarioIdentityCommand(nome, email, senha, confirmacaoSenha);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Senha");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("A senha é obrigatória"));
        }

        [Fact(DisplayName = "CriarUsuarioIdentityCommand - Senha com Menos de 6 Caracteres - Deve Falhar na Validação")]
        [Trait("Core", "Commands - Identity - CriarUsuarioIdentity")]
        public void CriarUsuarioIdentityCommand_SenhaComMenosDe6Caracteres_DeveFalharNaValidacao()
        {
            // Arrange
            var nome = "João Silva";
            var email = "joao@example.com";
            var senha = "12345"; // 5 caracteres - menor que o mínimo
            var confirmacaoSenha = "12345";

            var command = new CriarUsuarioIdentityCommand(nome, email, senha, confirmacaoSenha);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Senha");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("A senha deve ter no mínimo 6 caracteres"));
        }

        #endregion

        #region Cenários de Falha - Confirmação de Senha

        [Fact(DisplayName = "CriarUsuarioIdentityCommand - Confirmação de Senha Vazia - Deve Falhar na Validação")]
        [Trait("Core", "Commands - Identity - CriarUsuarioIdentity")]
        public void CriarUsuarioIdentityCommand_ConfirmacaoSenhaVazia_DeveFalharNaValidacao()
        {
            // Arrange
            var nome = "João Silva";
            var email = "joao@example.com";
            var senha = "Senha123";
            var confirmacaoSenha = string.Empty;

            var command = new CriarUsuarioIdentityCommand(nome, email, senha, confirmacaoSenha);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "ConfirmacaoSenha");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("A confirmação de senha é obrigatória"));
        }

        [Fact(DisplayName = "CriarUsuarioIdentityCommand - Confirmação de Senha Nula - Deve Falhar na Validação")]
        [Trait("Core", "Commands - Identity - CriarUsuarioIdentity")]
        public void CriarUsuarioIdentityCommand_ConfirmacaoSenhaNula_DeveFalharNaValidacao()
        {
            // Arrange
            var nome = "João Silva";
            var email = "joao@example.com";
            var senha = "Senha123";
            string confirmacaoSenha = null!;

            var command = new CriarUsuarioIdentityCommand(nome, email, senha, confirmacaoSenha);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "ConfirmacaoSenha");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("A confirmação de senha é obrigatória"));
        }

        [Fact(DisplayName = "CriarUsuarioIdentityCommand - Senha e Confirmação de Senha Diferentes - Deve Falhar na Validação")]
        [Trait("Core", "Commands - Identity - CriarUsuarioIdentity")]
        public void CriarUsuarioIdentityCommand_SenhaEConfirmacaoSenhaDiferentes_DeveFalharNaValidacao()
        {
            // Arrange
            var nome = "João Silva";
            var email = "joao@example.com";
            var senha = "Senha123";
            var confirmacaoSenha = "Senha456"; // Diferente da senha

            var command = new CriarUsuarioIdentityCommand(nome, email, senha, confirmacaoSenha);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "ConfirmacaoSenha");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("A senha e a confirmação de senha não conferem"));
        }

        #endregion

        #region Cenários de Falha - Múltiplos Campos

        [Fact(DisplayName = "CriarUsuarioIdentityCommand - Múltiplos Campos Inválidos - Deve Falhar na Validação")]
        [Trait("Core", "Commands - Identity - CriarUsuarioIdentity")]
        public void CriarUsuarioIdentityCommand_MultiplosCamposInvalidos_DeveFalharNaValidacao()
        {
            // Arrange
            var nome = "Jo"; // Inválido - menos de 3 caracteres
            var email = "emailinvalido"; // Inválido - não é um email válido
            var senha = "123"; // Inválido - menos de 6 caracteres
            var confirmacaoSenha = "456"; // Inválido - diferente da senha

            var command = new CriarUsuarioIdentityCommand(nome, email, senha, confirmacaoSenha);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().HaveCountGreaterThan(1);
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Nome");
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Email");
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Senha");
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "ConfirmacaoSenha");
        }

        #endregion
    }
}
