using Twitter.Data;

namespace Twitter.WebApp.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController(ITwitterData data) : base(data)
        {
        }
    }
}