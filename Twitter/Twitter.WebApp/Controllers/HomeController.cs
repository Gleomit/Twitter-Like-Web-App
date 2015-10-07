namespace Twitter.WebApp.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Twitter.Data;

    public class HomeController : BaseController
    {
        public HomeController(ITwitterData data) : base(data)
        {
        }

        public HomeController() : this(new TwitterData(new TwitterDbContext()))
        {
            
        }

        public ActionResult Index()
        {
            this.Data.Tweets.All().Count();
            return View();
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