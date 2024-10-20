using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace Application.Models.Web.Mvc.RazorPages
{
	/// <summary>
	/// Base class for razor-pages that is not EPiServer content razor-pages.
	/// </summary>
	public abstract class SitePageModel : PageModel
	{
		#region Fields

		public const string ViewDataTitleKey = "title";

		#endregion

		#region Constructors

		protected SitePageModel(IStringLocalizerFactory localizerFactory)
		{
			this.Localizer = (localizerFactory ?? throw new ArgumentNullException(nameof(localizerFactory))).Create(this.GetType());
			this.SiteName = this.Localizer[":common.site-name"];
		}

		#endregion

		#region Properties

		protected IStringLocalizer Localizer { get; }
		protected string SiteName { get; }

		#endregion

		#region Methods

		public override void OnPageHandlerExecuted(PageHandlerExecutedContext context)
		{
			base.OnPageHandlerExecuted(context);

			this.ViewData[ViewDataTitleKey] ??= $"{this.Localizer[ViewDataTitleKey]} - {this.SiteName}";
		}

		#endregion
	}
}