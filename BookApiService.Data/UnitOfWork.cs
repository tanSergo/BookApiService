using BookApiService.Core;
using BookApiService.Core.Repositories;
using BookApiService.Data.Repositories;
using System.Threading.Tasks;

namespace BookApiService.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookDBContext _context;
        private BookRepository _bookRepository;

        public UnitOfWork(BookDBContext context)
        {
            this._context = context;
        }

        public IBookRepository Books => _bookRepository = _bookRepository ?? new BookRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
