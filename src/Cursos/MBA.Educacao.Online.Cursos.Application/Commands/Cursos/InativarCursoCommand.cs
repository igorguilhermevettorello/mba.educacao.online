using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Cursos.Application.Validators;
using MBA.Educacao.Online.Cursos.Domain.Enums;

namespace MBA.Educacao.Online.Cursos.Application.Commands.Cursos
{
    public class InativarCursoCommand : Command
    {
        public Guid CursoId { get; set; }

        public InativarCursoCommand(Guid cursoId)
        {
            CursoId = cursoId;
        }

        public override bool IsValid()
        {
            ValidationResult = new InativarCursoCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }

        
    }
}