using System.Text.Json.Serialization;

namespace ChikoRokoBot.Sorter.Models
{
    public record RequestValueId(
        [property: JsonPropertyName("json")] RequestParameterId Parameter
    );

    public record RequestParameterId(
        [property: JsonPropertyName("id")] int ToyId
    );
}

