using BookApiService.Core.Enums;
using BookApiService.Core.Models.Interfaces;

namespace BookApiService.Core.Models
{
    public class Book : IBook
    {
        public int Id { get; set; }
        public string Author { get; set ; }
        public string Title { get ; set; }
        public int PublicationYear { get; set; }
        public Bookbindings Bookbinding {get; set; }
        public AgeCategories AgeCategorie { get; set; }
        public Genres[] Genres { get; set; }
    }
}
