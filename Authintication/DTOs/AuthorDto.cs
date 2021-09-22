using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authintication.DTOs
{
    public class AuthorDto
    {
        public string FullName { get; set; }

    }
    public class AuthorBookDto
    {
        public string FullName { get; set; }
        public List<string> BookTiltes { get; set; }


    }
}
