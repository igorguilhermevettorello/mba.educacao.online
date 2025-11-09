using FluentAssertions;
using MBA.Educacao.Online.Alunos.Domain.Entities;
using MBA.Educacao.Online.Alunos.Domain.Enums;

namespace MBA.Educacao.Online.Alunos.Test.DomainUnit
{
    public class HistoricoAprendizadoTest
    {
        #region 1. Criar Histórico Aprendizado

        [Fact(DisplayName = "Criar Histórico - Com Aula ID - Deve Criar com Sucesso")]
        [Trait("Alunos", "HistoricoAprendizado - Domain")]
        public void CriarHistorico_ComAulaId_DeveCriarComSucesso()
        {
            // Arrange
            var aulaId = Guid.NewGuid();

            // Act
            var historico = new HistoricoAprendizado(aulaId);

            // Assert
            historico.Should().NotBeNull();
            historico.AulaId.Should().Be(aulaId);
            historico.DataInicio.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            historico.DataConclusao.Should().BeNull();
            historico.Status.Should().Be(StatusAprendizadoEnum.EmAndamento);
        }

        [Fact(DisplayName = "Criar Histórico - Sem Aula ID - Deve Criar com Sucesso")]
        [Trait("Alunos", "HistoricoAprendizado - Domain")]
        public void CriarHistorico_SemAulaId_DeveCriarComSucesso()
        {
            // Act
            var historico = new HistoricoAprendizado();

            // Assert
            historico.Should().NotBeNull();
            historico.AulaId.Should().BeNull();
            historico.DataInicio.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            historico.Status.Should().Be(StatusAprendizadoEnum.EmAndamento);
        }

        [Fact(DisplayName = "Criar Histórico - Factory Method - Deve Criar com Sucesso")]
        [Trait("Alunos", "HistoricoAprendizado - Domain")]
        public void CriarHistorico_FactoryMethod_DeveCriarComSucesso()
        {
            // Arrange
            var aulaId = Guid.NewGuid();

            // Act
            var historico = HistoricoAprendizado.Criar(aulaId);

            // Assert
            historico.Should().NotBeNull();
            historico.AulaId.Should().Be(aulaId);
            historico.Status.Should().Be(StatusAprendizadoEnum.EmAndamento);
        }

        #endregion

        #region 2. Iniciar Estudo

        [Fact(DisplayName = "Iniciar Estudo - Status Pendente - Deve Atualizar para Em Andamento")]
        [Trait("Alunos", "HistoricoAprendizado - Domain")]
        public void IniciarEstudo_StatusPendente_DeveAtualizarParaEmAndamento()
        {
            // Arrange
            var aulaId = Guid.NewGuid();
            var historico = new HistoricoAprendizado(aulaId);

            // Act
            var historicoAtualizado = historico.IniciarEstudo();

            // Assert
            historicoAtualizado.Status.Should().Be(StatusAprendizadoEnum.EmAndamento);
            historicoAtualizado.AulaId.Should().Be(aulaId);
        }

        [Fact(DisplayName = "Iniciar Estudo - Status Concluído - Deve Lançar Exceção")]
        [Trait("Alunos", "HistoricoAprendizado - Domain")]
        public void IniciarEstudo_StatusConcluido_DeveLancarExcecao()
        {
            // Arrange
            var aulaId = Guid.NewGuid();
            var historico = new HistoricoAprendizado(aulaId);
            var historicoFinalizado = historico.FinalizarEstudo();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => historicoFinalizado.IniciarEstudo());
        }

        #endregion

        #region 3. Finalizar Estudo

        [Fact(DisplayName = "Finalizar Estudo - Status Em Andamento - Deve Finalizar com Sucesso")]
        [Trait("Alunos", "HistoricoAprendizado - Domain")]
        public void FinalizarEstudo_StatusEmAndamento_DeveFinalizarComSucesso()
        {
            // Arrange
            var aulaId = Guid.NewGuid();
            var historico = new HistoricoAprendizado(aulaId);

            // Act
            var historicoFinalizado = historico.FinalizarEstudo();

            // Assert
            historicoFinalizado.Status.Should().Be(StatusAprendizadoEnum.Concluido);
            historicoFinalizado.DataConclusao.Should().NotBeNull();
            historicoFinalizado.DataConclusao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Fact(DisplayName = "Finalizar Estudo - Status Concluído - Deve Lançar Exceção")]
        [Trait("Alunos", "HistoricoAprendizado - Domain")]
        public void FinalizarEstudo_StatusConcluido_DeveLancarExcecao()
        {
            // Arrange
            var aulaId = Guid.NewGuid();
            var historico = new HistoricoAprendizado(aulaId);
            var historicoFinalizado = historico.FinalizarEstudo();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => historicoFinalizado.FinalizarEstudo());
        }

        #endregion

        #region 4. Esta Concluído

        [Fact(DisplayName = "Esta Concluído - Status Concluído - Deve Retornar True")]
        [Trait("Alunos", "HistoricoAprendizado - Domain")]
        public void EstaConcluido_StatusConcluido_DeveRetornarTrue()
        {
            // Arrange
            var aulaId = Guid.NewGuid();
            var historico = new HistoricoAprendizado(aulaId);
            var historicoFinalizado = historico.FinalizarEstudo();

            // Act
            var resultado = historicoFinalizado.EstaConcluido();

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact(DisplayName = "Esta Concluído - Status Em Andamento - Deve Retornar False")]
        [Trait("Alunos", "HistoricoAprendizado - Domain")]
        public void EstaConcluido_StatusEmAndamento_DeveRetornarFalse()
        {
            // Arrange
            var aulaId = Guid.NewGuid();
            var historico = new HistoricoAprendizado(aulaId);

            // Act
            var resultado = historico.EstaConcluido();

            // Assert
            resultado.Should().BeFalse();
        }

        #endregion

        #region 5. Esta Em Andamento

        [Fact(DisplayName = "Esta Em Andamento - Status Em Andamento - Deve Retornar True")]
        [Trait("Alunos", "HistoricoAprendizado - Domain")]
        public void EstaEmAndamento_StatusEmAndamento_DeveRetornarTrue()
        {
            // Arrange
            var aulaId = Guid.NewGuid();
            var historico = new HistoricoAprendizado(aulaId);

            // Act
            var resultado = historico.EstaEmAndamento();

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact(DisplayName = "Esta Em Andamento - Status Concluído - Deve Retornar False")]
        [Trait("Alunos", "HistoricoAprendizado - Domain")]
        public void EstaEmAndamento_StatusConcluido_DeveRetornarFalse()
        {
            // Arrange
            var aulaId = Guid.NewGuid();
            var historico = new HistoricoAprendizado(aulaId);
            var historicoFinalizado = historico.FinalizarEstudo();

            // Act
            var resultado = historicoFinalizado.EstaEmAndamento();

            // Assert
            resultado.Should().BeFalse();
        }

        #endregion
    }
}

