namespace Twitter.App.Controllers
{
    using System.Web.Mvc;
    using Twitter.App.Models.BindingModels.Tweet;
    using Twitter.Data.UnitOfWork;

    public class TweetsController : BaseController
    {
        public TweetsController(ITwitterData data)
            : base(data)
        {
        }

        [HttpPost]
        [Authorize]
        public ActionResult Tweet(CreateTweetBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return Json(this.ModelState);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Reply(int id)
        {
            return null;
        }
    }
}