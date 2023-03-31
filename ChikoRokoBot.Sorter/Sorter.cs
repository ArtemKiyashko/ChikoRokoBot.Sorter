using System;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Data.Tables;
using Azure.Storage.Queues;
using ChikoRokoBot.Sorter.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ChikoRokoBot.Sorter
{
    public class Sorter
    {
        private readonly QueueClient _queueClient;
        private readonly TableClient _tableClient;
        private readonly IMapper _mapper;
        private readonly string PARTITION_NAME = "primary";

        public Sorter(QueueClient queueClient, TableClient tableClient, IMapper mapper)
        {
            _queueClient = queueClient;
            _tableClient = tableClient;
            _mapper = mapper;
        }

        [FunctionName("Sorter")]
        public async Task Run([QueueTrigger("alldrops", Connection = "AzureWebJobsStorage")]Root myQueueItem, ILogger log)
        {
            var incomingDrops = myQueueItem?.Props?.PageProps?.Drops;
            if (incomingDrops is null) return;

            foreach (var drop in incomingDrops)
            {
                var tableEntityResponse = await _tableClient.GetEntityIfExistsAsync<DropTableEntity>(PARTITION_NAME, drop.Id?.ToString());
                if (tableEntityResponse.HasValue) continue;

                var tableEntity = _mapper.Map<DropTableEntity>(drop);
                tableEntity.PartitionKey = PARTITION_NAME;
                tableEntity.RowKey = drop.Id?.ToString();

                await _tableClient.AddEntityAsync(tableEntity);

                await _queueClient.SendMessageAsync(JsonSerializer.Serialize(drop));                
            }
        }
    }
}

