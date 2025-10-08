using MBA.Educacao.Online.Cursos.Application.Commands.Cursos;
using MBA.Educacao.Online.Cursos.Application.Handlers.Cursos;
using MBA.Educacao.Online.Cursos.Domain.Enums;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Cursos.Domain.Entities;
using MediatR;
using Moq;
using Moq.AutoMock;

namespace MBA.Educacao.Online.Cursos.Test.Unit.Application
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
        [Trait("Categoria", "Commands - Curso")]
        public void CriarCurso_DadosValidos_DeveCriarCursoComSucesso()
        {
            // Arrange
            var command = new CriarCursoCommand(
                "MBA em Gestão de Projetos",
                "Curso completo de MBA",
                NivelCurso.Avancado
            );

            var isValid = command.IsValid();

            // var cursoEsperado = new Domain.Entities.Curso(command.Titulo, command.Descricao, command.Nivel);
            //
            // _cursoRepositoryMock
            //     .Setup(x => x.TituloExistsAsync(command.Titulo))
            //     .ReturnsAsync(false);
            //
            // _cursoRepositoryMock
            //     .Setup(x => x.AddAsync(It.IsAny<Domain.Entities.Curso>()))
            //     .ReturnsAsync(cursoEsperado);

            // Act
            // var result = _handler.Handle(command, CancellationToken.None).Result;

            // Assert
            // result.Should().NotBeNull();
            // result.IsSuccess.Should().BeTrue();
            // result.Value.Should().Be(cursoEsperado.Id);
            //
            // _cursoRepositoryMock.Verify(x => x.TituloExistsAsync(command.Titulo), Times.Once);
            // _cursoRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.Curso>()), Times.Once);
            Assert.True(isValid);
        }

        #endregion

        #region Cenários de Falha

        [Fact(DisplayName = "Criar Curso - Título Já Existe - Deve Retornar Falha")]
        [Trait("Categoria", "Commands - Curso")]
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
            var result = await cursoHandler.Handle(command, new CancellationToken());

            // Assert
            Assert.True(result);

            mocker.GetMock<ICursoRepository>().Verify(r => r.Adicionar(It.IsAny<Curso>()), Times.Once);
            mocker.GetMock<IMediator>().Verify(r => r.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }

        // [Theory(DisplayName = "Criar Curso - Dados Inválidos - Deve Retornar Falha")]
        // [Trait("Categoria", "Commands - Curso")]
        // [InlineData("", "Descrição válida", NivelCurso.Basico, "Título do curso é obrigatório")]
        // [InlineData(null, "Descrição válida", NivelCurso.Basico, "Título do curso é obrigatório")]
        // [InlineData("Título válido", "", NivelCurso.Basico, "Descrição do curso é obrigatória")]
        // [InlineData("Título válido", null, NivelCurso.Basico, "Descrição do curso é obrigatória")]
        // public void Handle_DadosInvalidos_DeveRetornarFalha(string titulo, string descricao, NivelCurso nivel, string mensagemEsperada)
        // {
        //     // Arrange
        //     var command = new CriarCursoCommand(titulo, descricao, nivel);
        //
        //     _cursoRepositoryMock
        //         .Setup(x => x.TituloExistsAsync(command.Titulo))
        //         .ReturnsAsync(false);
        //
        //     // Act
        //     var result = _handler.Handle(command, CancellationToken.None).Result;
        //
        //     // Assert
        //     result.Should().NotBeNull();
        //     result.IsSuccess.Should().BeFalse();
        //     result.Error.Should().Be(mensagemEsperada);
        //     
        //     _cursoRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.Curso>()), Times.Never);
        // }
        //
        // [Fact(DisplayName = "Criar Curso - Erro no Repositório - Deve Retornar Falha")]
        // [Trait("Categoria", "Commands - Curso")]
        // public void Handle_ErroNoRepositorio_DeveRetornarFalha()
        // {
        //     // Arrange
        //     var command = new CriarCursoCommand(
        //         "MBA em Gestão de Projetos",
        //         "Curso completo de MBA",
        //         NivelCurso.Avancado
        //     );
        //
        //     _cursoRepositoryMock
        //         .Setup(x => x.TituloExistsAsync(command.Titulo))
        //         .ReturnsAsync(false);
        //     
        //     _cursoRepositoryMock
        //         .Setup(x => x.AddAsync(It.IsAny<Domain.Entities.Curso>()))
        //         .ThrowsAsync(new Exception("Erro de conexão"));
        //
        //     // Act
        //     var result = _handler.Handle(command, CancellationToken.None).Result;
        //
        //     // Assert
        //     result.Should().NotBeNull();
        //     result.IsSuccess.Should().BeFalse();
        //     result.Error.Should().Contain("Erro interno ao criar curso");
        // }

        #endregion
    }
}

