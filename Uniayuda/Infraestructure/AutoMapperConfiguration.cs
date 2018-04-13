using Uniayuda.Models;
using AutoMapper;
using Cross;
using Entities.Entities;
using Entities.Enums;
using System;
using System.Linq;

namespace Uniayuda.Infraestructure
{
    public class AutoMapperConfiguration
    {
        public static IMapper _mapper;
        public static void ConfigureMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, ProfileViewModel>()
                .ForMember(x => x.CountryId, y => y.MapFrom(x => x.CountryId.ToString()))
                .ForMember(x => x.ProfessionId, y => y.MapFrom(x => x.ProfessionId.ToString()))
                .ForMember(x => x.URLPhoto, y => y.MapFrom(x => x.Photos.Any(z => z.Active) ? x.Photos.FirstOrDefault(z => z.Active).Path : string.Empty))
                .ForMember(x => x.Investment, y => y.MapFrom(x => x.Purchases.Where(z => z.State == PurchaseStatus.Approved).Sum(z => z.Value)))
                .ForMember(x => x.Professions, y => y.Ignore())
                .ForMember(x => x.Countries, y => y.Ignore());

                cfg.CreateMap<ProfileViewModel, User>()
                .ForMember(x => x.TwoFactorEnabled, y => y.Ignore())
                .ForMember(x => x.SecurityStamp, y => y.Ignore())
                .ForMember(x => x.Roles, y => y.Ignore())
                .ForMember(x => x.Purchases, y => y.Ignore())
                .ForMember(x => x.ProfessionId, y => y.MapFrom(x => Guid.Parse(x.ProfessionId)))
                .ForMember(x => x.PhoneNumberConfirmed, y => y.Ignore())
                .ForMember(x => x.PhoneNumber, y => y.Ignore())
                .ForMember(x => x.PasswordHash, y => y.Ignore())
                .ForMember(x => x.Logins, y => y.Ignore())
                .ForMember(x => x.LockoutEndDateUtc, y => y.Ignore())
                .ForMember(x => x.LockoutEnabled, y => y.Ignore())
                .ForMember(x => x.LastEmailResended, y => y.Ignore())
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.EmailConfirmed, y => y.Ignore())
                .ForMember(x => x.CountryId, y => y.MapFrom(x => Guid.Parse(x.CountryId)))
                .ForMember(x => x.Claims, y => y.Ignore())
                .ForMember(x => x.AccessFailedCount, y => y.Ignore())
                .ForMember(x => x.Email, y => y.Ignore())
                .ForMember(x => x.UserName, y => y.Ignore())
                .ForMember(x => x.Country, y => y.Ignore())
                .ForMember(x => x.Profession, y => y.Ignore())
                .ForMember(x => x.LastTimePasswordRestored, y => y.Ignore())
                .ForMember(x => x.Photos, y => y.Ignore());

                cfg.CreateMap<RegisterViewModel, User>()
                .ForMember(x => x.TwoFactorEnabled, y => y.Ignore())
                .ForMember(x => x.Sentence, y => y.Ignore())
                .ForMember(x => x.SecurityStamp, y => y.Ignore())
                .ForMember(x => x.Roles, y => y.Ignore())
                .ForMember(x => x.Purchases, y => y.Ignore())
                .ForMember(x => x.ProfessionId, y => y.MapFrom(x => Guid.Parse(Constants.ProfessionNone)))
                .ForMember(x => x.Pleasures, y => y.Ignore())
                .ForMember(x => x.PhoneNumberConfirmed, y => y.Ignore())
                .ForMember(x => x.PhoneNumber, y => y.Ignore())
                .ForMember(x => x.PersonalLink, y => y.Ignore())
                .ForMember(x => x.PasswordHash, y => y.Ignore())
                .ForMember(x => x.Name, y => y.Ignore())
                .ForMember(x => x.Logins, y => y.Ignore())
                .ForMember(x => x.LockoutEndDateUtc, y => y.Ignore())
                .ForMember(x => x.LockoutEnabled, y => y.Ignore())
                .ForMember(x => x.LastName, y => y.Ignore())
                .ForMember(x => x.LastEmailResended, y => y.Ignore())
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.EmailConfirmed, y => y.Ignore())
                .ForMember(x => x.CountryId, y => y.MapFrom(x => Guid.Parse(Constants.CountryNone)))
                .ForMember(x => x.Claims, y => y.Ignore())
                .ForMember(x => x.BornDate, y => y.Ignore())
                .ForMember(x => x.AccessFailedCount, y => y.Ignore())
                .ForMember(x => x.Country, y => y.Ignore())
                .ForMember(x => x.Profession, y => y.Ignore())
                .ForMember(x => x.LastTimePasswordRestored, y => y.Ignore())
                .ForMember(x => x.Photos, y => y.Ignore());
            });

            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();
        }
    }
}