namespace MBA.Educacao.Online.Core.Domain.Interfaces.Repositories
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}