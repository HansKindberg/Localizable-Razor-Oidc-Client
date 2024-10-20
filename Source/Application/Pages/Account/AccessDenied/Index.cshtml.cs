using Application.Models.Logging.Extensions;
using Application.Models.Web.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Application.Pages.Account.AccessDenied
{
	[AllowAnonymous]
	public class Index(IStringLocalizerFactory localizerFactory, ILoggerFactory loggerFactory) : SitePageModel(localizerFactory)
	{
		#region Fields

		private readonly ILogger _logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger<Index>();

		#endregion

		#region Methods

		public async Task<IActionResult> OnGet()
		{
			await Task.CompletedTask;

			this._logger.LogInformationIfEnabled($"Access denied for user: {(this.User.Identity!.IsAuthenticated ? $"\"{this.User.Identity.Name}\"" : "Anonymous")}");

			return this.Page();
		}

		#endregion
	}
}