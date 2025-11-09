using FluentAssertions;
using MBA.Educacao.Online.Pagamentos.Domain.Entities;
using MBA.Educacao.Online.Pagamentos.Domain.Enums;

namespace MBA.Educacao.Online.Pagamentos.Test.DomainUnit
{
    public class PagamentoTest
    {
        #region 1. Criar Pagamento

        [Fact(DisplayName = "Criar Pagamento - Dados Válidos - Deve Criar com Sucesso")]
        [Trait("Pagamentos", "Pagamento - Domain")]
        public void CriarPagamento_DadosValidos_DeveCriarComSucesso()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var valor = 1000m;
            var nomeCartao = "João Silva";
            var numeroCartao = "4111111111111111";
            var expiracaoCartao = "12/25";
            var cvvCartao = "123";

            // Act
            var pagamento = new Pagamento(pedidoId, valor, nomeCartao, numeroCartao, expiracaoCartao, cvvCartao);

            // Assert
            pagamento.Should().NotBeNull();
            pagamento.PedidoId.Should().Be(pedidoId);
            pagamento.Valor.Should().Be(valor);
            pagamento.NomeCartao.Should().Be(nomeCartao);
            pagamento.NumeroCartao.Should().Be(numeroCartao);
            pagamento.ExpiracaoCartao.Should().Be(expiracaoCartao);
            pagamento.CvvCartao.Should().Be(cvvCartao);
            pagamento.Status.Should().Be("Pendente");
        }

        [Fact(DisplayName = "Criar Pagamento - Pedido ID Vazio - Deve Lançar Exceção")]
        [Trait("Pagamentos", "Pagamento - Domain")]
        public void CriarPagamento_PedidoIdVazio_DeveLancarExcecao()
        {
            // Arrange
            var pedidoId = Guid.Empty;
            var valor = 1000m;
            var nomeCartao = "João Silva";
            var numeroCartao = "4111111111111111";
            var expiracaoCartao = "12/25";
            var cvvCartao = "123";

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new Pagamento(pedidoId, valor, nomeCartao, numeroCartao, expiracaoCartao, cvvCartao));
        }

        [Theory(DisplayName = "Criar Pagamento - Valor Inválido - Deve Lançar Exceção")]
        [Trait("Pagamentos", "Pagamento - Domain")]
        [InlineData(0)]
        [InlineData(-100)]
        [InlineData(1000000)]
        public void CriarPagamento_ValorInvalido_DeveLancarExcecao(decimal valor)
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var nomeCartao = "João Silva";
            var numeroCartao = "4111111111111111";
            var expiracaoCartao = "12/25";
            var cvvCartao = "123";

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new Pagamento(pedidoId, valor, nomeCartao, numeroCartao, expiracaoCartao, cvvCartao));
        }

        [Theory(DisplayName = "Criar Pagamento - Nome Cartão Inválido - Deve Lançar Exceção")]
        [Trait("Pagamentos", "Pagamento - Domain")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("Jo")]
        public void CriarPagamento_NomeCartaoInvalido_DeveLancarExcecao(string nomeCartao)
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var valor = 1000m;
            var numeroCartao = "4111111111111111";
            var expiracaoCartao = "12/25";
            var cvvCartao = "123";

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new Pagamento(pedidoId, valor, nomeCartao, numeroCartao, expiracaoCartao, cvvCartao));
        }

        [Theory(DisplayName = "Criar Pagamento - Número Cartão Inválido - Deve Lançar Exceção")]
        [Trait("Pagamentos", "Pagamento - Domain")]
        [InlineData("")]
        [InlineData("123")]
        [InlineData("12345678901234567890")]
        [InlineData("ABCD1234EFGH5678")]
        public void CriarPagamento_NumeroCartaoInvalido_DeveLancarExcecao(string numeroCartao)
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var valor = 1000m;
            var nomeCartao = "João Silva";
            var expiracaoCartao = "12/25";
            var cvvCartao = "123";

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new Pagamento(pedidoId, valor, nomeCartao, numeroCartao, expiracaoCartao, cvvCartao));
        }

        [Theory(DisplayName = "Criar Pagamento - CVV Inválido - Deve Lançar Exceção")]
        [Trait("Pagamentos", "Pagamento - Domain")]
        [InlineData("")]
        [InlineData("12")]
        [InlineData("12345")]
        [InlineData("ABC")]
        public void CriarPagamento_CvvInvalido_DeveLancarExcecao(string cvvCartao)
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var valor = 1000m;
            var nomeCartao = "João Silva";
            var numeroCartao = "4111111111111111";
            var expiracaoCartao = "12/25";

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new Pagamento(pedidoId, valor, nomeCartao, numeroCartao, expiracaoCartao, cvvCartao));
        }

        #endregion

        #region 2. Atualizar Status

        [Fact(DisplayName = "Atualizar Status - Status Válido - Deve Atualizar com Sucesso")]
        [Trait("Pagamentos", "Pagamento - Domain")]
        public void AtualizarStatus_StatusValido_DeveAtualizarComSucesso()
        {
            // Arrange
            var pagamento = new Pagamento(
                Guid.NewGuid(), 1000m, "João Silva", "4111111111111111", "12/25", "123");

            // Act
            pagamento.AtualizarStatus("Aprovado");

            // Assert
            pagamento.Status.Should().Be("Aprovado");
        }

        [Fact(DisplayName = "Atualizar Status - Status Vazio - Deve Lançar Exceção")]
        [Trait("Pagamentos", "Pagamento - Domain")]
        public void AtualizarStatus_StatusVazio_DeveLancarExcecao()
        {
            // Arrange
            var pagamento = new Pagamento(
                Guid.NewGuid(), 1000m, "João Silva", "4111111111111111", "12/25", "123");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => pagamento.AtualizarStatus(""));
        }

        #endregion

        #region 3. Associar Transação

        [Fact(DisplayName = "Associar Transação - Transação Válida - Deve Associar com Sucesso")]
        [Trait("Pagamentos", "Pagamento - Domain")]
        public void AssociarTransacao_TransacaoValida_DeveAssociarComSucesso()
        {
            // Arrange
            var pagamento = new Pagamento(
                Guid.NewGuid(), 1000m, "João Silva", "4111111111111111", "12/25", "123");
            var transacao = new Transacao(Guid.NewGuid(), pagamento.Id, 1000m, StatusTransacao.Autorizado);

            // Act
            pagamento.AssociarTransacao(transacao);

            // Assert
            pagamento.Transacao.Should().NotBeNull();
            pagamento.Transacao.Should().Be(transacao);
        }

        [Fact(DisplayName = "Associar Transação - Transação Nula - Deve Lançar Exceção")]
        [Trait("Pagamentos", "Pagamento - Domain")]
        public void AssociarTransacao_TransacaoNula_DeveLancarExcecao()
        {
            // Arrange
            var pagamento = new Pagamento(
                Guid.NewGuid(), 1000m, "João Silva", "4111111111111111", "12/25", "123");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => pagamento.AssociarTransacao(null!));
        }

        [Fact(DisplayName = "Associar Transação - Transação Já Associada - Deve Lançar Exceção")]
        [Trait("Pagamentos", "Pagamento - Domain")]
        public void AssociarTransacao_TransacaoJaAssociada_DeveLancarExcecao()
        {
            // Arrange
            var pagamento = new Pagamento(
                Guid.NewGuid(), 1000m, "João Silva", "4111111111111111", "12/25", "123");
            var transacao1 = new Transacao(Guid.NewGuid(), pagamento.Id, 1000m, StatusTransacao.Autorizado);
            var transacao2 = new Transacao(Guid.NewGuid(), pagamento.Id, 1000m, StatusTransacao.Autorizado);
            pagamento.AssociarTransacao(transacao1);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => pagamento.AssociarTransacao(transacao2));
        }

        #endregion

        #region 4. Métodos de Verificação

        [Theory(DisplayName = "Foi Aprovado - Status Aprovado/Pago - Deve Retornar True")]
        [Trait("Pagamentos", "Pagamento - Domain")]
        [InlineData("Aprovado")]
        [InlineData("Pago")]
        public void FoiAprovado_StatusAprovadoOuPago_DeveRetornarTrue(string status)
        {
            // Arrange
            var pagamento = new Pagamento(
                Guid.NewGuid(), 1000m, "João Silva", "4111111111111111", "12/25", "123");
            pagamento.AtualizarStatus(status);

            // Act
            var resultado = pagamento.FoiAprovado();

            // Assert
            resultado.Should().BeTrue();
        }

        [Theory(DisplayName = "Foi Recusado - Status Recusado/Negado - Deve Retornar True")]
        [Trait("Pagamentos", "Pagamento - Domain")]
        [InlineData("Recusado")]
        [InlineData("Negado")]
        public void FoiRecusado_StatusRecusadoOuNegado_DeveRetornarTrue(string status)
        {
            // Arrange
            var pagamento = new Pagamento(
                Guid.NewGuid(), 1000m, "João Silva", "4111111111111111", "12/25", "123");
            pagamento.AtualizarStatus(status);

            // Act
            var resultado = pagamento.FoiRecusado();

            // Assert
            resultado.Should().BeTrue();
        }

        #endregion
    }
}

