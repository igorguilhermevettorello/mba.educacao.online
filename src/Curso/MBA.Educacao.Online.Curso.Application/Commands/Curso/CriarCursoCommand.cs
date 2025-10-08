using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Curso.Application.Validators;
using MBA.Educacao.Online.Curso.Domain.Enums;
using MediatR;

namespace MBA.Educacao.Online.Curso.Application.Commands.Curso
{
    public class CriarCursoCommand : Command
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public NivelCurso Nivel { get; set; }

        public CriarCursoCommand(string titulo, string descricao, NivelCurso nivel) 
        {
            Titulo = titulo;       
            Descricao = descricao;
            Nivel = nivel;
        }

        public override bool IsValid()
        {
            ValidationResult = new CriarCursoCommandValidator().Validate(this); 
            return ValidationResult.IsValid;
        }
    }
}


