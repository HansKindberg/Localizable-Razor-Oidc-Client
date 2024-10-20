using Application.Models.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Application.Models.Web.Identity
{
	public class UserStore : UserStore<User, Role, IdentityContext>
	{
		#region Constructors

		public UserStore(IdentityContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
		{
			this.AutoSaveChanges = false;
		}

		#endregion
	}
}