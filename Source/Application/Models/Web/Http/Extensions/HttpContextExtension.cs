namespace Application.Models.Web.Http.Extensions
{
	public static class HttpContextExtension
	{
		#region Methods

		public static bool IsActivePage(this HttpContext httpContext, string page)
		{
			ArgumentNullException.ThrowIfNull(httpContext);

			return httpContext.GetRouteValue("page") is string routePage && string.Equals(page, routePage, StringComparison.OrdinalIgnoreCase);
		}

		#endregion
	}
}