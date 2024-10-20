using Application.Models.Web;
using Application.Models.Web.Identity;
using Application.Models.Web.Mvc.Extensions;
using Application.Models.Web.Mvc.RazorPages;
using Application.Models.Web.Navigation.Attributes;
using Application.Models.Web.Routing;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Application.Pages.Account.SignOut
{
	[AllowAnonymous]
	[SignOutUnavailable]
	public class Index(IIdentityFacade identity, IStringLocalizerFactory localizerFactory, ILoggerFactory loggerFactory) : SitePageModel(localizerFactory)
	{
		#region Fields

		private const bool _confirmationRequired = true;
		private readonly IIdentityFacade _identity = identity ?? throw new ArgumentNullException(nameof(identity));
		private readonly ILogger _logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger<Index>();
		private const string _signedOutPath = "/Account/SignedOut/Index";

		#endregion

		#region Properties

		[BindProperty]
		public string? ReturnUrl { get; set; }

		#endregion

		#region Methods

		private async Task<IDictionary<string, string>> CreateRouteValues()
		{
			await Task.CompletedTask;

			var routeValues = new Dictionary<string, string>();

			if(this.RouteData.Values[RouteKeys.UiCulture] is string uiCultureRoute)
				routeValues.Add(RouteKeys.UiCulture, uiCultureRoute);

			if(!string.IsNullOrEmpty(this.ReturnUrl))
				routeValues.Add(QueryStringKeys.ReturnUrl, this.ReturnUrl);

			return routeValues;
		}

		public async Task<IActionResult> OnGet(string? returnUrl)
		{
			await Task.CompletedTask;

			this.ReturnUrl = this.ResolveAndValidateReturnUrl(returnUrl);

			var confirm = _confirmationRequired;

			if(!this.User.Identity!.IsAuthenticated)
				confirm = false;

			return confirm ? this.Page() : await this.OnPost();
		}

		public async Task<IActionResult> OnPost()
		{
			await Task.CompletedTask;

			this.ReturnUrl = this.ResolveAndValidateReturnUrl(this.ReturnUrl);

			if(this.User.Identity!.IsAuthenticated)
			{
				await this._identity.SignOut();

				var identityProvider = this.User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

				if(identityProvider != null)
				{
					var redirectUri = this.Url.Page(_signedOutPath, await this.CreateRouteValues());

					return this.SignOut(new AuthenticationProperties { RedirectUri = redirectUri }, OpenIdConnectDefaults.AuthenticationScheme);
				}
			}

			return this.RedirectToPage(_signedOutPath, await this.CreateRouteValues());
		}

		private string? ResolveAndValidateReturnUrl(string? returnUrl)
		{
			return string.IsNullOrEmpty(returnUrl) ? null : this.Url.ResolveAndValidateReturnUrl(this._logger, returnUrl);
		}

		#endregion
	}
}