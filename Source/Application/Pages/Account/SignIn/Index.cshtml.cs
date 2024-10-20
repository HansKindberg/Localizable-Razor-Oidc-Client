using Application.Models.Web;
using Application.Models.Web.Identity;
using Application.Models.Web.Mvc.Extensions;
using Application.Models.Web.Mvc.RazorPages;
using Application.Models.Web.Navigation.Attributes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

#pragma warning disable CS0162 // Unreachable code detected
// ReSharper disable HeuristicUnreachableCode
namespace Application.Pages.Account.SignIn
{
	[AllowAnonymous]
	[SignInUnavailable]
	public class Index(IIdentityFacade identity, IStringLocalizerFactory localizerFactory, ILoggerFactory loggerFactory) : SitePageModel(localizerFactory)
	{
		#region Fields

		private const bool _formsAuthenticationEnabled = true;
		private readonly IIdentityFacade _identity = identity ?? throw new ArgumentNullException(nameof(identity));
		private readonly ILogger _logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger<Index>();

		#endregion

		#region Properties

		[BindProperty]
		public string? Password { get; set; }

		[BindProperty]
		public string? ReturnUrl { get; set; }

		[BindProperty]
		public string? UserName { get; set; }

		#endregion

		#region Methods

		public async Task<IActionResult> OnGet(bool idp, string? returnUrl)
		{
			await Task.CompletedTask;

			this.ReturnUrl = this.Url.ResolveAndValidateReturnUrl(this._logger, returnUrl);

			if(idp || !_formsAuthenticationEnabled)
			{
				const string scheme = OpenIdConnectDefaults.AuthenticationScheme;

				var authenticationProperties = new AuthenticationProperties
				{
					Items =
					{
						{ QueryStringKeys.ReturnUrl, this.ReturnUrl },
						{ QueryStringKeys.Scheme, scheme }
					},
					RedirectUri = this.Url.Page("/Account/SignIn/Callback")
				};

				return this.Challenge(authenticationProperties, scheme);
			}

			return this.Page();
		}

		public async Task<IActionResult> OnPost()
		{
			if(!_formsAuthenticationEnabled)
				throw new InvalidOperationException("Forms authentication is not enabled.");

			this.ReturnUrl = this.Url.ResolveAndValidateReturnUrl(this._logger, this.ReturnUrl);

			if(string.IsNullOrEmpty(this.UserName) || string.IsNullOrEmpty(this.Password))
			{
				var requiredFormat = this.Localizer["form/error/required-format"];

				if(string.IsNullOrEmpty(this.UserName))
					this.ModelState.AddModelError($"1-{nameof(this.UserName)}", string.Format(null, requiredFormat, this.Localizer["form/username"]));

				if(string.IsNullOrEmpty(this.Password))
					this.ModelState.AddModelError($"2-{nameof(this.Password)}", string.Format(null, requiredFormat, this.Localizer["form/password"]));
			}
			else
			{
				if(await this._identity.SignIn(this.Password, this.UserName))
					return this.Redirect(this.ReturnUrl);

				this.ModelState.AddModelError("3-Invalid-Credentials", string.Format(null, this.Localizer["form/error/invalid-credentials"], this.Localizer["form/username"], this.Localizer["form/password"]));
			}

			return this.Page();
		}

		#endregion
	}
}
// ReSharper restore HeuristicUnreachableCode
#pragma warning restore CS0162 // Unreachable code detected