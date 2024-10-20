using Application.Models.Web.Identity;
using Application.Models.Web.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Application.Pages.Account.Users
{
	public class Index(IIdentityFacade identity, IStringLocalizerFactory localizerFactory) : SitePageModel(localizerFactory)
	{
		#region Fields

		private readonly IIdentityFacade _identity = identity ?? throw new ArgumentNullException(nameof(identity));

		#endregion

		#region Properties

		public bool Search { get; set; }
		public string? UserName { get; set; }
		public IList<UserModel>? Users { get; set; }

		#endregion

		#region Methods

		public async Task<IActionResult> OnGet(string? userName, bool search)
		{
			this.Search = search;
			this.UserName = userName;

			if(this.Search && !string.IsNullOrEmpty(userName))
				this.Users = await this._identity.FindUsers(userName);

			return this.Page();
		}

		#endregion
	}
}