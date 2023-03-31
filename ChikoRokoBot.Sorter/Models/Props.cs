using System;
using System.Text.Json.Serialization;

namespace ChikoRokoBot.Sorter.Models
{
    public record Props(
        [property: JsonPropertyName("pageProps")] PageProps PageProps,
        [property: JsonPropertyName("__N_SSP")] bool NSSP
    );
}

