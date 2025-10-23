using MBA.Educacao.Online.Cursos.Domain.Entities;
using MediatR;

namespace MBA.Educacao.Online.Cursos.Application.Commands.Aulas
{
    public class ObterAulaPorIdCommand : IRequest<Aula?>
    {
        public Guid Id { get; set; }

        public ObterAulaPorIdCommand(Guid id)
        {
            Id = id;
        }
    }
}

