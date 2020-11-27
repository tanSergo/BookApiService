using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiService.Api.DTOs
{
    public class CreateBookDto
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public int PublicationYear { get; set; }
        public string Bookbinding { get; set; }
        public string AgeCategorie { get; set; }
        public string[] Genres { get; set; }
    }
}
