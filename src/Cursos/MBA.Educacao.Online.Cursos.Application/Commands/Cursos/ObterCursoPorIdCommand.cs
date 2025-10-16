using MBA.Educacao.Online.Cursos.Domain.Entities;
using MediatR;

namespace MBA.Educacao.Online.Cursos.Application.Commands.Cursos
{
    public class ObterCursoPorIdCommand : IRequest<Curso>
    {
        public Guid Id { get; set; }

        public ObterCursoPorIdCommand(Guid id)
        {
            Id = id;
        }
    }
}

