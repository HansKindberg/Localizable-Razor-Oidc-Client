using Application.Models.Web.Mvc.Extensions;
using Application.Models.Web.Mvc.RazorPages;
using Application.Models.Web.Navigation.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Application.Pages.Account.SignedOut
{
	[AllowAnonymous]
	[SignInUnavailable]
	public class Index(IStringLocalizerFactory localizerFactory, ILoggerFactory loggerFactory) : SitePageModel(localizerFactory)
	{
		#region Fields

		private readonly ILogger _logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger<Index>();

		#endregion

		#region Properties

		public string? ReturnUrl { get; set; }

		#endregion

		#region Methods

		public async Task<IActionResult> OnGet(string? returnUrl)
		{
			await Task.CompletedTask;

			if(!string.IsNullOrWhiteSpace(returnUrl))
				this.ReturnUrl = this.Url.ResolveAndValidateReturnUrl(this._logger, returnUrl);

			return this.Page();
		}

		#endregion
	}
}