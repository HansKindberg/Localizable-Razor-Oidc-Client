using Application.Models.Web.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Application.Pages.Account
{
	[Authorize]
	public class Index(IStringLocalizerFactory localizerFactory) : SitePageModel(localizerFactory)
	{
		#region Methods

		public async Task<IActionResult> OnGet()
		{
			await Task.CompletedTask;

			return this.Page();
		}

		#endregion
	}
}