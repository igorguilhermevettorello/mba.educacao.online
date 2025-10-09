using MBA.Educacao.Online.Cursos.Domain.Entities;

namespace MBA.Educacao.Online.Cursos.Test.Unit.Domain
{
    public class AulaTest
    {
        #region 1. Criar Aula

        [Fact(DisplayName = "Criar Entidade Aula - Dados Válidos - Deve Criar Aula com Sucesso")]
        [Trait("Aula", "Criação")]
        public void CriarEntidadeAula_DadosValidos_DeveCriarAulaComSucesso()
        {
            // Arrange
            var titulo = "Introdução ao Clean Architecture";
            var descricao = "Aula introdutória sobre os conceitos de Clean Architecture";
            var duracaoMinutos = 60;
            var ordem = 1;

            // Act
            var aula = new Aula(titulo, descricao, duracaoMinutos, ordem);

            // Assert
            Assert.NotNull(aula);
            Assert.Equal(titulo, aula.Titulo);
            Assert.Equal(descricao, aula.Descricao);
            Assert.Equal(duracaoMinutos, aula.DuracaoMinutos);
            Assert.Equal(ordem, aula.Ordem);
            Assert.True(aula.Ativa);
            Assert.NotEqual(Guid.Empty, aula.Id);
            Assert.True(aula.DataCriacao <= DateTime.UtcNow);
        }

        [Theory(DisplayName = "Criar Entidade Aula - Dados Inválidos - Deve Lançar Exceção")]
        [Trait("Aula", "Criação")]
        [InlineData("", "Descrição válida", 60, 1)]
        [InlineData(null, "Descrição válida", 60, 1)]
        [InlineData("Título válido", "", 60, 1)]
        [InlineData("Título válido", null, 60, 1)]
        [InlineData("Título válido", "Descrição válida", 0, 1)]
        [InlineData("Título válido", "Descrição válida", -1, 1)]
        [InlineData("Título válido", "Descrição válida", 60, 0)]
        [InlineData("Título válido", "Descrição válida", 60, -1)]
        [InlineData("", "", 0, 0)]
        [InlineData(null, null, -1, -1)]
        public void CriarEntidadeAula_DadosInvalidos_DeveLancarExcecao(string titulo, string descricao, int duracaoMinutos, int ordem)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Aula(titulo, descricao, duracaoMinutos, ordem));
        }

        #endregion
    
    }
}

