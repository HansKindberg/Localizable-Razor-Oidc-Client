using System.Globalization;
using Microsoft.Extensions.Options;

namespace Application.Models.Web.Routing
{
	public abstract class LocalizationRouteConstraint(IOptionsMonitor<RequestLocalizationOptions> optionsMonitor, string routeKey) : IRouteConstraint
	{
		#region Properties

		protected IOptionsMonitor<RequestLocalizationOptions> OptionsMonitor { get; } = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
		protected string RouteKey { get; } = routeKey ?? throw new ArgumentNullException(nameof(routeKey));
		protected abstract IEnumerable<CultureInfo> SupportedCultures { get; }

		#endregion

		#region Methods

		public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
		{
			var match = string.Equals(this.RouteKey, routeKey, StringComparison.OrdinalIgnoreCase) && this.SupportedCultures.Any(culture => culture.Name.Equals(values[routeKey] as string, StringComparison.OrdinalIgnoreCase));

			return match;
		}

		#endregion
	}
}