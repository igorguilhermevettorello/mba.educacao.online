using FluentAssertions;
using MBA.Educacao.Online.Alunos.Application.Commands;
using MBA.Educacao.Online.Alunos.Application.Handlers.Commands;
using MBA.Educacao.Online.Alunos.Domain.Entities;
using MBA.Educacao.Online.Alunos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Core.Domain.Notifications;
using MBA.Educacao.Online.Cursos.Domain.Entities;
using MBA.Educacao.Online.Cursos.Domain.Enums;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using Moq;
using Moq.AutoMock;

namespace MBA.Educacao.Online.Alunos.Test.ApplicationUnit
{
    public class AcessarEstudoAulaCommandHandlerTest
    {
        private readonly AutoMocker _mocker;
        private readonly AcessarEstudoAulaCommandHandler _handler;

        public AcessarEstudoAulaCommandHandlerTest()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<AcessarEstudoAulaCommandHandler>();
        }

        #region Cenários de Sucesso

        [Fact(DisplayName = "Acessar Estudo - Dados Válidos Primeira Vez - Deve Criar Histórico com Sucesso")]
        [Trait("Alunos", "Handlers - Commands - AcessarEstudoAula")]
        public async Task Handle_DadosValidosPrimeiraVez_DeveCriarHistoricoComSucesso()
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

            var matricula = new Matricula(alunoId, cursoId, DateTime.Now.AddMonths(12));
            typeof(Matricula).GetProperty("Id")!.SetValue(matricula, matriculaId);

            var command = new AcessarEstudoAulaCommand(alunoId, matriculaId, cursoId, aulaId);

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
            matricula.HistoricosAprendizado.Should().HaveCount(1);
            matricula.HistoricosAprendizado.First().AulaId.Should().Be(aulaId);
            _mocker.GetMock<IMatriculaRepository>().Verify(r => r.Alterar(It.IsAny<Matricula>()), Times.Once);
            _mocker.GetMock<IMatriculaRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Acessar Estudo - Histórico Já Existe - Deve Retornar True (Idempotência)")]
        [Trait("Alunos", "Handlers - Commands - AcessarEstudoAula")]
        public async Task Handle_HistoricoJaExiste_DeveRetornarTrueIdempotencia()
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

            var matricula = new Matricula(alunoId, cursoId, DateTime.Now.AddMonths(12));
            typeof(Matricula).GetProperty("Id")!.SetValue(matricula, matriculaId);
            matricula.IniciarAprendizado(aulaId); // Histórico já existe

            var command = new AcessarEstudoAulaCommand(alunoId, matriculaId, cursoId, aulaId);

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
            resultado.Should().BeTrue();
            matricula.HistoricosAprendizado.Should().HaveCount(1); // Mantém apenas 1
            _mocker.GetMock<IMatriculaRepository>().Verify(r => r.Alterar(It.IsAny<Matricula>()), Times.Never);
            _mocker.GetMock<IMatriculaRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Never);
        }

        #endregion

        #region Cenários de Falha - Validação

        [Fact(DisplayName = "Acessar Estudo - Command Inválido - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - AcessarEstudoAula")]
        public async Task Handle_CommandInvalido_DeveRetornarFalso()
        {
            // Arrange
            var command = new AcessarEstudoAulaCommand(Guid.Empty, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<INotificador>().Verify(n => n.Handle(It.IsAny<Notificacao>()), Times.AtLeastOnce);
        }

        #endregion

        #region Cenários de Falha - Curso

        [Fact(DisplayName = "Acessar Estudo - Curso Não Encontrado - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - AcessarEstudoAula")]
        public async Task Handle_CursoNaoEncontrado_DeveRetornarFalso()
        {
            // Arrange
            var command = new AcessarEstudoAulaCommand(
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
            _mocker.GetMock<INotificador>().Verify(
                n => n.Handle(It.Is<Notificacao>(not => not.Campo == "CursoId")),
                Times.Once
            );
        }

        #endregion

        #region Cenários de Falha - Aula

        [Fact(DisplayName = "Acessar Estudo - Aula Não Encontrada - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - AcessarEstudoAula")]
        public async Task Handle_AulaNaoEncontrada_DeveRetornarFalso()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var curso = new Curso("MBA em Gestão", "Curso completo", "Eduardo Pires", NivelCurso.Avancado, 1000);

            var command = new AcessarEstudoAulaCommand(
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
            _mocker.GetMock<INotificador>().Verify(
                n => n.Handle(It.Is<Notificacao>(not => not.Campo == "AulaId")),
                Times.Once
            );
        }

        [Fact(DisplayName = "Acessar Estudo - Aula Não Pertence ao Curso - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - AcessarEstudoAula")]
        public async Task Handle_AulaNaoPertenceAoCurso_DeveRetornarFalso()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var outroCursoId = Guid.NewGuid();
            var aulaId = Guid.NewGuid();

            var curso = new Curso("MBA em Gestão", "Curso completo", "Eduardo Pires", NivelCurso.Avancado, 1000);
            var aula = new Aula("Aula 1", "Introdução", 60, 1);
            typeof(Aula).GetProperty("CursoId")!.SetValue(aula, outroCursoId); // Aula de outro curso
            typeof(Aula).GetProperty("Id")!.SetValue(aula, aulaId);

            var command = new AcessarEstudoAulaCommand(
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

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<INotificador>().Verify(
                n => n.Handle(It.Is<Notificacao>(not =>
                    not.Campo == "AulaId" &&
                    not.Mensagem == "Aula não pertence ao curso informado")),
                Times.Once
            );
        }

        #endregion

        #region Cenários de Falha - Matrícula

        [Fact(DisplayName = "Acessar Estudo - Matrícula Não Encontrada - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - AcessarEstudoAula")]
        public async Task Handle_MatriculaNaoEncontrada_DeveRetornarFalso()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var aulaId = Guid.NewGuid();

            var curso = new Curso("MBA em Gestão", "Curso completo", "Eduardo Pires", NivelCurso.Avancado, 1000);
            var aula = new Aula("Aula 1", "Introdução", 60, 1);
            typeof(Aula).GetProperty("CursoId")!.SetValue(aula, cursoId);
            typeof(Aula).GetProperty("Id")!.SetValue(aula, aulaId);

            var command = new AcessarEstudoAulaCommand(
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
            _mocker.GetMock<INotificador>().Verify(
                n => n.Handle(It.Is<Notificacao>(not => not.Campo == "MatriculaId")),
                Times.Once
            );
        }

        [Fact(DisplayName = "Acessar Estudo - Matrícula Não Pertence ao Aluno - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - AcessarEstudoAula")]
        public async Task Handle_MatriculaNaoPertenceAoAluno_DeveRetornarFalso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var outroAlunoId = Guid.NewGuid();
            var matriculaId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var aulaId = Guid.NewGuid();

            var curso = new Curso("MBA em Gestão", "Curso completo", "Eduardo Pires", NivelCurso.Avancado, 1000);
            var aula = new Aula("Aula 1", "Introdução", 60, 1);
            typeof(Aula).GetProperty("CursoId")!.SetValue(aula, cursoId);
            typeof(Aula).GetProperty("Id")!.SetValue(aula, aulaId);

            var matricula = new Matricula(outroAlunoId, cursoId, DateTime.Now.AddMonths(12)); // Matrícula de outro aluno
            typeof(Matricula).GetProperty("Id")!.SetValue(matricula, matriculaId);

            var command = new AcessarEstudoAulaCommand(alunoId, matriculaId, cursoId, aulaId);

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
            _mocker.GetMock<INotificador>().Verify(
                n => n.Handle(It.Is<Notificacao>(not =>
                    not.Campo == "Matricula" &&
                    not.Mensagem == "Matrícula não pertence ao aluno informado")),
                Times.Once
            );
        }

        [Fact(DisplayName = "Acessar Estudo - Curso da Matrícula Diferente - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - AcessarEstudoAula")]
        public async Task Handle_CursoMatriculaDiferente_DeveRetornarFalso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var matriculaId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var outroCursoId = Guid.NewGuid();
            var aulaId = Guid.NewGuid();

            var curso = new Curso("MBA em Gestão", "Curso completo", "Eduardo Pires", NivelCurso.Avancado, 1000);
            var aula = new Aula("Aula 1", "Introdução", 60, 1);
            typeof(Aula).GetProperty("CursoId")!.SetValue(aula, cursoId);
            typeof(Aula).GetProperty("Id")!.SetValue(aula, aulaId);

            var matricula = new Matricula(alunoId, outroCursoId, DateTime.Now.AddMonths(12)); // Matrícula de outro curso
            typeof(Matricula).GetProperty("Id")!.SetValue(matricula, matriculaId);

            var command = new AcessarEstudoAulaCommand(alunoId, matriculaId, cursoId, aulaId);

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
            _mocker.GetMock<INotificador>().Verify(
                n => n.Handle(It.Is<Notificacao>(not =>
                    not.Campo == "CursoId" &&
                    not.Mensagem == "Curso não corresponde à matrícula informada")),
                Times.Once
            );
        }

        [Fact(DisplayName = "Acessar Estudo - Matrícula Inativa - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - AcessarEstudoAula")]
        public async Task Handle_MatriculaInativa_DeveRetornarFalso()
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

            var matricula = new Matricula(alunoId, cursoId, DateTime.Now.AddMonths(12));
            typeof(Matricula).GetProperty("Id")!.SetValue(matricula, matriculaId);
            matricula.Cancelar(); // Matrícula inativa

            var command = new AcessarEstudoAulaCommand(alunoId, matriculaId, cursoId, aulaId);

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
            _mocker.GetMock<INotificador>().Verify(
                n => n.Handle(It.Is<Notificacao>(not =>
                    not.Campo == "Matricula" &&
                    not.Mensagem == "Matrícula não está ativa")),
                Times.Once
            );
        }

        #endregion

        #region Cenários de Falha - Persistência

        [Fact(DisplayName = "Acessar Estudo - Erro ao Salvar - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - AcessarEstudoAula")]
        public async Task Handle_ErroAoSalvar_DeveRetornarFalso()
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

            var matricula = new Matricula(alunoId, cursoId, DateTime.Now.AddMonths(12));
            typeof(Matricula).GetProperty("Id")!.SetValue(matricula, matriculaId);

            var command = new AcessarEstudoAulaCommand(alunoId, matriculaId, cursoId, aulaId);

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
            unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(false); // Simula erro ao salvar

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.UnitOfWork)
                .Returns(unitOfWorkMock.Object);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<INotificador>().Verify(
                n => n.Handle(It.Is<Notificacao>(not =>
                    not.Campo == "Estudo" &&
                    not.Mensagem == "Erro ao registrar acesso à aula")),
                Times.Once
            );
        }

        #endregion

        #region Cenários de Integração

        [Fact(DisplayName = "Acessar Estudo - Fluxo Completo - Deve Executar Todas as Validações")]
        [Trait("Alunos", "Handlers - Commands - AcessarEstudoAula - Integração")]
        public async Task Handle_FluxoCompleto_DeveExecutarTodasValidacoes()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var matriculaId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var aulaId = Guid.NewGuid();

            var curso = new Curso("MBA em Gestão de Projetos", "Curso completo de MBA", "Eduardo Pires", NivelCurso.Avancado, 1000);
            var aula = new Aula("Introdução ao Gerenciamento", "Aula introdutória sobre gerenciamento de projetos", 90, 1);
            typeof(Aula).GetProperty("CursoId")!.SetValue(aula, cursoId);
            typeof(Aula).GetProperty("Id")!.SetValue(aula, aulaId);

            var matricula = new Matricula(alunoId, cursoId, DateTime.Now.AddYears(1));
            typeof(Matricula).GetProperty("Id")!.SetValue(matricula, matriculaId);

            var command = new AcessarEstudoAulaCommand(alunoId, matriculaId, cursoId, aulaId);

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

            // Verificações detalhadas
            _mocker.GetMock<ICursoRepository>().Verify(r => r.BuscarPorIdAsync(cursoId), Times.Once, "Deve buscar o curso");
            _mocker.GetMock<IAulaRepository>().Verify(r => r.BuscarPorIdAsync(aulaId), Times.Once, "Deve buscar a aula");
            _mocker.GetMock<IMatriculaRepository>().Verify(r => r.BuscarPorId(matriculaId), Times.Once, "Deve buscar a matrícula");
            _mocker.GetMock<IMatriculaRepository>().Verify(r => r.Alterar(It.IsAny<Matricula>()), Times.Once, "Deve alterar a matrícula");
            _mocker.GetMock<IMatriculaRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once, "Deve fazer commit");

            // Verifica que nenhuma notificação de erro foi enviada
            _mocker.GetMock<INotificador>().Verify(n => n.Handle(It.IsAny<Notificacao>()), Times.Never);

            // Verifica que o histórico foi criado
            matricula.HistoricosAprendizado.Should().HaveCount(1);
            matricula.HistoricosAprendizado.First().AulaId.Should().Be(aulaId);
        }

        #endregion
    }
}

