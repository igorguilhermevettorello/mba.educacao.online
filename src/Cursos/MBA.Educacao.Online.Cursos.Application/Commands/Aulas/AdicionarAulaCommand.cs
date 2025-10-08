using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Cursos.Application.Validators;

namespace MBA.Educacao.Online.Cursos.Application.Commands.Aulas
{
    public class AdicionarAulaCommand : Command
    { 
        public Guid CursoId { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public int DuracaoMinutos { get; set; }
        public int Ordem { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new AdicionarAulaCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }  
}


