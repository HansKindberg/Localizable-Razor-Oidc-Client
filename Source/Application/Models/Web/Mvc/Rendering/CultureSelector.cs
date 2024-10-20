using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Models.Web.Mvc.Rendering
{
	public class CultureSelector
	{
		#region Properties

		public IList<SelectListItem> List { get; } = [];
		public SelectListItem Selected => this.List.FirstOrDefault(item => item.Selected) ?? new SelectListItem("-", "~/", true);

		#endregion
	}
}