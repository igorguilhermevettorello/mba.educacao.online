using MBA.Educacao.Online.Alunos.Domain.Entities;
using MediatR;

namespace MBA.Educacao.Online.Alunos.Application.Queries
{
    public class ObterCursosMatriculadosQuery : IRequest<IEnumerable<Matricula>>
    {
        public Guid AlunoId { get; set; }

        public ObterCursosMatriculadosQuery(Guid alunoId)
        {
            AlunoId = alunoId;
        }
    }
}

