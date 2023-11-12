using System.Text.Json;
using AutoMapper;
using ChikoRokoBot.Sorter.Models;

namespace ChikoRokoBot.Sorter.Mappers
{
	public class PrimaryMapperProfile : Profile
	{
		public PrimaryMapperProfile()
		{
			CreateMap<Drop, DropTableEntity>()
				.ForMember(entity => entity.Slug, opt => opt.MapFrom(drop => drop.Toy.Slug))
				.ForMember(entity => entity.RarityType, opt => opt.MapFrom(drop => drop.Toy.RarityType))
                .ForMember(entity => entity.DropJson, opt => opt.MapFrom(drop => JsonSerializer.Serialize(drop, new JsonSerializerOptions { WriteIndented = false })));

			CreateMap<DropTableEntity, Drop>()
				.ConstructUsing((entity, context) => { return JsonSerializer.Deserialize<Drop>(entity.DropJson, (JsonSerializerOptions)null); });
		}
	}
}

