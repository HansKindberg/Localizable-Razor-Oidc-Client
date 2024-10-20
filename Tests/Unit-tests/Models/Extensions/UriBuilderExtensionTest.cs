using System.Globalization;
using Application.Models.Extensions;
using Application.Models.Globalization;
using Application.Models.Web.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Moq;
using UnitTests.Helpers;

namespace UnitTests.Models.Extensions
{
	public class UriBuilderExtensionTest
	{
		#region Methods

		[Fact]
		public async Task ClearSegmentsFromCultureRoutes_IfSegmentsAreEmpty_ShouldWorkProperly()
		{
			await Task.CompletedTask;

			var localization = CreateRequestLocalizationOptions();
			var segments = new List<string>();

			UriBuilderExtension.ClearSegmentsFromCultureRoutes(localization, segments);

			Assert.Empty(segments);
		}

		[Fact]
		public async Task ClearSegmentsFromCultureRoutes_IfSegmentsContainsNoCultureRoutes_ShouldWorkProperly()
		{
			await Task.CompletedTask;

			var localization = CreateRequestLocalizationOptions();

			var segments = new List<string>
			{
				string.Empty,
			};
			UriBuilderExtension.ClearSegmentsFromCultureRoutes(localization, segments);
			Assert.Single(segments);
			Assert.Equal(string.Empty, segments[0]);

			segments.Add(string.Empty);
			UriBuilderExtension.ClearSegmentsFromCultureRoutes(localization, segments);
			Assert.Equal(2, segments.Count);
			Assert.Equal(string.Empty, segments[0]);
			Assert.Equal(string.Empty, segments[1]);

			segments.Add(string.Empty);
			UriBuilderExtension.ClearSegmentsFromCultureRoutes(localization, segments);
			Assert.Equal(3, segments.Count);
			Assert.Equal(string.Empty, segments[0]);
			Assert.Equal(string.Empty, segments[1]);
			Assert.Equal(string.Empty, segments[2]);

			segments[1] = "Test";
			UriBuilderExtension.ClearSegmentsFromCultureRoutes(localization, segments);
			Assert.Equal(3, segments.Count);
			Assert.Equal(string.Empty, segments[0]);
			Assert.Equal("Test", segments[1]);
			Assert.Equal(string.Empty, segments[2]);
		}

		[Fact]
		public async Task ClearSegmentsFromCultureRoutes_IfSegmentsStartsWithUiCulture_ShouldWorkProperly_1()
		{
			await Task.CompletedTask;

			var localization = CreateRequestLocalizationOptions();

			var segments = new List<string>
			{
				"sv-SE"
			};
			UriBuilderExtension.ClearSegmentsFromCultureRoutes(localization, segments);
			Assert.Empty(segments);
		}

		[Fact]
		public async Task ClearSegmentsFromCultureRoutes_IfSegmentsStartsWithUiCulture_ShouldWorkProperly_2()
		{
			await Task.CompletedTask;

			var localization = CreateRequestLocalizationOptions();

			var segments = new List<string>
			{
				"fi-fi"
			};
			UriBuilderExtension.ClearSegmentsFromCultureRoutes(localization, segments);
			Assert.Empty(segments);
		}

		[Fact]
		public async Task ClearSegmentsFromCultureRoutes_IfSegmentsStartsWithUiCulture_ShouldWorkProperly_3()
		{
			await Task.CompletedTask;

			var localization = CreateRequestLocalizationOptions();

			var segments = new List<string>
			{
				"FR-FR"
			};
			UriBuilderExtension.ClearSegmentsFromCultureRoutes(localization, segments);
			Assert.Empty(segments);
		}

		[Fact]
		public async Task ClearSegmentsFromCultureRoutes_IfSegmentsStartsWithUiCulture_ShouldWorkProperly_4()
		{
			await Task.CompletedTask;

			var localization = CreateRequestLocalizationOptions();

			var segments = new List<string>
			{
				string.Empty,
				"sv-SE"
			};
			UriBuilderExtension.ClearSegmentsFromCultureRoutes(localization, segments);
			Assert.Single(segments);
			Assert.Equal(string.Empty, segments[0]);
		}

		[Fact]
		public async Task ClearSegmentsFromCultureRoutes_IfSegmentsStartsWithUiCulture_ShouldWorkProperly_5()
		{
			await Task.CompletedTask;

			var localization = CreateRequestLocalizationOptions();

			var segments = new List<string>
			{
				string.Empty,
				"fi-fi"
			};
			UriBuilderExtension.ClearSegmentsFromCultureRoutes(localization, segments);
			Assert.Single(segments);
			Assert.Equal(string.Empty, segments[0]);
		}

		[Fact]
		public async Task ClearSegmentsFromCultureRoutes_IfSegmentsStartsWithUiCulture_ShouldWorkProperly_6()
		{
			await Task.CompletedTask;

			var localization = CreateRequestLocalizationOptions();

			var segments = new List<string>
			{
				string.Empty,
				"FR-FR"
			};
			UriBuilderExtension.ClearSegmentsFromCultureRoutes(localization, segments);
			Assert.Single(segments);
			Assert.Equal(string.Empty, segments[0]);
		}

		[Fact]
		public async Task ClearSegmentsFromCultureRoutes_IfSegmentsStartsWithUiCulture_ShouldWorkProperly_7()
		{
			await Task.CompletedTask;

			var localization = CreateRequestLocalizationOptions();

			var segments = new List<string>
			{
				string.Empty,
				"FR-fr",
				"sv-SE"
			};
			UriBuilderExtension.ClearSegmentsFromCultureRoutes(localization, segments);
			Assert.Equal(2, segments.Count);
			Assert.Equal(string.Empty, segments[0]);
			Assert.Equal("sv-SE", segments[1]);
		}

		[Fact]
		public async Task ClearSegmentsFromCultureRoutes_IfSegmentsStartsWithUiCultureThatIsNotSupported_ShouldWorkProperly()
		{
			await Task.CompletedTask;

			var localization = CreateRequestLocalizationOptions();

			var segments = new List<string>
			{
				"it-IT"
			};
			UriBuilderExtension.ClearSegmentsFromCultureRoutes(localization, segments);
			Assert.Single(segments);
			Assert.Equal("it-IT", segments[0]);

			segments.Insert(0, string.Empty);
			UriBuilderExtension.ClearSegmentsFromCultureRoutes(localization, segments);
			Assert.Equal(2, segments.Count);
			Assert.Equal(string.Empty, segments[0]);
			Assert.Equal("it-IT", segments[1]);
		}

		[Fact]
		public async Task Create_WithFiveParameters_IfTheCultureRoutesAreNotPartOfTheUrl_ShouldWorkProperly_1()
		{
			await Task.CompletedTask;

			var uriBuilder = UriBuilderExtension.Create([], CreateCultureContext(CultureInfo.GetCultureInfo("en-001"), CultureInfo.GetCultureInfo("en")), new object(), CreateRequestLocalizationOptions(), "/Account");

			Assert.NotNull(uriBuilder);
			Assert.Empty(uriBuilder.Fragment);
			Assert.Equal("relative-host", uriBuilder.Host);
			Assert.Equal("/en/Account", uriBuilder.Path);
			Assert.Equal(-1, uriBuilder.Port);
			Assert.Empty(uriBuilder.Query);
			Assert.Equal("relative-scheme", uriBuilder.Scheme);
		}

		[Fact]
		public async Task Create_WithFiveParameters_IfTheCultureRoutesAreNotPartOfTheUrl_ShouldWorkProperly_2()
		{
			await Task.CompletedTask;

			var routes = new RouteValueDictionary
			{
				{ RouteKeys.UiCulture, "en" }
			};

			var uriBuilder = UriBuilderExtension.Create(routes, CreateCultureContext(CultureInfo.GetCultureInfo("en-001"), CultureInfo.GetCultureInfo("en")), new object(), CreateRequestLocalizationOptions(), "/Account");

			Assert.NotNull(uriBuilder);
			Assert.Empty(uriBuilder.Fragment);
			Assert.Equal("relative-host", uriBuilder.Host);
			Assert.Equal("/en/Account", uriBuilder.Path);
			Assert.Equal(-1, uriBuilder.Port);
			Assert.Empty(uriBuilder.Query);
			Assert.Equal("relative-scheme", uriBuilder.Scheme);
		}

		[Fact]
		public async Task Create_WithFiveParameters_IfTheUrlParameterIsAPathAndQuery_ShouldWorkProperly()
		{
			await Task.CompletedTask;

			var uriBuilder = UriBuilderExtension.Create([], CreateCultureContext(), new object(), CreateRequestLocalizationOptions(), "/path?Query=value");

			Assert.NotNull(uriBuilder);
			Assert.Empty(uriBuilder.Fragment);
			Assert.Equal("relative-host", uriBuilder.Host);
			Assert.Equal("/path", uriBuilder.Path);
			Assert.Equal(-1, uriBuilder.Port);
			Assert.Equal("?Query=value", uriBuilder.Query);
			Assert.Equal("relative-scheme", uriBuilder.Scheme);
		}

		[Fact]
		public async Task Create_WithFiveParameters_IfTheUrlParameterIsASlash_ShouldWorkProperly()
		{
			await Task.CompletedTask;

			var uriBuilder = UriBuilderExtension.Create([], CreateCultureContext(), new object(), CreateRequestLocalizationOptions(), "/");

			Assert.NotNull(uriBuilder);
			Assert.Empty(uriBuilder.Fragment);
			Assert.Equal("relative-host", uriBuilder.Host);
			Assert.Equal("/", uriBuilder.Path);
			Assert.Equal(-1, uriBuilder.Port);
			Assert.Empty(uriBuilder.Query);
			Assert.Equal("relative-scheme", uriBuilder.Scheme);
		}

		[Fact]
		public async Task Create_WithFiveParameters_IfTheUrlParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			await Task.CompletedTask;

			var argumentNullException = Assert.Throws<ArgumentNullException>(() => UriBuilderExtension.Create([], CreateCultureContext(), new object(), CreateRequestLocalizationOptions(), null!));

			Assert.NotNull(argumentNullException);
			Assert.Equal("url", argumentNullException.ParamName);
			Assert.StartsWith("Value cannot be null.", argumentNullException.Message);
		}

		private static ICultureContext CreateCultureContext()
		{
			return CreateCultureContext(CultureInfo.GetCultureInfo(LocalizationHelper.DefaultCurrentCulture), CultureInfo.GetCultureInfo(LocalizationHelper.DefaultCurrentUiCulture));
		}

		private static ICultureContext CreateCultureContext(CultureInfo currentCulture, CultureInfo currentUiCulture)
		{
			var cultureContextMock = new Mock<ICultureContext>();

			cultureContextMock.Setup(cultureContext => cultureContext.CurrentCulture).Returns(currentCulture);
			cultureContextMock.Setup(cultureContext => cultureContext.CurrentUiCulture).Returns(currentUiCulture);
			cultureContextMock.Setup(cultureContext => cultureContext.MasterCulture).Returns(CultureInfo.GetCultureInfo(LocalizationHelper.MasterCulture));
			cultureContextMock.Setup(cultureContext => cultureContext.MasterUiCulture).Returns(CultureInfo.GetCultureInfo(LocalizationHelper.MasterUiCulture));

			return cultureContextMock.Object;
		}

		private static RequestLocalizationOptions CreateRequestLocalizationOptions()
		{
			var options = new RequestLocalizationOptions();

			options.SupportedCultures!.Clear();
			foreach(var culture in LocalizationHelper.Cultures)
			{
				options.SupportedCultures.Add(CultureInfo.GetCultureInfo(culture));
			}

			options.SupportedUICultures!.Clear();
			foreach(var uiCulture in LocalizationHelper.UiCultures)
			{
				options.SupportedUICultures.Add(CultureInfo.GetCultureInfo(uiCulture));
			}

			return options;
		}

		[Fact]
		public async Task GetResolvedSegments_Test_Test_Test_Test_Test_Test_Test_Test_Test_Test_Test_Test_Test_Test_Test_Test_Test_Test()
		{
			await Task.CompletedTask;

			const string uiCultureName = "en";

			var uiCulture = CultureInfo.GetCultureInfo(uiCultureName);

			var routes = new RouteValueDictionary
			{
				{ "page", "/Index" },
				{ "ui-culture", uiCultureName }
			};

			var segments = UriBuilderExtension.GetResolvedSegments(CreateCultureContext(CultureInfo.GetCultureInfo("en-001"), uiCulture), CreateRequestLocalizationOptions(), "/Account", routes, uiCulture);

			Assert.Equal(3, segments.Count);
			Assert.Equal(string.Empty, segments[0]);
			Assert.Equal("en", segments[1]);
			Assert.Equal("Account", segments[2]);
		}

		[Fact]
		public async Task ResolveUiCultureRouteSegment_ShouldWorkProperly_1()
		{
			await Task.CompletedTask;

			var cultureContext = CreateCultureContext(CultureInfo.GetCultureInfo("sv-SE"), CultureInfo.GetCultureInfo("sv"));
			var segments = new List<string>();

			UriBuilderExtension.ResolveUiCultureRouteSegment(cultureContext, CreateRequestLocalizationOptions(), [], segments, cultureContext.CurrentUiCulture);

			Assert.Empty(segments);
		}

		[Fact]
		public async Task ResolveUiCultureRouteSegment_ShouldWorkProperly_2()
		{
			await Task.CompletedTask;

			var cultureContext = CreateCultureContext(CultureInfo.GetCultureInfo("sv-SE"), CultureInfo.GetCultureInfo("sv"));
			var rotues = new RouteValueDictionary
			{
				{ RouteKeys.UiCulture, cultureContext.CurrentUiCulture.Name }
			};
			var segments = new List<string>();

			UriBuilderExtension.ResolveUiCultureRouteSegment(cultureContext, CreateRequestLocalizationOptions(), rotues, segments, cultureContext.CurrentUiCulture);

			Assert.Single(segments);
			Assert.Equal("sv", segments[0]);
		}

		[Fact]
		public async Task ResolveUiCultureRouteSegment_ShouldWorkProperly_3()
		{
			await Task.CompletedTask;

			var cultureContext = CreateCultureContext(CultureInfo.GetCultureInfo("en-001"), CultureInfo.GetCultureInfo("en"));
			var segments = new List<string>();

			UriBuilderExtension.ResolveUiCultureRouteSegment(cultureContext, CreateRequestLocalizationOptions(), [], segments, cultureContext.CurrentUiCulture);

			Assert.Single(segments);
			Assert.Equal("en", segments[0]);
		}

		[Fact]
		public async Task ResolveUiCultureRouteSegment_ShouldWorkProperly_4()
		{
			await Task.CompletedTask;

			var cultureContext = CreateCultureContext(CultureInfo.GetCultureInfo("en-001"), CultureInfo.GetCultureInfo("en"));
			var segments = new List<string>
			{
				string.Empty
			};

			UriBuilderExtension.ResolveUiCultureRouteSegment(cultureContext, CreateRequestLocalizationOptions(), [], segments, cultureContext.CurrentUiCulture);

			Assert.Equal(2, segments.Count);
			Assert.Equal(string.Empty, segments[0]);
			Assert.Equal("en", segments[1]);
		}

		#endregion
	}
}