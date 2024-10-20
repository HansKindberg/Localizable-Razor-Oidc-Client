using System.Text.Encodings.Web;
using Application.Models.Extensions;
using Application.Models.Globalization;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;

namespace Application.Models.Web.Mvc.ViewFeatures
{
	/// <summary>
	/// This class should be registered as a singleton.
	/// [Microsoft.AspNetCore.Mvc.ViewFeatures.DefaultHtmlGenerator](https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/Mvc.ViewFeatures/src/DefaultHtmlGenerator.cs)
	/// </summary>
	public class HtmlGenerator(IAntiforgery antiforgery, ICultureContext cultureContext, HtmlEncoder htmlEncoder, IOptionsMonitor<RequestLocalizationOptions> localizationOptionsMonitor, IModelMetadataProvider metadataProvider, IOptions<MvcViewOptions> optionsAccessor, IUrlHelperFactory urlHelperFactory, ValidationHtmlAttributeProvider validationAttributeProvider) : DefaultHtmlGenerator(antiforgery, optionsAccessor, metadataProvider, urlHelperFactory, htmlEncoder, validationAttributeProvider)
	{
		#region Fields

		private readonly ICultureContext _cultureContext = cultureContext ?? throw new ArgumentNullException(nameof(cultureContext));
		private readonly IOptionsMonitor<RequestLocalizationOptions> _localizationOptionsMonitor = localizationOptionsMonitor ?? throw new ArgumentNullException(nameof(localizationOptionsMonitor));
		private readonly IUrlHelperFactory _urlHelperFactory = urlHelperFactory ?? throw new ArgumentNullException(nameof(urlHelperFactory));

		#endregion

		#region Methods

		public override TagBuilder GenerateActionLink(ViewContext viewContext, string linkText, string actionName, string controllerName, string protocol, string hostname, string fragment, object routeValues, object htmlAttributes)
		{
			ArgumentNullException.ThrowIfNull(viewContext);
			ArgumentNullException.ThrowIfNull(linkText);

			var urlHelper = this._urlHelperFactory.GetUrlHelper(viewContext);
			var url = urlHelper.Action(actionName, controllerName, routeValues, protocol, hostname, fragment);

			url = this.GetResolvedUrl(urlHelper.ActionContext.RouteData.Values, routeValues, url);

			return this.GenerateLink(linkText, url, htmlAttributes);
		}

		public override TagBuilder GeneratePageLink(ViewContext viewContext, string linkText, string pageName, string pageHandler, string protocol, string hostname, string fragment, object routeValues, object htmlAttributes)
		{
			ArgumentNullException.ThrowIfNull(viewContext);
			ArgumentNullException.ThrowIfNull(linkText);

			var urlHelper = this._urlHelperFactory.GetUrlHelper(viewContext);
			var url = urlHelper.Page(pageName, pageHandler, routeValues, protocol, hostname, fragment);

			url = this.GetResolvedUrl(urlHelper.ActionContext.RouteData.Values, routeValues, url);

			return this.GenerateLink(linkText, url, htmlAttributes);
		}

		public override TagBuilder GenerateRouteLink(ViewContext viewContext, string linkText, string routeName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
		{
			ArgumentNullException.ThrowIfNull(viewContext);
			ArgumentNullException.ThrowIfNull(linkText);

			var urlHelper = this._urlHelperFactory.GetUrlHelper(viewContext);
			var url = urlHelper.RouteUrl(routeName, routeValues, protocol, hostName, fragment);

			url = this.GetResolvedUrl(urlHelper.ActionContext.RouteData.Values, routeValues, url);

			return this.GenerateLink(linkText, url, htmlAttributes);
		}

		private string? GetResolvedUrl(RouteValueDictionary contextRoutes, object explicitRoutes, string? url)
		{
			if(url == null)
				return url;

			var uriBuilder = UriBuilderExtension.Create(contextRoutes, this._cultureContext, explicitRoutes, this._localizationOptionsMonitor.CurrentValue, url);

			if(!uriBuilder.IsAbsolute())
			{
				uriBuilder.Path = uriBuilder.Path.TrimEnd(UriBuilderExtension.PathSeparator);

				if(uriBuilder.Path.EndsWith("/Index", StringComparison.OrdinalIgnoreCase))
					uriBuilder.Path = uriBuilder.Path[..^6];
			}

			return uriBuilder.IsAbsolute() ? uriBuilder.Uri.ToString() : uriBuilder.PathAndQueryAndFragment();
		}

		#endregion
	}
}