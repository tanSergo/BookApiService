using AutoMapper;
using BookApiService.Api.DTOs;
using BookApiService.Core.Enums;
using BookApiService.Core.Models;
using BookApiService.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace BookApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public BooksController(IBookService bookService, IMapper mapper, ILogger<BooksController> logger)
        {
            this._bookService = bookService;
            this._mapper = mapper;
            this._logger = logger;
        }

        /// <summary>
        /// Получить книги
        /// </summary>
        /// <param name="filter">Настройки фильтрации</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadBookDto>>> Get([FromQuery] Filter[] filter)
        {
            //api/books?filter=[{"property":"genres","value":"0"},{"property":"publicationYear","value":1980}]
            string author = null;
            string title = null;
            int? publicationYear = null;
            Bookbindings bookbinding = Bookbindings.NotSet;
            AgeCategories ageCategorie = AgeCategories.NotSet;
            List<Genres> genres = null;
            foreach (var f in filter)
            {
                switch (f.property)
                {
                    case "author":
                        author = string.IsNullOrWhiteSpace(f.value) ? null : f.value;
                        break;
                    case "title":
                        title = string.IsNullOrWhiteSpace(f.value) ? null : f.value;
                        break;
                    case "publicationYear":
                        publicationYear = int.TryParse(f.value, out var tmpPublicationYear) ? (int?)tmpPublicationYear : null;
                        break;
                    case "bookbinding":
                        if (!string.IsNullOrWhiteSpace(f.value))
                        {
                            if (Enum.TryParse(f.value, true, out Bookbindings newBookbinding))
                                bookbinding = newBookbinding;
                        }
                        break;
                    case "ageCategorie":
                        if (!string.IsNullOrWhiteSpace(f.value))
                        {
                                if (Enum.TryParse(f.value, true, out AgeCategories newAgeCategorie))
                                    ageCategorie = newAgeCategorie;
                        }
                        break;
                    case "genres":
                        if (!string.IsNullOrWhiteSpace(f.value))
                        {
                            genres = new List<Genres>();
                            foreach (var v in f.value.Split(','))
                            {
                                if (Enum.TryParse(v, true, out Genres genre))
                                    genres.Add(genre);
                            }
                        }
                        break;
                }
            }

            if (string.IsNullOrEmpty(author) && string.IsNullOrEmpty(title) && !publicationYear.HasValue && bookbinding == Bookbindings.NotSet && genres.Count == 0 && ageCategorie == AgeCategories.NotSet)
            {
                var books = await _bookService.GetAllBooks();
                var readBooks = _mapper.Map<IEnumerable<Book>, IEnumerable<ReadBookDto>>(books);
                return Ok(readBooks);
            }
            else
            {
                Expression<Func<Book, bool>> criteria = book => book.Author.Contains(author) || book.Title.Contains(title) || book.PublicationYear.Equals(publicationYear)
                || book.Bookbinding.HasFlag(bookbinding) || book.Genres.Any(s => genres.Any(d => s.HasFlag(d))) || book.AgeCategorie.HasFlag(ageCategorie);

                var books = _bookService.FindBooks(criteria);
                var readBooks = _mapper.Map<IEnumerable<Book>, IEnumerable<ReadBookDto>>(books);
                return Ok(readBooks);
            }
        }
    }
}
