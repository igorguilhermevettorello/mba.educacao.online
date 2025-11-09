using MBA.Educacao.Online.Alunos.Application.Validators;
using MBA.Educacao.Online.Core.Domain.Messages;

namespace MBA.Educacao.Online.Alunos.Application.Commands
{
    public class AcessarEstudoAulaCommand : Command
    {
        public Guid AlunoId { get; set; }
        public Guid MatriculaId { get; set; }
        public Guid CursoId { get; set; }
        public Guid AulaId { get; set; }

        public AcessarEstudoAulaCommand(Guid alunoId, Guid matriculaId, Guid cursoId, Guid aulaId)
        {
            AlunoId = alunoId;
            MatriculaId = matriculaId;
            CursoId = cursoId;
            AulaId = aulaId;
        }

        public override bool IsValid()
        {
            ValidationResult = new AcessarEstudoAulaCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

