using System.Text.Json.Serialization;

namespace ChikoRokoBot.Sorter.Models
{
    public record ToyModelResponse(
        [property: JsonPropertyName("toyId")] int ToyId,
        [property: JsonPropertyName("archiveName")] string ArchiveName,
        [property: JsonPropertyName("hash")] string Hash
    );
}

