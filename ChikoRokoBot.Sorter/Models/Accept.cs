using System;
using System.Text.Json.Serialization;

namespace ChikoRokoBot.Sorter.Models
{
    public record Accept(
        [property: JsonPropertyName("webp")] bool Webp
    );
}

