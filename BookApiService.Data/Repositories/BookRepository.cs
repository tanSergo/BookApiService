using BookApiService.Core.Models;
using BookApiService.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookApiService.Data.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {

        public BookRepository(BookDBContext context)
            : base(context)
        { }

        public IEnumerable<Book> Find(Expression<Func<Book, bool>> predicate)
        {
            return BookDBContext.Set<Book>().Where(predicate);
        }

        public async Task<Book> First()
        {
            return await BookDBContext.Set<Book>().FirstAsync();
        }

        private BookDBContext BookDBContext
        {
            get { return _context as BookDBContext; }
        }
    }
}
