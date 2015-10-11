namespace Twitter.App.Controllers
{
    using System.Web.Mvc;

    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Twitter.App.Constants;
    using Twitter.App.Models.ViewModels.Tweet;
    using Twitter.Data.UnitOfWork;

    public class UsersController : BaseController
    {
        public UsersController(ITwitterData data)
            : base(data)
        {
        }

        [HttpGet]
        public ActionResult UserProfile(string username)
        {
            return View(username);
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
                .Select(TweetViewModel.Create)
                .ToList();

            return View("~/Views/Users/Index.cshtml", tweets);
        }
    }
}