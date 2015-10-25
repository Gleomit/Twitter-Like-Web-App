namespace Twitter.App.Providers
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.SignalR;
    using Twitter.Data;
    using Twitter.Data.UnitOfWork;

    public class MyIdProvider : IUserIdProvider
    {
        private TwitterData data;

        public MyIdProvider()
        {
            data = new TwitterData(new TwitterDbContext());
        }

        public string GetUserId(IRequest request)
        {
            return request.User.Identity.GetUserId();
        }
    }
}