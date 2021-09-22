using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authintication.DTOs
{
    public class PublisherDto
    {
        public string Name { get; set; }

    }

    public class PublisherBooksDto
    {
        public string Name { get; set; }
        public List<BooksAuthorsDto> BooksAuthors { get; set; }

    }
    public class BooksAuthorsDto
    {
        public string BookName { get; set; }
        public List<string> BookAuthors { get; set; }

    }
}
