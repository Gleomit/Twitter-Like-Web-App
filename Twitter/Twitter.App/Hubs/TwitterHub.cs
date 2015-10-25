namespace Twitter.App.Hubs
{
    using Microsoft.AspNet.SignalR;
    using System.Collections.Generic;

    public class TwitterHub : Hub
    {
        public void InformFollowers(IEnumerable<string> followersUsernames, int tweetId)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<TwitterHub>();

            hubContext.Clients.Users(new List<string>(followersUsernames)).receiveTweet(tweetId);
        }

        public void NotificationInform(IEnumerable<string> followersUsernames)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<TwitterHub>();

            hubContext.Clients.Users(new List<string>(followersUsernames)).receiveNotification();
        }
    }
}