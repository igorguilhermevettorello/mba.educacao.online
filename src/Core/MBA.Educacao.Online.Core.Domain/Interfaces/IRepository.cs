using MBA.Educacao.Online.Core.Domain.Models;

namespace MBA.Educacao.Online.Core.Data.Interfaces
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }    
}

