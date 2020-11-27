using BookApiService.Core;
using BookApiService.Core.Models;
using BookApiService.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookApiService.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<Book> CreateBook(Book newBook)
        {
            await _unitOfWork.Books.AddAsync(newBook);
            await _unitOfWork.CommitAsync();
            return newBook;
        }

        public async Task DeleteBook(Book book)
        {
            _unitOfWork.Books.Remove(book);
            await _unitOfWork.CommitAsync();
        }

        public IEnumerable<Book> FindBooks(Expression<Func<Book, bool>> predicate)
        {
            return _unitOfWork.Books.Find(predicate);
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _unitOfWork.Books.GetAllAsync();
        }

        public async Task<Book> GetBookById(int id)
        {
            return await _unitOfWork.Books.GetByIdAsync(id);
        }

        public async Task UpdateBook(Book bookToBeUpdated, Book book)
        {
            bookToBeUpdated.Author = book.Author;
            bookToBeUpdated.Title = book.Title;
            bookToBeUpdated.PublicationYear = book.PublicationYear;
            bookToBeUpdated.Bookbinding = book.Bookbinding;
            bookToBeUpdated.AgeCategorie = book.AgeCategorie;
            bookToBeUpdated.Genres = book.Genres;

            await _unitOfWork.CommitAsync();
        }

        public async Task<Book> GetFirstBook()
        {
            return await _unitOfWork.Books.First();
        }
    }
}
