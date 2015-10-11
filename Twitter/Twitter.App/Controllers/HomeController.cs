namespace Twitter.App.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Twitter.App.Models.ViewModels.Tweet;

    using System.Collections.Generic;
    using Microsoft.AspNet.Identity;
    using Twitter.App.Constants;
    using Twitter.Data.UnitOfWork;

    public class HomeController : BaseController
    {
        public HomeController(ITwitterData data) : base(data)
        {
        }

        public ActionResult Index(int page = AppConstants.DefaultPageIndex)
        {
            List<TweetViewModel> tweets = new List<TweetViewModel>();

            if (this.User.Identity.IsAuthenticated)
            {
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

            tweets = this.Data.Tweets.All()
                .OrderByDescending(t => t.TweetDate)
                .Skip((page - 1)*AppConstants.DefaultPageSize)
                .Take(AppConstants.DefaultPageSize)
                .Select(TweetViewModel.Create)
                .ToList();

            return View(tweets);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}