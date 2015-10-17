namespace Twitter.App.Models.ViewModels.User
{
    using Twitter.App.Infrastructure.Mapping;

    public class UserMiniViewModel : IMapFrom<Twitter.Models.User>
    {
        public string Username { get; set; }
        
        public string ProfileImageBase64 { get; set; }
    }
}