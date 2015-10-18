namespace Twitter.App.Models.BindingModels.Tweet
{
    using System.ComponentModel.DataAnnotations;

    public class ReTweetBindingModel
    {
        [Required]
        public int TweetId { get; set; }

        [Required, MinLength(0)]
        public string Content { get; set; }
    }
}