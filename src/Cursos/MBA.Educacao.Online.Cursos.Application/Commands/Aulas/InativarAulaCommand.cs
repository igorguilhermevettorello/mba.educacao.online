using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Cursos.Application.Validators;

namespace MBA.Educacao.Online.Cursos.Application.Commands.Aulas
{
    public class InativarAulaCommand : Command
    {
        public Guid Id { get; set; }

        public InativarAulaCommand(Guid id)
        {
            Id = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new InativarAulaCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

