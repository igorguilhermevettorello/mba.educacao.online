using FluentAssertions;
using MBA.Educacao.Online.Alunos.Application.Commands;
using MBA.Educacao.Online.Alunos.Application.Handlers.Commands;
using MBA.Educacao.Online.Alunos.Domain.Entities;
using MBA.Educacao.Online.Alunos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Core.Domain.DTOs;
using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;
using Moq;
using Moq.AutoMock;

namespace MBA.Educacao.Online.Alunos.Test.ApplicationUnit
{
    public class ProcessarMatriculaCommandHandlerTest
    {
        private readonly AutoMocker _mocker;
        private readonly ProcessarMatriculaCommandHandler _handler;

        public ProcessarMatriculaCommandHandlerTest()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<ProcessarMatriculaCommandHandler>();
        }

        #region Cenários de Sucesso

        [Fact(DisplayName = "Processar Matrícula - Dados Válidos - Deve Processar com Sucesso")]
        [Trait("Alunos", "Handlers - Commands - ProcessarMatricula")]
        public async Task Handle_DadosValidos_DeveProcessarComSucesso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var pedidoId = Guid.NewGuid();
            var cursoId1 = Guid.NewGuid();
            var cursoId2 = Guid.NewGuid();

            var listaCursos = new ListaCursosPedidoDto
            {
                PedidoId = pedidoId,
                Itens = new List<Item>
                {
                    new Item { Id = cursoId1, Descricao = 1, Valor = 1000m },
                    new Item { Id = cursoId2, Descricao = 2, Valor = 1200m }
                }
            };

            var command = new ProcessarMatriculaCommand(alunoId, pedidoId, listaCursos);

            var aluno = new Aluno(alunoId, "João Silva", "joao@teste.com");

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.BuscarPorId(alunoId))
                .Returns(aluno);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.BuscarPorAlunoECurso(alunoId, It.IsAny<Guid>()))
                .Returns((Matricula)null!);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.Adicionar(It.IsAny<Matricula>()));

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.Alterar(It.IsAny<Aluno>()));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(true);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.UnitOfWork)
                .Returns(unitOfWorkMock.Object);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeTrue();
            _mocker.GetMock<IMatriculaRepository>().Verify(r => r.Adicionar(It.IsAny<Matricula>()), Times.Exactly(2));
            _mocker.GetMock<IAlunoRepository>().Verify(r => r.Alterar(It.IsAny<Aluno>()), Times.Once);
            _mocker.GetMock<IMatriculaRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Processar Matrícula - Um Curso - Deve Processar com Sucesso")]
        [Trait("Alunos", "Handlers - Commands - ProcessarMatricula")]
        public async Task Handle_UmCurso_DeveProcessarComSucesso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var pedidoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();

            var listaCursos = new ListaCursosPedidoDto
            {
                PedidoId = pedidoId,
                Itens = new List<Item>
                {
                    new Item { Id = cursoId, Descricao = 1, Valor = 1000m }
                }
            };

            var command = new ProcessarMatriculaCommand(alunoId, pedidoId, listaCursos);

            var aluno = new Aluno(alunoId, "João Silva", "joao@teste.com");

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.BuscarPorId(alunoId))
                .Returns(aluno);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.BuscarPorAlunoECurso(alunoId, cursoId))
                .Returns((Matricula)null!);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.Adicionar(It.IsAny<Matricula>()));

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.Alterar(It.IsAny<Aluno>()));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(true);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.UnitOfWork)
                .Returns(unitOfWorkMock.Object);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeTrue();
            _mocker.GetMock<IMatriculaRepository>().Verify(r => r.Adicionar(It.IsAny<Matricula>()), Times.Once);
        }

        #endregion

        #region Cenários de Falha

        [Fact(DisplayName = "Processar Matrícula - Command Inválido - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - ProcessarMatricula")]
        public async Task Handle_CommandInvalido_DeveRetornarFalso()
        {
            // Arrange
            var command = new ProcessarMatriculaCommand(Guid.Empty, Guid.NewGuid(), null!);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<INotificador>().Verify(n => n.Handle(It.IsAny<Core.Domain.Notifications.Notificacao>()), Times.AtLeastOnce);
        }

        [Fact(DisplayName = "Processar Matrícula - Aluno Não Encontrado - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - ProcessarMatricula")]
        public async Task Handle_AlunoNaoEncontrado_DeveRetornarFalso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var pedidoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();

            var listaCursos = new ListaCursosPedidoDto
            {
                PedidoId = pedidoId,
                Itens = new List<Item>
                {
                    new Item { Id = cursoId, Descricao = 1, Valor = 1000m }
                }
            };

            var command = new ProcessarMatriculaCommand(alunoId, pedidoId, listaCursos);

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.BuscarPorId(alunoId))
                .Returns((Aluno)null!);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<INotificador>().Verify(n => n.Handle(It.IsAny<Core.Domain.Notifications.Notificacao>()), Times.AtLeastOnce);
        }

        [Fact(DisplayName = "Processar Matrícula - Aluno Inativo - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - ProcessarMatricula")]
        public async Task Handle_AlunoInativo_DeveRetornarFalso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var pedidoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();

            var listaCursos = new ListaCursosPedidoDto
            {
                PedidoId = pedidoId,
                Itens = new List<Item>
                {
                    new Item { Id = cursoId, Descricao = 1, Valor = 1000m }
                }
            };

            var command = new ProcessarMatriculaCommand(alunoId, pedidoId, listaCursos);

            var aluno = new Aluno(alunoId, "João Silva", "joao@teste.com");
            aluno.Inativar();

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.BuscarPorId(alunoId))
                .Returns(aluno);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<INotificador>().Verify(n => n.Handle(It.IsAny<Core.Domain.Notifications.Notificacao>()), Times.AtLeastOnce);
        }

        [Fact(DisplayName = "Processar Matrícula - Lista Cursos Vazia - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - ProcessarMatricula")]
        public async Task Handle_ListaCursosVazia_DeveRetornarFalso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var pedidoId = Guid.NewGuid();

            var listaCursos = new ListaCursosPedidoDto
            {
                PedidoId = pedidoId,
                Itens = new List<Item>()
            };

            var command = new ProcessarMatriculaCommand(alunoId, pedidoId, listaCursos);

            var aluno = new Aluno(alunoId, "João Silva", "joao@teste.com");

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.BuscarPorId(alunoId))
                .Returns(aluno);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<INotificador>().Verify(n => n.Handle(It.IsAny<Core.Domain.Notifications.Notificacao>()), Times.AtLeastOnce);
        }

        [Fact(DisplayName = "Processar Matrícula - Matrícula Já Existente - Deve Ignorar e Continuar")]
        [Trait("Alunos", "Handlers - Commands - ProcessarMatricula")]
        public async Task Handle_MatriculaJaExistente_DeveIgnorarEContinuar()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var pedidoId = Guid.NewGuid();
            var cursoId1 = Guid.NewGuid();
            var cursoId2 = Guid.NewGuid();

            var listaCursos = new ListaCursosPedidoDto
            {
                PedidoId = pedidoId,
                Itens = new List<Item>
                {
                    new Item { Id = cursoId1, Descricao = 1, Valor = 1000m },
                    new Item { Id = cursoId2, Descricao = 2, Valor = 1200m }
                }
            };

            var command = new ProcessarMatriculaCommand(alunoId, pedidoId, listaCursos);

            var aluno = new Aluno(alunoId, "João Silva", "joao@teste.com");
            var matriculaExistente = new Matricula(alunoId, cursoId1, DateTime.Now.AddYears(1));

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.BuscarPorId(alunoId))
                .Returns(aluno);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.BuscarPorAlunoECurso(alunoId, cursoId1))
                .Returns(matriculaExistente);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.BuscarPorAlunoECurso(alunoId, cursoId2))
                .Returns((Matricula)null!);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.Adicionar(It.IsAny<Matricula>()));

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.Alterar(It.IsAny<Aluno>()));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(true);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.UnitOfWork)
                .Returns(unitOfWorkMock.Object);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeTrue();
            _mocker.GetMock<IMatriculaRepository>().Verify(r => r.Adicionar(It.IsAny<Matricula>()), Times.Once); // Apenas 1 matrícula nova
        }

        [Fact(DisplayName = "Processar Matrícula - Todas Matrículas Já Existem - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - ProcessarMatricula")]
        public async Task Handle_TodasMatriculasJaExistem_DeveRetornarFalso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var pedidoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();

            var listaCursos = new ListaCursosPedidoDto
            {
                PedidoId = pedidoId,
                Itens = new List<Item>
                {
                    new Item { Id = cursoId, Descricao = 1, Valor = 1000m }
                }
            };

            var command = new ProcessarMatriculaCommand(alunoId, pedidoId, listaCursos);

            var aluno = new Aluno(alunoId, "João Silva", "joao@teste.com");
            var matriculaExistente = new Matricula(alunoId, cursoId, DateTime.Now.AddYears(1));

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.BuscarPorId(alunoId))
                .Returns(aluno);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.BuscarPorAlunoECurso(alunoId, cursoId))
                .Returns(matriculaExistente);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<INotificador>().Verify(n => n.Handle(It.IsAny<Core.Domain.Notifications.Notificacao>()), Times.AtLeastOnce);
            _mocker.GetMock<IMatriculaRepository>().Verify(r => r.Adicionar(It.IsAny<Matricula>()), Times.Never);
        }

        [Fact(DisplayName = "Processar Matrícula - Erro ao Salvar - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - ProcessarMatricula")]
        public async Task Handle_ErroAoSalvar_DeveRetornarFalso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var pedidoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();

            var listaCursos = new ListaCursosPedidoDto
            {
                PedidoId = pedidoId,
                Itens = new List<Item>
                {
                    new Item { Id = cursoId, Descricao = 1, Valor = 1000m }
                }
            };

            var command = new ProcessarMatriculaCommand(alunoId, pedidoId, listaCursos);

            var aluno = new Aluno(alunoId, "João Silva", "joao@teste.com");

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.BuscarPorId(alunoId))
                .Returns(aluno);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.BuscarPorAlunoECurso(alunoId, cursoId))
                .Returns((Matricula)null!);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.Adicionar(It.IsAny<Matricula>()));

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.Alterar(It.IsAny<Aluno>()));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(false); // Simula erro ao salvar

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.UnitOfWork)
                .Returns(unitOfWorkMock.Object);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<INotificador>().Verify(n => n.Handle(It.IsAny<Core.Domain.Notifications.Notificacao>()), Times.AtLeastOnce);
        }

        #endregion
    }
}

