﻿namespace Twitter.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Twitter.Models.Enumerations;

    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public NotificationType Type { get; set; }
    }
}