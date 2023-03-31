using System;
using System.Text.Json.Serialization;

namespace ChikoRokoBot.Sorter.Models
{
    public record Query(
        [property: JsonPropertyName("slug")] string Slug
    );
}

