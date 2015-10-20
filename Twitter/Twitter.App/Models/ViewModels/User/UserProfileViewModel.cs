namespace Twitter.App.Models.ViewModels.User
{
    using System;
    using Twitter.App.Infrastructure.Mapping;
    using Twitter.Models;
    using AutoMapper;
    using System.Collections.Generic;
    using System.Linq;
    using Twitter.App.Models.ViewModels.Tweet;

    public class UserProfileViewModel : IMapFrom<User>, ICustomMappings
    {
        public string Username { get; set; }

        public string AboutMe { get; set; }

        public string ProfileImageBase64 { get; set; }

        public string BackgroundImageBase64 { get; set; }

        public DateTime RegistrationDate { get; set; }

        public int FollowingCount { get; set; }

        public int FollowersCount { get; set; }

        public int FavouritesCount { get; set; }

        public List<TweetViewModel> Tweets { get; set; } 

        public bool Followed { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<User, UserProfileViewModel>()
               .ForMember(u => u.FollowingCount, opt => opt.MapFrom(u => u.FollowedUsers.Count))
               .ForMember(u => u.FollowersCount, opt => opt.MapFrom(u => u.Followers.Count))
               .ForMember(u => u.FavouritesCount, opt => opt.MapFrom(u => u.FavouritedTweets.Count))
               .ForMember(u => u.Tweets, opt => opt.MapFrom(u => u.Tweets.Where(t => t.ReplyToId == null)))
               .ReverseMap();
        }
    }
}