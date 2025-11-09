using FluentAssertions;
using MBA.Educacao.Online.Vendas.Domain.Entities;
using MBA.Educacao.Online.Vendas.Domain.Enum;

namespace MBA.Educacao.Online.Vendas.Test.DomainUnit
{
    public class PedidoTest
    {
        #region 1. Criar Pedido

        [Fact(DisplayName = "Criar Pedido - Dados Válidos - Deve Criar com Sucesso")]
        [Trait("Vendas", "Pedido - Domain")]
        public void CriarPedido_DadosValidos_DeveCriarComSucesso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();

            // Act
            var pedido = new Pedido(alunoId);

            // Assert
            pedido.Should().NotBeNull();
            pedido.AlunoId.Should().Be(alunoId);
            pedido.ValorTotal.Should().Be(0);
            pedido.PedidoItens.Should().BeEmpty();
        }

        [Fact(DisplayName = "Criar Pedido - Aluno ID Vazio - Deve Lançar Exceção")]
        [Trait("Vendas", "Pedido - Domain")]
        public void CriarPedido_AlunoIdVazio_DeveLancarExcecao()
        {
            // Arrange
            var alunoId = Guid.Empty;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Pedido(alunoId));
        }

        [Fact(DisplayName = "Criar Pedido - Factory Method - Deve Criar em Status Rascunho")]
        [Trait("Vendas", "Pedido - Domain")]
        public void CriarPedido_FactoryMethod_DeveCriarEmStatusRascunho()
        {
            // Arrange
            var alunoId = Guid.NewGuid();

            // Act
            var pedido = Pedido.PedidoFactory.CriarPedido(alunoId);

            // Assert
            pedido.Should().NotBeNull();
            pedido.AlunoId.Should().Be(alunoId);
            pedido.PedidoStatus.Should().Be(PedidoStatusEnum.Rascunho);
        }

        #endregion

        #region 2. Adicionar Item

        [Fact(DisplayName = "Adicionar Item - Dados Válidos - Deve Adicionar com Sucesso")]
        [Trait("Vendas", "Pedido - Domain")]
        public void AdicionarItem_DadosValidos_DeveAdicionarComSucesso()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.CriarPedido(Guid.NewGuid());
            var item = new PedidoItem(Guid.NewGuid(), "MBA em Gestão", 1000m);

            // Act
            pedido.AdicionarItem(item);

            // Assert
            pedido.PedidoItens.Should().HaveCount(1);
            pedido.ValorTotal.Should().Be(1000m);
        }

        [Fact(DisplayName = "Adicionar Item - Item Nulo - Deve Lançar Exceção")]
        [Trait("Vendas", "Pedido - Domain")]
        public void AdicionarItem_ItemNulo_DeveLancarExcecao()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.CriarPedido(Guid.NewGuid());

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => pedido.AdicionarItem(null!));
        }

        [Fact(DisplayName = "Adicionar Item - Pedido Pago - Deve Lançar Exceção")]
        [Trait("Vendas", "Pedido - Domain")]
        public void AdicionarItem_PedidoPago_DeveLancarExcecao()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.CriarPedido(Guid.NewGuid());
            var item = new PedidoItem(Guid.NewGuid(), "MBA em Gestão", 1000m);
            pedido.AdicionarItem(item);
            pedido.AtualizarStatusPago();

            var novoItem = new PedidoItem(Guid.NewGuid(), "MBA em TI", 1200m);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => pedido.AdicionarItem(novoItem));
        }

        [Fact(DisplayName = "Adicionar Item - Item Duplicado - Deve Lançar Exceção")]
        [Trait("Vendas", "Pedido - Domain")]
        public void AdicionarItem_ItemDuplicado_DeveLancarExcecao()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.CriarPedido(Guid.NewGuid());
            var cursoId = Guid.NewGuid();
            var item1 = new PedidoItem(cursoId, "MBA em Gestão", 1000m);
            pedido.AdicionarItem(item1);

            var item2 = new PedidoItem(cursoId, "MBA em Gestão", 1000m);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => pedido.AdicionarItem(item2));
        }

        #endregion

        #region 3. Remover Item

        [Fact(DisplayName = "Remover Item - Item Existente - Deve Remover com Sucesso")]
        [Trait("Vendas", "Pedido - Domain")]
        public void RemoverItem_ItemExistente_DeveRemoverComSucesso()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.CriarPedido(Guid.NewGuid());
            var item = new PedidoItem(Guid.NewGuid(), "MBA em Gestão", 1000m);
            pedido.AdicionarItem(item);

            // Act
            pedido.RemoverItem(item);

            // Assert
            pedido.PedidoItens.Should().BeEmpty();
            pedido.ValorTotal.Should().Be(0);
        }

        [Fact(DisplayName = "Remover Item - Pedido Pago - Deve Lançar Exceção")]
        [Trait("Vendas", "Pedido - Domain")]
        public void RemoverItem_PedidoPago_DeveLancarExcecao()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.CriarPedido(Guid.NewGuid());
            var item = new PedidoItem(Guid.NewGuid(), "MBA em Gestão", 1000m);
            pedido.AdicionarItem(item);
            pedido.AtualizarStatusPago();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => pedido.RemoverItem(item));
        }

        [Fact(DisplayName = "Remover Item Por Curso ID - Item Existente - Deve Remover com Sucesso")]
        [Trait("Vendas", "Pedido - Domain")]
        public void RemoverItemPorCursoId_ItemExistente_DeveRemoverComSucesso()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.CriarPedido(Guid.NewGuid());
            var cursoId = Guid.NewGuid();
            var item = new PedidoItem(cursoId, "MBA em Gestão", 1000m);
            pedido.AdicionarItem(item);

            // Act
            pedido.RemoverItemPorCursoId(cursoId);

            // Assert
            pedido.PedidoItens.Should().BeEmpty();
        }

        #endregion

        #region 4. Atualizar Status

        [Fact(DisplayName = "Atualizar Status - Para Iniciado - Deve Atualizar com Sucesso")]
        [Trait("Vendas", "Pedido - Domain")]
        public void AtualizarStatus_ParaIniciado_DeveAtualizarComSucesso()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.CriarPedido(Guid.NewGuid());
            var item = new PedidoItem(Guid.NewGuid(), "MBA em Gestão", 1000m);
            pedido.AdicionarItem(item);

            // Act
            pedido.AtualizarStatusIniciado();

            // Assert
            pedido.PedidoStatus.Should().Be(PedidoStatusEnum.Iniciado);
        }

        [Fact(DisplayName = "Atualizar Status - Iniciado Sem Itens - Deve Lançar Exceção")]
        [Trait("Vendas", "Pedido - Domain")]
        public void AtualizarStatus_IniciadoSemItens_DeveLancarExcecao()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.CriarPedido(Guid.NewGuid());

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => pedido.AtualizarStatusIniciado());
        }

        [Fact(DisplayName = "Atualizar Status - Para Pago - Deve Atualizar com Sucesso")]
        [Trait("Vendas", "Pedido - Domain")]
        public void AtualizarStatus_ParaPago_DeveAtualizarComSucesso()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.CriarPedido(Guid.NewGuid());
            var item = new PedidoItem(Guid.NewGuid(), "MBA em Gestão", 1000m);
            pedido.AdicionarItem(item);

            // Act
            pedido.AtualizarStatusPago();

            // Assert
            pedido.PedidoStatus.Should().Be(PedidoStatusEnum.Pago);
        }

        [Fact(DisplayName = "Atualizar Status - Pago Sem Itens - Deve Lançar Exceção")]
        [Trait("Vendas", "Pedido - Domain")]
        public void AtualizarStatus_PagoSemItens_DeveLancarExcecao()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.CriarPedido(Guid.NewGuid());

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => pedido.AtualizarStatusPago());
        }

        [Fact(DisplayName = "Atualizar Status - Para Cancelado - Deve Atualizar com Sucesso")]
        [Trait("Vendas", "Pedido - Domain")]
        public void AtualizarStatus_ParaCancelado_DeveAtualizarComSucesso()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.CriarPedido(Guid.NewGuid());

            // Act
            pedido.AtualizarStatusCancelado();

            // Assert
            pedido.PedidoStatus.Should().Be(PedidoStatusEnum.Cancelado);
        }

        [Fact(DisplayName = "Atualizar Status - Cancelar Pedido Pago - Deve Lançar Exceção")]
        [Trait("Vendas", "Pedido - Domain")]
        public void AtualizarStatus_CancelarPedidoPago_DeveLancarExcecao()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.CriarPedido(Guid.NewGuid());
            var item = new PedidoItem(Guid.NewGuid(), "MBA em Gestão", 1000m);
            pedido.AdicionarItem(item);
            pedido.AtualizarStatusPago();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => pedido.AtualizarStatusCancelado());
        }

        #endregion

        #region 5. Calcular Valor

        [Fact(DisplayName = "Calcular Valor - Múltiplos Itens - Deve Calcular Corretamente")]
        [Trait("Vendas", "Pedido - Domain")]
        public void CalcularValor_MultiplosItens_DeveCalcularCorretamente()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.CriarPedido(Guid.NewGuid());
            var item1 = new PedidoItem(Guid.NewGuid(), "MBA em Gestão", 1000m);
            var item2 = new PedidoItem(Guid.NewGuid(), "MBA em TI", 1200m);
            var item3 = new PedidoItem(Guid.NewGuid(), "MBA em Marketing", 900m);

            // Act
            pedido.AdicionarItem(item1);
            pedido.AdicionarItem(item2);
            pedido.AdicionarItem(item3);

            // Assert
            pedido.ValorTotal.Should().Be(3100m);
        }

        #endregion

        #region 6. Métodos Auxiliares

        [Fact(DisplayName = "Esta Pago - Pedido Pago - Deve Retornar True")]
        [Trait("Vendas", "Pedido - Domain")]
        public void EstaPago_PedidoPago_DeveRetornarTrue()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.CriarPedido(Guid.NewGuid());
            var item = new PedidoItem(Guid.NewGuid(), "MBA em Gestão", 1000m);
            pedido.AdicionarItem(item);
            pedido.AtualizarStatusPago();

            // Act
            var resultado = pedido.EstaPago();

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact(DisplayName = "Esta Cancelado - Pedido Cancelado - Deve Retornar True")]
        [Trait("Vendas", "Pedido - Domain")]
        public void EstaCancelado_PedidoCancelado_DeveRetornarTrue()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.CriarPedido(Guid.NewGuid());
            pedido.AtualizarStatusCancelado();

            // Act
            var resultado = pedido.EstaCancelado();

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact(DisplayName = "Obter Quantidade Itens - Múltiplos Itens - Deve Retornar Quantidade Correta")]
        [Trait("Vendas", "Pedido - Domain")]
        public void ObterQuantidadeItens_MultiplosItens_DeveRetornarQuantidadeCorreta()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.CriarPedido(Guid.NewGuid());
            pedido.AdicionarItem(new PedidoItem(Guid.NewGuid(), "MBA 1", 1000m));
            pedido.AdicionarItem(new PedidoItem(Guid.NewGuid(), "MBA 2", 1200m));
            pedido.AdicionarItem(new PedidoItem(Guid.NewGuid(), "MBA 3", 900m));

            // Act
            var quantidade = pedido.ObterQuantidadeItens();

            // Assert
            quantidade.Should().Be(3);
        }

        [Fact(DisplayName = "Possui Itens - Com Itens - Deve Retornar True")]
        [Trait("Vendas", "Pedido - Domain")]
        public void PossuiItens_ComItens_DeveRetornarTrue()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.CriarPedido(Guid.NewGuid());
            pedido.AdicionarItem(new PedidoItem(Guid.NewGuid(), "MBA em Gestão", 1000m));

            // Act
            var resultado = pedido.PossuiItens();

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact(DisplayName = "Obter Valor Médio - Múltiplos Itens - Deve Calcular Corretamente")]
        [Trait("Vendas", "Pedido - Domain")]
        public void ObterValorMedio_MultiplosItens_DeveCalcularCorretamente()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.CriarPedido(Guid.NewGuid());
            pedido.AdicionarItem(new PedidoItem(Guid.NewGuid(), "MBA 1", 1000m));
            pedido.AdicionarItem(new PedidoItem(Guid.NewGuid(), "MBA 2", 2000m));

            // Act
            var valorMedio = pedido.ObterValorMedio();

            // Assert
            valorMedio.Should().Be(1500m);
        }

        #endregion

        #region 7. Definir Código e Data

        [Fact(DisplayName = "Definir Código - Código Válido - Deve Definir com Sucesso")]
        [Trait("Vendas", "Pedido - Domain")]
        public void DefinirCodigo_CodigoValido_DeveDefinirComSucesso()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.CriarPedido(Guid.NewGuid());
            var codigo = 12345;

            // Act
            pedido.DefinirCodigo(codigo);

            // Assert
            pedido.Codigo.Should().Be(codigo);
        }

        [Fact(DisplayName = "Definir Código - Código Inválido - Deve Lançar Exceção")]
        [Trait("Vendas", "Pedido - Domain")]
        public void DefinirCodigo_CodigoInvalido_DeveLancarExcecao()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.CriarPedido(Guid.NewGuid());
            var codigo = 0;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => pedido.DefinirCodigo(codigo));
        }

        [Fact(DisplayName = "Definir Código - Código Já Definido - Deve Lançar Exceção")]
        [Trait("Vendas", "Pedido - Domain")]
        public void DefinirCodigo_CodigoJaDefinido_DeveLancarExcecao()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.CriarPedido(Guid.NewGuid());
            pedido.DefinirCodigo(12345);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => pedido.DefinirCodigo(67890));
        }

        #endregion
    }
}

