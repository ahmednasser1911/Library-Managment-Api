using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authintication.DTOs
{
    public class AddBooksToUserDto
    {
        public string Email { get; set; }
        public int BookId { get; set; }

    }

    public class AddBooksToUserVm
    {
        public string UserName { get; set; }
        public List<string> BookTitles { get; set; }

    }
}
