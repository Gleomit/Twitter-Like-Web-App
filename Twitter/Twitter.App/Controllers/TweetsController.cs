namespace Twitter.App.Controllers
{
    using System;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Twitter.Models;
    using Twitter.Models.Enumerations;

    using System.Web.Mvc;
    using Twitter.App.Models.BindingModels.Tweet;
    using Twitter.Data.UnitOfWork;
    using System.Collections.Generic;
    using AutoMapper;
    using Twitter.App.Hubs;
    using Twitter.App.Models.ViewModels.Tweet;
    using System.Net;
    using Newtonsoft.Json;

    public class TweetsController : BaseController
    {
        public TweetsController(ITwitterData data)
            : base(data)
        {
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Tweet(CreateTweetBindingModel model)
        {
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Missing data");
            }

            if (!this.ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(this.ModelState));
            }

            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            var tweet = new Tweet()
            {
                Content = model.Content,
                UserId = user.Id,
                TweetDate = DateTime.Now
            };

            this.Data.Tweets.Add(tweet);

            this.Data.SaveChanges();

            TwitterHub hub = new TwitterHub();

            hub.InformFollowers(user.Followers.Select(u => u.UserName).ToList(), tweet.Id);

            return PartialView("_FullTweetPartial", Mapper.Map<TweetViewModel>(tweet));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reply(int id, ReplyTweetBindingModel model)
        {
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Missing data");
            }

            if (!this.ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(this.ModelState));
            }

            var replyTo = this.Data.Tweets.Find(id);

            if (replyTo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Tweet not found");
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

        [Authorize]
        [HttpGet]
        public ActionResult Retweet(int id)
        {
            var tweet = this.Data.Tweets.Find(id);

            if (tweet == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Tweet not found.");
            }

            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            if (user.Tweets.Any(t => t.ReplyToId == tweet.Id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Retweeting retweet is not possible at the time.");
            }

            var viewModel = Mapper.Map<RetweetViewModel>(tweet);
            viewModel.Content = "";

            return PartialView("_ReTweetPartial", viewModel);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Tweet()
        {
            return PartialView("_CreateTweetPartial", new CreateTweetViewModel());
        }

        [Authorize]
        [HttpPost]
        public ActionResult Favourite(int id)
        {
            var tweet = this.Data.Tweets.Find(id);

            if (tweet == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Tweet not found.");
            }

            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            if (user.FavouritedTweets.Any(t => t == tweet))
            {
                user.FavouritedTweets.Remove(tweet);
            }
            else
            {
                user.FavouritedTweets.Add(tweet);

                tweet.User.Notifications.Add(new Notification()
                {
                    Content = "favourite",
                    RecipientId = tweet.UserId,
                    Date = DateTime.Now,
                    CreatorId = user.Id,
                    Type = NotificationType.FavouriteTweet,
                });

                TwitterHub hub = new TwitterHub();

                hub.NotificationInform(new List<string>() { tweet.User.UserName });
            }

            this.Data.SaveChanges();

            this.Response.StatusCode = (int) HttpStatusCode.OK;
            return this.Content(" " + tweet.FavouritedBy.Count());
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetTweet(int id)
        {
            var tweet = this.Data.Tweets.Find(id);

            if (tweet == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Tweet not found.");
            }

            var viewModel = Mapper.Map<TweetViewModel>(tweet);

            return PartialView("_FullTweetPartial", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Retweet(ReTweetBindingModel model)
        {
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Missing data");
            }

            if (!this.ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(this.ModelState));
            }

            var tweet = this.Data.Tweets.Find(model.TweetId);

            if (tweet == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Tweet not found.");
            }

            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            if (tweet.UserId == user.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "You can't retweet your own tweets.");
            }

            if (user.Tweets.Any(t => t.ReplyToId == tweet.Id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Retweeting retweet is not possible at the time.");
            }

            this.Data.Tweets.Add(new Tweet()
            {
                Content = model.Content,
                UserId = user.Id,
                TweetDate = DateTime.Now,
                RetweetedTweetId = tweet.Id
            });

            tweet.User.Notifications.Add(new Notification()
            {
                Content = "Retweet",
                CreatorId = user.Id,
                RecipientId = tweet.UserId,
                Date = DateTime.Now,
                Type = NotificationType.Retweet
            });

            this.Data.SaveChanges();

            TwitterHub hub = new TwitterHub();

            hub.NotificationInform(new List<string>() { tweet.User.UserName });

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public ActionResult Share(int id)
        {
            var tweet = this.Data.Tweets.Find(id);

            if (tweet == null)
            {
                throw new Exception("Tweet not found");
            }

            return null;
        }
    }
}