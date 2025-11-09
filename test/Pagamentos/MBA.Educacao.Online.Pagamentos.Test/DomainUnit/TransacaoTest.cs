using FluentAssertions;
using MBA.Educacao.Online.Pagamentos.Domain.Entities;
using MBA.Educacao.Online.Pagamentos.Domain.Enums;

namespace MBA.Educacao.Online.Pagamentos.Test.DomainUnit
{
    public class TransacaoTest
    {
        #region 1. Criar Transação

        [Fact(DisplayName = "Criar Transação - Dados Válidos - Deve Criar com Sucesso")]
        [Trait("Pagamentos", "Transação - Domain")]
        public void CriarTransacao_DadosValidos_DeveCriarComSucesso()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var pagamentoId = Guid.NewGuid();
            var total = 1000m;
            var status = StatusTransacao.Autorizado;

            // Act
            var transacao = new Transacao(pedidoId, pagamentoId, total, status);

            // Assert
            transacao.Should().NotBeNull();
            transacao.PedidoId.Should().Be(pedidoId);
            transacao.PagamentoId.Should().Be(pagamentoId);
            transacao.Total.Should().Be(total);
            transacao.StatusTransacao.Should().Be(status);
            transacao.DataTransacao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact(DisplayName = "Criar Transação - Pedido ID Vazio - Deve Lançar Exceção")]
        [Trait("Pagamentos", "Transação - Domain")]
        public void CriarTransacao_PedidoIdVazio_DeveLancarExcecao()
        {
            // Arrange
            var pedidoId = Guid.Empty;
            var pagamentoId = Guid.NewGuid();
            var total = 1000m;
            var status = StatusTransacao.Autorizado;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Transacao(pedidoId, pagamentoId, total, status));
        }

        [Theory(DisplayName = "Criar Transação - Total Inválido - Deve Lançar Exceção")]
        [Trait("Pagamentos", "Transação - Domain")]
        [InlineData(0)]
        [InlineData(-100)]
        [InlineData(1000000)]
        public void CriarTransacao_TotalInvalido_DeveLancarExcecao(decimal total)
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var pagamentoId = Guid.NewGuid();
            var status = StatusTransacao.Autorizado;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Transacao(pedidoId, pagamentoId, total, status));
        }

        #endregion

        #region 2. Atualizar Status

        [Fact(DisplayName = "Atualizar Status - Para Pago - Deve Atualizar com Sucesso")]
        [Trait("Pagamentos", "Transação - Domain")]
        public void AtualizarStatus_ParaPago_DeveAtualizarComSucesso()
        {
            // Arrange
            var transacao = new Transacao(Guid.NewGuid(), Guid.NewGuid(), 1000m, StatusTransacao.Autorizado);

            // Act
            transacao.AtualizarStatus(StatusTransacao.Pago);

            // Assert
            transacao.StatusTransacao.Should().Be(StatusTransacao.Pago);
        }

        [Fact(DisplayName = "Atualizar Status - Transação Já Paga - Deve Lançar Exceção")]
        [Trait("Pagamentos", "Transação - Domain")]
        public void AtualizarStatus_TransacaoJaPaga_DeveLancarExcecao()
        {
            // Arrange
            var transacao = new Transacao(Guid.NewGuid(), Guid.NewGuid(), 1000m, StatusTransacao.Pago);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => transacao.AtualizarStatus(StatusTransacao.Recusado));
        }

        [Fact(DisplayName = "Atualizar Status - Transação Cancelada - Deve Lançar Exceção")]
        [Trait("Pagamentos", "Transação - Domain")]
        public void AtualizarStatus_TransacaoCancelada_DeveLancarExcecao()
        {
            // Arrange
            var transacao = new Transacao(Guid.NewGuid(), Guid.NewGuid(), 1000m, StatusTransacao.Cancelado);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => transacao.AtualizarStatus(StatusTransacao.Pago));
        }

        #endregion

        #region 3. Marcar Como Pago

        [Fact(DisplayName = "Marcar Como Pago - Transação Autorizada - Deve Marcar com Sucesso")]
        [Trait("Pagamentos", "Transação - Domain")]
        public void MarcarComoPago_TransacaoAutorizada_DeveMarcarComSucesso()
        {
            // Arrange
            var transacao = new Transacao(Guid.NewGuid(), Guid.NewGuid(), 1000m, StatusTransacao.Autorizado);

            // Act
            transacao.MarcarComoPago();

            // Assert
            transacao.StatusTransacao.Should().Be(StatusTransacao.Pago);
        }

        [Fact(DisplayName = "Marcar Como Pago - Transação Já Paga - Deve Lançar Exceção")]
        [Trait("Pagamentos", "Transação - Domain")]
        public void MarcarComoPago_TransacaoJaPaga_DeveLancarExcecao()
        {
            // Arrange
            var transacao = new Transacao(Guid.NewGuid(), Guid.NewGuid(), 1000m, StatusTransacao.Pago);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => transacao.MarcarComoPago());
        }

        #endregion

        #region 4. Marcar Como Recusado

        [Fact(DisplayName = "Marcar Como Recusado - Transação Autorizada - Deve Marcar com Sucesso")]
        [Trait("Pagamentos", "Transação - Domain")]
        public void MarcarComoRecusado_TransacaoAutorizada_DeveMarcarComSucesso()
        {
            // Arrange
            var transacao = new Transacao(Guid.NewGuid(), Guid.NewGuid(), 1000m, StatusTransacao.Autorizado);

            // Act
            transacao.MarcarComoRecusado();

            // Assert
            transacao.StatusTransacao.Should().Be(StatusTransacao.Recusado);
        }

        [Fact(DisplayName = "Marcar Como Recusado - Transação Paga - Deve Lançar Exceção")]
        [Trait("Pagamentos", "Transação - Domain")]
        public void MarcarComoRecusado_TransacaoPaga_DeveLancarExcecao()
        {
            // Arrange
            var transacao = new Transacao(Guid.NewGuid(), Guid.NewGuid(), 1000m, StatusTransacao.Pago);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => transacao.MarcarComoRecusado());
        }

        #endregion

        #region 5. Cancelar

        [Fact(DisplayName = "Cancelar - Transação Autorizada - Deve Cancelar com Sucesso")]
        [Trait("Pagamentos", "Transação - Domain")]
        public void Cancelar_TransacaoAutorizada_DeveCancelarComSucesso()
        {
            // Arrange
            var transacao = new Transacao(Guid.NewGuid(), Guid.NewGuid(), 1000m, StatusTransacao.Autorizado);

            // Act
            transacao.Cancelar();

            // Assert
            transacao.StatusTransacao.Should().Be(StatusTransacao.Cancelado);
        }

        [Fact(DisplayName = "Cancelar - Transação Paga - Deve Lançar Exceção")]
        [Trait("Pagamentos", "Transação - Domain")]
        public void Cancelar_TransacaoPaga_DeveLancarExcecao()
        {
            // Arrange
            var transacao = new Transacao(Guid.NewGuid(), Guid.NewGuid(), 1000m, StatusTransacao.Pago);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => transacao.Cancelar());
        }

        #endregion

        #region 6. Métodos de Verificação

        [Fact(DisplayName = "Foi Pago - Transação Paga - Deve Retornar True")]
        [Trait("Pagamentos", "Transação - Domain")]
        public void FoiPago_TransacaoPaga_DeveRetornarTrue()
        {
            // Arrange
            var transacao = new Transacao(Guid.NewGuid(), Guid.NewGuid(), 1000m, StatusTransacao.Autorizado);
            transacao.MarcarComoPago();

            // Act
            var resultado = transacao.FoiPago();

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact(DisplayName = "Foi Recusado - Transação Recusada - Deve Retornar True")]
        [Trait("Pagamentos", "Transação - Domain")]
        public void FoiRecusado_TransacaoRecusada_DeveRetornarTrue()
        {
            // Arrange
            var transacao = new Transacao(Guid.NewGuid(), Guid.NewGuid(), 1000m, StatusTransacao.Autorizado);
            transacao.MarcarComoRecusado();

            // Act
            var resultado = transacao.FoiRecusado();

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact(DisplayName = "Esta Cancelado - Transação Cancelada - Deve Retornar True")]
        [Trait("Pagamentos", "Transação - Domain")]
        public void EstaCancelado_TransacaoCancelada_DeveRetornarTrue()
        {
            // Arrange
            var transacao = new Transacao(Guid.NewGuid(), Guid.NewGuid(), 1000m, StatusTransacao.Autorizado);
            transacao.Cancelar();

            // Act
            var resultado = transacao.EstaCancelado();

            // Assert
            resultado.Should().BeTrue();
        }

        #endregion
    }
}

