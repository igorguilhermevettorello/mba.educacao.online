using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Core.Domain.Models;

namespace MBA.Educacao.Online.Pagamentos.Domain.Entities
{
    public class Pagamento : Entity, IAggregateRoot
    {
        public Guid PedidoId { get; private set; }
        public string Status { get; private set; }
        public decimal Valor { get; private set; }

        public string NomeCartao { get; private set; }
        public string NumeroCartao { get; private set; }
        public string ExpiracaoCartao { get; private set; }
        public string CvvCartao { get; private set; }

        // EF. Rel.
        public Transacao Transacao { get; private set; }

        protected Pagamento() 
        {
            NomeCartao = string.Empty;
            NumeroCartao = string.Empty;
            ExpiracaoCartao = string.Empty;
            CvvCartao = string.Empty;
            Status = string.Empty;
        }

        public Pagamento(Guid pedidoId, decimal valor, string nomeCartao, string numeroCartao, 
            string expiracaoCartao, string cvvCartao)
        {
            ValidarPedidoId(pedidoId);
            ValidarValor(valor);
            ValidarNomeCartao(nomeCartao);
            ValidarNumeroCartao(numeroCartao);
            ValidarExpiracaoCartao(expiracaoCartao);
            ValidarCvvCartao(cvvCartao);

            PedidoId = pedidoId;
            Valor = valor;
            NomeCartao = nomeCartao;
            NumeroCartao = numeroCartao;
            ExpiracaoCartao = expiracaoCartao;
            CvvCartao = cvvCartao;
            Status = "Pendente";
        }

        public void AtualizarStatus(string novoStatus)
        {
            if (string.IsNullOrWhiteSpace(novoStatus))
                throw new ArgumentException("Status do pagamento é obrigatório", nameof(novoStatus));

            Status = novoStatus;
        }

        public void AssociarTransacao(Transacao transacao)
        {
            if (transacao == null)
                throw new ArgumentNullException(nameof(transacao), "Transação não pode ser nula");

            if (Transacao != null)
                throw new InvalidOperationException("Pagamento já possui uma transação associada");

            Transacao = transacao;
        }

        public bool FoiAprovado()
        {
            return Status == "Aprovado" || Status == "Pago";
        }

        public bool FoiRecusado()
        {
            return Status == "Recusado" || Status == "Negado";
        }

        private static void ValidarPedidoId(Guid pedidoId)
        {
            if (pedidoId == Guid.Empty)
                throw new ArgumentException("ID do pedido é inválido", nameof(pedidoId));
        }

        private static void ValidarValor(decimal valor)
        {
            if (valor <= 0)
                throw new ArgumentException("Valor do pagamento deve ser maior que zero", nameof(valor));

            if (valor > 999999.99m)
                throw new ArgumentException("Valor do pagamento excede o limite máximo permitido", nameof(valor));
        }

        private static void ValidarNomeCartao(string nomeCartao)
        {
            if (string.IsNullOrWhiteSpace(nomeCartao))
                throw new ArgumentException("Nome no cartão é obrigatório", nameof(nomeCartao));

            if (nomeCartao.Length < 3)
                throw new ArgumentException("Nome no cartão deve ter no mínimo 3 caracteres", nameof(nomeCartao));

            if (nomeCartao.Length > 100)
                throw new ArgumentException("Nome no cartão deve ter no máximo 100 caracteres", nameof(nomeCartao));
        }

        private static void ValidarNumeroCartao(string numeroCartao)
        {
            if (string.IsNullOrWhiteSpace(numeroCartao))
                throw new ArgumentException("Número do cartão é obrigatório", nameof(numeroCartao));

            // Remove espaços para validação
            var numeroLimpo = numeroCartao.Replace(" ", "");

            if (numeroLimpo.Length < 13 || numeroLimpo.Length > 19)
                throw new ArgumentException("Número do cartão deve ter entre 13 e 19 dígitos", nameof(numeroCartao));

            if (!numeroLimpo.All(char.IsDigit))
                throw new ArgumentException("Número do cartão deve conter apenas dígitos", nameof(numeroCartao));
        }

        private static void ValidarExpiracaoCartao(string expiracaoCartao)
        {
            if (string.IsNullOrWhiteSpace(expiracaoCartao))
                throw new ArgumentException("Data de expiração do cartão é obrigatória", nameof(expiracaoCartao));

            // Validação de formato MM/YY
            var regexExpiracao = new System.Text.RegularExpressions.Regex(@"^(0[1-9]|1[0-2])\/\d{2}$");
            if (!regexExpiracao.IsMatch(expiracaoCartao))
                throw new ArgumentException("Data de expiração deve estar no formato MM/YY", nameof(expiracaoCartao));

            // Validação de data não expirada
            var partes = expiracaoCartao.Split('/');
            var mes = int.Parse(partes[0]);
            var ano = int.Parse(partes[1]) + 2000; // Converte YY para YYYY

            var dataExpiracao = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));
            if (dataExpiracao < DateTime.Now)
                throw new ArgumentException("Cartão expirado", nameof(expiracaoCartao));
        }

        private static void ValidarCvvCartao(string cvvCartao)
        {
            if (string.IsNullOrWhiteSpace(cvvCartao))
                throw new ArgumentException("CVV do cartão é obrigatório", nameof(cvvCartao));

            if (cvvCartao.Length < 3 || cvvCartao.Length > 4)
                throw new ArgumentException("CVV deve ter 3 ou 4 dígitos", nameof(cvvCartao));

            if (!cvvCartao.All(char.IsDigit))
                throw new ArgumentException("CVV deve conter apenas dígitos", nameof(cvvCartao));
        }
    }
}