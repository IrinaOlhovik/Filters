using Autofac;
using Autofac.Integration.Mvc;
using Hangfire;
using Microsoft.Owin;
using Owin;
using System.Web.Mvc;
using WebShop.Controllers;
using WebShop.Core;
using WebShop.Models;

[assembly: OwinStartupAttribute(typeof(WebShop.Startup))]
namespace WebShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new DataModule(app));

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            var container = builder.Build();

            // REPLACE THE MVC DEPENDENCY RESOLVER WITH AUTOFAC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //Register With OWIN
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();

            ConfigureAuth(app);
            //GlobalConfiguration.Configuration
            //    .UseSqlServerStorage("DefaultConnection");

            //app.UseHangfireDashboard("/myJobDashbord", new DashboardOptions()
            //{
            //    Authorization = new[] { new HangfireAuthorizationFilter()}
            //});
            //RecurringJob.AddOrUpdate(
            //    () => ProductsController.ClearImages(), Cron.Minutely()
            //);
            //app.UseHangfireServer();
        }
    }
}
