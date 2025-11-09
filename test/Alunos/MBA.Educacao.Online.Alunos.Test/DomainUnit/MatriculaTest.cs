using FluentAssertions;
using MBA.Educacao.Online.Alunos.Domain.Entities;
using MBA.Educacao.Online.Alunos.Domain.Enums;

namespace MBA.Educacao.Online.Alunos.Test.DomainUnit
{
    public class MatriculaTest
    {
        #region 1. Criar Matrícula

        [Fact(DisplayName = "Criar Matrícula - Dados Válidos - Deve Criar com Sucesso")]
        [Trait("Alunos", "Matrícula - Domain")]
        public void CriarMatricula_DadosValidos_DeveCriarComSucesso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var dataValidade = DateTime.Now.AddMonths(12);

            // Act
            var matricula = new Matricula(alunoId, cursoId, dataValidade);

            // Assert
            matricula.Should().NotBeNull();
            matricula.AlunoId.Should().Be(alunoId);
            matricula.CursoId.Should().Be(cursoId);
            matricula.DataValidade.Should().Be(dataValidade);
            matricula.Ativo.Should().BeTrue();
            matricula.ProgressoPercentual.Should().Be(0);
            matricula.HistoricosAprendizado.Should().BeEmpty();
        }

        [Fact(DisplayName = "Criar Matrícula - Curso ID Inválido - Deve Lançar Exceção")]
        [Trait("Alunos", "Matrícula - Domain")]
        public void CriarMatricula_CursoIdInvalido_DeveLancarExcecao()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.Empty;
            var dataValidade = DateTime.Now.AddMonths(12);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Matricula(alunoId, cursoId, dataValidade));
        }

        [Fact(DisplayName = "Criar Matrícula - Data Validade Passada - Deve Lançar Exceção")]
        [Trait("Alunos", "Matrícula - Domain")]
        public void CriarMatricula_DataValidadePassada_DeveLancarExcecao()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var dataValidade = DateTime.Now.AddMonths(-1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Matricula(alunoId, cursoId, dataValidade));
        }

        #endregion

        #region 2. Cancelar Matrícula

        [Fact(DisplayName = "Cancelar Matrícula - Matrícula Ativa - Deve Cancelar com Sucesso")]
        [Trait("Alunos", "Matrícula - Domain")]
        public void CancelarMatricula_MatriculaAtiva_DeveCancelarComSucesso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var dataValidade = DateTime.Now.AddMonths(12);
            var matricula = new Matricula(alunoId, cursoId, dataValidade);

            // Act
            matricula.Cancelar();

            // Assert
            matricula.Ativo.Should().BeFalse();
        }

        [Fact(DisplayName = "Cancelar Matrícula - Matrícula Já Cancelada - Deve Lançar Exceção")]
        [Trait("Alunos", "Matrícula - Domain")]
        public void CancelarMatricula_MatriculaJaCancelada_DeveLancarExcecao()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var dataValidade = DateTime.Now.AddMonths(12);
            var matricula = new Matricula(alunoId, cursoId, dataValidade);
            matricula.Cancelar();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => matricula.Cancelar());
        }

        #endregion

        #region 3. Reativar Matrícula

        [Fact(DisplayName = "Reativar Matrícula - Matrícula Cancelada e Não Vencida - Deve Reativar com Sucesso")]
        [Trait("Alunos", "Matrícula - Domain")]
        public void ReativarMatricula_MatriculaCanceladaNaoVencida_DeveReativarComSucesso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var dataValidade = DateTime.Now.AddMonths(12);
            var matricula = new Matricula(alunoId, cursoId, dataValidade);
            matricula.Cancelar();

            // Act
            matricula.Reativar();

            // Assert
            matricula.Ativo.Should().BeTrue();
        }

        [Fact(DisplayName = "Reativar Matrícula - Matrícula Já Ativa - Deve Lançar Exceção")]
        [Trait("Alunos", "Matrícula - Domain")]
        public void ReativarMatricula_MatriculaJaAtiva_DeveLancarExcecao()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var dataValidade = DateTime.Now.AddMonths(12);
            var matricula = new Matricula(alunoId, cursoId, dataValidade);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => matricula.Reativar());
        }

        #endregion

        #region 4. Estender Validade

        [Fact(DisplayName = "Estender Validade - Nova Data Futura - Deve Estender com Sucesso")]
        [Trait("Alunos", "Matrícula - Domain")]
        public void EstenderValidade_NovaDataFutura_DeveEstenderComSucesso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var dataValidade = DateTime.Now.AddMonths(6);
            var matricula = new Matricula(alunoId, cursoId, dataValidade);
            var novaDataValidade = DateTime.Now.AddMonths(12);

            // Act
            matricula.EstenderValidade(novaDataValidade);

            // Assert
            matricula.DataValidade.Should().Be(novaDataValidade);
        }

        [Fact(DisplayName = "Estender Validade - Nova Data Anterior - Deve Lançar Exceção")]
        [Trait("Alunos", "Matrícula - Domain")]
        public void EstenderValidade_NovaDataAnterior_DeveLancarExcecao()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var dataValidade = DateTime.Now.AddMonths(12);
            var matricula = new Matricula(alunoId, cursoId, dataValidade);
            var novaDataValidade = DateTime.Now.AddMonths(6);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => matricula.EstenderValidade(novaDataValidade));
        }

        #endregion

        #region 5. Iniciar Aprendizado

        [Fact(DisplayName = "Iniciar Aprendizado - Matrícula Ativa - Deve Criar Histórico")]
        [Trait("Alunos", "Matrícula - Domain")]
        public void IniciarAprendizado_MatriculaAtiva_DeveCriarHistorico()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var aulaId = Guid.NewGuid();
            var dataValidade = DateTime.Now.AddMonths(12);
            var matricula = new Matricula(alunoId, cursoId, dataValidade);

            // Act
            matricula.IniciarAprendizado(aulaId);

            // Assert
            matricula.HistoricosAprendizado.Should().HaveCount(1);
            matricula.HistoricosAprendizado.First().AulaId.Should().Be(aulaId);
            matricula.HistoricosAprendizado.First().Status.Should().Be(StatusAprendizadoEnum.EmAndamento);
        }

        [Fact(DisplayName = "Iniciar Aprendizado - Matrícula Inativa - Deve Lançar Exceção")]
        [Trait("Alunos", "Matrícula - Domain")]
        public void IniciarAprendizado_MatriculaInativa_DeveLancarExcecao()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var aulaId = Guid.NewGuid();
            var dataValidade = DateTime.Now.AddMonths(12);
            var matricula = new Matricula(alunoId, cursoId, dataValidade);
            matricula.Cancelar();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => matricula.IniciarAprendizado(aulaId));
        }

        #endregion

        #region 6. Iniciar Estudo Aula

        [Fact(DisplayName = "Iniciar Estudo Aula - Histórico Existe - Deve Atualizar Status")]
        [Trait("Alunos", "Matrícula - Domain")]
        public void IniciarEstudoAula_HistoricoExiste_DeveAtualizarStatus()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var aulaId = Guid.NewGuid();
            var dataValidade = DateTime.Now.AddMonths(12);
            var matricula = new Matricula(alunoId, cursoId, dataValidade);
            matricula.IniciarAprendizado(aulaId);

            // Act
            matricula.IniciarEstudoAula(aulaId);

            // Assert
            matricula.HistoricosAprendizado.Should().HaveCount(1);
            matricula.HistoricosAprendizado.First().Status.Should().Be(StatusAprendizadoEnum.EmAndamento);
        }

        #endregion

        #region 7. Finalizar Estudo Aula

        [Fact(DisplayName = "Finalizar Estudo Aula - Histórico Em Andamento - Deve Finalizar")]
        [Trait("Alunos", "Matrícula - Domain")]
        public void FinalizarEstudoAula_HistoricoEmAndamento_DeveFinalizar()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var aulaId = Guid.NewGuid();
            var dataValidade = DateTime.Now.AddMonths(12);
            var matricula = new Matricula(alunoId, cursoId, dataValidade);
            matricula.IniciarAprendizado(aulaId);
            var totalAulasCurso = 10;

            // Act
            matricula.FinalizarEstudoAula(aulaId, totalAulasCurso);

            // Assert
            matricula.HistoricosAprendizado.First().Status.Should().Be(StatusAprendizadoEnum.Concluido);
            matricula.HistoricosAprendizado.First().DataConclusao.Should().NotBeNull();
            matricula.ProgressoPercentual.Should().Be(10); // 1 de 10 aulas = 10%
        }

        [Fact(DisplayName = "Finalizar Estudo Aula - Histórico Não Existe - Deve Lançar Exceção")]
        [Trait("Alunos", "Matrícula - Domain")]
        public void FinalizarEstudoAula_HistoricoNaoExiste_DeveLancarExcecao()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var aulaId = Guid.NewGuid();
            var dataValidade = DateTime.Now.AddMonths(12);
            var matricula = new Matricula(alunoId, cursoId, dataValidade);
            var totalAulasCurso = 10;

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => matricula.FinalizarEstudoAula(aulaId, totalAulasCurso));
        }

        #endregion

        #region 8. Calcular Progresso Geral

        [Fact(DisplayName = "Calcular Progresso Geral - Múltiplas Aulas Concluídas - Deve Calcular Corretamente")]
        [Trait("Alunos", "Matrícula - Domain")]
        public void CalcularProgressoGeral_MultiplasAulasConcluidas_DeveCalcularCorretamente()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var dataValidade = DateTime.Now.AddMonths(12);
            var matricula = new Matricula(alunoId, cursoId, dataValidade);
            var totalAulasCurso = 10;

            // Criar e finalizar 3 aulas
            for (int i = 0; i < 3; i++)
            {
                var aulaId = Guid.NewGuid();
                matricula.IniciarAprendizado(aulaId);
                matricula.FinalizarEstudoAula(aulaId, totalAulasCurso);
            }

            // Assert
            matricula.ProgressoPercentual.Should().Be(30); // 3 de 10 = 30%
        }

        #endregion

        #region 9. Esta Vencida

        [Fact(DisplayName = "Esta Vencida - Data Validade Futura - Deve Retornar False")]
        [Trait("Alunos", "Matrícula - Domain")]
        public void EstaVencida_DataValidadeFutura_DeveRetornarFalse()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var dataValidade = DateTime.Now.AddMonths(12);
            var matricula = new Matricula(alunoId, cursoId, dataValidade);

            // Act
            var resultado = matricula.EstaVencida();

            // Assert
            resultado.Should().BeFalse();
        }

        #endregion

        #region 10. Obter Total Históricos

        [Fact(DisplayName = "Obter Total Históricos - Múltiplos Históricos - Deve Retornar Total Correto")]
        [Trait("Alunos", "Matrícula - Domain")]
        public void ObterTotalHistoricos_MultiplosHistoricos_DeveRetornarTotalCorreto()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var dataValidade = DateTime.Now.AddMonths(12);
            var matricula = new Matricula(alunoId, cursoId, dataValidade);

            // Criar 5 históricos
            for (int i = 0; i < 5; i++)
            {
                matricula.IniciarAprendizado(Guid.NewGuid());
            }

            // Act
            var total = matricula.ObterTotalHistoricos();

            // Assert
            total.Should().Be(5);
        }

        #endregion
    }
}

