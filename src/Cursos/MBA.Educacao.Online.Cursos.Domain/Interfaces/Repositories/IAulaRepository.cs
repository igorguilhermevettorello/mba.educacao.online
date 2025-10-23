using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Cursos.Domain.Entities;

namespace MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories
{
    public interface IAulaRepository : IDisposable
    {
        IUnitOfWork UnitOfWork { get; }
        void Adicionar(Aula aula);
        void Alterar(Aula aula);
        void Remover(Aula aula);
        Task<Aula?> BuscarPorIdAsync(Guid id);
        Task<IEnumerable<Aula>> ObterTodasAsync();
        Task<IEnumerable<Aula>> ObterAtivasAsync();
        Task<IEnumerable<Aula>> ObterPorCursoIdAsync(Guid cursoId);
        Task<IEnumerable<Aula>> ObterAtivasPorCursoIdAsync(Guid cursoId);
    }
}

