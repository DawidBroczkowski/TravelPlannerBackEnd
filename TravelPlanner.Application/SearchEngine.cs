using Accord.Fuzzy;
using Microsoft.Extensions.DependencyInjection;
using TravelPlanner.Application.Services.Graphs;
using TravelPlanner.Infrastructure.Repositories.Interfaces;
using TravelPlanner.Shared.DTOs.Attraction;
using TravelPlanner.Shared.DTOs.Trail;

namespace TravelPlanner.Application
{
    public class SearchEngine : ISearchEngine
    {
        private readonly IAttractionRepository _attractionRepository;
        private readonly IGraphService _graphService;
        private InferenceSystem _fis = null!;

        // Linguistic variables
        private LinguisticVariable _travelTime = null!;
        private LinguisticVariable _visitTime = null!;
        private LinguisticVariable _popularity = null!;
        private LinguisticVariable _energyExpenditure = null!;
        private LinguisticVariable _score = null!;
        private Database _database = null!;

        public SearchEngine(IServiceProvider serviceProvider)
        {
            _attractionRepository = serviceProvider.GetRequiredService<IAttractionRepository>();
            _graphService = serviceProvider.GetRequiredService<IGraphService>();
            defineVariables();
        }

        public async Task<List<GetAttractionRecommendationDto>> FindNextAttractionAsync(
            List<GetAttractionInTrailDto> attractions,
            List<string> modesOfTransportation,
            string travelTimePreference,
            string visitTimePreference,
            string popularityPreference,
            string energyExpenditurePreference,
            int top)
        {
            defineRules(travelTimePreference, visitTimePreference, popularityPreference, energyExpenditurePreference);

            var provinceId = attractions.Last().Attraction!.Address!.Locality!.Province!.Id;
            var otherAttractions = await _attractionRepository.GetAttractionsAsync(1, int.MaxValue,
                false, cancellationToken: default, provinceId: provinceId);
            otherAttractions = otherAttractions.Where(x => !attractions.Select(y => y.Attraction!.Id).Contains(x.Id)).ToList();

            var graph = await _graphService.GetGraphAsync();
            var start = attractions.Last();
            List<GetAttractionRecommendationDto> nextAttractions = new();
            foreach (var mode in modesOfTransportation)
            {
                foreach (var end in otherAttractions)
                {
                    var route = graph.ModeRoutes[mode][start.Attraction!.Id][end.Id];
                    _fis.SetInput("TravelTime", (float)route.Time);
                    _fis.SetInput("VisitTime", (float)end.AverageVisitDuration.TotalMilliseconds);
                    _fis.SetInput("Popularity", (float)end.Popularity);
                    _fis.SetInput("EnergyExpenditure", (float)end.EnergyLevel);

                    try
                    {
                        var score = _fis.Evaluate("Score");

                        GetAttractionRecommendationDto destination = new()
                        {
                            Attraction = end,
                            TransportationMode = mode,
                            TravelTime = route.Time,
                            TravelDistance = route.Distance,
                            Score = score
                        };
                        nextAttractions.Add(destination);
                    }
                    catch (Exception ex)
                    {
                        // Log the exception and continue
                        Console.WriteLine($"Error evaluating FIS: {ex.Message}");
                    }
                }
            }

            // Get top attractions, for each non-unique attraction, add 1 to top value
            top += nextAttractions.Count - nextAttractions.Select(x => x.Attraction!.Id).Distinct().Count();
            List<GetAttractionRecommendationDto> result = nextAttractions.OrderByDescending(x => x.Score).Take(top).ToList();
            return result;
        }

        // Max values need to be changed
        private void defineVariables()
        {
            // Define the TravelTime linguistic variable
            _travelTime = new LinguisticVariable("TravelTime", 0, 14400000); // Range: 0 to 4 hours
            var veryShort
                = new FuzzySet // 0-5 minutes
                ("VeryShort", new TrapezoidalFunction(0, 0, 0, 300000));
            var shortTime
                = new FuzzySet // 3-20 minutes
                ("Short", new TrapezoidalFunction(180000, 600000, 900000, 1200000));
            var mediumTime
                = new FuzzySet // 15-60 minutes
                ("Medium", new TrapezoidalFunction(900000, 1800000, 3600000));
            var longTime
                = new FuzzySet // 40 minutes to 4 hours
                ("Long", new TrapezoidalFunction(2400000, 7200000, 14400000, 14400000));

            // Add fuzzy sets to TravelTime
            _travelTime.AddLabel(veryShort);
            _travelTime.AddLabel(shortTime);
            _travelTime.AddLabel(mediumTime);
            _travelTime.AddLabel(longTime);

            // Define the VisitTime linguistic variable
            _visitTime = new LinguisticVariable("VisitTime", 0, 21600000); // Range: 0 to 6 hours
            var veryShortVisit = new FuzzySet("VeryShortVisit", new TrapezoidalFunction(0, 0, 0, 900000)); // 0-15 minutes
            var shortVisit = new FuzzySet("ShortVisit", new TrapezoidalFunction(600000, 1800000, 2700000, 3600000)); // 10-60 minutes
            var mediumVisit = new FuzzySet("MediumVisit", new TrapezoidalFunction(2400000, 5400000, 10800000)); // 40 minutes to 3 hours
            var longVisit = new FuzzySet("LongVisit", new TrapezoidalFunction(7200000, 14400000, 21600000, 21600000)); // 2-6 hours

            // Add fuzzy sets to VisitTime
            _visitTime.AddLabel(veryShortVisit);
            _visitTime.AddLabel(shortVisit);
            _visitTime.AddLabel(mediumVisit);
            _visitTime.AddLabel(longVisit);

            // Define the Popularity linguistic variable
            _popularity = new LinguisticVariable("Popularity", 0, 100); // Range: 0 to 100
            var lowPopularity = new FuzzySet("Low", new TrapezoidalFunction(0, 25, 50));
            var mediumPopularity = new FuzzySet("Medium", new TrapezoidalFunction(25, 40, 60, 75));
            var highPopularity = new FuzzySet("High", new TrapezoidalFunction(50, 100, 100));

            // Add fuzzy sets to Popularity
            _popularity.AddLabel(lowPopularity);
            _popularity.AddLabel(mediumPopularity);
            _popularity.AddLabel(highPopularity);

            // Define the EnergyExpenditure linguistic variable
            _energyExpenditure = new LinguisticVariable("EnergyExpenditure", 0, 100); // Range: 0 to 100
            var lowEnergy = new FuzzySet("Low", new TrapezoidalFunction(0, 0, 35));
            var mediumEnergy = new FuzzySet("Medium", new TrapezoidalFunction(25, 50, 75));
            var highEnergy = new FuzzySet("High", new TrapezoidalFunction(50, 100, 100));

            // Add fuzzy sets to EnergyExpenditure
            _energyExpenditure.AddLabel(lowEnergy);
            _energyExpenditure.AddLabel(mediumEnergy);
            _energyExpenditure.AddLabel(highEnergy);

            // Define the Score linguistic variable
            _score = new LinguisticVariable("Score", 0, 100); // Range: 0 to 100
            var lowScore = new FuzzySet("Low", new TrapezoidalFunction(0, 0, 15, 50));
            var mediumScore = new FuzzySet("Medium", new TrapezoidalFunction(25, 40, 60, 75));
            var highScore = new FuzzySet("High", new TrapezoidalFunction(50, 100, 100));

            // Add fuzzy sets to Score
            _score.AddLabel(lowScore);
            _score.AddLabel(mediumScore);
            _score.AddLabel(highScore);

            _database = new Database();
            _database.AddVariable(_travelTime);
            _database.AddVariable(_visitTime);
            _database.AddVariable(_popularity);
            _database.AddVariable(_energyExpenditure);
            _database.AddVariable(_score);
        }

        private void defineRules(string travelTimePreference, string visitTimePreference,
            string popularityPreference, string energyExpenditurePreference)
        {
            _fis = new InferenceSystem(_database, new CentroidDefuzzifier(100));

            // Add rules for TravelTime
            DefineTravelTimeRules(travelTimePreference);

            // Add rules for VisitTime
            DefineVisitTimeRules(visitTimePreference);

            // Add rules for Popularity
            DefinePopularityRules(popularityPreference);

            // Add rules for EnergyExpenditure
            DefineEnergyExpenditureRules(energyExpenditurePreference);
        }

        private void DefineTravelTimeRules(string preference)
        {
            if (preference == "short")
            {
                _fis.NewRule("Rule1", "IF TravelTime IS VeryShort OR TravelTime IS Short THEN Score IS High");
                _fis.NewRule("Rule2", "IF TravelTime IS Medium THEN Score IS Medium");
                _fis.NewRule("Rule3", "IF TravelTime IS Long THEN Score IS Low");
            }
            else if (preference == "medium")
            {
                _fis.NewRule("Rule4", "IF TravelTime IS VeryShort THEN Score IS Low");
                _fis.NewRule("Rule5", "IF TravelTime IS Medium THEN Score IS High");
                _fis.NewRule("Rule6", "IF TravelTime IS Long THEN Score IS Low");
            }
            else if (preference == "long")
            {
                _fis.NewRule("Rule7", "IF TravelTime IS Long THEN Score IS High");
            }
            else if (preference == "none")
            {
                _fis.NewRule("DefaultTravelTimeRule", "IF TravelTime IS VeryShort OR TravelTime IS Short OR TravelTime IS Medium OR TravelTime IS Long THEN Score IS Medium");
            }
        }

        private void DefineVisitTimeRules(string preference)
        {
            if (preference == "short")
            {
                _fis.NewRule("Rule8", "IF VisitTime IS VeryShortVisit OR VisitTime IS ShortVisit THEN Score IS High");
                _fis.NewRule("Rule9", "IF VisitTime IS MediumVisit THEN Score IS Medium");
                _fis.NewRule("Rule10", "IF VisitTime IS LongVisit THEN Score IS Low");
            }
            else if (preference == "medium")
            {
                _fis.NewRule("Rule11", "IF VisitTime IS VeryShortVisit THEN Score IS Low");
                _fis.NewRule("Rule12", "IF VisitTime IS MediumVisit THEN Score IS High");
                _fis.NewRule("Rule13", "IF VisitTime IS LongVisit THEN Score IS Medium");
            }
            else if (preference == "long")
            {
                _fis.NewRule("Rule14", "IF VisitTime IS LongVisit THEN Score IS High");
            }
            else if (preference == "none")
            {
                _fis.NewRule("DefaultVisitTimeRule", "IF VisitTime IS VeryShortVisit OR VisitTime IS ShortVisit OR VisitTime IS MediumVisit OR VisitTime IS LongVisit THEN Score IS Medium");
            }
        }

        private void DefinePopularityRules(string preference)
        {
            if (preference == "low")
            {
                _fis.NewRule("Rule15", "IF Popularity IS Low THEN Score IS High");
                _fis.NewRule("Rule16", "IF Popularity IS Medium THEN Score IS Medium");
                _fis.NewRule("Rule17", "IF Popularity IS High THEN Score IS Low");
            }
            else if (preference == "medium")
            {
                _fis.NewRule("Rule18", "IF Popularity IS Low THEN Score IS Low");
                _fis.NewRule("Rule19", "IF Popularity IS Medium THEN Score IS High");
                _fis.NewRule("Rule20", "IF Popularity IS High THEN Score IS Medium");
            }
            else if (preference == "high")
            {
                _fis.NewRule("Rule21", "IF Popularity IS High THEN Score IS High");
            }
            else if (preference == "none")
            {
                _fis.NewRule("DefaultPopularityRule", "IF Popularity IS Low " +
                    "OR Popularity IS Medium OR Popularity IS High THEN Score IS Medium");
            }
        }

        private void DefineEnergyExpenditureRules(string preference)
        {
            if (preference == "low")
            {
                _fis.NewRule("Rule22", "IF EnergyExpenditure IS Low THEN Score IS High");
                _fis.NewRule("Rule23", "IF EnergyExpenditure IS Medium THEN Score IS Medium");
                _fis.NewRule("Rule24", "IF EnergyExpenditure IS High THEN Score IS Low");
            }
            else if (preference == "medium")
            {
                _fis.NewRule("Rule25", "IF EnergyExpenditure IS Low THEN Score IS Low");
                _fis.NewRule("Rule26", "IF EnergyExpenditure IS Medium THEN Score IS High");
                _fis.NewRule("Rule27", "IF EnergyExpenditure IS High THEN Score IS Medium");
            }
            else if (preference == "high")
            {
                _fis.NewRule("Rule28", "IF EnergyExpenditure IS High THEN Score IS High");
            }
            else if (preference == "none")
            {
                _fis.NewRule("DefaultEnergyExpenditureRule", "IF EnergyExpenditure IS Low OR EnergyExpenditure IS Medium OR EnergyExpenditure IS High THEN Score IS Medium");
            }
        }
    }
}
