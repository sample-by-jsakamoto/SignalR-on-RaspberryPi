using System;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace myapp
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
                //FileSystem = new PhysicalFileSystem(AppDomain.CurrentDomain.BaseDirectory)
                FileSystem = new EmbeddedResourceFileSystem2("myapp")
            });
        }
        //private void ConfigureFileServer(IAppBuilder app)
        //{
        //    var fileSystem = new EmbeddedResourceFileSystem2("myapp");
        //    app.UseDefaultFiles(new DefaultFilesOptions
        //    {
        //        DefaultFileNames = new[] { "index.html" }.ToList(),
        //        FileSystem = fileSystem
        //    });
        //    app.UseStaticFiles(new StaticFileOptions
        //    {
        //        FileSystem = fileSystem
        //    });
        //}
    }
}
