using BookApiService.Core.Models.Interfaces;

namespace BookApiService.Core.Models
{
    public class Filter : IFilter
    {
        public string property { get; set; }
        public string value { get; set; }
    }
}
