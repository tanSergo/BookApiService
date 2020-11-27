using BookApiService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookApiService.Core.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
        IEnumerable<Book> Find(Expression<Func<Book, bool>> predicate);
        public Task<Book> First();
    }
}