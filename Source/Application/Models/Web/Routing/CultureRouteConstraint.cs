using System.Globalization;
using Microsoft.Extensions.Options;

namespace Application.Models.Web.Routing
{
	public class CultureRouteConstraint(IOptionsMonitor<RequestLocalizationOptions> optionsMonitor) : LocalizationRouteConstraint(optionsMonitor, RouteKeys.Culture)
	{
		#region Properties

		protected override IEnumerable<CultureInfo> SupportedCultures => this.OptionsMonitor.CurrentValue.SupportedCultures!;

		#endregion
	}
}