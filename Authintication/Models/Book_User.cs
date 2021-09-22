using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authintication.Models
{
    public class Book_User
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }

    }
}
