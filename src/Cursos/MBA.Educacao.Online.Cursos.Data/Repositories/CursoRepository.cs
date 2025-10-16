using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Cursos.Data.Context;
using MBA.Educacao.Online.Cursos.Domain.Entities;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

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
        
        public void Adicionar(Curso curso)
        {
            _context.Cursos.Add(curso);
        }

        public void Alterar(Curso curso)
        {
            _context.Cursos.Update(curso);
        }

        public void Remover(Curso curso)
        {
            _context.Cursos.Remove(curso);
        }

        public async Task<Curso> BuscarPorIdAsync(Guid id)
        {
            return await _context.Cursos
                .Include(c => c.Aulas)
                .Include(c => c.ConteudoProgramatico)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Curso>> ObterTodosAsync()
        {
            return await _context.Cursos
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Curso>> ObterAtivosAsync()
        {
            return await _context.Cursos
                .Where(c => c.Ativo)
                .AsNoTracking()
                .ToListAsync();
        }

        public void IDisposable()
        {
            _context?.Dispose();
        }
        
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}

