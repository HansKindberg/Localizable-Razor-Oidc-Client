using Application.Models.Logging.Extensions;
using Application.Models.Web.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace Application.Models.Web.Mvc.Extensions
{
	public static class UrlHelperExtension
	{
		#region Methods

		public static CultureSelector CreateCultureSelector(this IUrlHelper url)
		{
			ArgumentNullException.ThrowIfNull(url);

			var cultureSelectorFactory = url.ActionContext.HttpContext.RequestServices.GetRequiredService<ICultureSelectorFactory>();

			return cultureSelectorFactory.Create(url);
		}

		public static string ResolveAndValidateReturnUrl(this IUrlHelper urlHelper, ILogger logger, string? returnUrl)
		{
			ArgumentNullException.ThrowIfNull(urlHelper);
			ArgumentNullException.ThrowIfNull(logger);

			returnUrl = urlHelper.ResolveReturnUrl(returnUrl);

			if(urlHelper.IsLocalUrl(returnUrl))
				return returnUrl;

			var message = $"The return-url \"{returnUrl}\" is invalid.";

			logger.LogErrorIfEnabled(message);

			throw new InvalidOperationException(message);
		}

		/// <summary>
		/// https://github.com/dotnet/aspnetcore/blob/c1bb9da12950a6f4e2b0d13bec2e8e0298d48f3e/src/Mvc/Mvc.Core/src/Routing/UrlHelperBase.cs#L51
		/// public virtual string? Content(string? contentPath) => Content(ActionContext.HttpContext, contentPath);
		///
		/// https://github.com/dotnet/aspnetcore/blob/c1bb9da12950a6f4e2b0d13bec2e8e0298d48f3e/src/Mvc/Mvc.Core/src/Routing/UrlHelperBase.cs#L296
		/// internal static string? Content(HttpContext httpContext, string? contentPath)
		/// {
		///		if (string.IsNullOrEmpty(contentPath))
		///		{
		///			return null;
		///		}
		///		else if (contentPath[0] == '~')
		///		{
		///			var segment = new PathString(contentPath.Substring(1));
		///			var applicationPath = httpContext.Request.PathBase;
		///
		///			var path = applicationPath.Add(segment);
		///			Debug.Assert(path.HasValue);
		///			return path.Value;
		///		}
		///
		///		return contentPath;
		/// }
		/// </summary>
		public static string ResolveReturnUrl(this IUrlHelper urlHelper, string? returnUrl)
		{
			ArgumentNullException.ThrowIfNull(urlHelper);

			// urlHelper.Content("~/") resolves the path correctly even if the application is hosted as a virtual directory.
			return string.IsNullOrEmpty(returnUrl) ? urlHelper.Content("~/") : returnUrl;
		}

		#endregion
	}
}