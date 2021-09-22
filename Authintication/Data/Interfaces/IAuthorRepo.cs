using Authintication.DTOs;
using Authintication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authintication.Data.Interfaces
{
    public interface IAuthorRepo
    {
        public List<AuthorBookDto> GetAuthors();
        public AuthorBookDto GetAuthorById(int Id);
        public Author GetAuthorByName(string name);
        public Author AddAuthor(Author author);
        public Author DeleteAuthor(int id);

    }
}
