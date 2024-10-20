using System.Globalization;
using Application.Models.Web.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Moq;

namespace UnitTests.Models.Web.Routing
{
	public class CultureRouteConstraintTest
	{
		#region Fields

		private static readonly IOptionsMonitor<RequestLocalizationOptions> _requestLocalizationOptionsMonitor = CreateRequestLocalizationOptionsMonitor(CreateRequestLocalizationOptions());

		#endregion

		#region Methods

		private static RequestLocalizationOptions CreateRequestLocalizationOptions()
		{
			var options = new RequestLocalizationOptions();

			options.SupportedCultures ??= [];
			options.SupportedUICultures ??= [];

			options.SupportedCultures.Add(CultureInfo.GetCultureInfo("en-001"));
			options.SupportedCultures.Add(CultureInfo.GetCultureInfo("sv-SE"));

			options.SupportedUICultures.Add(CultureInfo.GetCultureInfo("en"));
			options.SupportedUICultures.Add(CultureInfo.GetCultureInfo("sv"));

			return options;
		}

		private static IOptionsMonitor<RequestLocalizationOptions> CreateRequestLocalizationOptionsMonitor(RequestLocalizationOptions options)
		{
			var optionsMonitorMock = new Mock<IOptionsMonitor<RequestLocalizationOptions>>();

			optionsMonitorMock.Setup(optionsMonitor => optionsMonitor.CurrentValue).Returns(options);

			return optionsMonitorMock.Object;
		}

		[Fact]
		public async Task Match_Test()
		{
			await Task.CompletedTask;

			var cultureRouteConstraint = new CultureRouteConstraint(_requestLocalizationOptionsMonitor);

			Assert.False(cultureRouteConstraint.Match(null, null, "Test", [], RouteDirection.IncomingRequest));
			Assert.False(cultureRouteConstraint.Match(null, null, RouteKeys.Culture, [], RouteDirection.IncomingRequest));
			Assert.False(cultureRouteConstraint.Match(null, null, RouteKeys.UiCulture, [], RouteDirection.IncomingRequest));

			var values = new RouteValueDictionary
			{
				{ RouteKeys.Culture, "en-001" },
				{ RouteKeys.UiCulture, "en" }
			};
			Assert.True(cultureRouteConstraint.Match(null, null, RouteKeys.Culture, values, RouteDirection.IncomingRequest));
			Assert.False(cultureRouteConstraint.Match(null, null, RouteKeys.UiCulture, values, RouteDirection.IncomingRequest));
		}

		#endregion
	}
}