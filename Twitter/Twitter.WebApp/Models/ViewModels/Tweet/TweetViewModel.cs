namespace Twitter.WebApp.Models.ViewModels.Tweet
{
    using System;

    using System.Linq.Expressions;
    using Twitter.Models;

    using System.Collections.Generic;
    using System.Linq;
    using Twitter.WebApp.Models.ViewModels.User;

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
                    User = new List<ApplicationUser>()
                    {
                        t.User
                    }.AsQueryable().Select(UserMouseOverViewModel.Create).FirstOrDefault()
                };
            }
        }
    }
}