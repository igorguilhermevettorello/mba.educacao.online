using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Cursos.Application.Validators;
using MBA.Educacao.Online.Cursos.Domain.Enums;

namespace MBA.Educacao.Online.Cursos.Application.Commands.Cursos
{
    public class AtualizarCursoCommand : Command
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public NivelCurso Nivel { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new AtualizarCursoCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
