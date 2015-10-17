namespace Twitter.App.Models.ViewModels.Tweet
{
    using System;
    using Twitter.Models;
    using Twitter.App.Infrastructure.Mapping;

    using AutoMapper;

    public class TweetViewModel : IMapFrom<Tweet>, ICustomMappings
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime TweetDate { get; set; }

        public string Username { get; set; }

        public string UserProfilePicture { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Tweet, TweetViewModel>()
               .ForMember(t => t.Username, opt => opt.MapFrom(t => t.User.UserName))
               .ForMember(t => t.UserProfilePicture, opt => opt.MapFrom(t => t.User.ProfileImageBase64))
               .ReverseMap();
        }
    }
}