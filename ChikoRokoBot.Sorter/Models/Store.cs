﻿using System;
using System.Text.Json.Serialization;

namespace ChikoRokoBot.Sorter.Models
{
    public record Store(
        [property: JsonPropertyName("id")] int? Id,
        [property: JsonPropertyName("toyid")] int? Toyid,
        [property: JsonPropertyName("profileid")] int? Profileid,
        [property: JsonPropertyName("price")] int? Price,
        [property: JsonPropertyName("published")] DateTime Published,
        [property: JsonPropertyName("storetype")] string Storetype,
        [property: JsonPropertyName("instanceid")] int? Instanceid
    );
}

