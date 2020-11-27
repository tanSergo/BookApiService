using BookApiService.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace BookApiService.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository Books { get; }
        Task<int> CommitAsync();
    }
}
