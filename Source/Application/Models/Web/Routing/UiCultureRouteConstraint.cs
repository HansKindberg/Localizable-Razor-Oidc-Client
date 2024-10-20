using System.Globalization;
using Microsoft.Extensions.Options;

namespace Application.Models.Web.Routing
{
	public class UiCultureRouteConstraint(IOptionsMonitor<RequestLocalizationOptions> optionsMonitor) : LocalizationRouteConstraint(optionsMonitor, RouteKeys.UiCulture)
	{
		#region Properties

		protected override IEnumerable<CultureInfo> SupportedCultures => this.OptionsMonitor.CurrentValue.SupportedUICultures!;

		#endregion
	}
}