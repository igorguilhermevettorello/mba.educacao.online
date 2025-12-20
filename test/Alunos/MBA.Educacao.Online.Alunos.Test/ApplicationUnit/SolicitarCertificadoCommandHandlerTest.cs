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
    public class SolicitarCertificadoCommandHandlerTest
    {
        private readonly AutoMocker _mocker;
        private readonly SolicitarCertificadoCommandHandler _handler;

        public SolicitarCertificadoCommandHandlerTest()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<SolicitarCertificadoCommandHandler>();
        }

        #region Cenários de Sucesso

        [Fact(DisplayName = "Solicitar Certificado - Dados Válidos - Deve Gerar Certificado com Sucesso")]
        [Trait("Alunos", "Handlers - Commands - SolicitarCertificado")]
        public async Task Handle_DadosValidos_DeveGerarCertificadoComSucesso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var matriculaId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();

            var curso = new Curso("MBA em Gestão", "Curso completo", "Eduardo Pires", NivelCurso.Avancado, 1000);
            var matricula = new Matricula(alunoId, cursoId, DateTime.Now.AddMonths(12));
            typeof(Matricula).GetProperty("Id")!.SetValue(matricula, matriculaId);
            typeof(Matricula).GetProperty("ProgressoPercentual")!.SetValue(matricula, 100); // 100% concluído

            var aluno = new Aluno(alunoId, "João Silva", "joao@teste.com");
            aluno.AdicionarMatricula(cursoId, DateTime.Now.AddMonths(12)); // Aluno precisa estar matriculado

            var command = new SolicitarCertificadoCommand(alunoId, matriculaId, cursoId);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.BuscarPorId(matriculaId))
                .Returns(matricula);

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.BuscarPorId(alunoId))
                .Returns(aluno);

            _mocker.GetMock<ICursoRepository>()
                .Setup(r => r.BuscarPorIdAsync(cursoId))
                .ReturnsAsync(curso);

            _mocker.GetMock<ICertificadoRepository>()
                .Setup(r => r.BuscarPorAlunoECurso(alunoId, cursoId))
                .Returns((Certificado)null!);

            _mocker.GetMock<ICertificadoRepository>()
                .Setup(r => r.Adicionar(It.IsAny<Certificado>()));

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.Alterar(It.IsAny<Aluno>()));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(true);

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.UnitOfWork)
                .Returns(unitOfWorkMock.Object);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeTrue();
            aluno.Certificados.Should().HaveCount(1);
            aluno.Certificados.First().CursoId.Should().Be(cursoId);
            _mocker.GetMock<ICertificadoRepository>().Verify(r => r.Adicionar(It.IsAny<Certificado>()), Times.Once);
            _mocker.GetMock<IAlunoRepository>().Verify(r => r.Alterar(It.IsAny<Aluno>()), Times.Once);
            _mocker.GetMock<IAlunoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        #endregion

        #region Cenários de Falha - Validação

        [Fact(DisplayName = "Solicitar Certificado - Command Inválido - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - SolicitarCertificado")]
        public async Task Handle_CommandInvalido_DeveRetornarFalso()
        {
            // Arrange
            var command = new SolicitarCertificadoCommand(Guid.Empty, Guid.NewGuid(), Guid.NewGuid());

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<INotificador>().Verify(n => n.Handle(It.IsAny<Notificacao>()), Times.AtLeastOnce);
        }

        #endregion

        #region Cenários de Falha - Matrícula

        [Fact(DisplayName = "Solicitar Certificado - Matrícula Não Encontrada - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - SolicitarCertificado")]
        public async Task Handle_MatriculaNaoEncontrada_DeveRetornarFalso()
        {
            // Arrange
            var command = new SolicitarCertificadoCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            );

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

        [Fact(DisplayName = "Solicitar Certificado - Matrícula Não Pertence ao Aluno - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - SolicitarCertificado")]
        public async Task Handle_MatriculaNaoPertenceAoAluno_DeveRetornarFalso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var outroAlunoId = Guid.NewGuid();
            var matriculaId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();

            var matricula = new Matricula(outroAlunoId, cursoId, DateTime.Now.AddMonths(12)); // Matrícula de outro aluno
            typeof(Matricula).GetProperty("Id")!.SetValue(matricula, matriculaId);

            var command = new SolicitarCertificadoCommand(alunoId, matriculaId, cursoId);

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

        [Fact(DisplayName = "Solicitar Certificado - Curso da Matrícula Diferente - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - SolicitarCertificado")]
        public async Task Handle_CursoMatriculaDiferente_DeveRetornarFalso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var matriculaId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var outroCursoId = Guid.NewGuid();

            var matricula = new Matricula(alunoId, outroCursoId, DateTime.Now.AddMonths(12)); // Matrícula de outro curso
            typeof(Matricula).GetProperty("Id")!.SetValue(matricula, matriculaId);

            var command = new SolicitarCertificadoCommand(alunoId, matriculaId, cursoId);

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

        [Fact(DisplayName = "Solicitar Certificado - Matrícula Inativa - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - SolicitarCertificado")]
        public async Task Handle_MatriculaInativa_DeveRetornarFalso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var matriculaId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();

            var matricula = new Matricula(alunoId, cursoId, DateTime.Now.AddMonths(12));
            typeof(Matricula).GetProperty("Id")!.SetValue(matricula, matriculaId);
            matricula.Cancelar(); // Matrícula inativa

            var command = new SolicitarCertificadoCommand(alunoId, matriculaId, cursoId);

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

        [Fact(DisplayName = "Solicitar Certificado - Progresso Não Está 100% - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - SolicitarCertificado")]
        public async Task Handle_ProgressoNaoEsta100Porcento_DeveRetornarFalso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var matriculaId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();

            var matricula = new Matricula(alunoId, cursoId, DateTime.Now.AddMonths(12));
            typeof(Matricula).GetProperty("Id")!.SetValue(matricula, matriculaId);
            typeof(Matricula).GetProperty("ProgressoPercentual")!.SetValue(matricula, 80); // 80% concluído

            var command = new SolicitarCertificadoCommand(alunoId, matriculaId, cursoId);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.BuscarPorId(matriculaId))
                .Returns(matricula);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<INotificador>().Verify(
                n => n.Handle(It.Is<Notificacao>(not =>
                    not.Campo == "Progresso" &&
                    not.Mensagem.Contains("O curso deve estar 100% concluído") &&
                    not.Mensagem.Contains("80%"))),
                Times.Once
            );
        }

        #endregion

        #region Cenários de Falha - Aluno

        [Fact(DisplayName = "Solicitar Certificado - Aluno Não Encontrado - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - SolicitarCertificado")]
        public async Task Handle_AlunoNaoEncontrado_DeveRetornarFalso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var matriculaId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();

            var matricula = new Matricula(alunoId, cursoId, DateTime.Now.AddMonths(12));
            typeof(Matricula).GetProperty("Id")!.SetValue(matricula, matriculaId);
            typeof(Matricula).GetProperty("ProgressoPercentual")!.SetValue(matricula, 100);

            var command = new SolicitarCertificadoCommand(alunoId, matriculaId, cursoId);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.BuscarPorId(matriculaId))
                .Returns(matricula);

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.BuscarPorId(alunoId))
                .Returns((Aluno)null!);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<INotificador>().Verify(
                n => n.Handle(It.Is<Notificacao>(not =>
                    not.Campo == "AlunoId" &&
                    not.Mensagem == "Aluno não encontrado")),
                Times.Once
            );
        }

        #endregion

        #region Cenários de Falha - Curso

        [Fact(DisplayName = "Solicitar Certificado - Curso Não Encontrado - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - SolicitarCertificado")]
        public async Task Handle_CursoNaoEncontrado_DeveRetornarFalso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var matriculaId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();

            var matricula = new Matricula(alunoId, cursoId, DateTime.Now.AddMonths(12));
            typeof(Matricula).GetProperty("Id")!.SetValue(matricula, matriculaId);
            typeof(Matricula).GetProperty("ProgressoPercentual")!.SetValue(matricula, 100);

            var aluno = new Aluno(alunoId, "João Silva", "joao@teste.com");

            var command = new SolicitarCertificadoCommand(alunoId, matriculaId, cursoId);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.BuscarPorId(matriculaId))
                .Returns(matricula);

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.BuscarPorId(alunoId))
                .Returns(aluno);

            _mocker.GetMock<ICursoRepository>()
                .Setup(r => r.BuscarPorIdAsync(cursoId))
                .ReturnsAsync((Curso)null!);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<INotificador>().Verify(
                n => n.Handle(It.Is<Notificacao>(not =>
                    not.Campo == "CursoId" &&
                    not.Mensagem == "Curso não encontrado")),
                Times.Once
            );
        }

        #endregion

        #region Cenários de Falha - Certificado Existente

        [Fact(DisplayName = "Solicitar Certificado - Certificado Já Existe - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - SolicitarCertificado")]
        public async Task Handle_CertificadoJaExiste_DeveRetornarFalso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var matriculaId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();

            var curso = new Curso("MBA em Gestão", "Curso completo", "Eduardo Pires", NivelCurso.Avancado, 1000);
            var matricula = new Matricula(alunoId, cursoId, DateTime.Now.AddMonths(12));
            typeof(Matricula).GetProperty("Id")!.SetValue(matricula, matriculaId);
            typeof(Matricula).GetProperty("ProgressoPercentual")!.SetValue(matricula, 100);

            var aluno = new Aluno(alunoId, "João Silva", "joao@teste.com");
            var certificadoExistente = new Certificado(cursoId, "CERT-EXISTENTE");

            var command = new SolicitarCertificadoCommand(alunoId, matriculaId, cursoId);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.BuscarPorId(matriculaId))
                .Returns(matricula);

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.BuscarPorId(alunoId))
                .Returns(aluno);

            _mocker.GetMock<ICursoRepository>()
                .Setup(r => r.BuscarPorIdAsync(cursoId))
                .ReturnsAsync(curso);

            _mocker.GetMock<ICertificadoRepository>()
                .Setup(r => r.BuscarPorAlunoECurso(alunoId, cursoId))
                .Returns(certificadoExistente);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<INotificador>().Verify(
                n => n.Handle(It.Is<Notificacao>(not =>
                    not.Campo == "Certificado" &&
                    not.Mensagem == "Já existe um certificado emitido para este curso")),
                Times.Once
            );
        }

        #endregion

        #region Cenários de Falha - Persistência

        [Fact(DisplayName = "Solicitar Certificado - Erro ao Salvar - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - SolicitarCertificado")]
        public async Task Handle_ErroAoSalvar_DeveRetornarFalso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var matriculaId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();

            var curso = new Curso("MBA em Gestão", "Curso completo", "Eduardo Pires", NivelCurso.Avancado, 1000);
            var matricula = new Matricula(alunoId, cursoId, DateTime.Now.AddMonths(12));
            typeof(Matricula).GetProperty("Id")!.SetValue(matricula, matriculaId);
            typeof(Matricula).GetProperty("ProgressoPercentual")!.SetValue(matricula, 100);

            var aluno = new Aluno(alunoId, "João Silva", "joao@teste.com");
            aluno.AdicionarMatricula(cursoId, DateTime.Now.AddMonths(12)); // Aluno precisa estar matriculado

            var command = new SolicitarCertificadoCommand(alunoId, matriculaId, cursoId);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.BuscarPorId(matriculaId))
                .Returns(matricula);

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.BuscarPorId(alunoId))
                .Returns(aluno);

            _mocker.GetMock<ICursoRepository>()
                .Setup(r => r.BuscarPorIdAsync(cursoId))
                .ReturnsAsync(curso);

            _mocker.GetMock<ICertificadoRepository>()
                .Setup(r => r.BuscarPorAlunoECurso(alunoId, cursoId))
                .Returns((Certificado)null!);

            _mocker.GetMock<ICertificadoRepository>()
                .Setup(r => r.Adicionar(It.IsAny<Certificado>()));

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.Alterar(It.IsAny<Aluno>()));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(false); // Simula erro ao salvar

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.UnitOfWork)
                .Returns(unitOfWorkMock.Object);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<INotificador>().Verify(
                n => n.Handle(It.Is<Notificacao>(not =>
                    not.Campo == "Certificado" &&
                    not.Mensagem == "Erro ao gerar certificado")),
                Times.Once
            );
        }

        #endregion

        #region Cenários de Falha - Exceções

        [Fact(DisplayName = "Solicitar Certificado - InvalidOperationException ao Adicionar Certificado - Deve Retornar Falso")]
        [Trait("Alunos", "Handlers - Commands - SolicitarCertificado")]
        public async Task Handle_InvalidOperationExceptionAoAdicionarCertificado_DeveRetornarFalso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var matriculaId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();

            var curso = new Curso("MBA em Gestão", "Curso completo", "Eduardo Pires", NivelCurso.Avancado, 1000);
            var matricula = new Matricula(alunoId, cursoId, DateTime.Now.AddMonths(12));
            typeof(Matricula).GetProperty("Id")!.SetValue(matricula, matriculaId);
            typeof(Matricula).GetProperty("ProgressoPercentual")!.SetValue(matricula, 100);

            // Aluno sem matrícula ativa para que AdicionarCertificado lance InvalidOperationException
            var aluno = new Aluno(alunoId, "João Silva", "joao@teste.com");

            var command = new SolicitarCertificadoCommand(alunoId, matriculaId, cursoId);

            _mocker.GetMock<IMatriculaRepository>()
                .Setup(r => r.BuscarPorId(matriculaId))
                .Returns(matricula);

            _mocker.GetMock<IAlunoRepository>()
                .Setup(r => r.BuscarPorId(alunoId))
                .Returns(aluno);

            _mocker.GetMock<ICursoRepository>()
                .Setup(r => r.BuscarPorIdAsync(cursoId))
                .ReturnsAsync(curso);

            _mocker.GetMock<ICertificadoRepository>()
                .Setup(r => r.BuscarPorAlunoECurso(alunoId, cursoId))
                .Returns((Certificado)null!);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mocker.GetMock<INotificador>().Verify(
                n => n.Handle(It.Is<Notificacao>(not =>
                    not.Campo == "Certificado" &&
                    not.Mensagem.Contains("Aluno deve estar matriculado no curso"))),
                Times.Once
            );
        }

        #endregion
    }
}
