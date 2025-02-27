namespace TravelPlanner.Shared.DTOs.Attraction.Location
{
    public record GetAddressDto
    {
        public int Id { get; set; }
        public string Street { get; set; } = string.Empty;
        public GetLocalityDto? Locality { get; set; }
        public int? LocalityId { get; set; }
        public int? ProvinceId { get; set; }
        public int? CountryId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
