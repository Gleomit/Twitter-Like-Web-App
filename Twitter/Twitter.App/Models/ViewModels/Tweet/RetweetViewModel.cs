namespace Twitter.App.Models.ViewModels.Tweet
{
    using System.ComponentModel.DataAnnotations;

    public class RetweetViewModel
    {
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public string UserPicture { get; set; }

        public string TweetContent { get; set; }

        public int TweetId { get; set; }

        public string Username { get; set; }
    }
}