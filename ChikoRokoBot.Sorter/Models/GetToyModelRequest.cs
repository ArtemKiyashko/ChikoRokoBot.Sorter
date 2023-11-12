using System.Text.Json.Serialization;

namespace ChikoRokoBot.Sorter.Models
{
    public record RequestValue(
        [property: JsonPropertyName("json")] RequestParameter Parameter
    );

    public record RequestParameter(
        [property: JsonPropertyName("toyId")] int ToyId
    );
}

