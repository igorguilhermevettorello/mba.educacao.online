using MBA.Educacao.Online.Alunos.Data.Context;
using MBA.Educacao.Online.Alunos.Domain.Entities;
using MBA.Educacao.Online.Alunos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MBA.Educacao.Online.Alunos.Data.Repositories
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly AlunoContext _context;

        public AlunoRepository(AlunoContext context)
        {
            _context = context;
        }
        
        public IUnitOfWork UnitOfWork => _context;
        
        public void Adicionar(Aluno aluno)
        {
            _context.Alunos.Add(aluno);
        }

        public void Alterar(Aluno aluno)
        {
            _context.Alunos.Update(aluno);
        }

        public void Remover(Aluno aluno)
        {
            _context.Alunos.Remove(aluno);
        }

        public Aluno? BuscarPorId(Guid id)
        {
            return _context.Alunos.AsNoTracking().FirstOrDefault(a => a.Id == id);
        }

        public Aluno? BuscarPorUsuarioId(Guid usuarioId)
        {
            return _context.Alunos.AsNoTracking().FirstOrDefault(a => a.Id == usuarioId);
        }

        public Aluno? BuscarPorEmail(string email)
        {
            return _context.Alunos.AsNoTracking().FirstOrDefault(a => a.Email == email);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}

