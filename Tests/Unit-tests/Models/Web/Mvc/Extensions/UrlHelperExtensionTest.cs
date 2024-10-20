using Application.Models.Web.Mvc.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace UnitTests.Models.Web.Mvc.Extensions
{
	public class UrlHelperExtensionTest
	{
		#region Methods

		private static UrlHelperBase CreateUrlHelper(string? pathBase)
		{
			var actionContext = new ActionContext
			{
				ActionDescriptor = new ActionDescriptor(),
				HttpContext = new DefaultHttpContext(),
				RouteData = new RouteData()
			};

			actionContext.HttpContext.Request.PathBase = pathBase;

			var urlHelperMock = new Mock<UrlHelperBase>(actionContext) { CallBase = true };

			return urlHelperMock.Object;
		}

		[Fact]
		public async Task ResolveReturnUrl_ShouldWorkProperly_1()
		{
			await Task.CompletedTask;

			var urlHelper = CreateUrlHelper(null);
			Assert.Equal("/", urlHelper.ResolveReturnUrl(null));
		}

		[Fact]
		public async Task ResolveReturnUrl_ShouldWorkProperly_2()
		{
			await Task.CompletedTask;

			var urlHelper = CreateUrlHelper(null);
			Assert.Equal("/", urlHelper.ResolveReturnUrl(string.Empty));
		}

		[Fact]
		public async Task ResolveReturnUrl_ShouldWorkProperly_3()
		{
			await Task.CompletedTask;

			const string returnUrl = "   ";
			var urlHelper = CreateUrlHelper(null);
			Assert.Equal(returnUrl, urlHelper.ResolveReturnUrl(returnUrl));
		}

		[Fact]
		public async Task ResolveReturnUrl_ShouldWorkProperly_4()
		{
			await Task.CompletedTask;

			var urlHelper = CreateUrlHelper("/Test");
			Assert.Equal("/Test/", urlHelper.ResolveReturnUrl(null));
		}

		[Fact]
		public async Task ResolveReturnUrl_ShouldWorkProperly_5()
		{
			await Task.CompletedTask;

			var urlHelper = CreateUrlHelper("/Test");
			Assert.Equal("/Test/", urlHelper.ResolveReturnUrl(string.Empty));
		}

		[Fact]
		public async Task ResolveReturnUrl_ShouldWorkProperly_6()
		{
			await Task.CompletedTask;

			const string returnUrl = "   ";
			var urlHelper = CreateUrlHelper("/Test");
			Assert.Equal(returnUrl, urlHelper.ResolveReturnUrl(returnUrl));
		}

		#endregion
	}
}