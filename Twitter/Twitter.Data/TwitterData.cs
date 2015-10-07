namespace Twitter.Data
{
    using System.Data.Entity;
    using Twitter.Data.Repositories;
    using Twitter.Models;

    public class TwitterData : ITwitterData
    {
        private DbContext context;

        private IRepository<User> users;
        private IRepository<Tweet> tweets;

        private IRepository<Message> messages;
        private IRepository<Notification> notifications;

        public TwitterData(DbContext context)
        {
            this.context = context;
        }

        public IRepository<User> Users
        {
            get
            {
                if (this.users == null)
                {
                    this.users = new GenericRepository<User>(this.context);
                }

                return this.users;
            }
        }

        public IRepository<Tweet> Tweets
        {
            get
            {
                if (this.tweets == null)
                {
                    this.tweets = new GenericRepository<Tweet>(this.context);
                }

                return this.tweets;
            }
        }

        public IRepository<Message> Messages
        {
            get
            {
                if (this.messages == null)
                {
                    this.messages = new GenericRepository<Message>(this.context);
                }

                return this.messages;
            }
        }

        public IRepository<Notification> Notifications
        {
            get
            {
                if (this.notifications == null)
                {
                    this.notifications = new GenericRepository<Notification>(this.context);
                }

                return this.notifications;
            }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }
    }
}
