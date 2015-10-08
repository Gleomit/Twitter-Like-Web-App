namespace Twitter.App.Controllers
{
    using Twitter.Data;

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