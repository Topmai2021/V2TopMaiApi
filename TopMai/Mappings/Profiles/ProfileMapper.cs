using AutoMapper;
using TopMai.Domain.DTO.Profiles;
using ProfileUser = Infraestructure.Entity.Entities.Profiles.Profile;
using Image = Infraestructure.Entity.Entities.Profiles.Image;
using Gender = Infraestructure.Entity.Entities.Profiles.Gender;

namespace TopMai.Mappings.Profiles;

internal sealed class ProfileMapper : Profile
{
	public ProfileMapper()
	{
		CreateMap<ConsultProfileDto, ProfileUser>()
			.ForMember(dest => dest.Image, opt => opt.MapFrom(src => GetImage(src.UrlImage)))
			.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => new Gender() { Name = src.StrGender}))
			.ReverseMap();
	}

	private static Image? GetImage(string urlImage) =>
		new()
		{
			UrlImage = urlImage,
		};
}
