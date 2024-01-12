using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ChikoRokoBot.Sorter.Models;
using Microsoft.Extensions.Logging;

namespace ChikoRokoBot.Sorter.Clients
{
	public class ChikoRokoClient
	{
        private readonly HttpClient _httpClient;
        private readonly ILogger<ChikoRokoClient> _logger;

        public ChikoRokoClient(HttpClient httpClient, ILogger<ChikoRokoClient> logger)
		{
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IDictionary<ModelUrlType, string>> GetModelUrls(int toyId)
        {
            var result = new Dictionary<ModelUrlType, string>();
            var request = new RequestValueToy(new RequestParameterToyId(toyId));
            var dictionaryRequest = new Dictionary<int, RequestValueToy>
            {
                { 0, request }
            };
            var json = JsonSerializer.Serialize(dictionaryRequest);

            try
            {
                var response = await _httpClient.GetStringAsync($"api/v2/ToyManager.getModel?batch=1&input={json}");
                var responseObject = JsonSerializer.Deserialize<List<GetResponse<ToyModelResponse>>>(response);

                if (string.IsNullOrEmpty(responseObject[0].Result.Data.Json.Hash))
                {
                    result.Add(ModelUrlType.usdz, $"https://chikoroko.b-cdn.net/toys/models/{responseObject[0].Result.Data.Json.ArchiveName}/scene.usdz");
                    result.Add(ModelUrlType.glb, $"https://chikoroko.b-cdn.net/toys/models/{responseObject[0].Result.Data.Json.ArchiveName}/scene.glb");
                }
                else
                {
                    result.Add(ModelUrlType.usdz, $"https://chikoroko.b-cdn.net/toys/models/{responseObject[0].Result.Data.Json.ArchiveName}/{responseObject[0].Result.Data.Json.Hash}/scene.usdz");
                    result.Add(ModelUrlType.glb, $"https://chikoroko.b-cdn.net/toys/models/{responseObject[0].Result.Data.Json.ArchiveName}/{responseObject[0].Result.Data.Json.Hash}/scene.glb");
                }

                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Cannot get toy model link", ex);
                return result;
            }
        }

        public async Task<IReadOnlyList<Toy>> GetToys(IEnumerable<int> Ids)
        {
            var result = new List<Toy>();

            var i = 0;

            var request = Ids.Select(id => new RequestValueId(new RequestParameterId(id))).ToDictionary((val) => i++);

            var json = JsonSerializer.Serialize(request);

            try
            {
                var response = await _httpClient.GetStringAsync($"api/v2/{Ids.Select(id => "ToyManager.getSingle").Aggregate((f, s) => f + "," + s)}?batch=1&input={json}");
                var responseObject = JsonSerializer.Deserialize<List<GetResponse<Toy>>>(response);

                return responseObject.Select(toy => toy.Result.Data.Json).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot get toy info", ex);
                return result;
            }
        }
    }
}

