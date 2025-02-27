namespace TravelPlanner.Domain.Models.Attractions.Location
{
    public record ZipCode
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public virtual Locality Locality { get; set; } = new();
    }
}