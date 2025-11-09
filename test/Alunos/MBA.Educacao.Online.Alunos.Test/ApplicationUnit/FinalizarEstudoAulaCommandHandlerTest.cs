using FluentAssertions;
using MBA.Educacao.Online.Alunos.Application.Commands;
using MBA.Educacao.Online.Alunos.Application.Handlers.Commands;
using MBA.Educacao.Online.Alunos.Domain.Entities;
using MBA.Educacao.Online.Alunos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Cursos.Domain.Entities;
using MBA.Educacao.Online.Cursos.Domain.Enums;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using Moq;
using Moq.AutoMock;

namespace MBA.Educacao.Online.Alunos.Test.ApplicationUnit
{
    public class FinalizarEstudoAulaCommandHandlerTest
    {
        private readonly AutoMocker _mocker;
        private readonly FinalizarEstudoAulaCommandHandler _handler;

        public FinalizarEstudoAulaCommandHandlerTest()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<FinalizarEstudoAulaCommandHandler>();
        }

        #region Cenários de Sucesso

        [Fact(DisplayName = "Finalizar Estudo - Dados Válidos - Deve Finalizar com Sucesso")]
        [Trait("Alunos", "Handlers - Commands")]
        public async Task Handle_DadosValidos_DeveFinalizarComSucesso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var matriculaId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var aulaId = Guid.NewGuid();

            var curso = new Curso("MBA em Gestão", "Curso completo", "Eduardo Pires", NivelCurso.Avancado, 1000);
            var aula = new Aula("Aula 1", "Introdução", 60, 1);
            typeof(Aula).GetProperty("CursoId")!.SetValue(aula, cursoId);
            typeof(Aula).GetProperty("Id")!.SetValue(aula, aulaId);

            // Adicionar aulas ao curso
            for (int i = 0; i < 10; i++)
            {
                curso.AdicionarAula(new Aula($"Aula {i+1}", $"Descrição {i+1}", 60, i+1));
            }

            var matricula = new Matricula(alunoId, cursoId, DateTime.Now.AddMonths(12));
            typeof(Matricula).GetProperty("Id")!.SetValue(matricula, matriculaId);
            matricula.IniciarAprendizado(aulaId);

            var command = new FinalizarEstudoAulaCommand(alunoId, matriculaId, cursoId, aulaId);

            _mocker.GetMock<ICursoRepository>()
                .Setup(r => r.BuscarPorIdAsync(cursoId))
                .ReturnsAsync(curso);

            _mocker.GetMock<IAulaRepository>()
                .Setup(r => r.BuscarPorIdAsync(aulaId))
                .ReturnsAsync(aula);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.BuscarPorId(matriculaId))
                .Returns(matricula);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.Alterar(It.IsAny<Matricula>()));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(true);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.UnitOfWork)
                .Returns(unitOfWorkMock.Object);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeTrue();
            _mocker.GetMock<IMatriculaRepository>().Verify(r => r.Alterar(It.IsAny<Matricula>()), Times.Once);
            _mocker.GetMock<IMatriculaRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        #endregion

        #region Cenários de Falha

        [Fact(DisplayName = "Finalizar Estudo - Command Inválido - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands")]
        public async Task Handle_CommandInvalido_DeveRetornarFalso()
        {
            // Arrange
            var command = new FinalizarEstudoAulaCommand(Guid.Empty, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<INotificador>().Verify(n => n.Handle(It.IsAny<Core.Domain.Notifications.Notificacao>()), Times.AtLeastOnce);
        }

        [Fact(DisplayName = "Finalizar Estudo - Curso Não Encontrado - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands")]
        public async Task Handle_CursoNaoEncontrado_DeveRetornarFalso()
        {
            // Arrange
            var command = new FinalizarEstudoAulaCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            );

            _mocker.GetMock<ICursoRepository>()
                .Setup(r => r.BuscarPorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Curso)null!);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<INotificador>().Verify(n => n.Handle(It.IsAny<Core.Domain.Notifications.Notificacao>()), Times.AtLeastOnce);
        }

        [Fact(DisplayName = "Finalizar Estudo - Aula Não Encontrada - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands")]
        public async Task Handle_AulaNaoEncontrada_DeveRetornarFalso()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var curso = new Curso("MBA em Gestão", "Curso completo", "Eduardo Pires", NivelCurso.Avancado, 1000);

            var command = new FinalizarEstudoAulaCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                cursoId,
                Guid.NewGuid()
            );

            _mocker.GetMock<ICursoRepository>()
                .Setup(r => r.BuscarPorIdAsync(cursoId))
                .ReturnsAsync(curso);

            _mocker.GetMock<IAulaRepository>()
                .Setup(r => r.BuscarPorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Aula)null!);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<INotificador>().Verify(n => n.Handle(It.IsAny<Core.Domain.Notifications.Notificacao>()), Times.AtLeastOnce);
        }

        [Fact(DisplayName = "Finalizar Estudo - Matrícula Não Encontrada - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands")]
        public async Task Handle_MatriculaNaoEncontrada_DeveRetornarFalso()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var aulaId = Guid.NewGuid();

            var curso = new Curso("MBA em Gestão", "Curso completo", "Eduardo Pires", NivelCurso.Avancado, 1000);
            var aula = new Aula("Aula 1", "Introdução", 60, 1);
            typeof(Aula).GetProperty("CursoId")!.SetValue(aula, cursoId);
            typeof(Aula).GetProperty("Id")!.SetValue(aula, aulaId);

            var command = new FinalizarEstudoAulaCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                cursoId,
                aulaId
            );

            _mocker.GetMock<ICursoRepository>()
                .Setup(r => r.BuscarPorIdAsync(cursoId))
                .ReturnsAsync(curso);

            _mocker.GetMock<IAulaRepository>()
                .Setup(r => r.BuscarPorIdAsync(aulaId))
                .ReturnsAsync(aula);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.BuscarPorId(It.IsAny<Guid>()))
                .Returns((Matricula)null!);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<INotificador>().Verify(n => n.Handle(It.IsAny<Core.Domain.Notifications.Notificacao>()), Times.AtLeastOnce);
        }

        [Fact(DisplayName = "Finalizar Estudo - Curso Sem Aulas - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands")]
        public async Task Handle_CursoSemAulas_DeveRetornarFalso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var matriculaId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var aulaId = Guid.NewGuid();

            var curso = new Curso("MBA em Gestão", "Curso completo", "Eduardo Pires", NivelCurso.Avancado, 1000);
            // Curso sem aulas

            var aula = new Aula("Aula 1", "Introdução", 60, 1);
            typeof(Aula).GetProperty("CursoId")!.SetValue(aula, cursoId);
            typeof(Aula).GetProperty("Id")!.SetValue(aula, aulaId);

            var matricula = new Matricula(alunoId, cursoId, DateTime.Now.AddMonths(12));
            typeof(Matricula).GetProperty("Id")!.SetValue(matricula, matriculaId);
            matricula.IniciarAprendizado(aulaId);

            var command = new FinalizarEstudoAulaCommand(alunoId, matriculaId, cursoId, aulaId);

            _mocker.GetMock<ICursoRepository>()
                .Setup(r => r.BuscarPorIdAsync(cursoId))
                .ReturnsAsync(curso);

            _mocker.GetMock<IAulaRepository>()
                .Setup(r => r.BuscarPorIdAsync(aulaId))
                .ReturnsAsync(aula);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.BuscarPorId(matriculaId))
                .Returns(matricula);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<INotificador>().Verify(n => n.Handle(It.IsAny<Core.Domain.Notifications.Notificacao>()), Times.AtLeastOnce);
        }

        #endregion
    }
}

