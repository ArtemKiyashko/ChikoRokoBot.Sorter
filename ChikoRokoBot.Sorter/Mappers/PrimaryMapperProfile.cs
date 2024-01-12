using System.Text.Json;
using AutoMapper;
using ChikoRokoBot.Sorter.Models;

namespace ChikoRokoBot.Sorter.Mappers
{
	public class PrimaryMapperProfile : Profile
	{
		public PrimaryMapperProfile()
		{
			CreateMap<Item, DropTableEntity>()
				.ForMember(entity => entity.Slug, opt => opt.MapFrom(drop => drop.Toy.Slug))
				.ForMember(entity => entity.RarityType, opt => opt.MapFrom(drop => drop.Toy.RarityType))
                .ForMember(entity => entity.DropJson, opt => opt.MapFrom(drop => JsonSerializer.Serialize(drop, new JsonSerializerOptions { WriteIndented = false })))
				.ForMember(entity => entity.ModelUrlGlb, opt => opt.MapFrom(drop => drop.Toy.ModelUrlGlb))
                .ForMember(entity => entity.ModelUrlUsdz, opt => opt.MapFrom(drop => drop.Toy.ModelUrlUsdz));

			CreateMap<DropTableEntity, Item>()
				.ConstructUsing((entity, context) => { return JsonSerializer.Deserialize<Item>(entity.DropJson, (JsonSerializerOptions)null); });
		}
	}
}

