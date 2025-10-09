using MBA.Educacao.Online.Core.Data.Interfaces;
using MBA.Educacao.Online.Cursos.Domain.Entities;

namespace MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories
{
    public interface ICursoRepository : IRepository<Curso>
    {
        void Adicionar(Curso curso);
    }
}

