using FluentAssertions;
using MBA.Educacao.Online.Alunos.Application.Commands;

namespace MBA.Educacao.Online.Alunos.Test.ApplicationUnit
{
    public class CriarAlunoCommandTest
    {
        #region Cenários de Sucesso

        [Fact(DisplayName = "CriarAlunoCommand - Dados Válidos - Deve Passar na Validação")]
        [Trait("Alunos", "Commands - CriarAluno")]
        public void CriarAlunoCommand_DadosValidos_DevePassarNaValidacao()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var nome = "João Silva";
            var email = "joao.silva@example.com";

            var command = new CriarAlunoCommand(usuarioId, nome, email);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeTrue();
            command.ValidationResult.Errors.Should().BeEmpty();
            command.UsuarioId.Should().Be(usuarioId);
            command.Nome.Should().Be(nome);
            command.Email.Should().Be(email);
            command.AggregateId.Should().Be(usuarioId);
        }

        [Fact(DisplayName = "CriarAlunoCommand - Nome com Tamanho Mínimo - Deve Passar na Validação")]
        [Trait("Alunos", "Commands - CriarAluno")]
        public void CriarAlunoCommand_NomeComTamanhoMinimo_DevePassarNaValidacao()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var nome = "Ana"; // 3 caracteres - tamanho mínimo
            var email = "ana@example.com";

            var command = new CriarAlunoCommand(usuarioId, nome, email);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeTrue();
            command.ValidationResult.Errors.Should().BeEmpty();
        }

        [Fact(DisplayName = "CriarAlunoCommand - Nome com Tamanho Máximo - Deve Passar na Validação")]
        [Trait("Alunos", "Commands - CriarAluno")]
        public void CriarAlunoCommand_NomeComTamanhoMaximo_DevePassarNaValidacao()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var nome = new string('A', 100); // 100 caracteres - tamanho máximo
            var email = "usuario@example.com";

            var command = new CriarAlunoCommand(usuarioId, nome, email);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeTrue();
            command.ValidationResult.Errors.Should().BeEmpty();
        }

        [Fact(DisplayName = "CriarAlunoCommand - Email com Tamanho Máximo - Deve Passar na Validação")]
        [Trait("Alunos", "Commands - CriarAluno")]
        public void CriarAlunoCommand_EmailComTamanhoMaximo_DevePassarNaValidacao()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var nome = "João Silva";
            var email = new string('a', 240) + "@example.com"; // 256 caracteres - tamanho máximo

            var command = new CriarAlunoCommand(usuarioId, nome, email);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeTrue();
            command.ValidationResult.Errors.Should().BeEmpty();
        }

        #endregion

        #region Cenários de Falha - UsuarioId

        [Fact(DisplayName = "CriarAlunoCommand - UsuarioId Vazio - Deve Falhar na Validação")]
        [Trait("Alunos", "Commands - CriarAluno")]
        public void CriarAlunoCommand_UsuarioIdVazio_DeveFalharNaValidacao()
        {
            // Arrange
            var usuarioId = Guid.Empty;
            var nome = "João Silva";
            var email = "joao.silva@example.com";

            var command = new CriarAlunoCommand(usuarioId, nome, email);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "UsuarioId");
            command.ValidationResult.Errors.Should().Contain(e =>
                e.ErrorMessage.Contains("O ID do usuário deve ser válido") ||
                e.ErrorMessage.Contains("O ID do usuário é obrigatório"));
        }

        #endregion

        #region Cenários de Falha - Nome

        [Fact(DisplayName = "CriarAlunoCommand - Nome Vazio - Deve Falhar na Validação")]
        [Trait("Alunos", "Commands - CriarAluno")]
        public void CriarAlunoCommand_NomeVazio_DeveFalharNaValidacao()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var nome = string.Empty;
            var email = "joao.silva@example.com";

            var command = new CriarAlunoCommand(usuarioId, nome, email);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Nome");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("O nome é obrigatório"));
        }

        [Fact(DisplayName = "CriarAlunoCommand - Nome Nulo - Deve Falhar na Validação")]
        [Trait("Alunos", "Commands - CriarAluno")]
        public void CriarAlunoCommand_NomeNulo_DeveFalharNaValidacao()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            string nome = null!;
            var email = "joao.silva@example.com";

            var command = new CriarAlunoCommand(usuarioId, nome, email);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Nome");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("O nome é obrigatório"));
        }

        [Fact(DisplayName = "CriarAlunoCommand - Nome com Menos de 3 Caracteres - Deve Falhar na Validação")]
        [Trait("Alunos", "Commands - CriarAluno")]
        public void CriarAlunoCommand_NomeComMenosDe3Caracteres_DeveFalharNaValidacao()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var nome = "Jo"; // 2 caracteres - abaixo do mínimo
            var email = "joao.silva@example.com";

            var command = new CriarAlunoCommand(usuarioId, nome, email);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Nome");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("O nome deve ter no mínimo 3 caracteres"));
        }

        [Fact(DisplayName = "CriarAlunoCommand - Nome com Mais de 100 Caracteres - Deve Falhar na Validação")]
        [Trait("Alunos", "Commands - CriarAluno")]
        public void CriarAlunoCommand_NomeComMaisDe100Caracteres_DeveFalharNaValidacao()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var nome = new string('A', 101); // 101 caracteres - acima do máximo
            var email = "joao.silva@example.com";

            var command = new CriarAlunoCommand(usuarioId, nome, email);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Nome");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("O nome deve ter no máximo 100 caracteres"));
        }

        #endregion

        #region Cenários de Falha - Email

        [Fact(DisplayName = "CriarAlunoCommand - Email Vazio - Deve Falhar na Validação")]
        [Trait("Alunos", "Commands - CriarAluno")]
        public void CriarAlunoCommand_EmailVazio_DeveFalharNaValidacao()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var nome = "João Silva";
            var email = string.Empty;

            var command = new CriarAlunoCommand(usuarioId, nome, email);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Email");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("O e-mail é obrigatório"));
        }

        [Fact(DisplayName = "CriarAlunoCommand - Email Nulo - Deve Falhar na Validação")]
        [Trait("Alunos", "Commands - CriarAluno")]
        public void CriarAlunoCommand_EmailNulo_DeveFalharNaValidacao()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var nome = "João Silva";
            string email = null!;

            var command = new CriarAlunoCommand(usuarioId, nome, email);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Email");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("O e-mail é obrigatório"));
        }

        [Fact(DisplayName = "CriarAlunoCommand - Email Inválido - Deve Falhar na Validação")]
        [Trait("Alunos", "Commands - CriarAluno")]
        public void CriarAlunoCommand_EmailInvalido_DeveFalharNaValidacao()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var nome = "João Silva";
            var email = "email-invalido"; // Email sem @ e domínio

            var command = new CriarAlunoCommand(usuarioId, nome, email);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Email");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("O e-mail informado é inválido"));
        }

        [Fact(DisplayName = "CriarAlunoCommand - Email sem @ - Deve Falhar na Validação")]
        [Trait("Alunos", "Commands - CriarAluno")]
        public void CriarAlunoCommand_EmailSemArroba_DeveFalharNaValidacao()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var nome = "João Silva";
            var email = "joao.silvaexample.com"; // Email sem @

            var command = new CriarAlunoCommand(usuarioId, nome, email);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Email");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("O e-mail informado é inválido"));
        }

        [Fact(DisplayName = "CriarAlunoCommand - Email sem Domínio - Deve Falhar na Validação")]
        [Trait("Alunos", "Commands - CriarAluno")]
        public void CriarAlunoCommand_EmailSemDominio_DeveFalharNaValidacao()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var nome = "João Silva";
            var email = "joao.silva@"; // Email sem domínio

            var command = new CriarAlunoCommand(usuarioId, nome, email);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Email");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("O e-mail informado é inválido"));
        }

        [Fact(DisplayName = "CriarAlunoCommand - Email com Mais de 256 Caracteres - Deve Falhar na Validação")]
        [Trait("Alunos", "Commands - CriarAluno")]
        public void CriarAlunoCommand_EmailComMaisDe256Caracteres_DeveFalharNaValidacao()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var nome = "João Silva";
            var email = new string('a', 250) + "@example.com"; // Mais de 256 caracteres

            var command = new CriarAlunoCommand(usuarioId, nome, email);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Email");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("O e-mail deve ter no máximo 256 caracteres"));
        }

        #endregion

        #region Cenários de Falha - Múltiplos Erros

        [Fact(DisplayName = "CriarAlunoCommand - Múltiplos Erros - Deve Retornar Todos os Erros")]
        [Trait("Alunos", "Commands - CriarAluno")]
        public void CriarAlunoCommand_MultiplosErros_DeveRetornarTodosOsErros()
        {
            // Arrange
            var usuarioId = Guid.Empty;
            var nome = "Jo"; // Nome muito curto
            var email = "email-invalido"; // Email inválido

            var command = new CriarAlunoCommand(usuarioId, nome, email);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().HaveCountGreaterOrEqualTo(3);
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "UsuarioId");
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Nome");
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Email");
        }

        [Fact(DisplayName = "CriarAlunoCommand - Todos os Campos Inválidos - Deve Retornar Todos os Erros")]
        [Trait("Alunos", "Commands - CriarAluno")]
        public void CriarAlunoCommand_TodosOsCamposInvalidos_DeveRetornarTodosOsErros()
        {
            // Arrange
            var usuarioId = Guid.Empty;
            string nome = null!;
            string email = null!;

            var command = new CriarAlunoCommand(usuarioId, nome, email);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().HaveCountGreaterOrEqualTo(3);
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "UsuarioId");
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Nome");
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "Email");
        }

        #endregion
    }
}
