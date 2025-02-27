using System.Text.Json.Serialization;

namespace TravelPlanner.Domain.Models.Graphs
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public record Details
    {
    }

    public record Hints
    {
        [JsonPropertyName("visited_nodes.sum")]
        public int VisitedNodesSum { get; set; }

        [JsonPropertyName("visited_nodes.average")]
        public double VisitedNodesAverage { get; set; }
    }

    public record Info
    {
        [JsonPropertyName("copyright")]
        public List<string> Copyrights { get; set; } = new List<string>();
        [JsonPropertyName("took")]
        public int Took { get; set; }
    }

    public record Instruction
    {
        [JsonPropertyName("distance")]
        public double Distance { get; set; }
        [JsonPropertyName("heading")]
        public double Heading { get; set; }
        [JsonPropertyName("sign")]
        public int Sign { get; set; }
        [JsonPropertyName("interval")]
        public List<int> Interval { get; set; } = new List<int>();
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
        [JsonPropertyName("time")]
        public int Time { get; set; }
        [JsonPropertyName("street_name")]
        public string StreetName { get; set; } = string.Empty;
        [JsonPropertyName("exit_number")]
        public int? ExitNumber { get; set; }
        [JsonPropertyName("exited")]
        public bool? Exited { get; set; }
        [JsonPropertyName("turn_angle")]
        public double? TurnAngle { get; set; }
        [JsonPropertyName("last_heading")]
        public double? LastHeading { get; set; } = new double?();
    }

    public record Path
    {
        [JsonPropertyName("distance")]
        public double Distance { get; set; }
        [JsonPropertyName("weight")]
        public double Weight { get; set; }
        [JsonPropertyName("time")]
        public int Time { get; set; }
        [JsonPropertyName("transfers")]
        public int Transfers { get; set; }
        [JsonPropertyName("points_encoded")]
        public bool PointsEncoded { get; set; }
        [JsonPropertyName("bbox")]
        public List<double> Bbox { get; set; } = new List<double>();
        [JsonPropertyName("points")]
        public Points Points { get; set; } = new Points();
        [JsonPropertyName("instructions")]
        public List<Instruction> Instructions { get; set; } = new List<Instruction>();
        [JsonPropertyName("legs")]
        public List<object> Legs { get; set; } = new List<object>();
        [JsonPropertyName("details")]
        public Details Details { get; set; } = new Details();
        [JsonPropertyName("ascend")]
        public double Ascend { get; set; }
        [JsonPropertyName("descend")]
        public double Descend { get; set; }
        [JsonPropertyName("snapped_waypoints")]
        public SnappedWaypoints SnappedWaypoints { get; set; } = new SnappedWaypoints();
    }

    public record Points
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        [JsonPropertyName("coordinates")]
        public List<List<double>> Coordinates { get; set; } = new List<List<double>>();
    }

    public record RouteResponse
    {
        [JsonPropertyName("hints")]
        public Hints Hints { get; set; } = new Hints();
        [JsonPropertyName("info")]
        public Info Info { get; set; } = new Info();
        [JsonPropertyName("paths")]
        public List<Path> Paths { get; set; } = new List<Path>();
    }

    public record SnappedWaypoints
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        [JsonPropertyName("coordinates")]
        public List<List<double>> Coordinates { get; set; } = new List<List<double>>();
    }


}
