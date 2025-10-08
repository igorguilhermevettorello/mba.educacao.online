using FluentValidation;
using MBA.Educacao.Online.Cursos.Application.Commands.Cursos;

namespace MBA.Educacao.Online.Cursos.Application.Validators
{
    public class AtualizarCursoCommandValidator : AbstractValidator<AtualizarCursoCommand>
    {
        public AtualizarCursoCommandValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty()
                .WithMessage("Título do curso é obrigatório")
                .MaximumLength(200)
                .WithMessage("Título do curso deve ter no máximo 200 caracteres")
                .MinimumLength(3)
                .WithMessage("Título do curso deve ter no mínimo 3 caracteres");

            RuleFor(x => x.Descricao)
                .NotEmpty()
                .WithMessage("Descrição do curso é obrigatória")
                .MaximumLength(1000)
                .WithMessage("Descrição do curso deve ter no máximo 1000 caracteres")
                .MinimumLength(10)
                .WithMessage("Descrição do curso deve ter no mínimo 10 caracteres");

            RuleFor(x => x.Nivel)
                .IsInEnum()
                .WithMessage("Nível do curso deve ser válido");
        }
    }
}
