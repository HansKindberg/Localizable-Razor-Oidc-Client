using System.Globalization;

namespace UnitTests.Helpers
{
	public static class WebHelper
	{
		#region Fields

		private const char _pathSeparator = '/';

		#endregion

		#region Methods

		public static void ResolvePathAndQuery(string? pathAndQuery, out string? cultureRoute, out CultureInfo currentCulture, out CultureInfo currentUiCulture, out string? path, out string? query, out string? uiCultureRoute)
		{
			cultureRoute = null;
			currentCulture = CultureInfo.GetCultureInfo(LocalizationHelper.DefaultCurrentCulture);
			currentUiCulture = CultureInfo.GetCultureInfo(LocalizationHelper.DefaultCurrentUiCulture);
			path = null;
			query = null;
			uiCultureRoute = null;

			if(string.IsNullOrEmpty(pathAndQuery))
				return;

			var parts = pathAndQuery.Split('?', 2);

			if(parts.Length > 0)
				path = parts[0];

			if(parts.Length > 1)
				query = "?" + parts[1];

			if(path == null)
				return;

			var segments = path.Trim(_pathSeparator).Split(_pathSeparator);

			if(segments.Length > 0)
			{
				var firstSegment = segments[0];

				if(LocalizationHelper.Cultures.Contains(firstSegment))
				{
					cultureRoute = firstSegment;
					currentCulture = CultureInfo.GetCultureInfo(firstSegment);
				}

				if(cultureRoute == null && LocalizationHelper.UiCultures.Contains(firstSegment))
				{
					currentUiCulture = CultureInfo.GetCultureInfo(firstSegment);
					uiCultureRoute = firstSegment;
				}
			}

			if(cultureRoute != null && uiCultureRoute == null)
				currentUiCulture = currentCulture.Parent;

			if(segments.Length > 1 && cultureRoute != null)
			{
				var secondSegment = segments[1];

				if(LocalizationHelper.UiCultures.Contains(secondSegment))
				{
					currentUiCulture = CultureInfo.GetCultureInfo(secondSegment);
					uiCultureRoute = secondSegment;
				}
			}
		}

		#endregion
	}
}