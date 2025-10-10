using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Cursos.Domain.Entities;

namespace MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories
{
    public interface ICursoRepository : IRepository<Curso>
    {
        void Adicionar(Curso curso);
        void Alterar(Curso curso);
        void Remover(Curso curso);
        Curso BuscarPorId(Guid id);
        void IDisposable();
    }
}

