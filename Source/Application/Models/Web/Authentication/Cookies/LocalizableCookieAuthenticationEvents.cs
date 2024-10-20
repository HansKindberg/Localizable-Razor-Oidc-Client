using Application.Models.Globalization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Application.Models.Web.Authentication.Cookies
{
	/// <summary>
	/// [MVC Core 3.0 options.LoginPath - add localization route parameter](https://stackoverflow.com/questions/58520985/mvc-core-3-0-options-loginpath-add-localization-route-parameter#answer-58522097)
	/// </summary>
	public class LocalizableCookieAuthenticationEvents(ICultureContext cultureContext) : CookieAuthenticationEvents
	{
		#region Fields

		private readonly ICultureContext _cultureContext = cultureContext ?? throw new ArgumentNullException(nameof(cultureContext));

		#endregion

		#region Methods

		private RedirectContext<CookieAuthenticationOptions> GetResolvedContext(RedirectContext<CookieAuthenticationOptions> context)
		{
			ArgumentNullException.ThrowIfNull(context);

			if(!this._cultureContext.CurrentUiCulture.Equals(this._cultureContext.MasterUiCulture))
			{
				var uriBuilder = new UriBuilder(context.RedirectUri);

				uriBuilder.Path = $"/{this._cultureContext.CurrentUiCulture}{uriBuilder.Path}";

				context.RedirectUri = uriBuilder.ToString();
			}

			return context;
		}

		public override async Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
		{
			await base.RedirectToAccessDenied(this.GetResolvedContext(context));
		}

		public override async Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
		{
			await base.RedirectToLogin(this.GetResolvedContext(context));
		}

		public override async Task RedirectToLogout(RedirectContext<CookieAuthenticationOptions> context)
		{
			await base.RedirectToLogout(this.GetResolvedContext(context));
		}

		public override async Task RedirectToReturnUrl(RedirectContext<CookieAuthenticationOptions> context)
		{
			await base.RedirectToReturnUrl(this.GetResolvedContext(context));
		}

		#endregion
	}
}