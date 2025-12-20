using FluentAssertions;
using MBA.Educacao.Online.Alunos.Application.Commands;
using MBA.Educacao.Online.Alunos.Application.Handlers.Commands;
using MBA.Educacao.Online.Alunos.Domain.Entities;
using MBA.Educacao.Online.Alunos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Core.Domain.Extensions;
using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Core.Domain.Notifications;
using Moq;
using Moq.AutoMock;

namespace MBA.Educacao.Online.Alunos.Test.ApplicationUnit
{
    public class CriarAlunoCommandHandlerTest
    {
        private readonly AutoMocker _mocker;
        private readonly CriarAlunoCommandHandler _handler;

        public CriarAlunoCommandHandlerTest()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<CriarAlunoCommandHandler>();
        }

        #region Cenários de Sucesso

        [Fact(DisplayName = "Criar Aluno - Dados Válidos - Deve Criar Aluno com Sucesso")]
        [Trait("Alunos", "Handlers - Commands - CriarAluno")]
        public async Task Handle_DadosValidos_DeveCriarAlunoComSucesso()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var nome = "João Silva";
            var email = "joao.silva@example.com";

            var command = new CriarAlunoCommand(usuarioId, nome, email);

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.BuscarPorUsuarioId(usuarioId))
                .Returns((Aluno)null!);

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.Adicionar(It.IsAny<Aluno>()));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(true);

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.UnitOfWork)
                .Returns(unitOfWorkMock.Object);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeTrue();
            _mocker.GetMock<IAlunoRepository>().Verify(r => r.BuscarPorUsuarioId(usuarioId), Times.Once);
            _mocker.GetMock<IAlunoRepository>().Verify(r => r.Adicionar(It.Is<Aluno>(a =>
                a.Id == usuarioId.Normalize() &&
                a.Nome == nome &&
                a.Email == email)), Times.Once);
            _mocker.GetMock<IAlunoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
            _mocker.GetMock<INotificador>().Verify(n => n.Handle(It.IsAny<Notificacao>()), Times.Never);
        }

        #endregion

        #region Cenários de Falha - Validação

        [Fact(DisplayName = "Criar Aluno - Command Inválido - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - CriarAluno")]
        public async Task Handle_CommandInvalido_DeveRetornarFalso()
        {
            // Arrange
            var command = new CriarAlunoCommand(Guid.Empty, "João Silva", "joao@example.com");

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<INotificador>().Verify(n => n.Handle(It.IsAny<Notificacao>()), Times.AtLeastOnce);
            _mocker.GetMock<IAlunoRepository>().Verify(r => r.BuscarPorUsuarioId(It.IsAny<Guid>()), Times.Never);
            _mocker.GetMock<IAlunoRepository>().Verify(r => r.Adicionar(It.IsAny<Aluno>()), Times.Never);
        }

        #endregion

        #region Cenários de Falha - Aluno Existente

        [Fact(DisplayName = "Criar Aluno - Aluno Já Existe - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - CriarAluno")]
        public async Task Handle_AlunoJaExiste_DeveRetornarFalso()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var nome = "João Silva";
            var email = "joao.silva@example.com";

            var alunoExistente = new Aluno(usuarioId, "João Existente", "joao.existente@example.com");

            var command = new CriarAlunoCommand(usuarioId, nome, email);

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.BuscarPorUsuarioId(usuarioId))
                .Returns(alunoExistente);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<INotificador>().Verify(
                n => n.Handle(It.Is<Notificacao>(not =>
                    not.Campo == "UsuarioId" &&
                    not.Mensagem == "Já existe um aluno cadastrado para este usuário")),
                Times.Once
            );
            _mocker.GetMock<IAlunoRepository>().Verify(r => r.Adicionar(It.IsAny<Aluno>()), Times.Never);
            _mocker.GetMock<IAlunoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Never);
        }

        #endregion

        #region Cenários de Falha - Persistência

        [Fact(DisplayName = "Criar Aluno - Erro ao Salvar - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - CriarAluno")]
        public async Task Handle_ErroAoSalvar_DeveRetornarFalso()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var nome = "João Silva";
            var email = "joao.silva@example.com";

            var command = new CriarAlunoCommand(usuarioId, nome, email);

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.BuscarPorUsuarioId(usuarioId))
                .Returns((Aluno)null!);

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.Adicionar(It.IsAny<Aluno>()));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(false); // Simula erro ao salvar

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.UnitOfWork)
                .Returns(unitOfWorkMock.Object);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<IAlunoRepository>().Verify(r => r.Adicionar(It.IsAny<Aluno>()), Times.Once);
            _mocker.GetMock<IAlunoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        #endregion
    }
}
