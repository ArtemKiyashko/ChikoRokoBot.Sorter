using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ChikoRokoBot.Sorter.Models
{
    public record Drop(
        [property: JsonPropertyName("items")] IReadOnlyList<Item> Items
    );
}

