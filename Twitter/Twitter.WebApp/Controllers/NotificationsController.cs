using Twitter.Data;

namespace Twitter.WebApp.Controllers
{
    public class NotificationsController : BaseController
    {
        public NotificationsController(ITwitterData data) : base(data)
        {
        }

        public NotificationsController() : this(new TwitterData(new TwitterDbContext()))
        {

        }
    }
}