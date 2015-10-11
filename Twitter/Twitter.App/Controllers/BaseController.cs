namespace Twitter.App.Controllers
{
    using System.Web.Mvc;
    using Twitter.Data.UnitOfWork;

    public class BaseController : Controller
    {
        private ITwitterData data;

        public BaseController(ITwitterData data)
        {
            this.data = data;
        }

        public ITwitterData Data
        {
            get { return this.data; }
        }
    }
}