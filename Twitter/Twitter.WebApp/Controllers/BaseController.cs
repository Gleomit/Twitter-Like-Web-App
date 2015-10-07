namespace Twitter.WebApp.Controllers
{
    using Twitter.Data;

    public class BaseController
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