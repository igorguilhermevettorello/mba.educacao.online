using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Cursos.Application.Validators;
using MBA.Educacao.Online.Cursos.Domain.Enums;

namespace MBA.Educacao.Online.Cursos.Application.Commands.Cursos
{
    public class CriarCursoCommand : Command
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Instrutor { get; set; }
        public NivelCurso Nivel { get; set; }
        public decimal Valor { get; set; }

        public CriarCursoCommand(string titulo, string descricao, string instrutor, NivelCurso nivel,  decimal valor) 
        {
            Titulo = titulo;       
            Descricao = descricao;
            Instrutor = instrutor;
            Nivel = nivel;
            Valor = valor;
        }

        public override bool IsValid()
        {
            ValidationResult = new CriarCursoCommandValidator().Validate(this); 
            return ValidationResult.IsValid;
        }
    }
}


