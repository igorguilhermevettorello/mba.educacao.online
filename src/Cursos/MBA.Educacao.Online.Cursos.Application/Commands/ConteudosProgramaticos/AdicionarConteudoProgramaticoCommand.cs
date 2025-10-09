using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Cursos.Application.Validators;

namespace MBA.Educacao.Online.Cursos.Application.Commands.ConteudosProgramaticos
{
    public class AdicionarConteudoProgramaticoCommand : Command
    {
        public Guid AulaId { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public int Ordem { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new AdicionarConteudoProgramaticoCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

