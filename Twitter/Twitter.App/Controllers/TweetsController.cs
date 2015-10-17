namespace Twitter.App.Controllers
{
    using System;
    using Microsoft.AspNet.Identity;
    using Twitter.Models;

    using System.Web.Mvc;
    using Twitter.App.Models.BindingModels.Tweet;
    using Twitter.Data.UnitOfWork;

    public class TweetsController : BaseController
    {
        public TweetsController(ITwitterData data)
            : base(data)
        {
        }

        [HttpPost]
        [Authorize]
        public ActionResult Tweet(CreateTweetBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return Json(this.ModelState);
            }

            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            this.Data.Tweets.Add(new Tweet()
            {
                Content = model.Content,
                UserId = user.Id,
                TweetDate = DateTime.Now,
            });

            this.Data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Reply(int id, CreateTweetBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return Json(this.ModelState);
            }

            var replyTo = this.Data.Tweets.Find(id);

            if (replyTo == null)
            {
                return this.Json("Not found");
            }

            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            this.Data.Tweets.Add(new Tweet()
            {
                Content = model.Content,
                UserId = user.Id,
                TweetDate = DateTime.Now,
                ReplyToId = replyTo.Id
            });

            this.Data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Retweet(int id, CreateTweetBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return Json(this.ModelState);
            }

            var retweet = this.Data.Tweets.Find(id);

            if (retweet == null)
            {
                return this.Json("Not found");
            }

            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            this.Data.Tweets.Add(new Tweet()
            {
                Content = model.Content,
                UserId = user.Id,
                TweetDate = DateTime.Now,
                RetweetedTweetId = retweet.Id
            });

            this.Data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}