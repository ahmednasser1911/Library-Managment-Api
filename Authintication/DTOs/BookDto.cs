using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authintication.DTOs
{
    public class BookDto
    {
        public string Title { get; set; }
        public string Descreption { get; set; }
        public bool IsRead { get; set; }
        public string Gen { get; set; }
        public string CoverUrl { get; set; }
        public DateTime? DateRead { get; set; }
        public int PublisherId { get; set; }
        public List<int> AuthorIds { get; set; }

    }

    public class BookAuthorDto
    {
        public string Title { get; set; }
        public string Descreption { get; set; }
        public bool IsRead { get; set; }
        public string Gen { get; set; }
        public string CoverUrl { get; set; }
        public DateTime? DateRead { get; set; }
        public string PublisherName { get; set; }
        public List<string> AuthorNames { get; set; }

    }
}
