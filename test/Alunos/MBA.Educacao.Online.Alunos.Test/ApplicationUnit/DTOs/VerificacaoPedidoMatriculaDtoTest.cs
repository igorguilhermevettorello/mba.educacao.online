using FluentAssertions;
using MBA.Educacao.Online.Alunos.Application.DTOs;

namespace MBA.Educacao.Online.Alunos.Test.ApplicationUnit.DTOs
{
    public class VerificacaoPedidoMatriculaDtoTest
    {
        #region 1. Criar DTO - Construtor Padrão

        [Fact(DisplayName = "Criar VerificacaoPedidoMatriculaDto - Construtor Padrão - Deve Inicializar Propriedades")]
        [Trait("Alunos", "DTOs - VerificacaoPedidoMatricula")]
        public void CriarVerificacaoPedidoMatriculaDto_ConstrutorPadrao_DeveInicializarPropriedades()
        {
            // Act
            var dto = new VerificacaoPedidoMatriculaDto();

            // Assert
            dto.Should().NotBeNull();
            dto.PodeProsseguir.Should().BeFalse();
            dto.MensagemErro.Should().BeEmpty();
            dto.CursosComMatriculaAtiva.Should().NotBeNull();
            dto.CursosComMatriculaAtiva.Should().BeEmpty();
        }

        #endregion

        #region 2. Factory Method - Sucesso

        [Fact(DisplayName = "Criar DTO - Factory Sucesso - Deve Criar com Status de Sucesso")]
        [Trait("Alunos", "DTOs - VerificacaoPedidoMatricula")]
        public void CriarDto_FactorySucesso_DeveCriarComStatusSucesso()
        {
            // Act
            var dto = VerificacaoPedidoMatriculaDto.Sucesso();

            // Assert
            dto.Should().NotBeNull();
            dto.PodeProsseguir.Should().BeTrue();
            dto.MensagemErro.Should().BeEmpty();
            dto.CursosComMatriculaAtiva.Should().BeEmpty();
        }

        #endregion

        #region 3. Factory Method - Falha

        [Fact(DisplayName = "Criar DTO - Factory Falha - Deve Criar com Mensagem de Erro")]
        [Trait("Alunos", "DTOs - VerificacaoPedidoMatricula")]
        public void CriarDto_FactoryFalha_DeveCriarComMensagemErro()
        {
            // Arrange
            var mensagem = "Aluno já possui matrícula ativa para este curso";
            var cursosComMatricula = new List<string> { "MBA em Gestão de Projetos" };

            // Act
            var dto = VerificacaoPedidoMatriculaDto.Falha(mensagem, cursosComMatricula);

            // Assert
            dto.Should().NotBeNull();
            dto.PodeProsseguir.Should().BeFalse();
            dto.MensagemErro.Should().Be(mensagem);
            dto.CursosComMatriculaAtiva.Should().HaveCount(1);
            dto.CursosComMatriculaAtiva.Should().Contain("MBA em Gestão de Projetos");
        }

        [Fact(DisplayName = "Criar DTO - Factory Falha Múltiplos Cursos - Deve Criar com Lista Completa")]
        [Trait("Alunos", "DTOs - VerificacaoPedidoMatricula")]
        public void CriarDto_FactoryFalhaMultiplosCursos_DeveCriarComListaCompleta()
        {
            // Arrange
            var mensagem = "Aluno já possui matrículas ativas";
            var cursosComMatricula = new List<string>
            {
                "MBA em Gestão de Projetos",
                "MBA em TI",
                "MBA em Marketing Digital"
            };

            // Act
            var dto = VerificacaoPedidoMatriculaDto.Falha(mensagem, cursosComMatricula);

            // Assert
            dto.Should().NotBeNull();
            dto.PodeProsseguir.Should().BeFalse();
            dto.MensagemErro.Should().Be(mensagem);
            dto.CursosComMatriculaAtiva.Should().HaveCount(3);
            dto.CursosComMatriculaAtiva.Should().Contain("MBA em Gestão de Projetos");
            dto.CursosComMatriculaAtiva.Should().Contain("MBA em TI");
            dto.CursosComMatriculaAtiva.Should().Contain("MBA em Marketing Digital");
        }

        [Fact(DisplayName = "Criar DTO - Factory Falha Lista Vazia - Deve Aceitar")]
        [Trait("Alunos", "DTOs - VerificacaoPedidoMatricula")]
        public void CriarDto_FactoryFalhaListaVazia_DeveAceitar()
        {
            // Arrange
            var mensagem = "Erro genérico";
            var cursosComMatricula = new List<string>();

            // Act
            var dto = VerificacaoPedidoMatriculaDto.Falha(mensagem, cursosComMatricula);

            // Assert
            dto.Should().NotBeNull();
            dto.PodeProsseguir.Should().BeFalse();
            dto.MensagemErro.Should().Be(mensagem);
            dto.CursosComMatriculaAtiva.Should().BeEmpty();
        }

        #endregion

        #region 4. Propriedades Mutáveis

        [Fact(DisplayName = "Modificar Propriedades - Deve Permitir Alteração")]
        [Trait("Alunos", "DTOs - VerificacaoPedidoMatricula")]
        public void ModificarPropriedades_DevePermitirAlteracao()
        {
            // Arrange
            var dto = new VerificacaoPedidoMatriculaDto();

            // Act
            dto.PodeProsseguir = true;
            dto.MensagemErro = "Nova mensagem";
            dto.CursosComMatriculaAtiva.Add("Curso 1");
            dto.CursosComMatriculaAtiva.Add("Curso 2");

            // Assert
            dto.PodeProsseguir.Should().BeTrue();
            dto.MensagemErro.Should().Be("Nova mensagem");
            dto.CursosComMatriculaAtiva.Should().HaveCount(2);
        }

        #endregion

        #region 5. Comparação de Instâncias

        [Fact(DisplayName = "Comparar Instâncias - Sucesso vs Falha - Devem Ser Diferentes")]
        [Trait("Alunos", "DTOs - VerificacaoPedidoMatricula")]
        public void CompararInstancias_SucessoVsFalha_DevemSerDiferentes()
        {
            // Arrange & Act
            var dtoSucesso = VerificacaoPedidoMatriculaDto.Sucesso();
            var dtoFalha = VerificacaoPedidoMatriculaDto.Falha("Erro", new List<string> { "Curso 1" });

            // Assert
            dtoSucesso.PodeProsseguir.Should().BeTrue();
            dtoFalha.PodeProsseguir.Should().BeFalse();

            dtoSucesso.MensagemErro.Should().BeEmpty();
            dtoFalha.MensagemErro.Should().NotBeEmpty();

            dtoSucesso.CursosComMatriculaAtiva.Should().BeEmpty();
            dtoFalha.CursosComMatriculaAtiva.Should().NotBeEmpty();
        }

        #endregion

        #region 6. Validação de Mensagens

        [Theory(DisplayName = "Criar DTO - Factory Falha Diferentes Mensagens - Deve Aceitar")]
        [Trait("Alunos", "DTOs - VerificacaoPedidoMatricula")]
        [InlineData("Aluno já possui matrícula ativa")]
        [InlineData("Curso não disponível")]
        [InlineData("Matrícula vencida")]
        [InlineData("")]
        public void CriarDto_FactoryFalhaDiferentesMensagens_DeveAceitar(string mensagem)
        {
            // Arrange & Act
            var dto = VerificacaoPedidoMatriculaDto.Falha(mensagem, new List<string>());

            // Assert
            dto.Should().NotBeNull();
            dto.PodeProsseguir.Should().BeFalse();
            dto.MensagemErro.Should().Be(mensagem);
        }

        #endregion

        #region 7. Cenário de Uso Real - API Response

        [Fact(DisplayName = "Cenário Real - Resposta API Sucesso - Deve Retornar DTO de Sucesso")]
        [Trait("Alunos", "DTOs - VerificacaoPedidoMatricula - Integração")]
        public void CenarioReal_RespostaApiSucesso_DeveRetornarDtoSucesso()
        {
            // Simula retorno de API quando verificação passa
            // Arrange & Act
            var verificacao = VerificacaoPedidoMatriculaDto.Sucesso();

            // Assert - Pode prosseguir com o processamento
            if (verificacao.PodeProsseguir)
            {
                verificacao.MensagemErro.Should().BeEmpty();
                verificacao.CursosComMatriculaAtiva.Should().BeEmpty();
            }

            verificacao.PodeProsseguir.Should().BeTrue();
        }

        [Fact(DisplayName = "Cenário Real - Resposta API Falha - Deve Retornar DTO com Detalhes do Erro")]
        [Trait("Alunos", "DTOs - VerificacaoPedidoMatricula - Integração")]
        public void CenarioReal_RespostaApiFalha_DeveRetornarDtoComDetalhesErro()
        {
            // Simula retorno de API quando verificação falha
            // Arrange
            var cursosConflitantes = new List<string>
            {
                "MBA em Gestão de Projetos",
                "MBA em Engenharia de Software"
            };

            // Act
            var verificacao = VerificacaoPedidoMatriculaDto.Falha(
                "Não é possível processar. O aluno já possui matrículas ativas nos seguintes cursos.",
                cursosConflitantes
            );

            // Assert - Não pode prosseguir
            verificacao.PodeProsseguir.Should().BeFalse();
            verificacao.MensagemErro.Should().NotBeEmpty();
            verificacao.CursosComMatriculaAtiva.Should().HaveCount(2);

            // Validação adicional do conteúdo
            foreach (var curso in cursosConflitantes)
            {
                verificacao.CursosComMatriculaAtiva.Should().Contain(curso);
            }
        }

        #endregion
    }
}

