using MBA.Educacao.Online.Cursos.Application.Commands.Cursos;
using MBA.Educacao.Online.Cursos.Application.Handlers.Cursos;
using MBA.Educacao.Online.Cursos.Domain.Entities;
using MBA.Educacao.Online.Cursos.Domain.Enums;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using Moq;
using Moq.AutoMock;

namespace MBA.Educacao.Online.Cursos.Test.ApplicationUnit
{
    public class InativarCursoCommandTest
    {
        #region Cenários de Validação

        [Fact(DisplayName = "Inativar Curso - Dados Válidos - Command Válido")]
        [Trait("Curso", "Commands - Curso")]
        public void InativarCurso_DadosValidos_CommandValido()
        {
            // Arrange
            var command = new InativarCursoCommand(Guid.NewGuid());

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.True(isValid);
        }

        [Fact(DisplayName = "Inativar Curso - Guid Inválido - Command Inválido")]
        [Trait("Curso", "Commands - Curso")]
        public void InativarCurso_GuidInvalido_CommandInvalido()
        {
            // Arrange
            var command = new InativarCursoCommand(Guid.Empty);

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.False(isValid);
        }

        #endregion

        #region Cenários de Sucesso - Handler

        [Fact(DisplayName = "Inativar Curso - Curso Encontrado - Deve Inativar com Sucesso")]
        [Trait("Curso", "Commands - Curso")]
        public async Task InativarCurso_CursoEncontrado_DeveInativarComSucesso()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var command = new InativarCursoCommand(cursoId);
            var curso = new Curso("MBA em Gestão", "Curso completo", "Eduardo Pires", NivelCurso.Avancado, 1000);

            var mocker = new AutoMocker();
            var cursoHandler = mocker.CreateInstance<InativarCursoCommandHandler>();

            mocker.GetMock<ICursoRepository>()
                .Setup(r => r.BuscarPorIdAsync(cursoId))
                .ReturnsAsync(curso);

            mocker.GetMock<ICursoRepository>()
                .Setup(r => r.Alterar(It.IsAny<Curso>()));

            mocker.GetMock<ICursoRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .ReturnsAsync(true);

            // Act
            var result = await cursoHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.False(curso.Ativo); // Verifica que o curso foi inativado
            mocker.GetMock<ICursoRepository>().Verify(r => r.BuscarPorIdAsync(cursoId), Times.Once);
            mocker.GetMock<ICursoRepository>().Verify(r => r.Alterar(It.IsAny<Curso>()), Times.Once);
            mocker.GetMock<ICursoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Inativar Curso - Curso Não Encontrado - Deve Retornar False")]
        [Trait("Curso", "Commands - Curso")]
        public async Task InativarCurso_CursoNaoEncontrado_DeveRetornarFalse()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var command = new InativarCursoCommand(cursoId);

            var mocker = new AutoMocker();
            var cursoHandler = mocker.CreateInstance<InativarCursoCommandHandler>();

            mocker.GetMock<ICursoRepository>()
                .Setup(r => r.BuscarPorIdAsync(cursoId))
                .ReturnsAsync((Curso)null!);

            // Act
            var result = await cursoHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            mocker.GetMock<ICursoRepository>().Verify(r => r.BuscarPorIdAsync(cursoId), Times.Once);
            mocker.GetMock<ICursoRepository>().Verify(r => r.Alterar(It.IsAny<Curso>()), Times.Never);
            mocker.GetMock<ICursoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Never);
        }

        [Fact(DisplayName = "Inativar Curso - Erro ao Salvar - Deve Retornar False")]
        [Trait("Curso", "Commands - Curso")]
        public async Task InativarCurso_ErroAoSalvar_DeveRetornarFalse()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var command = new InativarCursoCommand(cursoId);
            var curso = new Curso("MBA em Gestão", "Curso completo", "Eduardo Pires", NivelCurso.Avancado, 1000);

            var mocker = new AutoMocker();
            var cursoHandler = mocker.CreateInstance<InativarCursoCommandHandler>();

            mocker.GetMock<ICursoRepository>()
                .Setup(r => r.BuscarPorIdAsync(cursoId))
                .ReturnsAsync(curso);

            mocker.GetMock<ICursoRepository>()
                .Setup(r => r.Alterar(It.IsAny<Curso>()));

            mocker.GetMock<ICursoRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .ReturnsAsync(false); // Simula erro ao salvar

            // Act
            var result = await cursoHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            Assert.False(curso.Ativo); // Curso foi inativado mas não foi salvo
            mocker.GetMock<ICursoRepository>().Verify(r => r.BuscarPorIdAsync(cursoId), Times.Once);
            mocker.GetMock<ICursoRepository>().Verify(r => r.Alterar(It.IsAny<Curso>()), Times.Once);
            mocker.GetMock<ICursoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        #endregion
    }
}
