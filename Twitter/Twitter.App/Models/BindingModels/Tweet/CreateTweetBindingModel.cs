namespace Twitter.App.Models.BindingModels.Tweet
{
    using System.ComponentModel.DataAnnotations;

    public class CreateTweetBindingModel
    {
        [Required, MinLength(0)]
        public string Content { get; set; }
    }
}