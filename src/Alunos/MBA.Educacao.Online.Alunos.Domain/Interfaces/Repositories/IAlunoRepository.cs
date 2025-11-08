using MBA.Educacao.Online.Alunos.Domain.Entities;
using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;

namespace MBA.Educacao.Online.Alunos.Domain.Interfaces.Repositories
{
    public interface IAlunoRepository : IRepository<Aluno>
    {
        void Adicionar(Aluno aluno);
        void Alterar(Aluno aluno);
        void Remover(Aluno aluno);
        Aluno? BuscarPorId(Guid id);
        Aluno? BuscarPorIdNoTracking(Guid id);
        Aluno? BuscarPorUsuarioId(Guid usuarioId);
        Aluno? BuscarPorEmail(string email);
    }
}

