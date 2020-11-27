using AutoMapper;
using BookApiService.Api.DTOs;
using BookApiService.Api.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using BookApiService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BookApiService.Core.Services;
using FluentValidation;

namespace StpTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public BookController(IBookService bookService, IMapper mapper, ILogger<BookController> logger)
        {
            this._bookService = bookService;
            this._mapper = mapper;
            this._logger = logger;
        }

        /// <summary>
        /// Получить первую книгу
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ReadBookDto>> Get()
        {
            var book = await _bookService.GetFirstBook();
            var readBook = _mapper.Map<Book, ReadBookDto>(book);
            return Ok(readBook);
        }

        /// <summary>
        /// Получить книгу по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadBookDto>> Get(int id)
        {
            var book = await _bookService.GetBookById(id);
            var readBook = _mapper.Map<Book, ReadBookDto>(book);

            return Ok(readBook);
        }

        
        /// <summary>
        /// Сохранить книгу
        /// </summary>
        /// <param name="createBookDto"></param>
        [HttpPost]
        public async Task<ActionResult<ReadBookDto>> PostAsync([FromBody] CreateBookDto createBookDto)
        {
            var validator = new CreateBookDtoValidator();

            var validationResult = await validator.ValidateAsync(createBookDto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors); // this needs refining, but for demo it is ok

            var bookToCreate = _mapper.Map<CreateBookDto, Book>(createBookDto);

            var newBook = await _bookService.CreateBook(bookToCreate);

            var book = await _bookService.GetBookById(newBook.Id);

            var musicResource = _mapper.Map<Book, ReadBookDto>(book);

            return Ok(musicResource);
        }

        /// <summary>
        /// Обновить книгу
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateBookDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ReadBookDto>> Put(int id, [FromBody] CreateBookDto updateBookDto)
        {
            var validator = new CreateBookDtoValidator();
            var validationResult = await validator.ValidateAsync(updateBookDto);

            var requestIsInvalid = id == 0 || !validationResult.IsValid;

            if (requestIsInvalid)
                return BadRequest(validationResult.Errors); // this needs refining, but for demo it is ok

            var bookToBeUpdate = await _bookService.GetBookById(id);

            if (bookToBeUpdate == null)
                return NotFound();

            var book = _mapper.Map<CreateBookDto, Book>(updateBookDto);

            await _bookService.UpdateBook(bookToBeUpdate, book);

            var updatedBook = await _bookService.GetBookById(id);
            var updatedBookDto = _mapper.Map<Book, ReadBookDto>(updatedBook);

            return Ok(updatedBookDto);
        }

        /// <summary>
        /// Удалить книгу
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
                return BadRequest();

            var book = await _bookService.GetBookById(id);

            if (book == null)
                return NotFound();

            await _bookService.DeleteBook(book);

            return NoContent();
        }
    }
}
