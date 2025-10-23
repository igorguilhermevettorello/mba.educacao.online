using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Cursos.Application.Validators;

namespace MBA.Educacao.Online.Cursos.Application.Commands.Aulas
{
    public class AtivarAulaCommand : Command
    {
        public Guid AulaId { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new AtivarAulaCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

