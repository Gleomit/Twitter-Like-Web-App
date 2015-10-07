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

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }
    }
}
