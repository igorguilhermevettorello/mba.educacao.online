using FluentAssertions;
using MBA.Educacao.Online.Alunos.Application.Commands;

namespace MBA.Educacao.Online.Alunos.Test.ApplicationUnit
{
    public class ValidatorsTest
    {
        #region IniciarEstudoAulaCommandValidator

        [Fact(DisplayName = "IniciarEstudoAulaValidator - Dados Válidos - Deve Passar na Validação")]
        [Trait("Alunos", "Validators")]
        public void IniciarEstudoAulaValidator_DadosValidos_DevePassarNaValidacao()
        {
            // Arrange
            var command = new IniciarEstudoAulaCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeTrue();
            command.ValidationResult.Errors.Should().BeEmpty();
        }

        [Fact(DisplayName = "IniciarEstudoAulaValidator - Aluno ID Vazio - Deve Falhar na Validação")]
        [Trait("Alunos", "Validators")]
        public void IniciarEstudoAulaValidator_AlunoIdVazio_DeveFalharNaValidacao()
        {
            // Arrange
            var command = new IniciarEstudoAulaCommand(
                Guid.Empty,
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "AlunoId");
        }

        [Fact(DisplayName = "IniciarEstudoAulaValidator - Matricula ID Vazio - Deve Falhar na Validação")]
        [Trait("Alunos", "Validators")]
        public void IniciarEstudoAulaValidator_MatriculaIdVazio_DeveFalharNaValidacao()
        {
            // Arrange
            var command = new IniciarEstudoAulaCommand(
                Guid.NewGuid(),
                Guid.Empty,
                Guid.NewGuid(),
                Guid.NewGuid()
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "MatriculaId");
        }

        [Fact(DisplayName = "IniciarEstudoAulaValidator - Curso ID Vazio - Deve Falhar na Validação")]
        [Trait("Alunos", "Validators")]
        public void IniciarEstudoAulaValidator_CursoIdVazio_DeveFalharNaValidacao()
        {
            // Arrange
            var command = new IniciarEstudoAulaCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.Empty,
                Guid.NewGuid()
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "CursoId");
        }

        [Fact(DisplayName = "IniciarEstudoAulaValidator - Aula ID Vazio - Deve Falhar na Validação")]
        [Trait("Alunos", "Validators")]
        public void IniciarEstudoAulaValidator_AulaIdVazio_DeveFalharNaValidacao()
        {
            // Arrange
            var command = new IniciarEstudoAulaCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.Empty
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "AulaId");
        }

        #endregion

        #region FinalizarEstudoAulaCommandValidator

        [Fact(DisplayName = "FinalizarEstudoAulaValidator - Dados Válidos - Deve Passar na Validação")]
        [Trait("Alunos", "Validators")]
        public void FinalizarEstudoAulaValidator_DadosValidos_DevePassarNaValidacao()
        {
            // Arrange
            var command = new FinalizarEstudoAulaCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeTrue();
            command.ValidationResult.Errors.Should().BeEmpty();
        }

        [Fact(DisplayName = "FinalizarEstudoAulaValidator - Aluno ID Vazio - Deve Falhar na Validação")]
        [Trait("Alunos", "Validators")]
        public void FinalizarEstudoAulaValidator_AlunoIdVazio_DeveFalharNaValidacao()
        {
            // Arrange
            var command = new FinalizarEstudoAulaCommand(
                Guid.Empty,
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "AlunoId");
        }

        [Fact(DisplayName = "FinalizarEstudoAulaValidator - Matricula ID Vazio - Deve Falhar na Validação")]
        [Trait("Alunos", "Validators")]
        public void FinalizarEstudoAulaValidator_MatriculaIdVazio_DeveFalharNaValidacao()
        {
            // Arrange
            var command = new FinalizarEstudoAulaCommand(
                Guid.NewGuid(),
                Guid.Empty,
                Guid.NewGuid(),
                Guid.NewGuid()
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "MatriculaId");
        }

        [Fact(DisplayName = "FinalizarEstudoAulaValidator - Curso ID Vazio - Deve Falhar na Validação")]
        [Trait("Alunos", "Validators")]
        public void FinalizarEstudoAulaValidator_CursoIdVazio_DeveFalharNaValidacao()
        {
            // Arrange
            var command = new FinalizarEstudoAulaCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.Empty,
                Guid.NewGuid()
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "CursoId");
        }

        [Fact(DisplayName = "FinalizarEstudoAulaValidator - Aula ID Vazio - Deve Falhar na Validação")]
        [Trait("Alunos", "Validators")]
        public void FinalizarEstudoAulaValidator_AulaIdVazio_DeveFalharNaValidacao()
        {
            // Arrange
            var command = new FinalizarEstudoAulaCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.Empty
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "AulaId");
        }

        #endregion
    }
}

