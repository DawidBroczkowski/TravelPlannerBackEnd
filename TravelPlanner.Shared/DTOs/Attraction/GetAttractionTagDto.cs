﻿namespace TravelPlanner.Shared.DTOs.Attraction
{
    public record GetAttractionTagDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
