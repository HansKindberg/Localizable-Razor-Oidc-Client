using Application.Models.Web.Routing;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Application.Models.Web.Mvc.ApplicationModels
{
	/// <summary>
	/// [Razor Pages Localisation - SEO-friendly URLs](https://www.mikesdotnetting.com/article/348/razor-pages-localisation-seo-friendly-urls)
	/// </summary>
	public class CultureRouteModelConvention : IPageRouteModelConvention
	{
		#region Methods

		public void Apply(PageRouteModel model)
		{
			ArgumentNullException.ThrowIfNull(model);

			foreach(var selector in model.Selectors.ToArray())
			{
				model.Selectors.Add(new SelectorModel
				{
					AttributeRouteModel = new AttributeRouteModel
					{
						Order = -3,
						Template = AttributeRouteModel.CombineTemplates($"{{{RouteKeys.Culture}:{RouteKeys.Culture}}}/{{{RouteKeys.UiCulture}:{RouteKeys.UiCulture}}}", selector.AttributeRouteModel!.Template)
					}
				});

				model.Selectors.Add(new SelectorModel
				{
					AttributeRouteModel = new AttributeRouteModel
					{
						Order = -2,
						Template = AttributeRouteModel.CombineTemplates($"{{{RouteKeys.Culture}:{RouteKeys.Culture}}}", selector.AttributeRouteModel!.Template)
					}
				});

				model.Selectors.Add(new SelectorModel
				{
					AttributeRouteModel = new AttributeRouteModel
					{
						Order = -1,
						Template = AttributeRouteModel.CombineTemplates($"{{{RouteKeys.UiCulture}:{RouteKeys.UiCulture}}}", selector.AttributeRouteModel!.Template)
					}
				});
			}
		}

		#endregion
	}
}