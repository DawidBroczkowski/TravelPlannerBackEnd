﻿namespace TravelPlanner.Shared.DTOs.Attraction.Location
{
    public record GetProvinceDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? CountryId { get; set; }
        public GetCountryDto? Country { get; set; }
    }
}
