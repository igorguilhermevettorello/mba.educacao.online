using MBA.Educacao.Online.Core.Domain.Interfaces;
using MBA.Educacao.Online.Cursos.Data.Context;
using MBA.Educacao.Online.Cursos.Domain.Entities;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;

namespace MBA.Educacao.Online.Cursos.Data.Repositories
{
    public class CursoRepository : ICursoRepository
    {
        private readonly CursoContext _context;

        public CursoRepository(CursoContext context)
        {
            _context = context;
        }
        public IUnitOfWork UnitOfWork => _context;
        
        public Task Adicionar(Curso curso)
        {
            throw new NotImplementedException();
        }

        public Task Alterar(Curso curso)
        {
            throw new NotImplementedException();
        }

        public Task Remover(Curso curso)
        {
            throw new NotImplementedException();
        }

        public Task<Curso> BuscarPorId(Guid id)
        {
            throw new NotImplementedException();
        }

        public void IDisposable()
        {
            throw new NotImplementedException();
        }
        
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

