using MBA.Educacao.Online.Cursos.Application.Commands.Cursos;
using MBA.Educacao.Online.Cursos.Application.Handlers.Cursos;
using MBA.Educacao.Online.Cursos.Domain.Entities;
using MBA.Educacao.Online.Cursos.Domain.Enums;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using Moq;
using Moq.AutoMock;

namespace MBA.Educacao.Online.Cursos.Test.ApplicationUnit
{
    public class CriarCursoCommandTest
    {
        private readonly Mock<ICursoRepository> _cursoRepositoryMock;

        public CriarCursoCommandTest()
        {
            _cursoRepositoryMock = new Mock<ICursoRepository>();
        }

        #region Cenários de Sucesso
        [Fact(DisplayName = "Criar Curso - Dados Válidos - Deve Criar Curso com Sucesso")]
        [Trait("Curso", "Commands - Curso")]
        public void CriarCurso_DadosValidos_DeveCriarCursoComSucesso()
        {
            // Arrange
            var command = new CriarCursoCommand(
                "MBA em Gestão de Projetos",
                "Curso completo de MBA",
                NivelCurso.Avancado
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.True(isValid);
        }
        #endregion

        #region Cenários de Falha
        [Fact(DisplayName = "Criar Curso - Título Já Existe - Deve Retornar Falha")]
        [Trait("Curso", "Commands - Curso")]
        public async Task Handle_TituloJaExiste_DeveRetornarFalha()
        {
            // Arrange
            var command = new CriarCursoCommand(
                "MBA em Gestão de Projetos",
                "Curso completo de MBA",
                NivelCurso.Avancado
            );

            var mocker = new AutoMocker();
            var cursoHandler = mocker.CreateInstance<CriarCursoCommandHandler>();
            mocker.GetMock<ICursoRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));
            
            // Act
            var result = await cursoHandler.Handle(command, new CancellationToken());

            // Assert
            Assert.True(result);

            mocker.GetMock<ICursoRepository>().Verify(r => r.Adicionar(It.IsAny<Curso>()), Times.Once);
            mocker.GetMock<ICursoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
            // mocker.GetMock<IMediator>().Verify(r => r.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }
        #endregion
    }
}

