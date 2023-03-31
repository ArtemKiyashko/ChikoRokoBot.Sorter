using System;
using AutoMapper;
using ChikoRokoBot.Sorter.Models;

namespace ChikoRokoBot.Sorter.Mappers
{
	public class PrimaryMapperProfile : Profile
	{
		public PrimaryMapperProfile()
		{
			CreateMap<Drop, DropTableEntity>().ForMember(entity => entity.Slug, opt => opt.MapFrom(drop => drop.Toy.Slug));
		}
	}
}

