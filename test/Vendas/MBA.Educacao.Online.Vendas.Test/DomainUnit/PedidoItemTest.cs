using FluentAssertions;
using MBA.Educacao.Online.Vendas.Domain.Entities;

namespace MBA.Educacao.Online.Vendas.Test.DomainUnit
{
    public class PedidoItemTest
    {
        #region 1. Criar Pedido Item

        [Fact(DisplayName = "Criar Pedido Item - Dados Válidos - Deve Criar com Sucesso")]
        [Trait("Vendas", "PedidoItem - Domain")]
        public void CriarPedidoItem_DadosValidos_DeveCriarComSucesso()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var cursoNome = "MBA em Gestão de Projetos";
            var valor = 1000m;

            // Act
            var pedidoItem = new PedidoItem(cursoId, cursoNome, valor);

            // Assert
            pedidoItem.Should().NotBeNull();
            pedidoItem.CursoId.Should().Be(cursoId);
            pedidoItem.CursoNome.Should().Be(cursoNome);
            pedidoItem.Valor.Should().Be(valor);
            pedidoItem.MatriculaId.Should().BeNull();
        }

        [Fact(DisplayName = "Criar Pedido Item - Curso ID Vazio - Deve Lançar Exceção")]
        [Trait("Vendas", "PedidoItem - Domain")]
        public void CriarPedidoItem_CursoIdVazio_DeveLancarExcecao()
        {
            // Arrange
            var cursoId = Guid.Empty;
            var cursoNome = "MBA em Gestão de Projetos";
            var valor = 1000m;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new PedidoItem(cursoId, cursoNome, valor));
        }

        [Theory(DisplayName = "Criar Pedido Item - Nome Inválido - Deve Lançar Exceção")]
        [Trait("Vendas", "PedidoItem - Domain")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("AB")]
        public void CriarPedidoItem_NomeInvalido_DeveLancarExcecao(string cursoNome)
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var valor = 1000m;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new PedidoItem(cursoId, cursoNome, valor));
        }

        [Theory(DisplayName = "Criar Pedido Item - Valor Inválido - Deve Lançar Exceção")]
        [Trait("Vendas", "PedidoItem - Domain")]
        [InlineData(0)]
        [InlineData(-100)]
        [InlineData(1000000)]
        public void CriarPedidoItem_ValorInvalido_DeveLancarExcecao(decimal valor)
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var cursoNome = "MBA em Gestão";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new PedidoItem(cursoId, cursoNome, valor));
        }

        #endregion

        #region 2. Atribuir Matrícula

        [Fact(DisplayName = "Atribuir Matrícula - Matricula ID Válido - Deve Atribuir com Sucesso")]
        [Trait("Vendas", "PedidoItem - Domain")]
        public void AtribuirMatricula_MatriculaIdValido_DeveAtribuirComSucesso()
        {
            // Arrange
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "MBA em Gestão", 1000m);
            var matriculaId = Guid.NewGuid();

            // Act
            pedidoItem.AtribuirMatricula(matriculaId);

            // Assert
            pedidoItem.MatriculaId.Should().Be(matriculaId);
        }

        [Fact(DisplayName = "Atribuir Matrícula - Matricula ID Vazio - Deve Lançar Exceção")]
        [Trait("Vendas", "PedidoItem - Domain")]
        public void AtribuirMatricula_MatriculaIdVazio_DeveLancarExcecao()
        {
            // Arrange
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "MBA em Gestão", 1000m);
            var matriculaId = Guid.Empty;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => pedidoItem.AtribuirMatricula(matriculaId));
        }

        [Fact(DisplayName = "Atribuir Matrícula - Matrícula Já Atribuída - Deve Lançar Exceção")]
        [Trait("Vendas", "PedidoItem - Domain")]
        public void AtribuirMatricula_MatriculaJaAtribuida_DeveLancarExcecao()
        {
            // Arrange
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "MBA em Gestão", 1000m);
            pedidoItem.AtribuirMatricula(Guid.NewGuid());

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => pedidoItem.AtribuirMatricula(Guid.NewGuid()));
        }

        #endregion

        #region 3. Associar Pedido

        [Fact(DisplayName = "Associar Pedido - Pedido ID Válido - Deve Associar com Sucesso")]
        [Trait("Vendas", "PedidoItem - Domain")]
        public void AssociarPedido_PedidoIdValido_DeveAssociarComSucesso()
        {
            // Arrange
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "MBA em Gestão", 1000m);
            var pedidoId = Guid.NewGuid();

            // Act
            pedidoItem.AssociarPedido(pedidoId);

            // Assert
            pedidoItem.PedidoId.Should().Be(pedidoId);
        }

        [Fact(DisplayName = "Associar Pedido - Pedido ID Vazio - Deve Lançar Exceção")]
        [Trait("Vendas", "PedidoItem - Domain")]
        public void AssociarPedido_PedidoIdVazio_DeveLancarExcecao()
        {
            // Arrange
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "MBA em Gestão", 1000m);
            var pedidoId = Guid.Empty;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => pedidoItem.AssociarPedido(pedidoId));
        }

        #endregion

        #region 4. Possui Matrícula

        [Fact(DisplayName = "Possui Matrícula - Com Matrícula - Deve Retornar True")]
        [Trait("Vendas", "PedidoItem - Domain")]
        public void PossuiMatricula_ComMatricula_DeveRetornarTrue()
        {
            // Arrange
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "MBA em Gestão", 1000m);
            pedidoItem.AtribuirMatricula(Guid.NewGuid());

            // Act
            var resultado = pedidoItem.PossuiMatricula();

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact(DisplayName = "Possui Matrícula - Sem Matrícula - Deve Retornar False")]
        [Trait("Vendas", "PedidoItem - Domain")]
        public void PossuiMatricula_SemMatricula_DeveRetornarFalse()
        {
            // Arrange
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "MBA em Gestão", 1000m);

            // Act
            var resultado = pedidoItem.PossuiMatricula();

            // Assert
            resultado.Should().BeFalse();
        }

        #endregion

        #region 5. IsValid

        [Fact(DisplayName = "IsValid - Item Válido - Deve Retornar True")]
        [Trait("Vendas", "PedidoItem - Domain")]
        public void IsValid_ItemValido_DeveRetornarTrue()
        {
            // Arrange
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "MBA em Gestão", 1000m);

            // Act
            var resultado = pedidoItem.IsValid();

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact(DisplayName = "IsValid - Item Com Curso ID Vazio - Deve Retornar False")]
        [Trait("Vendas", "PedidoItem - Domain")]
        public void IsValid_ItemComCursoIdVazio_DeveRetornarFalse()
        {
            // Arrange
            var pedidoItem = new PedidoItem();
            typeof(PedidoItem).GetProperty("CursoId")!.SetValue(pedidoItem, Guid.Empty);
            typeof(PedidoItem).GetProperty("CursoNome")!.SetValue(pedidoItem, "MBA em Gestão");
            typeof(PedidoItem).GetProperty("Valor")!.SetValue(pedidoItem, 1000m);

            // Act
            var resultado = pedidoItem.IsValid();

            // Assert
            resultado.Should().BeFalse();
        }

        #endregion
    }
}

