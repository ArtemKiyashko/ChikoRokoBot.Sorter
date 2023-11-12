using System;

namespace ChikoRokoBot.Sorter.Options
{
	public class SorterOptions
	{
		public string NotificationQueueName { get; set; } = "notifydrops";
        public string StorageAccount { get; set; } = "UseDevelopmentStorage=true";
        public string DropsTableName { get; set; } = "drops";
        public string UsersTableName { get; set; } = "users";
        public Uri ChikoRokoBaseAddress { get; set; } = new Uri("https://artoys.app/");
        public string ChikoRokoSessionId { get; set; }
    }
}

