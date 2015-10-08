namespace Twitter.App.Controllers
{
    using Twitter.Data;
    using System.Web.Mvc;

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