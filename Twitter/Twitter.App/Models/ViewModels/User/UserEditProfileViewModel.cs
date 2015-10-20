using System.Web;

namespace Twitter.App.Models.ViewModels.User
{
    using System.ComponentModel.DataAnnotations;
    using Twitter.App.Infrastructure.Mapping;

    public class UserEditProfileViewModel : IMapFrom<Twitter.Models.User>
    {
        //[Required]
        public string Nickname { get; set; }

        //[Required]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ProfileImageData { get; set; }
    }
}