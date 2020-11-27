using BookApiService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookApiService.Core.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooks();
        Task<Book> GetBookById(int id);
        IEnumerable<Book> FindBooks(Expression<Func<Book, bool>> predicate);
        Task<Book> CreateBook(Book newBook);
        Task UpdateBook(Book bookToBeUpdated, Book book);
        Task DeleteBook(Book book);
        public Task<Book> GetFirstBook();
    }
}
