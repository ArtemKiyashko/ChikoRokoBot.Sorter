using System.Text.Json.Serialization;

namespace ChikoRokoBot.Sorter.Models
{
    public record Data(
        [property: JsonPropertyName("json")] Json Json
    );

    public record Json(
        [property: JsonPropertyName("toyId")] int ToyId,
        [property: JsonPropertyName("archiveName")] string ArchiveName,
        [property: JsonPropertyName("hash")] string Hash
    );

    public record Result(
        [property: JsonPropertyName("data")] Data Data
    );

    public record GetToyModelResponse(
        [property: JsonPropertyName("result")] Result Result
    );
}

