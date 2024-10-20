using System.Globalization;
using Application.Models.Globalization;
using Application.Models.Web.Routing;
using Microsoft.AspNetCore.Http.Extensions;

namespace Application.Models.Extensions
{
	public static class UriBuilderExtension
	{
		#region Fields

		private const string _relativeHost = "relative-host";
		private const string _relativeScheme = "relative-scheme";
		public const char PathSeparator = '/';

		#endregion

		#region Methods

		public static void ClearSegmentsFromCultureRoutes(RequestLocalizationOptions localization, IList<string> segments)
		{
			ArgumentNullException.ThrowIfNull(localization);
			ArgumentNullException.ThrowIfNull(segments);

			if(segments.Count == 0 || segments is [""] || segments.All(string.IsNullOrEmpty))
				return;

			var addLeadingEmptySegment = RemoveLeadingEmptySegment(segments);

			if(segments.Count == 0)
				return;

			foreach(var supportedCulture in localization.SupportedCultures ?? [])
			{
				if(!segments[0].Equals(supportedCulture.Name, StringComparison.OrdinalIgnoreCase))
					continue;

				segments.RemoveAt(0);
				break;
			}

			if(segments.Count > 0)
			{
				foreach(var supportedUiCulture in localization.SupportedUICultures ?? [])
				{
					if(!segments[0].Equals(supportedUiCulture.Name, StringComparison.OrdinalIgnoreCase))
						continue;

					segments.RemoveAt(0);
					break;
				}
			}

			if(!addLeadingEmptySegment)
				return;

			segments.Insert(0, string.Empty);
		}

		/// <summary>
		/// Creates a "relative" uri-builder.
		/// </summary>
		public static UriBuilder Create(HttpRequest httpRequest)
		{
			ArgumentNullException.ThrowIfNull(httpRequest);

			var uriBuilder = new UriBuilder(httpRequest.GetDisplayUrl())
			{
				Host = _relativeHost,
				Port = -1,
				Scheme = _relativeScheme
			};

			return uriBuilder;
		}

		public static UriBuilder Create(RouteValueDictionary contextRoutes, ICultureContext cultureContext, object explicitRoutes, RequestLocalizationOptions localization, string url)
		{
			ArgumentNullException.ThrowIfNull(contextRoutes);
			ArgumentNullException.ThrowIfNull(cultureContext);
			ArgumentNullException.ThrowIfNull(url);

			UriBuilder uriBuilder;

			if(Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri))
			{
				if(uri.IsAbsoluteUri)
				{
					uriBuilder = new UriBuilder(uri);
				}
				else
				{
					var parts = uri.OriginalString.Split('?', 2);

					uriBuilder = new UriBuilder
					{
						Host = _relativeHost,
						Path = parts[0],
						Port = -1,
						Scheme = _relativeScheme
					};

					if(parts.Length > 1)
						uriBuilder.Query = parts[1];
				}
			}
			else
			{
				throw new InvalidOperationException($"Could not create an uri-builder from url {url.ToStringRepresentation()}.");
			}

			var rotues = new RouteValueDictionary(explicitRoutes);

			foreach(var item in contextRoutes)
			{
				if(rotues.ContainsKey(item.Key))
					continue;

				rotues.Add(item.Key, item.Value);
			}

			uriBuilder.ResolvePath(cultureContext, localization, rotues, cultureContext.CurrentUiCulture);

			return uriBuilder;
		}

		public static List<string> GetResolvedSegments(ICultureContext cultureContext, RequestLocalizationOptions localization, string path, RouteValueDictionary routes, CultureInfo uiCulture)
		{
			ArgumentNullException.ThrowIfNull(path);

			var segments = path.Split(PathSeparator).ToList();

			ResolveUiCultureRouteSegment(cultureContext, localization, routes, segments, uiCulture);

			return segments;
		}

		public static bool IsAbsolute(this UriBuilder uriBuilder)
		{
			ArgumentNullException.ThrowIfNull(uriBuilder);

			return !(uriBuilder.Host.Equals(_relativeHost, StringComparison.OrdinalIgnoreCase) && uriBuilder.Scheme.Equals(_relativeScheme, StringComparison.OrdinalIgnoreCase));
		}

		public static string PathAndQueryAndFragment(this UriBuilder uriBuilder)
		{
			ArgumentNullException.ThrowIfNull(uriBuilder);

			return uriBuilder.Path + uriBuilder.Query + uriBuilder.Fragment;
		}

		public static bool RemoveLeadingEmptySegment(IList<string> segments)
		{
			ArgumentNullException.ThrowIfNull(segments);

			if(segments.Count == 0)
				return false;

			if(segments[0] != string.Empty)
				return false;

			segments.RemoveAt(0);
			return true;
		}

		/// <summary>
		/// Resolves the path regarding localization.
		/// </summary>
		public static void ResolvePath(this UriBuilder uriBuilder, ICultureContext cultureContext, RequestLocalizationOptions localization, RouteValueDictionary routes, CultureInfo uiCulture)
		{
			ArgumentNullException.ThrowIfNull(uriBuilder);

			var segments = GetResolvedSegments(cultureContext, localization, uriBuilder.Path, routes, uiCulture);

			uriBuilder.Path = string.Join(PathSeparator, segments);
		}

		public static void ResolveUiCultureRouteSegment(ICultureContext cultureContext, RequestLocalizationOptions localization, RouteValueDictionary routes, IList<string> segments, CultureInfo uiCulture)
		{
			ArgumentNullException.ThrowIfNull(cultureContext);
			ArgumentNullException.ThrowIfNull(routes);
			ArgumentNullException.ThrowIfNull(segments);
			ArgumentNullException.ThrowIfNull(uiCulture);

			var initialNumberOfSegments = segments.Count;
			ClearSegmentsFromCultureRoutes(localization, segments);

			var masterUiCulture = cultureContext.MasterUiCulture;

			var uiCultureRoute = routes[RouteKeys.UiCulture] as string;

			if(!masterUiCulture.Equals(uiCulture) || (masterUiCulture.Equals(uiCulture) && masterUiCulture.Name.Equals(uiCultureRoute, StringComparison.OrdinalIgnoreCase)))
			{
				var addLeadingEmptySegment = RemoveLeadingEmptySegment(segments);

				if(segments.Count == 0 || segments[0] != string.Empty || initialNumberOfSegments != 2)
					segments.Insert(0, uiCulture.Name);
				else
					segments[0] = uiCulture.Name;

				if(!addLeadingEmptySegment)
					return;

				segments.Insert(0, string.Empty);
			}
		}

		#endregion
	}
}