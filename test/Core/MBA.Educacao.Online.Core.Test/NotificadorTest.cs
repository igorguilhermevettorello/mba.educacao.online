using FluentAssertions;
using MBA.Educacao.Online.Core.Domain.Notifications;

namespace MBA.Educacao.Online.Core.Test
{
    public class NotificadorTest
    {
        #region 1. Criar Notificador

        [Fact(DisplayName = "Criar Notificador - Deve Criar com Lista Vazia")]
        [Trait("Core", "Notificador")]
        public void CriarNotificador_DeveCriarComListaVazia()
        {
            // Act
            var notificador = new Notificador();

            // Assert
            notificador.Should().NotBeNull();
            notificador.ObterNotificacoes().Should().BeEmpty();
            notificador.TemNotificacao().Should().BeFalse();
        }

        #endregion

        #region 2. Handle Notificação

        [Fact(DisplayName = "Handle Notificação - Deve Adicionar Notificação")]
        [Trait("Core", "Notificador")]
        public void HandleNotificacao_DeveAdicionarNotificacao()
        {
            // Arrange
            var notificador = new Notificador();
            var notificacao = new Notificacao
            {
                Campo = "Nome",
                Mensagem = "Nome é obrigatório"
            };

            // Act
            notificador.Handle(notificacao);

            // Assert
            notificador.ObterNotificacoes().Should().HaveCount(1);
            notificador.ObterNotificacoes().First().Campo.Should().Be("Nome");
            notificador.ObterNotificacoes().First().Mensagem.Should().Be("Nome é obrigatório");
        }

        [Fact(DisplayName = "Handle Múltiplas Notificações - Deve Adicionar Todas")]
        [Trait("Core", "Notificador")]
        public void HandleMultiplasNotificacoes_DeveAdicionarTodas()
        {
            // Arrange
            var notificador = new Notificador();
            var notificacao1 = new Notificacao
            {
                Campo = "Nome",
                Mensagem = "Nome é obrigatório"
            };
            var notificacao2 = new Notificacao
            {
                Campo = "Email",
                Mensagem = "Email é inválido"
            };
            var notificacao3 = new Notificacao
            {
                Campo = "CPF",
                Mensagem = "CPF é obrigatório"
            };

            // Act
            notificador.Handle(notificacao1);
            notificador.Handle(notificacao2);
            notificador.Handle(notificacao3);

            // Assert
            notificador.ObterNotificacoes().Should().HaveCount(3);
        }

        #endregion

        #region 3. Tem Notificação

        [Fact(DisplayName = "Tem Notificação - Com Notificações - Deve Retornar True")]
        [Trait("Core", "Notificador")]
        public void TemNotificacao_ComNotificacoes_DeveRetornarTrue()
        {
            // Arrange
            var notificador = new Notificador();
            var notificacao = new Notificacao
            {
                Campo = "Nome",
                Mensagem = "Nome é obrigatório"
            };
            notificador.Handle(notificacao);

            // Act
            var resultado = notificador.TemNotificacao();

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact(DisplayName = "Tem Notificação - Sem Notificações - Deve Retornar False")]
        [Trait("Core", "Notificador")]
        public void TemNotificacao_SemNotificacoes_DeveRetornarFalse()
        {
            // Arrange
            var notificador = new Notificador();

            // Act
            var resultado = notificador.TemNotificacao();

            // Assert
            resultado.Should().BeFalse();
        }

        #endregion

        #region 4. Obter Notificações

        [Fact(DisplayName = "Obter Notificações - Com Notificações - Deve Retornar Lista Completa")]
        [Trait("Core", "Notificador")]
        public void ObterNotificacoes_ComNotificacoes_DeveRetornarListaCompleta()
        {
            // Arrange
            var notificador = new Notificador();
            var notificacao1 = new Notificacao { Campo = "Campo1", Mensagem = "Mensagem1" };
            var notificacao2 = new Notificacao { Campo = "Campo2", Mensagem = "Mensagem2" };
            notificador.Handle(notificacao1);
            notificador.Handle(notificacao2);

            // Act
            var notificacoes = notificador.ObterNotificacoes();

            // Assert
            notificacoes.Should().HaveCount(2);
            notificacoes[0].Campo.Should().Be("Campo1");
            notificacoes[1].Campo.Should().Be("Campo2");
        }

        [Fact(DisplayName = "Obter Notificações - Sem Notificações - Deve Retornar Lista Vazia")]
        [Trait("Core", "Notificador")]
        public void ObterNotificacoes_SemNotificacoes_DeveRetornarListaVazia()
        {
            // Arrange
            var notificador = new Notificador();

            // Act
            var notificacoes = notificador.ObterNotificacoes();

            // Assert
            notificacoes.Should().BeEmpty();
        }

        #endregion

        #region 5. Cenários de Integração

        [Fact(DisplayName = "Cenário Completo - Validação de Formulário - Deve Acumular Todas as Notificações")]
        [Trait("Core", "Notificador - Integração")]
        public void CenarioCompleto_ValidacaoFormulario_DeveAcumularTodasNotificacoes()
        {
            // Arrange
            var notificador = new Notificador();

            // Simula validação de um formulário com múltiplos erros
            var erros = new[]
            {
                new Notificacao { Campo = "Nome", Mensagem = "Nome é obrigatório" },
                new Notificacao { Campo = "Email", Mensagem = "Email é inválido" },
                new Notificacao { Campo = "CPF", Mensagem = "CPF é obrigatório" },
                new Notificacao { Campo = "DataNascimento", Mensagem = "Data de nascimento é obrigatória" },
                new Notificacao { Campo = "Telefone", Mensagem = "Telefone é inválido" }
            };

            // Act
            foreach (var erro in erros)
            {
                notificador.Handle(erro);
            }

            // Assert
            notificador.TemNotificacao().Should().BeTrue();
            notificador.ObterNotificacoes().Should().HaveCount(5);
            notificador.ObterNotificacoes().Should().Contain(n => n.Campo == "Nome");
            notificador.ObterNotificacoes().Should().Contain(n => n.Campo == "Email");
            notificador.ObterNotificacoes().Should().Contain(n => n.Campo == "CPF");
            notificador.ObterNotificacoes().Should().Contain(n => n.Campo == "DataNascimento");
            notificador.ObterNotificacoes().Should().Contain(n => n.Campo == "Telefone");
        }

        #endregion

        #region 6. Notificação - Propriedades

        [Fact(DisplayName = "Notificação - Criar com Propriedades - Deve Atribuir Valores Corretamente")]
        [Trait("Core", "Notificacao")]
        public void Notificacao_CriarComPropriedades_DeveAtribuirValoresCorretamente()
        {
            // Arrange & Act
            var notificacao = new Notificacao
            {
                Campo = "Email",
                Mensagem = "Email inválido"
            };

            // Assert
            notificacao.Should().NotBeNull();
            notificacao.Campo.Should().Be("Email");
            notificacao.Mensagem.Should().Be("Email inválido");
        }

        [Fact(DisplayName = "Notificação - Criar Sem Valores - Deve Permitir Null")]
        [Trait("Core", "Notificacao")]
        public void Notificacao_CriarSemValores_DevePermitirNull()
        {
            // Arrange & Act
            var notificacao = new Notificacao();

            // Assert
            notificacao.Should().NotBeNull();
            notificacao.Campo.Should().BeNull();
            notificacao.Mensagem.Should().BeNull();
        }

        #endregion
    }
}

