namespace MBA.Educacao.Online.Core.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}