﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Twitter.WebApp.Startup))]
namespace Twitter.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
