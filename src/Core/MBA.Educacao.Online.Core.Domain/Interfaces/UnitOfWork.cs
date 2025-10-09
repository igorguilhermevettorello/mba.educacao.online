namespace MBA.Educacao.Online.Core.Data.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}

