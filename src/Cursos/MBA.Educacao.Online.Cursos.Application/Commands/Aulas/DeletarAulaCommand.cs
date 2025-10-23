using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Cursos.Application.Validators;

namespace MBA.Educacao.Online.Cursos.Application.Commands.Aulas
{
    public class DeletarAulaCommand : Command
    {
        public Guid Id { get; set; }

        public DeletarAulaCommand(Guid id)
        {
            Id = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new DeletarAulaCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

