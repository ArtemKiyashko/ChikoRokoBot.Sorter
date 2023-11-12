using System.Net;
using System.Net.Http;
using Azure.Identity;
using Azure.Storage.Queues;
using ChikoRokoBot.Sorter.Clients;
using ChikoRokoBot.Sorter.Mappers;
using ChikoRokoBot.Sorter.Options;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(ChikoRokoBot.Sorter.Startup))]
namespace ChikoRokoBot.Sorter
{
    public class Startup : FunctionsStartup
    {
        private IConfigurationRoot _functionConfig;
        private SorterOptions _sorterOptions = new();

        public override void Configure(IFunctionsHostBuilder builder)
        {
            _functionConfig = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            builder.Services.Configure<SorterOptions>(_functionConfig.GetSection("SorterOptions"));
            _functionConfig.GetSection("SorterOptions").Bind(_sorterOptions);

            builder.Services.AddAzureClients(clientBuilder => {
                clientBuilder.UseCredential(new DefaultAzureCredential());
                clientBuilder
                    .AddQueueServiceClient(_sorterOptions.StorageAccount)
                    .ConfigureOptions((options) => { options.MessageEncoding = QueueMessageEncoding.Base64; });
                clientBuilder.AddTableServiceClient(_sorterOptions.StorageAccount);
            });

            builder.Services.AddScoped<QueueClient>((factory) => {
                var service = factory.GetRequiredService<QueueServiceClient>();
                var client = service.GetQueueClient(_sorterOptions.NotificationQueueName);
                client.CreateIfNotExists();
                return client;
            });

            builder.Services.AddAutoMapper(typeof(PrimaryMapperProfile));



            builder.Services
                .AddHttpClient<ChikoRokoClient>(client => client.BaseAddress = _sorterOptions.ChikoRokoBaseAddress)
                .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var messageHandler = new HttpClientHandler();
                var cookieContainer = new CookieContainer();

                cookieContainer.Add(_sorterOptions.ChikoRokoBaseAddress, new Cookie("sessionid", _sorterOptions.ChikoRokoSessionId));

                messageHandler.CookieContainer = cookieContainer;
                messageHandler.UseCookies = true;

                return messageHandler;
            });
        }
    }
}

