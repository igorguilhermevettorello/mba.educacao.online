using MBA.Educacao.Online.Cursos.Application.Commands.Cursos;
using MBA.Educacao.Online.Cursos.Application.Handlers.Cursos;
using MBA.Educacao.Online.Cursos.Domain.Entities;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using Moq;
using Moq.AutoMock;

namespace MBA.Educacao.Online.Cursos.Test.ApplicationUnit
{
    public class InativarCursoCommandTest
    {
        private readonly Mock<ICursoRepository> _cursoRepositoryMock;

        public InativarCursoCommandTest()
        {
            _cursoRepositoryMock = new Mock<ICursoRepository>();
        }

        #region Cenários de Sucesso
        [Fact(DisplayName = "Inativar Curso - Dados Válidos - Deve Inativar Curso com Sucesso")]
        [Trait("Curso", "Commands - Curso")]
        public void InativarCurso_DadosValidos_DeveInativarCursoComSucesso()
        {
            // Arrange
            var command = new InativarCursoCommand(Guid.NewGuid());

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.True(isValid);
        }
        #endregion

        #region Cenários de Sucesso
        [Fact(DisplayName = "Inativar Curso - Guid inválido - Deve Retornar Falha")]
        [Trait("Curso", "Commands - Curso")]
        public void InativarCurso_GuidInvalido_DeveRetornarFalha()
        {
            // Arrange
            var command = new InativarCursoCommand(Guid.Empty);

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.False(isValid);
        }
        #endregion


        #region Cenários de alterar registro com sucesso
        [Fact(DisplayName = "Inativar Curso - Alterar Registro - Deve Inativar Curso com Sucesso")]
        [Trait("Curso", "Commands - Curso")]
        public async Task InativarCurso_AlterarRegistro_DeveInativarCursoComSucesso()
        {
            // Arrange
            var command = new InativarCursoCommand(Guid.NewGuid());

            var mocker = new AutoMocker();
            var cursoHandler = mocker.CreateInstance<InativarCursoCommandHandler>();
            mocker.GetMock<ICursoRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await cursoHandler.Handle(command, new CancellationToken());

            // Assert
            Assert.True(result);
            mocker.GetMock<ICursoRepository>().Verify(r => r.Alterar(It.IsAny<Curso>()), Times.Once);
            mocker.GetMock<ICursoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);            
        }
        #endregion
    }


}
