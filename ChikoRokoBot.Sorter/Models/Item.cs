using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ChikoRokoBot.Sorter.Models
{
    public record Item(
        [property: JsonProperty("id")]
        [property: JsonPropertyName("id")] int Id,
        [property: JsonProperty("title")]
        [property: JsonPropertyName("title")] string Title,
        [property: JsonProperty("description")]
        [property: JsonPropertyName("description")] string Description,
        [property: JsonProperty("created")]
        [property: JsonPropertyName("created")] DateTime Created,
        [property: JsonProperty("finish")]
        [property: JsonPropertyName("finish")] DateTime Finish,
        [property: JsonProperty("start")]
        [property: JsonPropertyName("start")] DateTime Start,
        [property: JsonProperty("toyid")]
        [property: JsonPropertyName("toyid")] int? Toyid,
        [property: JsonProperty("mechanic")]
        [property: JsonPropertyName("mechanic")] string Mechanic,
        [property: JsonProperty("allowedCountries")]
        [property: JsonPropertyName("allowedCountries")] IReadOnlyList<object> AllowedCountries,
        [property: JsonProperty("blindBoxId")]
        [property: JsonPropertyName("blindBoxId")] int? BlindBoxId,
        [property: JsonProperty("alternativeCollect")]
        [property: JsonPropertyName("alternativeCollect")] string AlternativeCollect,
        [property: JsonProperty("order")]
        [property: JsonPropertyName("order")] int Order
    )
    {
        [JsonPropertyName("toy")]
        public Toy Toy { get; set; }
    }
}

