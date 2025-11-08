using MBA.Educacao.Online.Alunos.Domain.Entities;
using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;

namespace MBA.Educacao.Online.Alunos.Domain.Interfaces.Repositories
{
    public interface IMatriculaRepository : IDisposable
    {
        IUnitOfWork UnitOfWork { get; }

        void Adicionar(Matricula matricula);
        void Alterar(Matricula matricula);
        void Remover(Matricula matricula);

        Matricula? BuscarPorId(Guid id);
        Matricula? BuscarPorAlunoECurso(Guid alunoId, Guid cursoId);
        IEnumerable<Matricula> BuscarPorAluno(Guid alunoId);
        IEnumerable<Matricula> BuscarPorCurso(Guid cursoId);
    }
}

