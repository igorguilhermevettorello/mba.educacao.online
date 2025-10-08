using FluentAssertions;
using MBA.Educacao.Online.Curso.Application.Commands.Curso;
using MBA.Educacao.Online.Curso.Application.Common.Interfaces;
using MBA.Educacao.Online.Curso.Application.Handlers.Curso;
using MBA.Educacao.Online.Curso.Domain.Entities;
using MBA.Educacao.Online.Curso.Domain.Enums;
using Moq;

namespace MBA.Educacao.Online.Curso.Test.Unit.Commands;

public class AtualizarCursoCommandTest
{
    private readonly Mock<ICursoRepository> _cursoRepositoryMock;
    private readonly AtualizarCursoCommandHandler _handler;

    public AtualizarCursoCommandTest()
    {
        _cursoRepositoryMock = new Mock<ICursoRepository>();
        _handler = new AtualizarCursoCommandHandler(_cursoRepositoryMock.Object);
    }

    #region Cenários de Sucesso

    [Fact(DisplayName = "Atualizar Curso - Dados Válidos - Deve Atualizar com Sucesso")]
    [Trait("Categoria", "Commands - Curso")]
    public void Handle_DadosValidos_DeveAtualizarComSucesso()
    {
        // Arrange
        var cursoId = Guid.NewGuid();
        var command = new AtualizarCursoCommand(
            cursoId,
            "MBA em Gestão de Projetos Atualizado",
            "Curso completo de MBA atualizado",
            NivelCurso.Avancado
        );

        var cursoExistente = new Domain.Entities.Curso(
            "MBA Original",
            "Descrição original",
            NivelCurso.Basico
        );

        _cursoRepositoryMock
            .Setup(x => x.GetByIdAsync(cursoId))
            .ReturnsAsync(cursoExistente);
        
        _cursoRepositoryMock
            .Setup(x => x.TituloExistsAsync(command.Titulo, cursoId))
            .ReturnsAsync(false);

        // Act
        var result = _handler.Handle(command, CancellationToken.None).Result;

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        
        _cursoRepositoryMock.Verify(x => x.GetByIdAsync(cursoId), Times.Once);
        _cursoRepositoryMock.Verify(x => x.TituloExistsAsync(command.Titulo, cursoId), Times.Once);
        _cursoRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Domain.Entities.Curso>()), Times.Once);
    }

    #endregion

    #region Cenários de Falha

    [Fact(DisplayName = "Atualizar Curso - Curso Não Encontrado - Deve Retornar Falha")]
    [Trait("Categoria", "Commands - Curso")]
    public void Handle_CursoNaoEncontrado_DeveRetornarFalha()
    {
        // Arrange
        var cursoId = Guid.NewGuid();
        var command = new AtualizarCursoCommand(
            cursoId,
            "MBA em Gestão de Projetos",
            "Curso completo de MBA",
            NivelCurso.Avancado
        );

        _cursoRepositoryMock
            .Setup(x => x.GetByIdAsync(cursoId))
            .ReturnsAsync((Domain.Entities.Curso?)null);

        // Act
        var result = _handler.Handle(command, CancellationToken.None).Result;

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Curso não encontrado.");
        
        _cursoRepositoryMock.Verify(x => x.GetByIdAsync(cursoId), Times.Once);
        _cursoRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Domain.Entities.Curso>()), Times.Never);
    }

    [Fact(DisplayName = "Atualizar Curso - Título Já Existe - Deve Retornar Falha")]
    [Trait("Categoria", "Commands - Curso")]
    public void Handle_TituloJaExiste_DeveRetornarFalha()
    {
        // Arrange
        var cursoId = Guid.NewGuid();
        var command = new AtualizarCursoCommand(
            cursoId,
            "MBA em Gestão de Projetos",
            "Curso completo de MBA",
            NivelCurso.Avancado
        );

        var cursoExistente = new Domain.Entities.Curso(
            "MBA Original",
            "Descrição original",
            NivelCurso.Basico
        );

        _cursoRepositoryMock
            .Setup(x => x.GetByIdAsync(cursoId))
            .ReturnsAsync(cursoExistente);
        
        _cursoRepositoryMock
            .Setup(x => x.TituloExistsAsync(command.Titulo, cursoId))
            .ReturnsAsync(true);

        // Act
        var result = _handler.Handle(command, CancellationToken.None).Result;

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Já existe outro curso com este título.");
        
        _cursoRepositoryMock.Verify(x => x.GetByIdAsync(cursoId), Times.Once);
        _cursoRepositoryMock.Verify(x => x.TituloExistsAsync(command.Titulo, cursoId), Times.Once);
        _cursoRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Domain.Entities.Curso>()), Times.Never);
    }

    [Theory(DisplayName = "Atualizar Curso - Dados Inválidos - Deve Retornar Falha")]
    [Trait("Categoria", "Commands - Curso")]
    [InlineData("", "Descrição válida", NivelCurso.Basico, "Título do curso é obrigatório")]
    [InlineData(null, "Descrição válida", NivelCurso.Basico, "Título do curso é obrigatório")]
    [InlineData("Título válido", "", NivelCurso.Basico, "Descrição do curso é obrigatória")]
    [InlineData("Título válido", null, NivelCurso.Basico, "Descrição do curso é obrigatória")]
    public void Handle_DadosInvalidos_DeveRetornarFalha(string titulo, string descricao, NivelCurso nivel, string mensagemEsperada)
    {
        // Arrange
        var cursoId = Guid.NewGuid();
        var command = new AtualizarCursoCommand(cursoId, titulo, descricao, nivel);

        var cursoExistente = new Domain.Entities.Curso(
            "MBA Original",
            "Descrição original",
            NivelCurso.Basico
        );

        _cursoRepositoryMock
            .Setup(x => x.GetByIdAsync(cursoId))
            .ReturnsAsync(cursoExistente);
        
        _cursoRepositoryMock
            .Setup(x => x.TituloExistsAsync(command.Titulo, cursoId))
            .ReturnsAsync(false);

        // Act
        var result = _handler.Handle(command, CancellationToken.None).Result;

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(mensagemEsperada);
        
        _cursoRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Domain.Entities.Curso>()), Times.Never);
    }

    #endregion
}
