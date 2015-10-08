namespace Twitter.App.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Twitter.Data;

    using Twitter.App.Models.ViewModels.Tweet;

    public class HomeController : BaseController
    {
        public HomeController(ITwitterData data) : base(data)
        {
        }

        public HomeController() : this(new TwitterData(new TwitterDbContext()))
        {
            
        }

        private const int DefaultPageSize = 10;
        private const int DefaultStartPage = 0;

        [Route("{page:int?}")]
        public ActionResult Index(int page = DefaultStartPage)
        {
            var tweets = this.Data.Tweets.All()
                .OrderByDescending(t => t.TweetDate)
                .Skip(page * DefaultPageSize)
                .Take(DefaultPageSize)
                .Select(TweetViewModel.Create)
                .ToList();

            return View(tweets);
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