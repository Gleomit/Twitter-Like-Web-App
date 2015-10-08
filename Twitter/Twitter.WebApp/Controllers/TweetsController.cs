using Twitter.Data;

namespace Twitter.WebApp.Controllers
{
    public class TweetsController : BaseController
    {
        public TweetsController(ITwitterData data) : base(data)
        {
        }

        public TweetsController() : this(new TwitterData(new TwitterDbContext()))
        {
            
        }
    }
}