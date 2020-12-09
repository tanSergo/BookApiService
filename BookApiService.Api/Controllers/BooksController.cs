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
        public async Task<ActionResult<IEnumerable<ReadBookDto>>> Get([FromQuery(Name = "filter")] Filter[] filter)
        {
            //api/books?filter=[{"property":"genres","value":"Poetry"},{"property":"publicationYear","value":"1993"}]
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

            if (string.IsNullOrEmpty(author) && string.IsNullOrEmpty(title) && !publicationYear.HasValue && bookbinding == Bookbindings.NotSet && genres == null && ageCategorie == AgeCategories.NotSet)
            {
                var books = await _bookService.GetAllBooks();
                var readBooks = _mapper.Map<IEnumerable<Book>, IEnumerable<ReadBookDto>>(books);
                return Ok(readBooks);
            }
            else
            {
                var criteria = GetCriteria(author, title, publicationYear, bookbinding, ageCategorie, genres);

                var books = _bookService.FindBooks(criteria).AsEnumerable();
                var readBooks = _mapper.Map<IEnumerable<Book>, IEnumerable<ReadBookDto>>(books);
                return Ok(readBooks);
            }
        }

        private static Expression<Func<Book, bool>> GetCriteria(string author, string title, int? publicationYear, Bookbindings bookbinding, AgeCategories ageCategorie, List<Genres> genres)
        {
            Expression<Func<Book, bool>> criteria = book =>
            (string.IsNullOrEmpty(author) || book.Author.Contains(author))
            && (string.IsNullOrEmpty(title) || book.Title.Contains(title))
            && (!publicationYear.HasValue || book.PublicationYear.Equals(publicationYear))
            && (bookbinding.HasFlag(Bookbindings.NotSet) || book.Bookbinding.HasFlag(bookbinding))
           //&& (genres == null || book.Genres.Contains(genres[0]))
           //&& (genres == null || genres.All(g => book.Genres.Any(x => x.HasFlag(g))))
           //&& (genres == null || book.Genres.All(g => genres.))
           && (ageCategorie.HasFlag(AgeCategories.NotSet) || book.AgeCategorie.HasFlag(ageCategorie));
            return criteria;
        }
    }
}
