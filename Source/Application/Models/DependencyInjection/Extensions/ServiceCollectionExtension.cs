using System.Globalization;
using Application.Models.Data.Identity;
using Application.Models.Globalization;
using Application.Models.Web.Authentication.Cookies;
using Application.Models.Web.Authentication.OpenIdConnect;
using Application.Models.Web.Identity;
using Application.Models.Web.Localization.Routing;
using Application.Models.Web.Mvc.ApplicationModels;
using Application.Models.Web.Mvc.Rendering;
using Application.Models.Web.Mvc.ViewFeatures;
using Application.Models.Web.Routing;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RegionOrebroLan.Localization.DependencyInjection.Extensions;

namespace Application.Models.DependencyInjection.Extensions
{
	public static class ServiceCollectionExtension
	{
		#region Fields

		private static readonly CultureInfo _masterCulture = CultureInfo.GetCultureInfo("sv-SE");
		private static readonly CultureInfo _masterUiCulture = CultureInfo.GetCultureInfo("sv");

		#endregion

		#region Methods

		public static IServiceCollection Add(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
		{
			ArgumentNullException.ThrowIfNull(services);
			ArgumentNullException.ThrowIfNull(configuration);
			ArgumentNullException.ThrowIfNull(hostEnvironment);

			services
				// AddIdentity adds authentication. To override that we need to put AddIdentity before our AddAuthentication.
				.AddIdentity(configuration, hostEnvironment)
				.AddAuthentication(options =>
				{
					options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
					options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				})
				.AddCookie(options =>
				{
					options.LoginPath = "/Account/SignIn";
					options.LogoutPath = "/Account/SignOut";

					options.EventsType = typeof(LocalizableCookieAuthenticationEvents);
				})
				.AddOpenIdConnect(options =>
				{
					options.Authority = "https://demo.duendesoftware.com";
					options.ClaimActions.MapAll();
					options.ClientId = "interactive.confidential";
					options.ClientSecret = "secret";
					options.GetClaimsFromUserInfoEndpoint = true;
					options.ResponseType = OidcConstants.ResponseTypes.Code;
					options.SaveTokens = true;
					options.Scope.Clear();
					options.Scope.Add("api");
					options.Scope.Add(OidcConstants.StandardScopes.Email);
					options.Scope.Add(OidcConstants.StandardScopes.OfflineAccess);
					options.Scope.Add(OidcConstants.StandardScopes.OpenId);
					options.Scope.Add(OidcConstants.StandardScopes.Profile);
					options.TokenValidationParameters = new TokenValidationParameters
					{
						NameClaimType = JwtClaimTypes.Name,
						RoleClaimType = JwtClaimTypes.Role,
					};
					options.UsePkce = true;

					options.EventsType = typeof(LocalizableOpenIdConnectEvents);
				}).Services
				.AddPathBasedLocalization(options => { options.FileResourcesDirectoryPath = "Resources/Localization"; }, true)
				.AddRazorPages().AddViewLocalization().Services
				.AddScoped<ICultureSelectorFactory, CultureSelectorFactory>()
				.AddScoped<LocalizableCookieAuthenticationEvents>()
				.AddScoped<LocalizableOpenIdConnectEvents>()
				.AddSingleton<ICultureContext>(_ => new CultureContext(_masterCulture, _masterUiCulture))
				.AddSingleton<IHtmlGenerator, HtmlGenerator>()
				.Configure<IdentityOptions>(options =>
				{
					options.ClaimsIdentity.EmailClaimType = JwtClaimTypes.Email;
					options.ClaimsIdentity.RoleClaimType = JwtClaimTypes.Role;
					options.ClaimsIdentity.UserIdClaimType = JwtClaimTypes.Subject;
					options.ClaimsIdentity.UserNameClaimType = JwtClaimTypes.Name;
				})
				.Configure<RazorPagesOptions>(options =>
				{
					const string folderPath = "/";
					/*
						If we just want to add culture routing for specific pages, under a specific sub-path, we could use:

							const string folderPath = "/Account";
					*/

					var convention = new CultureRouteModelConvention();
					options.Conventions.AddFolderRouteModelConvention(folderPath, model => { convention.Apply(model); });

					/*
						Notes:
						- https://github.com/dotnet/aspnetcore/issues/17868#issuecomment-565608080
						- https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/Mvc.Core/src/Filters/FilterCollection.cs
					*/
				})
				.Configure<RequestLocalizationOptions>(options =>
				{
					options.AddInitialRequestCultureProvider(new RouteDataRequestCultureProvider { Options = options });

					options.SupportedCultures!.Clear();
					options.SupportedCultures.Add(_masterCulture);
					options.SupportedCultures.Add(CultureInfo.GetCultureInfo("de-DE"));
					options.SupportedCultures.Add(CultureInfo.GetCultureInfo("en-001"));
					options.SupportedCultures.Add(CultureInfo.GetCultureInfo("fi-FI"));
					options.SupportedCultures.Add(CultureInfo.GetCultureInfo("fr-FR"));

					options.SupportedUICultures!.Clear();
					options.SupportedUICultures.Add(_masterUiCulture);
					options.SupportedUICultures.Add(CultureInfo.GetCultureInfo("de"));
					options.SupportedUICultures.Add(CultureInfo.GetCultureInfo("en"));
					options.SupportedUICultures.Add(CultureInfo.GetCultureInfo("fi"));
					options.SupportedUICultures.Add(CultureInfo.GetCultureInfo("fr"));
				})
				.Configure<RouteOptions>(options =>
				{
					options.ConstraintMap.Add(RouteKeys.Culture, typeof(CultureRouteConstraint));
					options.ConstraintMap.Add(RouteKeys.UiCulture, typeof(UiCultureRouteConstraint));
				});

			return services;
		}

		private static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
		{
			ArgumentNullException.ThrowIfNull(services);
			ArgumentNullException.ThrowIfNull(configuration);
			ArgumentNullException.ThrowIfNull(hostEnvironment);

			services.AddDbContext<IdentityContext>(options => { options.UseSqlite(configuration.GetConnectionString("Database")); });

			services.AddIdentity<User, Role>()
				.AddDefaultTokenProviders()
				.AddEntityFrameworkStores<IdentityContext>()
				.AddUserManager<UserManager>()
				.AddUserStore<UserStore>();

			services.AddScoped<IIdentityFacade, IdentityFacade>();
			services.AddScoped<IdentityContext>();

			return services;
		}

		#endregion
	}
}