using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authintication.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Descreption { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public string Gen { get; set; }
        public string CoverUrl { get; set; }
        public DateTime DateAdded { get; set; }
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }
        public List<Book_Author> Book_Authors { get; set; }
        public List<Book_User> Book_Users { get; set; }

    }
}
