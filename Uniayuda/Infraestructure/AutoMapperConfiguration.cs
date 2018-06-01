using AutoMapper;
using Cross.Helpers;
using Entities.DatabaseEntities;
using Uniayuda.Models;

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
                .ForMember(x => x.IsFromDashboard, y => y.Ignore());

                cfg.CreateMap<ProfileViewModel, User>()
                .ForMember(x => x.TwoFactorEnabled, y => y.Ignore())
                .ForMember(x => x.SecurityStamp, y => y.Ignore())
                .ForMember(x => x.Roles, y => y.Ignore())
                .ForMember(x => x.PhoneNumberConfirmed, y => y.Ignore())
                .ForMember(x => x.PhoneNumber, y => y.Ignore())
                .ForMember(x => x.PasswordHash, y => y.Ignore())
                .ForMember(x => x.Logins, y => y.Ignore())
                .ForMember(x => x.LockoutEndDateUtc, y => y.Ignore())
                .ForMember(x => x.LockoutEnabled, y => y.Ignore())
                .ForMember(x => x.LastEmailResended, y => y.Ignore())
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.EmailConfirmed, y => y.Ignore())
                .ForMember(x => x.Claims, y => y.Ignore())
                .ForMember(x => x.AccessFailedCount, y => y.Ignore())
                .ForMember(x => x.Email, y => y.Ignore())
                .ForMember(x => x.UserName, y => y.MapFrom(z => z.Email))
                .ForMember(x => x.LastTimePasswordRestored, y => y.Ignore())
                .ForMember(x => x.GivenAssessments, y => y.Ignore())
                .ForMember(x => x.GivenComments, y => y.Ignore())
                .ForMember(x => x.Posts, y => y.Ignore())
                .ForMember(x => x.History, y => y.Ignore());

                cfg.CreateMap<RegisterViewModel, User>()
                .ForMember(x => x.TwoFactorEnabled, y => y.Ignore())
                .ForMember(x => x.SecurityStamp, y => y.Ignore())
                .ForMember(x => x.Roles, y => y.Ignore())
                .ForMember(x => x.PhoneNumberConfirmed, y => y.Ignore())
                .ForMember(x => x.PhoneNumber, y => y.Ignore())
                .ForMember(x => x.PasswordHash, y => y.Ignore())
                .ForMember(x => x.Name, y => y.Ignore())
                .ForMember(x => x.Logins, y => y.Ignore())
                .ForMember(x => x.LockoutEndDateUtc, y => y.Ignore())
                .ForMember(x => x.LockoutEnabled, y => y.Ignore())
                .ForMember(x => x.LastName, y => y.Ignore())
                .ForMember(x => x.LastEmailResended, y => y.Ignore())
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.EmailConfirmed, y => y.Ignore())
                .ForMember(x => x.Claims, y => y.Ignore())
                .ForMember(x => x.BornDate, y => y.Ignore())
                .ForMember(x => x.AccessFailedCount, y => y.Ignore())
                .ForMember(x => x.LastTimePasswordRestored, y => y.Ignore())
                .ForMember(x => x.UserName, y => y.MapFrom(z => z.Email))
                .ForMember(x => x.GivenAssessments, y => y.Ignore())
                .ForMember(x => x.GivenComments, y => y.Ignore())
                .ForMember(x => x.Posts, y => y.Ignore())
                .ForMember(x => x.History, y => y.Ignore());

                cfg.CreateMap<Post, PostViewModel>()
                .ForMember(x => x.IsEdition, y => y.Ignore())
                .ForMember(x => x.AssesmentAverage, y => y.Ignore())
                .ForMember(x => x.UserAssesment, y => y.Ignore());

                cfg.CreateMap<PostViewModel, Post>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.Assessments, y => y.Ignore())
                .ForMember(x => x.Comments, y => y.Ignore())
                .ForMember(x => x.CreatedDate, y => y.Ignore())
                .ForMember(x => x.EditedDate, y => y.Ignore())
                .ForMember(x => x.History, y => y.Ignore())
                .ForMember(x => x.User, y => y.Ignore())
                .ForMember(x => x.UserId, y => y.Ignore())
                .ForMember(x => x.Title, y => y.MapFrom(z => TextHelper.CapitalizeEachWord(z.Title)));
            });

            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();
        }
    }
}