using Authintication.DTOs;
using Authintication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authintication.Data.Interfaces
{
    public interface IBookRepo
    {
        public List<BookAuthorDto> GetBooks();
        public BookAuthorDto GetBookById(int Id);
        public Book GetBookByTitle(string title);

        public Book AddBook(BookDto dto);
        public Book DeleteBook(Book book);

    }
}
