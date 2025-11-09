using MBA.Educacao.Online.Alunos.Application.Validators;
using MBA.Educacao.Online.Core.Domain.Messages;

namespace MBA.Educacao.Online.Alunos.Application.Commands
{
    public class SolicitarCertificadoCommand : Command
    {
        public Guid AlunoId { get; set; }
        public Guid MatriculaId { get; set; }
        public Guid CursoId { get; set; }

        public SolicitarCertificadoCommand(Guid alunoId, Guid matriculaId, Guid cursoId)
        {
            AlunoId = alunoId;
            MatriculaId = matriculaId;
            CursoId = cursoId;
        }

        public override bool IsValid()
        {
            ValidationResult = new SolicitarCertificadoCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

