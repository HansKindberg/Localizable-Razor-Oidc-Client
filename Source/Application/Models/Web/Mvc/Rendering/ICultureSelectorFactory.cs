using Microsoft.AspNetCore.Mvc;

namespace Application.Models.Web.Mvc.Rendering
{
	public interface ICultureSelectorFactory
	{
		#region Methods

		CultureSelector Create(IUrlHelper urlHelper);

		#endregion
	}
}