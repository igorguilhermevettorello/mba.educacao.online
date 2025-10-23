using MBA.Educacao.Online.Cursos.Domain.Entities;
using MediatR;

namespace MBA.Educacao.Online.Cursos.Application.Commands.Aulas
{
    public class ListarAulasCommand : IRequest<IEnumerable<Aula>>
    {
        public Guid? CursoId { get; set; }
        public bool ApenasAtivas { get; set; }

        public ListarAulasCommand(Guid? cursoId = null, bool apenasAtivas = false)
        {
            CursoId = cursoId;
            ApenasAtivas = apenasAtivas;
        }
    }
}

