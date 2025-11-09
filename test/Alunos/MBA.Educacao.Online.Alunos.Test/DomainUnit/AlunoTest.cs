using FluentAssertions;
using MBA.Educacao.Online.Alunos.Domain.Entities;


namespace MBA.Educacao.Online.Alunos.Test.DomainUnit
{
    public class AlunoTest
    {
        #region 1. Criar Aluno

        [Fact(DisplayName = "Criar Aluno - Dados Válidos - Deve Criar com Sucesso")]
        [Trait("Alunos", "Aluno - Domain")]
        public void CriarAluno_DadosValidos_DeveCriarComSucesso()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var nome = "João Silva";
            var email = "joao.silva@teste.com";

            // Act
            var aluno = new Aluno(usuarioId, nome, email);

            // Assert
            aluno.Should().NotBeNull();
            aluno.Id.Should().Be(usuarioId);
            aluno.Nome.Should().Be(nome);
            aluno.Email.Should().Be(email);
            aluno.Ativo.Should().BeTrue();
            aluno.DataCadastro.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            aluno.Matriculas.Should().BeEmpty();
            aluno.Certificados.Should().BeEmpty();
        }

        [Theory(DisplayName = "Criar Aluno - Dados Inválidos - Deve Lançar Exceção")]
        [Trait("Alunos", "Aluno - Domain")]
        [InlineData("", "joao@teste.com")]
        [InlineData("Jo", "joao@teste.com")]
        [InlineData("João Silva", "")]
        [InlineData("João Silva", "emailinvalido")]
        public void CriarAluno_DadosInvalidos_DeveLancarExcecao(string nome, string email)
        {
            // Arrange
            var usuarioId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Aluno(usuarioId, nome, email));
        }

        [Fact(DisplayName = "Criar Aluno - Usuario ID Vazio - Deve Lançar Exceção")]
        [Trait("Alunos", "Aluno - Domain")]
        public void CriarAluno_UsuarioIdVazio_DeveLancarExcecao()
        {
            // Arrange
            var usuarioId = Guid.Empty;
            var nome = "João Silva";
            var email = "joao@teste.com";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Aluno(usuarioId, nome, email));
        }

        #endregion

        #region 2. Ativar e Inativar

        [Fact(DisplayName = "Inativar Aluno - Aluno Ativo - Deve Inativar com Sucesso")]
        [Trait("Alunos", "Aluno - Domain")]
        public void InativarAluno_AlunoAtivo_DeveInativarComSucesso()
        {
            // Arrange
            var aluno = new Aluno(Guid.NewGuid(), "João Silva", "joao@teste.com");

            // Act
            aluno.Inativar();

            // Assert
            aluno.Ativo.Should().BeFalse();
        }

        [Fact(DisplayName = "Ativar Aluno - Aluno Inativo - Deve Ativar com Sucesso")]
        [Trait("Alunos", "Aluno - Domain")]
        public void AtivarAluno_AlunoInativo_DeveAtivarComSucesso()
        {
            // Arrange
            var aluno = new Aluno(Guid.NewGuid(), "João Silva", "joao@teste.com");
            aluno.Inativar();

            // Act
            aluno.Ativar();

            // Assert
            aluno.Ativo.Should().BeTrue();
        }

        #endregion

        #region 3. Atualizar Nome e Email

        [Fact(DisplayName = "Atualizar Nome - Nome Válido - Deve Atualizar com Sucesso")]
        [Trait("Alunos", "Aluno - Domain")]
        public void AtualizarNome_NomeValido_DeveAtualizarComSucesso()
        {
            // Arrange
            var aluno = new Aluno(Guid.NewGuid(), "João Silva", "joao@teste.com");
            var novoNome = "João Pedro Silva";

            // Act
            aluno.AtualizarNome(novoNome);

            // Assert
            aluno.Nome.Should().Be(novoNome);
        }

        [Fact(DisplayName = "Atualizar Email - Email Válido - Deve Atualizar com Sucesso")]
        [Trait("Alunos", "Aluno - Domain")]
        public void AtualizarEmail_EmailValido_DeveAtualizarComSucesso()
        {
            // Arrange
            var aluno = new Aluno(Guid.NewGuid(), "João Silva", "joao@teste.com");
            var novoEmail = "joao.pedro@teste.com";

            // Act
            aluno.AtualizarEmail(novoEmail);

            // Assert
            aluno.Email.Should().Be(novoEmail);
        }

        [Theory(DisplayName = "Atualizar Nome - Nome Inválido - Deve Lançar Exceção")]
        [Trait("Alunos", "Aluno - Domain")]
        [InlineData("")]
        [InlineData("Jo")]
        public void AtualizarNome_NomeInvalido_DeveLancarExcecao(string nome)
        {
            // Arrange
            var aluno = new Aluno(Guid.NewGuid(), "João Silva", "joao@teste.com");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => aluno.AtualizarNome(nome));
        }

        #endregion

        #region 4. Adicionar Matrícula

        [Fact(DisplayName = "Adicionar Matrícula - Curso Não Matriculado - Deve Adicionar com Sucesso")]
        [Trait("Alunos", "Aluno - Domain")]
        public void AdicionarMatricula_CursoNaoMatriculado_DeveAdicionarComSucesso()
        {
            // Arrange
            var aluno = new Aluno(Guid.NewGuid(), "João Silva", "joao@teste.com");
            var cursoId = Guid.NewGuid();
            var dataValidade = DateTime.Now.AddMonths(12);

            // Act
            aluno.AdicionarMatricula(cursoId, dataValidade);

            // Assert
            aluno.Matriculas.Should().HaveCount(1);
            aluno.Matriculas.First().CursoId.Should().Be(cursoId);
        }

        [Fact(DisplayName = "Adicionar Matrícula - Curso Já Matriculado - Deve Lançar Exceção")]
        [Trait("Alunos", "Aluno - Domain")]
        public void AdicionarMatricula_CursoJaMatriculado_DeveLancarExcecao()
        {
            // Arrange
            var aluno = new Aluno(Guid.NewGuid(), "João Silva", "joao@teste.com");
            var cursoId = Guid.NewGuid();
            var dataValidade = DateTime.Now.AddMonths(12);
            aluno.AdicionarMatricula(cursoId, dataValidade);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => aluno.AdicionarMatricula(cursoId, dataValidade));
        }

        #endregion

        #region 5. Cancelar Matrícula

        [Fact(DisplayName = "Cancelar Matrícula - Matrícula Existente - Deve Cancelar com Sucesso")]
        [Trait("Alunos", "Aluno - Domain")]
        public void CancelarMatricula_MatriculaExistente_DeveCancelarComSucesso()
        {
            // Arrange
            var aluno = new Aluno(Guid.NewGuid(), "João Silva", "joao@teste.com");
            var cursoId = Guid.NewGuid();
            var dataValidade = DateTime.Now.AddMonths(12);
            aluno.AdicionarMatricula(cursoId, dataValidade);

            // Act
            aluno.CancelarMatricula(cursoId);

            // Assert
            aluno.Matriculas.First().Ativo.Should().BeFalse();
        }

        [Fact(DisplayName = "Cancelar Matrícula - Matrícula Não Existente - Deve Lançar Exceção")]
        [Trait("Alunos", "Aluno - Domain")]
        public void CancelarMatricula_MatriculaNaoExistente_DeveLancarExcecao()
        {
            // Arrange
            var aluno = new Aluno(Guid.NewGuid(), "João Silva", "joao@teste.com");
            var cursoId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => aluno.CancelarMatricula(cursoId));
        }

        #endregion

        #region 6. Esta Matriculado No Curso

        [Fact(DisplayName = "Esta Matriculado - Curso Com Matrícula Ativa - Deve Retornar True")]
        [Trait("Alunos", "Aluno - Domain")]
        public void EstaMatriculado_CursoComMatriculaAtiva_DeveRetornarTrue()
        {
            // Arrange
            var aluno = new Aluno(Guid.NewGuid(), "João Silva", "joao@teste.com");
            var cursoId = Guid.NewGuid();
            var dataValidade = DateTime.Now.AddMonths(12);
            aluno.AdicionarMatricula(cursoId, dataValidade);

            // Act
            var resultado = aluno.EstaMatriculadoNoCurso(cursoId);

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact(DisplayName = "Esta Matriculado - Curso Sem Matrícula - Deve Retornar False")]
        [Trait("Alunos", "Aluno - Domain")]
        public void EstaMatriculado_CursoSemMatricula_DeveRetornarFalse()
        {
            // Arrange
            var aluno = new Aluno(Guid.NewGuid(), "João Silva", "joao@teste.com");
            var cursoId = Guid.NewGuid();

            // Act
            var resultado = aluno.EstaMatriculadoNoCurso(cursoId);

            // Assert
            resultado.Should().BeFalse();
        }

        #endregion

        #region 7. Adicionar Certificado

        [Fact(DisplayName = "Adicionar Certificado - Aluno Matriculado - Deve Adicionar com Sucesso")]
        [Trait("Alunos", "Aluno - Domain")]
        public void AdicionarCertificado_AlunoMatriculado_DeveAdicionarComSucesso()
        {
            // Arrange
            var aluno = new Aluno(Guid.NewGuid(), "João Silva", "joao@teste.com");
            var cursoId = Guid.NewGuid();
            var dataValidade = DateTime.Now.AddMonths(12);
            aluno.AdicionarMatricula(cursoId, dataValidade);

            // Act
            aluno.AdicionarCertificado(cursoId, "CERT-2024-001");

            // Assert
            aluno.Certificados.Should().HaveCount(1);
        }

        [Fact(DisplayName = "Adicionar Certificado - Aluno Não Matriculado - Deve Lançar Exceção")]
        [Trait("Alunos", "Aluno - Domain")]
        public void AdicionarCertificado_AlunoNaoMatriculado_DeveLancarExcecao()
        {
            // Arrange
            var aluno = new Aluno(Guid.NewGuid(), "João Silva", "joao@teste.com");
            var cursoId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => aluno.AdicionarCertificado(cursoId, "CERT-2024-001"));
        }

        #endregion

        #region 8. Iniciar Aprendizado

        [Fact(DisplayName = "Iniciar Aprendizado - Aluno Matriculado - Deve Iniciar com Sucesso")]
        [Trait("Alunos", "Aluno - Domain")]
        public void IniciarAprendizado_AlunoMatriculado_DeveIniciarComSucesso()
        {
            // Arrange
            var aluno = new Aluno(Guid.NewGuid(), "João Silva", "joao@teste.com");
            var cursoId = Guid.NewGuid();
            var aulaId = Guid.NewGuid();
            var dataValidade = DateTime.Now.AddMonths(12);
            aluno.AdicionarMatricula(cursoId, dataValidade);

            // Act
            aluno.IniciarAprendizado(cursoId, aulaId);

            // Assert
            var matricula = aluno.Matriculas.First();
            matricula.HistoricosAprendizado.Should().HaveCount(1);
        }

        [Fact(DisplayName = "Iniciar Aprendizado - Aluno Não Matriculado - Deve Lançar Exceção")]
        [Trait("Alunos", "Aluno - Domain")]
        public void IniciarAprendizado_AlunoNaoMatriculado_DeveLancarExcecao()
        {
            // Arrange
            var aluno = new Aluno(Guid.NewGuid(), "João Silva", "joao@teste.com");
            var cursoId = Guid.NewGuid();
            var aulaId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => aluno.IniciarAprendizado(cursoId, aulaId));
        }

        #endregion

        #region 9. Obter Progresso No Curso

        [Fact(DisplayName = "Obter Progresso - Aluno Matriculado Com Progresso - Deve Retornar Progresso")]
        [Trait("Alunos", "Aluno - Domain")]
        public void ObterProgresso_AlunoMatriculadoComProgresso_DeveRetornarProgresso()
        {
            // Arrange
            var aluno = new Aluno(Guid.NewGuid(), "João Silva", "joao@teste.com");
            var cursoId = Guid.NewGuid();
            var dataValidade = DateTime.Now.AddMonths(12);
            aluno.AdicionarMatricula(cursoId, dataValidade);

            // Act
            var progresso = aluno.ObterProgressoNoCurso(cursoId);

            // Assert
            progresso.Should().BeGreaterOrEqualTo(0);
        }

        [Fact(DisplayName = "Obter Progresso - Aluno Não Matriculado - Deve Retornar Zero")]
        [Trait("Alunos", "Aluno - Domain")]
        public void ObterProgresso_AlunoNaoMatriculado_DeveRetornarZero()
        {
            // Arrange
            var aluno = new Aluno(Guid.NewGuid(), "João Silva", "joao@teste.com");
            var cursoId = Guid.NewGuid();

            // Act
            var progresso = aluno.ObterProgressoNoCurso(cursoId);

            // Assert
            progresso.Should().Be(0);
        }

        #endregion

        #region 10. Obter Total Matriculas e Certificados

        [Fact(DisplayName = "Obter Total Matriculas Ativas - Com Múltiplas Matrículas - Deve Retornar Total Correto")]
        [Trait("Alunos", "Aluno - Domain")]
        public void ObterTotalMatriculasAtivas_ComMultiplasMatriculas_DeveRetornarTotalCorreto()
        {
            // Arrange
            var aluno = new Aluno(Guid.NewGuid(), "João Silva", "joao@teste.com");
            var dataValidade = DateTime.Now.AddMonths(12);
            aluno.AdicionarMatricula(Guid.NewGuid(), dataValidade);
            aluno.AdicionarMatricula(Guid.NewGuid(), dataValidade);
            aluno.AdicionarMatricula(Guid.NewGuid(), dataValidade);

            // Act
            var total = aluno.ObterTotalMatriculasAtivas();

            // Assert
            total.Should().Be(3);
        }

        #endregion
    }
}

