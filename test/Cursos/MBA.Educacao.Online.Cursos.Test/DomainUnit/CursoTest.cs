using MBA.Educacao.Online.Cursos.Domain.Entities;
using MBA.Educacao.Online.Cursos.Domain.Enums;
using MBA.Educacao.Online.Cursos.Domain.ValueObjects;

namespace MBA.Educacao.Online.Cursos.Test.Unit.Domain
{
    public class CursoTest
    {
        #region 1. Criar Curso

        [Fact(DisplayName = "Criar Entidade Curso - Dados Válidos - Deve Criar Curso com Sucesso")]
        [Trait("Curso", "Criação")]
        public void CriarEntidadeCurso_DadosValidos_DeveCriarCursoComSucesso()
        {
            // Arrange
            var titulo = "MBA em Gestão de Projetos";
            var descricao = "Curso completo de MBA em Gestão de Projetos";
            var instrutor = "Eduardo Pires";
            var nivel = NivelCurso.Avancado;
            var valor = 10;

            // Act
            var curso = new Curso(titulo, descricao, instrutor, nivel, valor);

            // Assert
            Assert.NotNull(curso);
            Assert.Equal(titulo, curso.Titulo);
            Assert.Equal(descricao, curso.Descricao);
            Assert.Equal(instrutor, curso.Instrutor);
            Assert.Equal(nivel, curso.Nivel);
            Assert.True(curso.Ativo);
            Assert.NotEqual(Guid.Empty, curso.Id);
            Assert.True(curso.DataCriacao <= DateTime.UtcNow);
            Assert.Empty(curso.Aulas);
        }

        [Theory(DisplayName = "Criar Entidade Curso - Dados Inválidos - Deve Lançar Exceção")]
        [Trait("Curso", "Criação")]
        [InlineData("", "Descrição válida", "Eduardo Pires", NivelCurso.Basico, 100)]
        [InlineData(null, "Descrição válida", "Eduardo Pires",NivelCurso.Basico, 100)]
        [InlineData("Título válido", "", "Eduardo Pires",NivelCurso.Basico, 100)]
        [InlineData("Título válido", null, "Eduardo Pires",NivelCurso.Basico, 100)]
        [InlineData("", "", "Eduardo Pires",NivelCurso.Basico, 100)]
        [InlineData(null, null, "Eduardo Pires",NivelCurso.Basico, 100)]
        public void CriarCurso_DadosInvalidos_DeveLancarExcecao(string titulo, string descricao, string instrutor, NivelCurso status, decimal valor)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Curso(titulo, descricao, instrutor,status, valor));
        }
        #endregion

        #region 2. Adicionar Aula

        [Fact(DisplayName = "Criar Entidade Curso - Adicionar Aula - Deve Criar Aula dentro do Curso")]
        [Trait("Curso", "Aula")]
        public void CriarEntidadeCurso_AdicionarAula_DeveCriarAulaDentroDoCurso()
        {
            // Arrange
            var curso = new Curso("MBA em Gestão de Projetos", "Curso completo de MBA", "Eduardo Pires", NivelCurso.Avancado, 1000);
            var aula = new Aula("Introdução", "Aula introdutória do curso", 60, 1);

            // Act
            curso.AdicionarAula(aula);

            // Assert
            Assert.Single(curso.Aulas);
            Assert.Contains(aula, curso.Aulas);
        }

        #endregion

        #region 3. Adicionar Conteúdo Programático

        [Fact(DisplayName = "Criar Entidade Curso - Adicionar Conteúdo Programático - Deve Criar Conteúdo Programático dentro do Curso")]
        [Trait("Curso", "Conteúdo Programático")]
        public void CriarEntidadeCurso_AdicionarConteudoProgramatico_DeveCriarConteudoProgramaticoDentroDoCurso()
        {
            // Arrange
            var curso = new Curso("MBA em Gestão de Projetos", "Curso completo de MBA", "Eduardo Pires", NivelCurso.Avancado, 1000);
            var conteudoProgramatico = new ConteudoProgramatico(
                "Ementa do curso",
                "Objetivo do curso",
                "Bibliografia do curso",
                "https://material.url"
            );

            // Act
            curso.AdicionarConteudoProgramatico(conteudoProgramatico);

            // Assert
            Assert.NotNull(curso.ConteudoProgramatico);
            Assert.Equal("Ementa do curso", curso.ConteudoProgramatico.Ementa);
            Assert.Equal("Objetivo do curso", curso.ConteudoProgramatico.Objetivo);
            Assert.Equal("Bibliografia do curso", curso.ConteudoProgramatico.Bibliografia);
            Assert.Equal("https://material.url", curso.ConteudoProgramatico.MaterialUrl);
        }

        #endregion

        #region 4. Alterar Informações do Curso

        [Fact(DisplayName = "Criar Entidade Curso - Alterar Informações Dados Válidos - Deve Alterar um Curso com Sucesso")]
        [Trait("Curso", "Alteração")]
        public void CriarEntidadeCurso_AlterarInformacoesDadosValidos_DeveAlterarCursoComSucesso()
        {
            // Arrange
            var curso = new Curso("MBA em Gestão de Projetos", "Curso completo de MBA", "Eduardo Pires", NivelCurso.Avancado, 1000);
            var novoTitulo = "MBA em Gestão Empresarial";
            var novaDescricao = "Curso completo de MBA em Gestão Empresarial";
            var novoInstrutor = "João Silva";
            var novoNivel = NivelCurso.Intermediario;
            var novoValor = 1500m;

            // Act
            curso.AtualizarInformacoes(novoTitulo, novaDescricao, novoInstrutor, novoNivel, novoValor);

            // Assert
            Assert.Equal(novoTitulo, curso.Titulo);
            Assert.Equal(novaDescricao, curso.Descricao);
            Assert.Equal(novoInstrutor, curso.Instrutor);
            Assert.Equal(novoNivel, curso.Nivel);
            Assert.Equal(novoValor, curso.Valor);
        }

        [Theory(DisplayName = "Criar Entidade Curso - Alterar Informações Dados Inválidos - Deve Lançar Exceção")]
        [Trait("Curso", "Alteração")]
        [InlineData("", "Descrição válida", "Eduardo Pires", NivelCurso.Basico, 100)]
        [InlineData(null, "Descrição válida", "Eduardo Pires", NivelCurso.Basico, 100)]
        [InlineData("Título válido", "", "Eduardo Pires", NivelCurso.Basico, 100)]
        [InlineData("Título válido", null, "Eduardo Pires", NivelCurso.Basico, 100)]
        [InlineData("Título válido", "Descrição válida", "", NivelCurso.Basico, 100)]
        [InlineData("Título válido", "Descrição válida", null, NivelCurso.Basico, 100)]
        public void CriarEntidadeCurso_AlterarInformacoesDadosInvalidos_DeveLancarExcecao(string titulo, string descricao, string instrutor, NivelCurso nivel, decimal valor)
        {
            // Arrange
            var curso = new Curso("MBA em Gestão de Projetos", "Curso completo de MBA", "Eduardo Pires", NivelCurso.Avancado, 1000);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => curso.AtualizarInformacoes(titulo, descricao, instrutor, nivel, valor));
        }

        #endregion

        #region 5. Alterar Conteúdo Programático

        [Theory(DisplayName = "Criar Entidade Curso - Alterar Conteúdo Programático Dados Inválidos - Deve Lançar Exceção")]
        [Trait("Curso", "Conteúdo Programático")]
        [InlineData("", "Objetivo válido", "Bibliografia válida", "https://url.com")]
        [InlineData(null, "Objetivo válido", "Bibliografia válida", "https://url.com")]
        [InlineData("Ementa válida", "", "Bibliografia válida", "https://url.com")]
        [InlineData("Ementa válida", null, "Bibliografia válida", "https://url.com")]
        [InlineData("Ementa válida", "Objetivo válido", "", "https://url.com")]
        [InlineData("Ementa válida", "Objetivo válido", null, "https://url.com")]
        [InlineData("Ementa válida", "Objetivo válido", "Bibliografia válida", "")]
        [InlineData("Ementa válida", "Objetivo válido", "Bibliografia válida", null)]
        public void CriarEntidadeCurso_AlterarConteudoProgramaticoDadosInvalidos_DeveLancarExcecao(string ementa, string objetivo, string bibliografia, string materialUrl)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new ConteudoProgramatico(ementa, objetivo, bibliografia, materialUrl));
        }

        #endregion

        #region 6. Alterar Status da Aula

        [Fact(DisplayName = "Criar Entidade Curso - Alterar Aula Ativo - Deve Alterar Aula para Ativo")]
        [Trait("Curso", "Aula")]
        public void CriarEntidadeCurso_AlterarAulaAtivo_DeveAlterarAulaParaAtivo()
        {
            // Arrange
            var curso = new Curso("MBA em Gestão de Projetos", "Curso completo de MBA", "Eduardo Pires", NivelCurso.Avancado, 1000);
            var aula = new Aula("Introdução", "Aula introdutória do curso", 60, 1);
            curso.AdicionarAula(aula);
            aula.Inativar();

            // Act
            aula.Ativar();

            // Assert
            Assert.True(aula.Ativa);
        }

        [Fact(DisplayName = "Criar Entidade Curso - Alterar Aula Inativo - Deve Alterar Aula para Inativo")]
        [Trait("Curso", "Aula")]
        public void CriarEntidadeCurso_AlterarAulaInativo_DeveAlterarAulaParaInativo()
        {
            // Arrange
            var curso = new Curso("MBA em Gestão de Projetos", "Curso completo de MBA", "Eduardo Pires", NivelCurso.Avancado, 1000);
            var aula = new Aula("Introdução", "Aula introdutória do curso", 60, 1);
            curso.AdicionarAula(aula);

            // Act
            aula.Inativar();

            // Assert
            Assert.False(aula.Ativa);
        }

        #endregion

        #region 7. Verificar Aula

        [Fact(DisplayName = "Criar Entidade Curso - Verificar Aula - Deve retornar status True")]
        [Trait("Curso", "Aula")]
        public void CriarEntidadeCurso_VerificarAula_DeveRetornarStatusTrue()
        {
            // Arrange
            var curso = new Curso("MBA em Gestão de Projetos", "Curso completo de MBA", "Eduardo Pires", NivelCurso.Avancado, 1000);
            var aula = new Aula("Introdução", "Aula introdutória do curso", 60, 1);
            curso.AdicionarAula(aula);

            // Act
            var resultado = curso.VerificarSeAulaEstaCadastrada(aula.Id);

            // Assert
            Assert.True(resultado);
        }

        [Fact(DisplayName = "Criar Entidade Curso - Verificar Aula - Deve retornar status False")]
        [Trait("Curso", "Aula")]
        public void CriarEntidadeCurso_VerificarAula_DeveRetornarStatusFalse()
        {
            // Arrange
            var curso = new Curso("MBA em Gestão de Projetos", "Curso completo de MBA", "Eduardo Pires", NivelCurso.Avancado, 1000);
            var aulaId = Guid.NewGuid();

            // Act
            var resultado = curso.VerificarSeAulaEstaCadastrada(aulaId);

            // Assert
            Assert.False(resultado);
        }

        #endregion
    
    }
}

