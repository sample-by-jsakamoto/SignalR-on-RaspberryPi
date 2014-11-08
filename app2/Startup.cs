using System;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace app2
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.MapSignalR();

            ConfigureFileServer(appBuilder);
        }

        private void ConfigureFileServer(IAppBuilder app)
        {
            app.UseFileServer(new FileServerOptions
            {
                EnableDirectoryBrowsing = false,
                FileSystem = new PhysicalFileSystem(AppDomain.CurrentDomain.BaseDirectory)
            });
        }
    }
}
