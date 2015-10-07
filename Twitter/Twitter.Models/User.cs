﻿namespace Twitter.Models
{
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser
    {
        private ICollection<User> followers;
        private ICollection<User> followedUsers;

        private ICollection<Tweet> tweets;

        private ICollection<Message> sentMessages;
        private ICollection<Message> receivedMessages;

        private ICollection<Notification> notifications;

        private ICollection<Tweet> favouritedTweets; 

        public User()
        {
            this.followers = new HashSet<User>();
            this.followedUsers = new HashSet<User>();

            this.tweets = new HashSet<Tweet>();

            this.sentMessages = new HashSet<Message>();
            this.receivedMessages = new HashSet<Message>();

            this.notifications = new HashSet<Notification>();

            this.favouritedTweets = new HashSet<Tweet>();
        }

        public virtual ICollection<User> Followers
        {
            get { return this.followers; }
            set { this.followers = value; }
        }

        public virtual ICollection<User> FollowedUsers
        {
            get { return this.followedUsers; }
            set { this.followedUsers = value; }
        }

        public virtual ICollection<Tweet> Tweets
        {
            get { return this.tweets; }
            set { this.tweets = value; }
        }

        public virtual ICollection<Message> ReceivedMessages
        {
            get { return this.receivedMessages; }
            set { this.receivedMessages = value; }
        }

        public virtual ICollection<Message> SentMessages
        {
            get { return this.sentMessages; }
            set { this.sentMessages = value; }
        }

        public virtual ICollection<Notification> Notifications
        {
            get { return this.notifications; }
            set { this.notifications = value; }
        }

        public virtual ICollection<Tweet> FavouritedTweets
        {
            get { return this.favouritedTweets; }
            set { this.favouritedTweets = value; }
        }
    }
}