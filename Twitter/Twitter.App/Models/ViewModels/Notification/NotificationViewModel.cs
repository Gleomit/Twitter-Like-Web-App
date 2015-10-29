namespace Twitter.App.Models.ViewModels.Notification
{
    using System;
    using AutoMapper;
    using Twitter.Models.Enumerations;

    public class NotificationViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime DateTime { get; set; }

        public NotificationType NotificationType { get; set; }

        public string NotifierUsername { get; set; }

        public string NotifierEmail { get; set; }

        public byte[] NotifierProfileImage { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Twitter.Models.Notification, NotificationViewModel>()
                .ForMember(n => n.NotifierUsername, opt => opt.MapFrom(n => n.Creator.UserName))
                .ForMember(n => n.NotifierEmail, opt => opt.MapFrom(n => n.Creator.Email))
                .ForMember(n => n.NotifierProfileImage, opt => opt.MapFrom(n => n.Creator.ProfileImageBase64))
                .ReverseMap();
        }
    }
}