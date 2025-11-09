using FluentValidation;
using MBA.Educacao.Online.Alunos.Application.Commands;

namespace MBA.Educacao.Online.Alunos.Application.Validators
{
    public class SolicitarCertificadoCommandValidator : AbstractValidator<SolicitarCertificadoCommand>
    {
        public SolicitarCertificadoCommandValidator()
        {
            RuleFor(c => c.AlunoId)
                .NotEqual(Guid.Empty)
                .WithMessage("O ID do aluno é obrigatório");

            RuleFor(c => c.MatriculaId)
                .NotEqual(Guid.Empty)
                .WithMessage("O ID da matrícula é obrigatório");

            RuleFor(c => c.CursoId)
                .NotEqual(Guid.Empty)
                .WithMessage("O ID do curso é obrigatório");
        }
    }
}

