using BookApiService.Core.Enums;

namespace BookApiService.Core.Models.Interfaces
{
    public interface IBook
    {
        public int Id { get; set; }
        string Author { get; set; }
        string Title { get; set; }
        int PublicationYear { get; set; }
        public AgeCategories AgeCategorie { get; set; }
        public Genres[] Genres { get; set; }
    }
}
