using Authintication.Data.Interfaces;
using Authintication.DTOs;
using Authintication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authintication.Data
{
    public class BookRepo : IBookRepo
    {
        private readonly AppDbContext bookContext;

        public BookRepo(AppDbContext bookContext)
        {
            this.bookContext = bookContext;
        }
        public Book AddBook(BookDto dto)
        {
            var book = new Book()
            {
                Title = dto.Title,
                Descreption = dto.Descreption,
                IsRead = dto.IsRead,
                DateRead = dto.IsRead ? dto.DateRead.Value : null,
                Gen = dto.Gen,
                CoverUrl = dto.CoverUrl,
                DateAdded = DateTime.Now,
                PublisherId = dto.PublisherId
            };
            bookContext.Books.Add(book);
            bookContext.SaveChanges();

            foreach (var id in dto.AuthorIds)
            {
                var book_author = new Book_Author()
                {
                    BookId = book.Id,
                    AuthorId = id
                };
                bookContext.Books_Authors.Add(book_author);
                bookContext.SaveChanges();
            }

            return book;
          
        }

        public Book DeleteBook(Book book)
        {
            bookContext.Books.Remove(book);
            bookContext.SaveChanges();
            return book;
        }

        public BookAuthorDto GetBookById(int Id)
        {
            var bookAuthor = bookContext.Books.Where(b => b.Id == Id).Select(book => new BookAuthorDto()
            {
                Title = book.Title,
                Descreption = book.Descreption,
                IsRead = book.IsRead,
                DateRead = book.IsRead ? book.DateRead.Value : null,
                Gen = book.Gen,
                CoverUrl = book.CoverUrl,
                PublisherName = book.Publisher.Name,
                AuthorNames = book.Book_Authors.Select(n => n.Author.FullName).ToList()
            }).FirstOrDefault() ;
            return bookAuthor;
        }

        public List<BookAuthorDto> GetBooks()
        {
            List<BookAuthorDto> books = new();

            foreach (var book in bookContext.Books.ToList())
            {
                var bookAuthor = GetBookById(book.Id);
                books.Add(bookAuthor);
            }
            return books;
        }

        public Book GetBookByTitle(string title)
        {
            var book = bookContext.Books.FirstOrDefault(b => b.Title == title);
            if (book == null) return null;
            return book;

        }
    }
}
