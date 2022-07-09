using Lection_2_BL;
using Lection_2_BL.DTOs;
using Lection_2_BL.Services.BooksService;
using Lection_2_DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lection_2_02_07.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBooksService _booksService;

        public BooksController(IBooksService booksService, ILogger<BooksController> logger)
        {
            _booksService = booksService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _booksService.GetAllBooks();
        }

        [HttpGet("{id}")]
        public async Task<Book> GetBookById(Guid id)
        {
            return await _booksService.GetBookById(id);
        }

        [HttpGet("full/{id}")]
        public async Task<BookDto> GetFullBookInfoById(Guid id)
        {
            return await _booksService.GetBookFullInfo(id);
        }

        [HttpPost]
        public IActionResult AddBook(Book book)
        {
            try
            {
                var result = _booksService.AddBook(book);

                return Created(result.ToString(), book);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(Guid id, Book book)
        {
            book.Id = id;

            var result = await _booksService.UpdateBook(book);

            return result ? StatusCode(200) : StatusCode(400);
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeleteBook(Guid id)
        {
            return await _booksService.DeleteBookById(id);
        }
    }
}
