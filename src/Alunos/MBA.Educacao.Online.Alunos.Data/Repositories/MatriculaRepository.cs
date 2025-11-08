using MBA.Educacao.Online.Alunos.Data.Context;
using MBA.Educacao.Online.Alunos.Domain.Entities;
using MBA.Educacao.Online.Alunos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MBA.Educacao.Online.Alunos.Data.Repositories
{
    public class MatriculaRepository : IMatriculaRepository
    {
        private readonly AlunoContext _context;

        public MatriculaRepository(AlunoContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Adicionar(Matricula matricula)
        {
            _context.Matriculas.Add(matricula);
        }

        public void Alterar(Matricula matricula)
        {
            _context.Matriculas.Update(matricula);
        }

        public void Remover(Matricula matricula)
        {
            _context.Matriculas.Remove(matricula);
        }

        public Matricula? BuscarPorId(Guid id)
        {
            return _context.Matriculas
                .Include(m => m.HistoricosAprendizado)
                .FirstOrDefault(m => m.Id == id);
        }

        public Matricula? BuscarPorAlunoECurso(Guid alunoId, Guid cursoId)
        {
            return _context.Matriculas
                .Include(m => m.HistoricosAprendizado)
                .FirstOrDefault(m => m.AlunoId == alunoId && m.CursoId == cursoId);
        }

        public IEnumerable<Matricula> BuscarPorAluno(Guid alunoId)
        {
            return _context.Matriculas
                .Include(m => m.HistoricosAprendizado)
                .Where(m => m.AlunoId == alunoId)
                .ToList();
        }

        public IEnumerable<Matricula> BuscarPorCurso(Guid cursoId)
        {
            return _context.Matriculas
                .Include(m => m.HistoricosAprendizado)
                .Where(m => m.CursoId == cursoId)
                .ToList();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}

