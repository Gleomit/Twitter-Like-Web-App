namespace Twitter.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Message
    {
        public Message()
        {
            this.Seen = false;
            this.Deleted = false;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string SenderId { get; set; }

        public virtual User Sender { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        public virtual User Receiver { get; set; }

        [Required]
        public string Content { get; set; }

        public bool Deleted { get; set; }

        public bool Seen { get; set; }

        [Required]
        public DateTime SentDateTime { get; set; }
    }
}