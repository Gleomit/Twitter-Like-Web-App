namespace Twitter.App.Infrastructure.Mapping
{
    using AutoMapper;

    public interface ICustomMappings
    {
        void CreateMappings(IConfiguration configuration);
    }
}