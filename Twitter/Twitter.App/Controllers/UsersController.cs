namespace Twitter.App.Controllers
{
    using System;
    using Twitter.App.Models.BindingModels.User;

    using AutoMapper;
    using Twitter.App.Models.ViewModels.User;
    using System.Web.Mvc;

    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Twitter.App.Constants;
    using Twitter.App.Models.ViewModels.Tweet;
    using Twitter.Data.UnitOfWork;
    using Twitter.Models;
    using Twitter.Models.Enumerations;

    using AutoMapper.QueryableExtensions;
    using System.Net;
    using System.Web;
    using Newtonsoft.Json;
    using Twitter.App.Hubs;

    public class UsersController : BaseController
    {
        public UsersController(ITwitterData data)
            : base(data)
        {
        }

        [Authorize]
        [HttpGet]
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

        [Authorize]
        [HttpPost]
        public ActionResult Follow(string username)
        {
            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "User not found.");
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            if (currentUser.UserName == user.UserName)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "You cannot follow yourself.");
            }

            user.Followers.Add(currentUser);

            user.Notifications.Add(new Notification()
            {
                Content = "followed you",
                CreatorId = currentUser.Id,
                RecipientId = user.Id,
                Date = DateTime.Now,
                Type = NotificationType.NewFollower
            });

            this.Data.SaveChanges();

            TwitterHub hub = new TwitterHub();

            hub.NotificationInform(new List<string>() { user.UserName });

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public ActionResult Profile(string username)
        {
            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "User not found.");
            }

            var userProfile = Mapper.Map<UserProfileViewModel>(user);

            userProfile.Followed = false;

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            if (currentUser != null)
            {
                if (currentUser.FollowedUsers.Any(u => u.Id == user.Id))
                {
                    userProfile.Followed = true;
                }

                if (currentUser.Id == user.Id)
                {
                    userProfile.IsMe = true;
                }
            }

            return View(userProfile);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Unfollow(string username)
        {
            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "User not found.");
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            if (!user.Followers.Any(u => u == currentUser))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "You cannot unfollow someone whom you currently don't follow.");
            }

            user.Followers.Remove(currentUser);

            this.Data.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public ActionResult Followers(string username, int page = AppConstants.DefaultPageIndex)
        {
            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "User not found.");
            }

            var followers = user.Followers
                .Skip((page - 1) * AppConstants.DefaultPageSize)
                .Take(AppConstants.DefaultPageSize)
                .AsQueryable()
                .ToList();

            return null;
        }

        [HttpGet]
        public ActionResult Following(string username, int page = AppConstants.DefaultPageIndex)
        {
            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "User not found.");
            }

            var following = user.FollowedUsers
                .Skip((page - 1) * AppConstants.DefaultPageSize)
                .Take(AppConstants.DefaultPageSize)
                .AsQueryable()
                .ToList();

            return null;
        }

        [HttpGet]
        public ActionResult Favourites(string username, int page = AppConstants.DefaultPageIndex)
        {
            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "User not found.");
            }

            var favouriteTweets = user.FavouritedTweets
                .Skip((page - 1) * AppConstants.DefaultPageSize)
                .Take(AppConstants.DefaultPageSize)
                .AsQueryable()
                .ProjectTo<TweetViewModel>()
                .ToList();

            return PartialView("~/Views/Tweets/_TweetsPartial.cshtml", favouriteTweets);
        }

        [HttpGet]
        public ActionResult Tweets(string username, int page = AppConstants.DefaultPageIndex)
        {
            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "User not found.");
            }

            var tweets = user.Tweets
                .Skip((page - 1) * AppConstants.DefaultPageSize)
                .Take(AppConstants.DefaultPageSize)
                .AsQueryable()
                .ProjectTo<TweetViewModel>()
                .ToList();

            return PartialView("~/Views/Tweets/_TweetsPartial.cshtml", tweets);
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditProfile()
        {
            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            var viewModel = new UserEditProfileViewModel()
            {
                Nickname = user.Nickname
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(UserEditProfileBindingModel model)
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

            //user.ProfileImageBase64 = GetBase64String(Request.Files[0]);
            user.Nickname = model.Nickname;

            this.Data.SaveChanges();

            return RedirectToAction("Profile", new { username = user.UserName });
        }

        private string GetBase64String(HttpPostedFileBase file)
        {
            byte[] fileBuffer = new byte[file.ContentLength];
            file.InputStream.Read(fileBuffer, 0, file.ContentLength);

            return Convert.ToBase64String(fileBuffer);
        }
    }
}