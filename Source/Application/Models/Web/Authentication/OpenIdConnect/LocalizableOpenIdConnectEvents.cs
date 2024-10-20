using Application.Models.Globalization;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Application.Models.Web.Authentication.OpenIdConnect
{
	public class LocalizableOpenIdConnectEvents(ICultureContext cultureContext) : OpenIdConnectEvents
	{
		#region Fields

		private readonly ICultureContext _cultureContext = cultureContext ?? throw new ArgumentNullException(nameof(cultureContext));

		#endregion

		#region Methods

		public override async Task RedirectToIdentityProvider(RedirectContext context)
		{
			context.ProtocolMessage.UiLocales = this._cultureContext.CurrentUiCulture.Name;

			await base.RedirectToIdentityProvider(context);
		}

		public override async Task RedirectToIdentityProviderForSignOut(RedirectContext context)
		{
			context.ProtocolMessage.UiLocales = this._cultureContext.CurrentUiCulture.Name;

			await base.RedirectToIdentityProviderForSignOut(context);
		}

		#endregion
	}
}