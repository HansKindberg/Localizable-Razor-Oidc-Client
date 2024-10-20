using Application.Models.Data.Identity;
using Application.Models.Web.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Web.Builder.Extensions
{
	public static class ApplicationBuilderExtension
	{
		#region Methods

		public static IApplicationBuilder Use(this IApplicationBuilder application, IWebHostEnvironment hostEnvironment)
		{
			ArgumentNullException.ThrowIfNull(application);
			ArgumentNullException.ThrowIfNull(hostEnvironment);

			application
				.UseDeveloperExceptionPage()
				.UseDatabase()
				.UseStaticFiles()
				.UseRouting()
				.UseRequestLocalization()
				.UseAuthentication()
				.UseAuthorization()
				.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });

			return application;
		}

		private static IApplicationBuilder UseDatabase(this IApplicationBuilder application)
		{
			ArgumentNullException.ThrowIfNull(application);

			using(var scope = application.ApplicationServices.CreateScope())
			{
				var identityContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();

				identityContext.Database.Migrate();

				var identity = scope.ServiceProvider.GetRequiredService<IIdentityFacade>();

				identity.CreateFirstUserIfNotExist("user@example.org", "P@ssword12", "User");
			}

			return application;
		}

		#endregion
	}
}