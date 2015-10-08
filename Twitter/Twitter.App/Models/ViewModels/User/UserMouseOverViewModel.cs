namespace Twitter.App.Models.ViewModels.User
{
    using System;
    using System.Linq.Expressions;
    using Twitter.Models;

    public class UserMouseOverViewModel
    {
        public string Id { get; set; }

        public string Nickname { get; set; }

        public int Tweets { get; set; }

        public int Followers { get; set; }

        public int Following { get; set; }

        public static Expression<Func<ApplicationUser, UserMouseOverViewModel>> Create
        {
            get
            {
                return u => new UserMouseOverViewModel()
                {
                    Id = u.Id,
                    Nickname = u.Nickname,
                    Tweets = u.Tweets.Count,
                    Followers = u.Followers.Count,
                    Following = u.FollowedUsers.Count
                };
            }
        }
    }
}