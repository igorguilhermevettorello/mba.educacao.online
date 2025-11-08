using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Vendas.Application.Validators;

namespace MBA.Educacao.Online.Vendas.Application.Commands
{
    public class ProcessarPagamentoPedidoCommand : Command
    {
        public Guid AlunoId { get; set; }
        public string NomeCartao { get; set; }
        public string NumeroCartao { get; set; }
        public string ExpiracaoCartao { get; set; }
        public string CvvCartao { get; set; }
        public Guid PedidoId { get; set; }

        public ProcessarPagamentoPedidoCommand(Guid alunoId, string nomeCartao, string numeroCartao, 
            string expiracaoCartao, string cvvCartao)
        {
            AlunoId = alunoId;
            NomeCartao = nomeCartao;
            NumeroCartao = numeroCartao;
            ExpiracaoCartao = expiracaoCartao;
            CvvCartao = cvvCartao;
        }

        public override bool IsValid()
        {
            ValidationResult = new ProcessarPagamentoPedidoCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

