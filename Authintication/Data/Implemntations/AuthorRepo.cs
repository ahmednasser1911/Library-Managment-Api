using Authintication.DTOs;
using Authintication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authintication.Data.Interfaces
{
    public class AuthorRepo : IAuthorRepo
    {
        private readonly AppDbContext authorContext;

        public AuthorRepo(AppDbContext authorContext)
        {
            this.authorContext = authorContext;
        }

        public Author AddAuthor(Author author)
        {
            authorContext.Authors.Add(author);
            authorContext.SaveChanges();
            return author;

        }

        public Author DeleteAuthor(int id)
        {
            var author = authorContext.Authors.Find(id);
            if(author != null)
            {
                authorContext.Authors.Remove(author);
                authorContext.SaveChanges();
                return author;
            }
            return null;

        }

        public AuthorBookDto GetAuthorById(int Id)
        {
            var author = authorContext.Authors.Where(a => a.Id == Id).Select(a => new AuthorBookDto()
            {
                FullName = a.FullName,
                BookTiltes = a.Book_Authors.Select(ba => ba.Book.Title).ToList()
            }).FirstOrDefault();
            return author;
        }

        public Author GetAuthorByName(string name)
        {
            return authorContext.Authors.SingleOrDefault(a => a.FullName == name);
        }

        public List<AuthorBookDto> GetAuthors()
        {
            List<AuthorBookDto> authors = new();

            foreach (var author in authorContext.Authors.ToList())
            {
                var bookAuthor = GetAuthorById(author.Id);
                authors.Add(bookAuthor);
            }
            return authors;
        }


    }
}
