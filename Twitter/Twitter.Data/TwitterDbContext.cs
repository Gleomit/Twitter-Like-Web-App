using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using Twitter.Data.Migrations;
using Twitter.Models;

namespace Twitter.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;

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

            modelBuilder.Entity<User>()
                .HasMany(u => u.Followers)
                .WithMany(u => u.FollowedUsers)
                .Map(m =>
                {
                    m.MapLeftKey("Follower_Id");
                    m.MapRightKey("FollowedBy_Id");
                    m.ToTable("UsersFollowers");
                });

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

            modelBuilder.Entity<Tweet>()
                .HasMany(t => t.Retweets)
                .WithOptional(t => t.RetweetedTweet);

            modelBuilder.Entity<Tweet>()
                .HasMany(t => t.ReplyTweets)
                .WithOptional(t => t.ReplyTo);

            base.OnModelCreating(modelBuilder);
        }
    }
}