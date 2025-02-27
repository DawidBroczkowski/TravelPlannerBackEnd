namespace TravelPlanner.Domain.Models.Attractions.Location
{
    public record Country
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public virtual List<Province> Provinces { get; set; } = new();
    }
}
