using FluentAssertions;
using MBA.Educacao.Online.Alunos.Application.Commands;

namespace MBA.Educacao.Online.Alunos.Test.ApplicationUnit
{
    public class SolicitarCertificadoCommandTest
    {
        #region Cenários de Sucesso

        [Fact(DisplayName = "SolicitarCertificadoCommand - Dados Válidos - Deve Passar na Validação")]
        [Trait("Alunos", "Commands - SolicitarCertificado")]
        public void SolicitarCertificadoCommand_DadosValidos_DevePassarNaValidacao()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var matriculaId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();

            var command = new SolicitarCertificadoCommand(alunoId, matriculaId, cursoId);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeTrue();
            command.ValidationResult.Errors.Should().BeEmpty();
            command.AlunoId.Should().Be(alunoId);
            command.MatriculaId.Should().Be(matriculaId);
            command.CursoId.Should().Be(cursoId);
        }

        #endregion

        #region Cenários de Falha - AlunoId

        [Fact(DisplayName = "SolicitarCertificadoCommand - AlunoId Vazio - Deve Falhar na Validação")]
        [Trait("Alunos", "Commands - SolicitarCertificado")]
        public void SolicitarCertificadoCommand_AlunoIdVazio_DeveFalharNaValidacao()
        {
            // Arrange
            var alunoId = Guid.Empty;
            var matriculaId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();

            var command = new SolicitarCertificadoCommand(alunoId, matriculaId, cursoId);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "AlunoId");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("O ID do aluno é obrigatório"));
        }

        #endregion

        #region Cenários de Falha - MatriculaId

        [Fact(DisplayName = "SolicitarCertificadoCommand - MatriculaId Vazio - Deve Falhar na Validação")]
        [Trait("Alunos", "Commands - SolicitarCertificado")]
        public void SolicitarCertificadoCommand_MatriculaIdVazio_DeveFalharNaValidacao()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var matriculaId = Guid.Empty;
            var cursoId = Guid.NewGuid();

            var command = new SolicitarCertificadoCommand(alunoId, matriculaId, cursoId);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "MatriculaId");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("O ID da matrícula é obrigatório"));
        }

        #endregion

        #region Cenários de Falha - CursoId

        [Fact(DisplayName = "SolicitarCertificadoCommand - CursoId Vazio - Deve Falhar na Validação")]
        [Trait("Alunos", "Commands - SolicitarCertificado")]
        public void SolicitarCertificadoCommand_CursoIdVazio_DeveFalharNaValidacao()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var matriculaId = Guid.NewGuid();
            var cursoId = Guid.Empty;

            var command = new SolicitarCertificadoCommand(alunoId, matriculaId, cursoId);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "CursoId");
            command.ValidationResult.Errors.Should().Contain(e => e.ErrorMessage.Contains("O ID do curso é obrigatório"));
        }

        #endregion

        #region Cenários de Falha - Múltiplos Erros

        [Fact(DisplayName = "SolicitarCertificadoCommand - AlunoId e MatriculaId Vazios - Deve Retornar Múltiplos Erros")]
        [Trait("Alunos", "Commands - SolicitarCertificado")]
        public void SolicitarCertificadoCommand_AlunoIdEMatriculaIdVazios_DeveRetornarMultiplosErros()
        {
            // Arrange
            var alunoId = Guid.Empty;
            var matriculaId = Guid.Empty;
            var cursoId = Guid.NewGuid();

            var command = new SolicitarCertificadoCommand(alunoId, matriculaId, cursoId);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().HaveCountGreaterOrEqualTo(2);
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "AlunoId");
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "MatriculaId");
        }

        [Fact(DisplayName = "SolicitarCertificadoCommand - AlunoId e CursoId Vazios - Deve Retornar Múltiplos Erros")]
        [Trait("Alunos", "Commands - SolicitarCertificado")]
        public void SolicitarCertificadoCommand_AlunoIdECursoIdVazios_DeveRetornarMultiplosErros()
        {
            // Arrange
            var alunoId = Guid.Empty;
            var matriculaId = Guid.NewGuid();
            var cursoId = Guid.Empty;

            var command = new SolicitarCertificadoCommand(alunoId, matriculaId, cursoId);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().HaveCountGreaterOrEqualTo(2);
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "AlunoId");
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "CursoId");
        }

        [Fact(DisplayName = "SolicitarCertificadoCommand - MatriculaId e CursoId Vazios - Deve Retornar Múltiplos Erros")]
        [Trait("Alunos", "Commands - SolicitarCertificado")]
        public void SolicitarCertificadoCommand_MatriculaIdECursoIdVazios_DeveRetornarMultiplosErros()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var matriculaId = Guid.Empty;
            var cursoId = Guid.Empty;

            var command = new SolicitarCertificadoCommand(alunoId, matriculaId, cursoId);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().HaveCountGreaterOrEqualTo(2);
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "MatriculaId");
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "CursoId");
        }

        [Fact(DisplayName = "SolicitarCertificadoCommand - Todos os Campos Vazios - Deve Retornar Todos os Erros")]
        [Trait("Alunos", "Commands - SolicitarCertificado")]
        public void SolicitarCertificadoCommand_TodosOsCamposVazios_DeveRetornarTodosOsErros()
        {
            // Arrange
            var alunoId = Guid.Empty;
            var matriculaId = Guid.Empty;
            var cursoId = Guid.Empty;

            var command = new SolicitarCertificadoCommand(alunoId, matriculaId, cursoId);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().HaveCount(3);
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "AlunoId");
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "MatriculaId");
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "CursoId");
        }

        #endregion
    }
}
