using System;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Azure;
using Azure.Data.Tables;
using Azure.Storage.Queues;
using ChikoRokoBot.Sorter.Clients;
using ChikoRokoBot.Sorter.Models;
using ChikoRokoBot.Sorter.Options;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChikoRokoBot.Sorter
{
    public class Sorter
    {
        private readonly QueueClient _queueClient;
        private readonly TableClient _dropTable;
        private readonly TableClient _userTable;
        private readonly IMapper _mapper;
        private readonly ChikoRokoClient _chikoRokoClient;
        private const string PARTITION_NAME = "primary";
        private readonly ILogger _logger;

        public Sorter(
            QueueClient queueClient,
            TableServiceClient tableServiceClient,
            IMapper mapper,
            IOptions<SorterOptions> options,
            ChikoRokoClient chikoRokoClient,
            ILogger<Sorter> logger)
        {
            _queueClient = queueClient;
            _dropTable = tableServiceClient.GetTableClient(options.Value.DropsTableName);
            _dropTable.CreateIfNotExists();
            _userTable = tableServiceClient.GetTableClient(options.Value.UsersTableName);
            _userTable.CreateIfNotExists();
            _mapper = mapper;
            _chikoRokoClient = chikoRokoClient;
            _logger = logger;
        }

        [FunctionName("Sorter")]
        public async Task Run([QueueTrigger("alldrops", Connection = "AzureWebJobsStorage")]Root myQueueItem)
        {
            var incomingDrops = myQueueItem?.Props?.PageProps?.Drops;
            if (incomingDrops is null) return;

            var users = _userTable.QueryAsync<UserTableEntity>(u => u.PartitionKey == PARTITION_NAME);

            foreach (var drop in incomingDrops)
            {
                var tableEntityResponse = await _dropTable.GetEntityIfExistsAsync<DropTableEntity>(PARTITION_NAME, drop.Id?.ToString());
                if (tableEntityResponse.HasValue) continue;

                var tableEntity = await CreateTableEntity(drop);

                if (drop.Toy is not null)
                {
                    drop.Toy.ModelUrlUsdz = tableEntity.ModelUrlUsdz;
                    drop.Toy.ModelUrlGlb = tableEntity.ModelUrlGlb;
                }

                await _dropTable.AddEntityAsync(tableEntity);
                await SendToAllUsers(users, drop);
            }
        }

        private async Task<DropTableEntity> CreateTableEntity(Drop drop)
        {
            var tableEntity = _mapper.Map<DropTableEntity>(drop);
            tableEntity.PartitionKey = PARTITION_NAME;
            tableEntity.RowKey = drop.Id?.ToString();

            if (tableEntity.Toyid.HasValue)
            {
                var modelUrls = await _chikoRokoClient.GetModelUrls(tableEntity.Toyid.Value);
                tableEntity.ModelUrlUsdz = modelUrls.ContainsKey(ModelUrlType.usdz) ? modelUrls[ModelUrlType.usdz] : default;
                tableEntity.ModelUrlGlb = modelUrls.ContainsKey(ModelUrlType.glb) ? modelUrls[ModelUrlType.glb] : default;
            }
            return tableEntity;
        }

        private async Task SendToAllUsers(AsyncPageable<UserTableEntity> users, Drop drop)
        {
            await foreach (var user in users)
            {
                var userDrop = new UserDrop(user.ChatId, user.TopicId, drop);
                try
                {
                    await _queueClient.SendMessageAsync(JsonSerializer.Serialize(userDrop));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending to notification queue. ChatId: {0}; TopicId: {1}; DropId: {2}", user.ChatId, user.TopicId, drop.Id);
                }
            }
        }
    }
}

