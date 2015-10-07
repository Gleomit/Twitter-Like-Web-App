namespace Twitter.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Tweet
    {
        private ICollection<User> favouritedBy;
        private ICollection<Tweet> replyTweets;
        private ICollection<Tweet> retweets;
        private ICollection<Report> reports; 

        public Tweet()
        {
            this.favouritedBy = new HashSet<User>();
            this.replyTweets = new HashSet<Tweet>();
            this.retweets = new HashSet<Tweet>();
            this.reports = new HashSet<Report>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        public int? RetweetedTweetId { get; set; }

        public virtual Tweet RetweetedTweet { get; set; }

        public int? ReplyToId { get; set; }

        public virtual Tweet ReplyTo { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime TweetDate { get; set; }

        public virtual ICollection<User> FavouritedBy
        {
            get { return this.favouritedBy; }
            set { this.favouritedBy = value; }
        }

        public virtual ICollection<Tweet> ReplyTweets
        {
            get { return this.replyTweets; }
            set { this.replyTweets = value; }
        }

        public virtual ICollection<Tweet> Retweets
        {
            get { return this.retweets; }
            set { this.retweets = value; }
        }

        public virtual ICollection<Report> Reports
        {
            get { return this.reports; }
            set { this.reports = value; }
        }
    }
}
