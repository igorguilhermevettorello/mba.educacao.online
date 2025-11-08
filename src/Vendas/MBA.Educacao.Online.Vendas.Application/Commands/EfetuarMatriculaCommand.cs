using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Vendas.Application.Validators;

namespace MBA.Educacao.Online.Vendas.Application.Commands
{
    public class EfetuarMatriculaCommand : Command
    {
        public Guid AlunoId { get; set; }
        //public string NomeCartao { get; set; }
        //public string NumeroCartao { get; set; }
        //public string ExpiracaoCartao { get; set; }
        //public string CvvCartao { get; set; }
        //public Guid PedidoId { get; set; }

        public EfetuarMatriculaCommand(Guid alunoId)
        {
            AggregateId = alunoId;
            AlunoId = alunoId;
        }

        public override bool IsValid()
        {
            ValidationResult = new EfetuarMatriculaCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
