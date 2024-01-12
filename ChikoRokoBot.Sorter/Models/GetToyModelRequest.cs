using System.Text.Json.Serialization;

namespace ChikoRokoBot.Sorter.Models
{
    public record RequestValueToy(
        [property: JsonPropertyName("json")] RequestParameterToyId Parameter
    );

    public record RequestParameterToyId(
        [property: JsonPropertyName("toyId")] int ToyId
    );
}

