using System.Net;
using AutoMapper.QueryableExtensions;
using Twitter.App.Models.ViewModels.Notification;

namespace Twitter.App.Controllers
{
    using System.Linq;
    using Microsoft.AspNet.Identity;

    using Twitter.Data.UnitOfWork;
    using System.Web.Mvc;

    using Twitter.App.Constants;
    using Twitter.Models.Enumerations;
    using WebGrease.Css.Extensions;

    public class NotificationsController : BaseController
    {
        public NotificationsController(ITwitterData data)
            : base(data)
        {
        }
        
        [Authorize]
        [HttpGet]
        public ActionResult All(int page = AppConstants.DefaultPageIndex)
        {
            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            var notifications = user.Notifications
                .OrderByDescending(n => n.Date)
                .Skip((page - 1)*AppConstants.DefaultPageSize)
                .Take(AppConstants.DefaultPageSize)
                .AsQueryable()
                .ProjectTo<NotificationViewModel>()
                .ToList();

            user.Notifications.Where(n => n.Status == NotificationStatus.NotSeen)
               .ForEach(n => n.Status = NotificationStatus.Seen);

            this.Data.SaveChanges();

            return View(notifications);
        }

        [Authorize]
        [HttpGet]
        public ActionResult FollowingUsers(int page = AppConstants.DefaultPageIndex)
        {
            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            var followersIds = user.FollowedUsers
                .Select(u => u.Id)
                .ToList();

            var notifications = user.Notifications
                .Where(n => followersIds.Contains(n.CreatorId))
                .OrderByDescending(n => n.Date)
                .Skip((page - 1) * AppConstants.DefaultPageSize)
                .Take(AppConstants.DefaultPageSize)
                .AsQueryable()
                .ProjectTo<NotificationViewModel>()
                .ToList();

            user.Notifications.Where(n => n.Status == NotificationStatus.NotSeen)
               .ForEach(n => n.Status = NotificationStatus.Seen);

            this.Data.SaveChanges();

            return View(notifications);
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetNotSeenNotificationsCount()
        {
            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            int notificationsCount = user.Notifications.Count(n => n.Status == NotificationStatus.NotSeen);

            return this.Content(notificationsCount.ToString());
        }

        [Authorize]
        [HttpGet]
        public ActionResult Show(int id)
        {
            var notification = this.Data.Notifications.Find(id);

            if (notification == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Notification not found.");
            }

            if (notification.Status == NotificationStatus.NotSeen)
            {
                notification.Status = NotificationStatus.Seen;
                this.Data.SaveChanges();
            }

            return null;
        }
    }
}