using MBA.Educacao.Online.Alunos.Application.Validators;
using MBA.Educacao.Online.Core.Domain.DTOs;
using MBA.Educacao.Online.Core.Domain.Messages;

namespace MBA.Educacao.Online.Alunos.Application.Commands
{
    public class ProcessarMatriculaCommand : Command
    {
        public Guid AlunoId { get; private set; }
        public Guid PedidoId { get; private set; }
        public ListaCursosPedidoDto ListaCursos { get; private set; }

        public ProcessarMatriculaCommand(Guid alunoId, Guid pedidoId, ListaCursosPedidoDto listaCursos)
        {
            AggregateId = alunoId;
            AlunoId = alunoId;
            PedidoId = pedidoId;
            ListaCursos = listaCursos;
        }

        public override bool IsValid()
        {
            ValidationResult = new ProcessarMatriculaCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
