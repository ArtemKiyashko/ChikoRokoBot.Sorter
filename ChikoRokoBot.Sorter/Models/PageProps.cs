using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ChikoRokoBot.Sorter.Models
{
    public record PageProps(
        [property: JsonPropertyName("query")] Query Query,
        [property: JsonPropertyName("agent")] string Agent,
        [property: JsonPropertyName("accept")] Accept Accept,
        [property: JsonPropertyName("session")] object? Session,
        [property: JsonPropertyName("profile")] object? Profile,
        [property: JsonPropertyName("collections")] IReadOnlyList<ToyCollection> Collections,
        [property: JsonPropertyName("artists")] IReadOnlyList<Artist> Artists,
        [property: JsonPropertyName("socials")] IReadOnlyList<Social> Socials,
        [property: JsonPropertyName("media")] IReadOnlyList<Social> Media,
        [property: JsonPropertyName("initialDrops")] Drop InitialDrops,
        [property: JsonPropertyName("banner")] Banner Banner
    );
}

