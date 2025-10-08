using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Cursos.Application.Validators;

namespace MBA.Educacao.Online.Cursos.Application.Commands.Cursos
{
    public class AtivarCursoCommand : Command
    {
        public Guid CursoId { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new AtivarCursoCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}


