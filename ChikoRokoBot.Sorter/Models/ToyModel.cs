using System;
using System.Text.Json.Serialization;

namespace ChikoRokoBot.Sorter.Models
{
    public record ToyModel(
        [property: JsonPropertyName("toyId")] int? ToyId,
        [property: JsonPropertyName("archiveName")] string ArchiveName
    );
}

