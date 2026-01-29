using System.Text.Json.Serialization;

namespace PoolBrackets_backend_dotnet.DTOs
{
    public class AddEventDto
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("venue")]
        public required string Venue { get; set; }

        [JsonPropertyName("location")]
        public required string Location { get; set; }

        [JsonPropertyName("date")]
        public required DateTime Date { get; set; }

        [JsonPropertyName("number_of_players")]
        public int? NumberOfPlayers { get; set; }

        [JsonPropertyName("slogan")]
        public string? Slogan { get; set; }

        [JsonPropertyName("format")]
        public string? Format { get; set; }

        [JsonPropertyName("entry_fee")]
        public decimal? EntryFee { get; set; }

        [JsonPropertyName("total_prize")]
        public decimal? TotalPrize { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}