namespace Twitter.Data
{
    using System.Data.Entity;

    using System.Data.Entity.ModelConfiguration.Conventions;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Twitter.Data.Migrations;
    using Twitter.Models;

    public class TwitterDbContext : IdentityDbContext<User>, ITwitterDbContext
    {
        public TwitterDbContext()
            : base("name=TwitterDbContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<TwitterDbContext, Configuration>());
        }

        public virtual IDbSet<Tweet> Tweets { get; set; }

        public virtual IDbSet<Message> Messages { get; set; }

        public virtual IDbSet<Notification> Notifications { get; set; }

        public static TwitterDbContext Create()
        {
            return new TwitterDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            // User Followers
            modelBuilder.Entity<User>()
                .HasMany(u => u.Followers)
                .WithMany(u => u.FollowedUsers)
                .Map(m =>
                {
                    m.MapLeftKey("Follower_Id");
                    m.MapRightKey("FollowedBy_Id");
                    m.ToTable("UsersFollowers");
                });

            // User-Tweet favourites/favouritedBy
            modelBuilder.Entity<User>()
                .HasMany(u => u.FavouritedTweets)
                .WithMany(t => t.FavouritedBy)
                .Map(
                    m =>
                    {
                        m.MapLeftKey("Favourite_Id");
                        m.MapRightKey("FavouritedBy_Id");
                        m.ToTable("FavouriteTweets");
                    });

            // Retweets
            modelBuilder.Entity<Tweet>()
               .HasOptional(t => t.RetweetedTweet)
               .WithMany(t => t.Retweets)
               .HasForeignKey(r => r.RetweetedTweetId);

            // Replies
            modelBuilder.Entity<Tweet>()
                .HasOptional(t => t.ReplyTo)
                .WithMany(t => t.ReplyTweets)
                .HasForeignKey(r => r.ReplyToId);

            // Tweet reports
            modelBuilder.Entity<Report>()
                .HasRequired(r => r.Tweet)
                .WithMany(t => t.Reports)
                .HasForeignKey(r => r.TweetId);

            // User reports
            modelBuilder.Entity<Report>()
                .HasRequired(r => r.User)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.UserId);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}