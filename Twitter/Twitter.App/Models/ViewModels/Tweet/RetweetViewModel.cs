namespace Twitter.App.Models.ViewModels.Tweet
{
    using System.ComponentModel.DataAnnotations;
    using Twitter.App.Infrastructure.Mapping;
    using AutoMapper;

    public class RetweetViewModel : IMapFrom<Twitter.Models.Tweet>, ICustomMappings
    {
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public string UserPicture { get; set; }

        public string TweetContent { get; set; }

        public int TweetId { get; set; }

        public string Username { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Twitter.Models.Tweet, RetweetViewModel>()
               .ForMember(t => t.Username, opt => opt.MapFrom(t => t.User.UserName))
               .ForMember(t => t.UserPicture, opt => opt.MapFrom(t => t.User.ProfileImageBase64))
               .ForMember(t => t.TweetContent, opt => opt.MapFrom(t => t.Content))
               .ForMember(t => t.TweetId, opt => opt.MapFrom(t => t.Id))
               .ReverseMap();
        }
    }
}