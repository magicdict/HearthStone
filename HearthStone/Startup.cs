using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;

namespace HearthStone
{
    public class Startup
    {
        public void Configure(IBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
            app.UseServices(services =>
            {
                services.AddMvc();
            });
            // Add static files to the request pipeline
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
