using Application.Models.Web.Routing;
using Microsoft.AspNetCore.Localization;

namespace Application.Models.Web.Localization.Routing
{
	public class RouteDataRequestCultureProvider : RequestCultureProvider
	{
		#region Methods

		public override async Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
		{
			await Task.CompletedTask;

			ArgumentNullException.ThrowIfNull(httpContext);

			var culture = httpContext.GetRouteValue(RouteKeys.Culture) as string;
			var uiCulture = httpContext.GetRouteValue(RouteKeys.UiCulture) as string;

			if(culture == null && uiCulture == null)
				return default;

			if(culture == null)
				culture = this.Options!.SupportedCultures!.FirstOrDefault(item => item.Name.StartsWith($"{uiCulture}-", StringComparison.OrdinalIgnoreCase))?.Name ?? uiCulture;
			else
				uiCulture ??= culture.Contains('-') ? culture.Split("-").First() : culture;

			var providerResultCulture = new ProviderCultureResult(culture, uiCulture);

			return providerResultCulture;
		}

		#endregion
	}
}