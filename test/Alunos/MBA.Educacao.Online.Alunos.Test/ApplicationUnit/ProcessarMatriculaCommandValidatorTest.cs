using FluentAssertions;
using MBA.Educacao.Online.Alunos.Application.Commands;
using MBA.Educacao.Online.Core.Domain.DTOs;

namespace MBA.Educacao.Online.Alunos.Test.ApplicationUnit
{
    public class ProcessarMatriculaCommandValidatorTest
    {
        #region Cenários de Sucesso

        [Fact(DisplayName = "ProcessarMatriculaValidator - Dados Válidos - Deve Passar na Validação")]
        [Trait("Alunos", "Validators - ProcessarMatricula")]
        public void ProcessarMatriculaValidator_DadosValidos_DevePassarNaValidacao()
        {
            // Arrange
            var listaCursos = new ListaCursosPedidoDto
            {
                PedidoId = Guid.NewGuid(),
                Itens = new List<Item>
                {
                    new Item { Id = Guid.NewGuid(), Descricao = 1, Valor = 1000m }
                }
            };

            var command = new ProcessarMatriculaCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                listaCursos
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeTrue();
            command.ValidationResult.Errors.Should().BeEmpty();
        }

        #endregion

        #region Cenários de Falha

        [Fact(DisplayName = "ProcessarMatriculaValidator - Aluno ID Vazio - Deve Falhar na Validação")]
        [Trait("Alunos", "Validators - ProcessarMatricula")]
        public void ProcessarMatriculaValidator_AlunoIdVazio_DeveFalharNaValidacao()
        {
            // Arrange
            var listaCursos = new ListaCursosPedidoDto
            {
                PedidoId = Guid.NewGuid(),
                Itens = new List<Item>
                {
                    new Item { Id = Guid.NewGuid(), Descricao = 1, Valor = 1000m }
                }
            };

            var command = new ProcessarMatriculaCommand(
                Guid.Empty,
                Guid.NewGuid(),
                listaCursos
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "AlunoId");
        }

        [Fact(DisplayName = "ProcessarMatriculaValidator - Pedido ID Vazio - Deve Falhar na Validação")]
        [Trait("Alunos", "Validators - ProcessarMatricula")]
        public void ProcessarMatriculaValidator_PedidoIdVazio_DeveFalharNaValidacao()
        {
            // Arrange
            var listaCursos = new ListaCursosPedidoDto
            {
                PedidoId = Guid.NewGuid(),
                Itens = new List<Item>
                {
                    new Item { Id = Guid.NewGuid(), Descricao = 1, Valor = 1000m }
                }
            };

            var command = new ProcessarMatriculaCommand(
                Guid.NewGuid(),
                Guid.Empty,
                listaCursos
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "PedidoId");
        }

        [Fact(DisplayName = "ProcessarMatriculaValidator - Lista Cursos Nula - Deve Falhar na Validação")]
        [Trait("Alunos", "Validators - ProcessarMatricula")]
        public void ProcessarMatriculaValidator_ListaCursosNula_DeveFalharNaValidacao()
        {
            // Arrange
            var command = new ProcessarMatriculaCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                null!
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "ListaCursos");
        }

        [Fact(DisplayName = "ProcessarMatriculaValidator - Lista Cursos Vazia - Deve Falhar na Validação")]
        [Trait("Alunos", "Validators - ProcessarMatricula")]
        public void ProcessarMatriculaValidator_ListaCursosVazia_DeveFalharNaValidacao()
        {
            // Arrange
            var listaCursos = new ListaCursosPedidoDto
            {
                PedidoId = Guid.NewGuid(),
                Itens = new List<Item>()
            };

            var command = new ProcessarMatriculaCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                listaCursos
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "ListaCursos.Itens");
        }

        [Fact(DisplayName = "ProcessarMatriculaValidator - Múltiplos Erros - Deve Retornar Todos os Erros")]
        [Trait("Alunos", "Validators - ProcessarMatricula")]
        public void ProcessarMatriculaValidator_MultiplosErros_DeveRetornarTodosOsErros()
        {
            // Arrange
            var command = new ProcessarMatriculaCommand(
                Guid.Empty,
                Guid.Empty,
                null!
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
            command.ValidationResult.Errors.Should().HaveCountGreaterOrEqualTo(3);
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "AlunoId");
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "PedidoId");
            command.ValidationResult.Errors.Should().Contain(e => e.PropertyName == "ListaCursos");
        }

        #endregion
    }
}

