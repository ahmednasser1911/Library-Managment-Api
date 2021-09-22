using Authintication.Data;
using Authintication.Data.Interfaces;
using Authintication.DTOs;
using Authintication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authintication.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepo bookRepo;
        private readonly AppDbContext appDbContext;
        private readonly IPublisherRepo publisherRepo;

        public BookController(IBookRepo bookRepo , AppDbContext appDbContext , IPublisherRepo publisherRepo)
        {
            this.bookRepo = bookRepo;
            this.appDbContext = appDbContext;
            this.publisherRepo = publisherRepo;
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddBook(BookDto dto)
        {
            
            var newBook = bookRepo.AddBook(dto);
            var publisher = publisherRepo.GetPublisherById(dto.PublisherId);
            Publisher publisher1 = new()
            {
                Name = publisher.Name
            };
            newBook.Publisher = publisher1;
            return Created("" ,new { Message = $"Book {newBook.Title} with Publisher ${publisher.Name} Add!" });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ListBooks()
        {
            var books = bookRepo.GetBooks();
            if(books.Any())
                return Ok(new { Message = "List Of All Books", Books = books });
            else 
                return Ok(new { Message = "No Books in the Database Yet!", Books = books });

        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteBook(int id)
        {
            var book = appDbContext.Books.FirstOrDefault(b => b.Id == id);
            if(book == null) 
                return NotFound(new { Message = "No Book Found!"});
            else
            {
                bookRepo.DeleteBook(book);
                return Ok(new { Message = "Book Deleted!", Book = book });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetBookById(int id)
        {
            var book = bookRepo.GetBookById(id);
            if (book == null) return NotFound(new { Message = "No Book Found!" });
            return Ok(new { Book = book });
        }
    }
}
