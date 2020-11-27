namespace BookApiService.Core.Models.Interfaces
{
    interface IFilter
    {
        string property { get; set; }
        string value { get; set; }
    }
}
