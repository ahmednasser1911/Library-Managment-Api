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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepo authorRepo;
        private readonly AppDbContext appDbContext;

        public AuthorController(IAuthorRepo authorRepo , AppDbContext appDbContext)
        {
            this.authorRepo = authorRepo;
            this.appDbContext = appDbContext;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IActionResult AddAuthor(AuthorDto dto)
        {
            var author = new Author()
            {
                FullName = dto.FullName
            };
            var newAuthor = authorRepo.AddAuthor(author);

            return Created("", new { Message = "Auhtor Add!", Author = newAuthor });
        }

        [HttpGet("ListAuthors")]
        public IActionResult ListAuthors()
        {
            var authors = authorRepo.GetAuthors();

            return Ok(new { Message = "List Of All Authors!", Authors = authors });
        }

        [HttpGet("GetAuthorById")]
        public IActionResult GetAuthorById(int id)
        {
            var author = authorRepo.GetAuthorById(id);

            return Ok(new { Message = $"Author ${id}", Author = author });
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteAuthor(int id)
        {
            var author = authorRepo.DeleteAuthor(id);
            if(author == null) 
                return NotFound(new { Message = $"Author ${id} Not Found!" });
            return Ok(new { Message = $"Author ${author.FullName} Deleted!", Author = author });
        }


    }
}
