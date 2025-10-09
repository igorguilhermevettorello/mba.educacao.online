using MBA.Educacao.Online.Core.Domain.Interfaces;
using MBA.Educacao.Online.Cursos.Domain.Entities;

namespace MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories
{
    public interface ICursoRepository : IRepository<Curso>
    {
        Task Adicionar(Curso curso);
        Task Alterar(Curso curso);
        Task Remover(Curso curso);
        Task<Curso> BuscarPorId(Guid id);
        void IDisposable();
    }
}

