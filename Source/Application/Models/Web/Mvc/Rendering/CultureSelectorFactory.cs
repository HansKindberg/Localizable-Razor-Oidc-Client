using Application.Models.Collections.Generic.Extensions;
using Application.Models.Extensions;
using Application.Models.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace Application.Models.Web.Mvc.Rendering
{
	public class CultureSelectorFactory(ICultureContext cultureContext, IOptionsMonitor<RequestLocalizationOptions> localizationOptionsMonitor) : ICultureSelectorFactory
	{
		#region Fields

		private readonly ICultureContext _cultureContext = cultureContext ?? throw new ArgumentNullException(nameof(cultureContext));
		private readonly IOptionsMonitor<RequestLocalizationOptions> _localizationOptionsMonitor = localizationOptionsMonitor ?? throw new ArgumentNullException(nameof(localizationOptionsMonitor));

		#endregion

		#region Methods

		public CultureSelector Create(IUrlHelper urlHelper)
		{
			ArgumentNullException.ThrowIfNull(urlHelper);

			var currentUiCulture = this._cultureContext.CurrentUiCulture;
			var localization = this._localizationOptionsMonitor.CurrentValue;

			var uriBuilder = UriBuilderExtension.Create(urlHelper.ActionContext.HttpContext.Request);
			var path = uriBuilder.Path;

			var cultureSelector = new CultureSelector();

			foreach(var uiCulture in localization.SupportedUICultures ?? [])
			{
				uriBuilder.Path = path;

				uriBuilder.ResolvePath(this._cultureContext, localization, urlHelper.ActionContext.RouteData.Values, uiCulture);

				cultureSelector.List.Add(new SelectListItem(uiCulture.NativeName, uriBuilder.PathAndQueryAndFragment(), uiCulture.Equals(currentUiCulture)));
			}

			cultureSelector.List.Sort((first, second) => string.Compare(first.Text, second.Text, StringComparison.OrdinalIgnoreCase));

			return cultureSelector;
		}

		#endregion
	}
}