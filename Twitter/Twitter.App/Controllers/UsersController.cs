namespace Twitter.App.Controllers
{
    using AutoMapper;
    using Twitter.App.Models.ViewModels.User;
    using System.Web.Mvc;

    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Twitter.App.Constants;
    using Twitter.App.Models.ViewModels.Tweet;
    using Twitter.Data.UnitOfWork;
    using AutoMapper.QueryableExtensions;

    public class UsersController : BaseController
    {
        public UsersController(ITwitterData data)
            : base(data)
        {
        }

        [Authorize]
        public ActionResult Index(int page = AppConstants.DefaultPageIndex)
        {
            List<TweetViewModel> tweets = new List<TweetViewModel>();

            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            var followersIds = user.FollowedUsers.Select(u => u.Id).ToList();

            tweets = this.Data.Tweets.All()
                .Where(t => t.UserId == user.Id || followersIds.Contains(t.UserId))
                .OrderByDescending(t => t.TweetDate)
                .Skip((page - 1)*AppConstants.DefaultPageSize)
                .Take(AppConstants.DefaultPageSize)
                .ProjectTo<TweetViewModel>()
                .ToList();

            return View("~/Views/Users/Index.cshtml", tweets);
        }

        [HttpGet]
        public ActionResult Profile(string username)
        {
            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                // Todo
            }

            var userProfile = Mapper.Map<UserProfileViewModel>(user);

            return View(userProfile);
        }

        public ActionResult Followers(string username)
        {
            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                // Todo
            }

            var followers = user.Followers.ToList();

            return null;
        }

        public ActionResult Following(string username)
        {
            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                // Todo
            }

            var following = user.FollowedUsers.ToList();

            return null;
        }

        public ActionResult Favourites(string username)
        {
            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                // Todo
            }

            return null;
        }

        public ActionResult Tweets(string username)
        {
            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                // Todo
            }

            return null;
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditProfile()
        {
            return null;
        }

    }
}