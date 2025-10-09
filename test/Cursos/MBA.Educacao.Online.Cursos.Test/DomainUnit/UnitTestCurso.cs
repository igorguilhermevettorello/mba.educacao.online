using MBA.Educacao.Online.Cursos.Domain.Entities;
using MBA.Educacao.Online.Cursos.Domain.Enums;
using MBA.Educacao.Online.Cursos.Domain.ValueObjects;

namespace MBA.Educacao.Online.Cursos.Test.Unit.Domain
{
    public class UnitTestCurso
    {
        #region 1. Criar Curso

        [Fact(DisplayName = "Criar Curso - Dados Válidos - Deve Criar Curso com Sucesso")]
        [Trait("Categoria", "Curso - Criação")]
        public void CriarCurso_DadosValidos_DeveCriarComSucesso()
        {
            // Arrange
            var titulo = "MBA em Gestão de Projetos";
            var descricao = "Curso completo de MBA em Gestão de Projetos";
            var nivel = NivelCurso.Intermediario;

            // Act
            var curso = new Curso(titulo, descricao, nivel);

            // Assert
            Assert.NotNull(curso);
            Assert.Equal(titulo, curso.Titulo);
            Assert.Equal(descricao, curso.Descricao);
            Assert.Equal(nivel, curso.Nivel);
            Assert.True(curso.Ativo);
            Assert.NotEqual(Guid.Empty, curso.Id);
            Assert.True(curso.DataCriacao <= DateTime.UtcNow);
            Assert.Empty(curso.Aulas);
        }

        [Theory(DisplayName = "Criar Curso - Dados Inválidos - Deve Lançar Exceção")]
        [Trait("Categoria", "Curso - Criação")]
        [InlineData("", "Descrição válida", NivelCurso.Basico)]
        [InlineData(null, "Descrição válida", NivelCurso.Basico)]
        [InlineData("Título válido", "", NivelCurso.Basico)]
        [InlineData("Título válido", null, NivelCurso.Basico)]
        [InlineData("", "", NivelCurso.Basico)]
        [InlineData(null, null, NivelCurso.Basico)]
        public void CriarCurso_DadosInvalidos_DeveLancarExcecao(string titulo, string descricao, NivelCurso status)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Curso(titulo, descricao, status));
        }

        #endregion

        #region 2. Adicionar Aula ao Curso

        [Fact(DisplayName = "Adicionar Aula - Aula Válida - Deve Adicionar ao Curso")]
        [Trait("Categoria", "Curso - Aulas")]
        public void AdicionarAula_AulaValida_DeveAdicionarAoCurso()
        {
            // Arrange
            var curso = new Curso("MBA em Gestão", "Descrição do curso", NivelCurso.Basico);
            var aula = new Aula("Introdução ao MBA", "Primeira aula do curso", 60, 1);

            // Act
            curso.AdicionarAula(aula);

            // Assert
            Assert.Single(curso.Aulas);
            Assert.Contains(aula, curso.Aulas);
            Assert.Equal(aula.Id, curso.Aulas.First().Id);
        }

        [Fact(DisplayName = "Adicionar Aula - Múltiplas Aulas - Deve Adicionar Todas ao Curso")]
        [Trait("Categoria", "Curso - Aulas")]
        public void AdicionarAula_MultiplasAulas_DeveAdicionarTodasAoCurso()
        {
            // Arrange
            var curso = new Curso("MBA em Gestão", "Descrição do curso", NivelCurso.Intermediario);
            var aula1 = new Aula("Aula 1", "Primeira aula", 60, 1);
            var aula2 = new Aula("Aula 2", "Segunda aula", 90, 2);
            var aula3 = new Aula("Aula 3", "Terceira aula", 45, 3);

            // Act
            curso.AdicionarAula(aula1);
            curso.AdicionarAula(aula2);
            curso.AdicionarAula(aula3);

            // Assert
            Assert.Equal(3, curso.Aulas.Count);
            Assert.Contains(aula1, curso.Aulas);
            Assert.Contains(aula2, curso.Aulas);
            Assert.Contains(aula3, curso.Aulas);
        }

        [Fact(DisplayName = "Adicionar Aula - Aula Nula - Deve Lançar Exceção")]
        [Trait("Categoria", "Curso - Aulas")]
        public void AdicionarAula_AulaNula_DeveLancarExcecao()
        {
            // Arrange
            var curso = new Curso("MBA em Gestão", "Descrição do curso", NivelCurso.Basico);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => curso.AdicionarAula(null!));
        }

        [Fact(DisplayName = "Adicionar Aula - Curso Inativo - Deve Lançar Exceção")]
        [Trait("Categoria", "Curso - Aulas")]
        public void AdicionarAula_CursoInativo_DeveLancarExcecao()
        {
            // Arrange
            var curso = new Curso("MBA em Gestão", "Descrição do curso", NivelCurso.Basico);
            curso.Inativar();
            var aula = new Aula("Aula", "Descrição", 60, 1);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => curso.AdicionarAula(aula));
        }

        #endregion

        #region 3. Validar o Conteúdo Programático

        [Fact(DisplayName = "Criar Conteúdo Programático - Dados Válidos - Deve Criar com Sucesso")]
        [Trait("Categoria", "Curso - Conteúdo Programático")]
        public void CriarConteudoProgramatico_DadosValidos_DeveCriarComSucesso()
        {
            // Arrange
            var titulo = "Módulo 1: Fundamentos";
            var descricao = "Fundamentos da gestão de projetos";
            var ordem = 1;

            // Act
            var conteudo = new ConteudoProgramatico(titulo, descricao, ordem);

            // Assert
            Assert.NotNull(conteudo);
            Assert.Equal(titulo, conteudo.Titulo);
            Assert.Equal(descricao, conteudo.Descricao);
            Assert.Equal(ordem, conteudo.Ordem);
            Assert.True(conteudo.Ativo);
            Assert.True(conteudo.DataCriacao <= DateTime.UtcNow);
        }

        [Theory(DisplayName = "Criar Conteúdo Programático - Dados Inválidos - Deve Lançar Exceção")]
        [Trait("Categoria", "Curso - Conteúdo Programático")]
        [InlineData("", "Descrição válida", 1)]
        [InlineData(null, "Descrição válida", 1)]
        [InlineData("Título válido", "", 1)]
        [InlineData("Título válido", null, 1)]
        [InlineData("Título válido", "Descrição válida", 0)]
        [InlineData("Título válido", "Descrição válida", -1)]
        public void CriarConteudoProgramatico_DadosInvalidos_DeveLancarExcecao(string titulo, string descricao, int ordem)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new ConteudoProgramatico(titulo, descricao, ordem));
        }

        #endregion

        //#region 4. Adicionar Conteúdo Programático



        //[Fact(DisplayName = "Adicionar Conteúdo Programático - Múltiplos Conteúdos - Deve Adicionar Todos ao Curso")]
        //[Trait("Categoria", "Curso - Conteúdo Programático")]
        //public void AdicionarConteudoProgramatico_MultiplosConteudos_DeveAdicionarTodosAoCurso()
        //{
        //    // Arrange
        //    var curso = new Curso("MBA em Gestão", "Descrição do curso", NivelCurso.Basico);
        //    var conteudo1 = new ConteudoProgramatico("Módulo 1", "Fundamentos", 1);
        //    var conteudo2 = new ConteudoProgramatico("Módulo 2", "Avançado", 2);
        //    var conteudo3 = new ConteudoProgramatico("Módulo 3", "Especialização", 3);

        //    // Act
        //    curso.AdicionarConteudoProgramatico(conteudo1);
        //    curso.AdicionarConteudoProgramatico(conteudo2);
        //    curso.AdicionarConteudoProgramatico(conteudo3);

        //    // Assert
        //    Assert.Equal(3, curso.ConteudosProgramaticos.Count);
        //    Assert.Contains(conteudo1, curso.ConteudosProgramaticos);
        //    Assert.Contains(conteudo2, curso.ConteudosProgramaticos);
        //    Assert.Contains(conteudo3, curso.ConteudosProgramaticos);
        //}

        //[Fact(DisplayName = "Adicionar Conteúdo Programático - Conteúdo Nulo - Deve Lançar Exceção")]
        //[Trait("Categoria", "Curso - Conteúdo Programático")]
        //public void AdicionarConteudoProgramatico_ConteudoNulo_DeveLancarExcecao()
        //{
        //    // Arrange
        //    var curso = new Curso("MBA em Gestão", "Descrição do curso", NivelCurso.Basico);

        //    // Act & Assert
        //    Assert.Throws<ArgumentNullException>(() => curso.AdicionarConteudoProgramatico(null!));
        //}

        //[Fact(DisplayName = "Adicionar Conteúdo Programático - Curso Inativo - Deve Lançar Exceção")]
        //[Trait("Categoria", "Curso - Conteúdo Programático")]
        //public void AdicionarConteudoProgramatico_CursoInativo_DeveLancarExcecao()
        //{
        //    // Arrange
        //    var curso = new Curso("MBA em Gestão", "Descrição do curso", NivelCurso.Basico);
        //    curso.Inativar();
        //    var conteudo = new ConteudoProgramatico("Módulo", "Descrição", 1);

        //    // Act & Assert
        //    Assert.Throws<InvalidOperationException>(() => curso.AdicionarConteudoProgramatico(conteudo));
        //}

        //#endregion

        #region 5. Verificar se Aula está cadastrada

        [Fact(DisplayName = "Verificar Aula - Aula Cadastrada - Deve Retornar Verdadeiro")]
        [Trait("Categoria", "Curso - Aulas")]
        public void VerificarSeAulaEstaCadastrada_AulaCadastrada_DeveRetornarVerdadeiro()
        {
            // Arrange
            var curso = new Curso("MBA em Gestão", "Descrição do curso", NivelCurso.Basico);
            var aula = new Aula("Aula 1", "Descrição", 60, 1);
            curso.AdicionarAula(aula);

            // Act
            var aulaEstaCadastrada = curso.VerificarSeAulaEstaCadastrada(aula.Id);

            // Assert
            Assert.True(aulaEstaCadastrada);
        }

        [Fact(DisplayName = "Verificar Aula - Aula Não Cadastrada - Deve Retornar Falso")]
        [Trait("Categoria", "Curso - Aulas")]
        public void VerificarSeAulaEstaCadastrada_AulaNaoCadastrada_DeveRetornarFalso()
        {
            // Arrange
            var curso = new Curso("MBA em Gestão", "Descrição do curso", NivelCurso.Basico);
            var aulaIdInexistente = Guid.NewGuid();

            // Act
            var aulaEstaCadastrada = curso.VerificarSeAulaEstaCadastrada(aulaIdInexistente);

            // Assert
            Assert.False(aulaEstaCadastrada);
        }

        [Fact(DisplayName = "Verificar Aula - Curso Sem Aulas - Deve Retornar Falso")]
        [Trait("Categoria", "Curso - Aulas")]
        public void VerificarSeAulaEstaCadastrada_CursoSemAulas_DeveRetornarFalso()
        {
            // Arrange
            var curso = new Curso("MBA em Gestão", "Descrição do curso", NivelCurso.Basico);
            var aulaId = Guid.NewGuid();

            // Act
            var aulaEstaCadastrada = curso.VerificarSeAulaEstaCadastrada(aulaId);

            // Assert
            Assert.False(aulaEstaCadastrada);
        }

        [Fact(DisplayName = "Verificar Aula - Curso com Múltiplas Aulas - Deve Verificar Corretamente")]
        [Trait("Categoria", "Curso - Aulas")]
        public void VerificarSeAulaEstaCadastrada_CursoComMultiplasAulas_DeveVerificarCorretamente()
        {
            // Arrange
            var curso = new Curso("MBA em Gestão", "Descrição do curso", NivelCurso.Basico);
            var aula1 = new Aula("Aula 1", "Descrição 1", 60, 1);
            var aula2 = new Aula("Aula 2", "Descrição 2", 90, 2);
            var aula3 = new Aula("Aula 3", "Descrição 3", 45, 3);

            curso.AdicionarAula(aula1);
            curso.AdicionarAula(aula2);
            curso.AdicionarAula(aula3);

            // Act & Assert
            Assert.True(curso.VerificarSeAulaEstaCadastrada(aula1.Id));
            Assert.True(curso.VerificarSeAulaEstaCadastrada(aula2.Id));
            Assert.True(curso.VerificarSeAulaEstaCadastrada(aula3.Id));
            Assert.False(curso.VerificarSeAulaEstaCadastrada(Guid.NewGuid()));
        }

        #endregion

        #region Status do Curso

        [Fact(DisplayName = "Criar Curso - Nível Básico - Deve Criar com Nível Básico")]
        [Trait("Categoria", "Curso - Nível")]
        public void CriarCurso_NivelBasico_DeveCriarComNivelBasico()
        {
            // Arrange
            var titulo = "Curso Básico";
            var descricao = "Curso para iniciantes";
            var status = NivelCurso.Basico;

            // Act
            var curso = new Curso(titulo, descricao, status);

            // Assert
            Assert.Equal(NivelCurso.Basico, curso.Nivel);
        }

        [Fact(DisplayName = "Criar Curso - Nível Intermediário - Deve Criar com Nível Intermediário")]
        [Trait("Categoria", "Curso - Nível")]
        public void CriarCurso_NivelIntermediario_DeveCriarComNivelIntermediario()
        {
            // Arrange
            var titulo = "Curso Intermediário";
            var descricao = "Curso para quem já tem conhecimento básico";
            var status = NivelCurso.Intermediario;

            // Act
            var curso = new Curso(titulo, descricao, status);

            // Assert
            Assert.Equal(NivelCurso.Intermediario, curso.Nivel);
        }

        [Fact(DisplayName = "Criar Curso - Nível Avançado - Deve Criar com Nível Avançado")]
        [Trait("Categoria", "Curso - Nível")]
        public void CriarCurso_NivelAvancado_DeveCriarComNivelAvancado()
        {
            // Arrange
            var titulo = "Curso Avançado";
            var descricao = "Curso para especialistas";
            var status = NivelCurso.Avancado;

            // Act
            var curso = new Curso(titulo, descricao, status);

            // Assert
            Assert.Equal(NivelCurso.Avancado, curso.Nivel);
        }

        [Fact(DisplayName = "Atualizar Nível - Nível Válido - Deve Atualizar Nível do Curso")]
        [Trait("Categoria", "Curso - Nível")]
        public void AtualizarNivel_NivelValido_DeveAtualizarNivelDoCurso()
        {
            // Arrange
            var curso = new Curso("Curso de Teste", "Descrição", NivelCurso.Basico);
            var novoStatus = NivelCurso.Avancado;

            // Act
            curso.AtualizarNivel(novoStatus);

            // Assert
            Assert.Equal(NivelCurso.Avancado, curso.Nivel);
        }

        [Theory(DisplayName = "Atualizar Nível - Qualquer Nível Válido - Deve Atualizar Nível do Curso")]
        [Trait("Categoria", "Curso - Nível")]
        [InlineData(NivelCurso.Basico, NivelCurso.Intermediario)]
        [InlineData(NivelCurso.Intermediario, NivelCurso.Avancado)]
        [InlineData(NivelCurso.Avancado, NivelCurso.Basico)]
        public void AtualizarNivel_QualquerNivelValido_DeveAtualizarNivelDoCurso(NivelCurso statusInicial, NivelCurso novoStatus)
        {
            // Arrange
            var curso = new Curso("Curso de Teste", "Descrição", statusInicial);

            // Act
            curso.AtualizarNivel(novoStatus);

            // Assert
            Assert.Equal(novoStatus, curso.Nivel);
        }

        [Fact(DisplayName = "Atualizar Nível - Nível Inválido - Deve Lançar Exceção")]
        [Trait("Categoria", "Curso - Nível")]
        public void AtualizarNivel_NivelInvalido_DeveLancarExcecao()
        {
            // Arrange
            var curso = new Curso("Curso de Teste", "Descrição", NivelCurso.Basico);
            var statusInvalido = (NivelCurso)999; // Valor fora do enum

            // Act & Assert
            Assert.Throws<ArgumentException>(() => curso.AtualizarNivel(statusInvalido));
        }

        [Fact(DisplayName = "Criar Curso - Nível Inválido - Deve Lançar Exceção")]
        [Trait("Categoria", "Curso - Nível")]
        public void CriarCurso_NivelInvalido_DeveLancarExcecao()
        {
            // Arrange
            var titulo = "Curso de Teste";
            var descricao = "Descrição válida";
            var statusInvalido = (NivelCurso)999; // Valor fora do enum

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Curso(titulo, descricao, statusInvalido));
        }

        #endregion

        //#region Testes de Integração

        //[Fact(DisplayName = "Gerenciar Curso - Curso Completo - Deve Gerenciar Aulas e Conteúdos Programáticos")]
        //[Trait("Categoria", "Curso - Integração")]
        //public void GerenciarCurso_CursoCompleto_DeveGerenciarAulasEConteudosProgramaticos()
        //{
        //    // Arrange
        //    var curso = new Curso("MBA em Gestão de Projetos", "Curso completo de MBA", NivelCurso.Avancado);

        //    var aula1 = new Aula("Introdução", "Aula introdutória", 60, 1);
        //    var aula2 = new Aula("Metodologias", "Metodologias ágeis", 90, 2);

        //    var conteudo1 = new ConteudoProgramatico("Fundamentos", "Conceitos básicos", 1);
        //    var conteudo2 = new ConteudoProgramatico("Avançado", "Tópicos avançados", 2);

        //    // Act
        //    curso.AdicionarAula(aula1);
        //    curso.AdicionarAula(aula2);
        //    curso.AdicionarConteudoProgramatico(conteudo1);
        //    curso.AdicionarConteudoProgramatico(conteudo2);

        //    // Assert
        //    Assert.Equal(2, curso.Aulas.Count);
        //    Assert.Equal(2, curso.ConteudosProgramaticos.Count);
        //    Assert.True(curso.VerificarSeAulaEstaCadastrada(aula1.Id));
        //    Assert.True(curso.VerificarSeAulaEstaCadastrada(aula2.Id));
        //    Assert.Contains(aula1, curso.Aulas);
        //    Assert.Contains(aula2, curso.Aulas);
        //    Assert.Contains(conteudo1, curso.ConteudosProgramaticos);
        //    Assert.Contains(conteudo2, curso.ConteudosProgramaticos);
        //}

        //#endregion
    }
}

