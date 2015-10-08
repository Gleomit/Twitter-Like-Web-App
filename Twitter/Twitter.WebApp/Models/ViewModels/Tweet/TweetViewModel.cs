namespace Twitter.WebApp.Models.ViewModels.Tweet
{
    using System;

    using System.Linq.Expressions;
    using Twitter.Models;

    public class TweetViewModel
    {
        public int Id { get; set; }
        
        public string Content { get; set; }

        public DateTime Date { get; set; }

        public static Expression<Func<Tweet, TweetViewModel>> Create
        {
            get
            {
                return t => new TweetViewModel()
                {
                    Id = t.Id,
                    Content = t.Content,
                    Date = t.TweetDate
                };
            }
        }
    }
}