using System;
namespace ChikoRokoBot.Sorter.Models
{
	public record UserDrop(
        long? ChatId,
        int? TopicId,
        Drop Drop);
}

