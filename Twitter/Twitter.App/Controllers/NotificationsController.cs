namespace Twitter.App.Controllers
{
    using System.Linq;
    using Microsoft.AspNet.Identity;

    using Twitter.Data.UnitOfWork;
    using System.Web.Mvc;

    using Twitter.App.Constants;

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
                .Skip(page - 1 * AppConstants.DefaultPageSize)
                .Take(AppConstants.DefaultPageSize)
                .ToList();

            return null;
        }

        [Authorize]
        [HttpGet]
        public ActionResult Show(int id)
        {
            var notification = this.Data.Notifications.Find(id);

            if (notification == null)
            {
                return this.Json("Not Found");
            }

            return null;
        }
    }
}