namespace Twitter.App.Models.ViewModels.Tweet
{
    using System;
    using System.Linq.Expressions;
    using Twitter.Models;
    using System.Collections.Generic;
    using System.Linq;
    using Twitter.App.Models.ViewModels.User;

    public class TweetViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime Date { get; set; }

        public UserMouseOverViewModel User { get; set; }

        public static Expression<Func<Tweet, TweetViewModel>> Create
        {
            get
            {
                return t => new TweetViewModel()
                {
                    Id = t.Id,
                    Content = t.Content,
                    Date = t.TweetDate,
                    User = new List<User>()
                    {
                        t.User
                    }.AsQueryable().Select(UserMouseOverViewModel.Create).FirstOrDefault()
                };
            }
        }
    }
}