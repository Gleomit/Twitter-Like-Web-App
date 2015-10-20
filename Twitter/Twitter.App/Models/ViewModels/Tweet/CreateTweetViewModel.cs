namespace Twitter.App.Models.ViewModels.Tweet
{
    using System.ComponentModel.DataAnnotations;

    public class CreateTweetViewModel
    {
        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; } 
    }
}