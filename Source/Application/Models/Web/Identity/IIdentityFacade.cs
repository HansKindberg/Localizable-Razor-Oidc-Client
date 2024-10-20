using System.Security.Claims;
using Application.Models.Data.Identity;
using Microsoft.AspNetCore.Authentication;

namespace Application.Models.Web.Identity
{
	public interface IIdentityFacade
	{
		#region Methods

		void CreateFirstUserIfNotExist(string email, string password, string userName);
		Task<IList<UserModel>> FindUsers(string userName);
		Task<bool> SignIn(string password, string userName);
		Task SignIn(IList<Claim> additionalClaims, AuthenticationProperties authenticationProperties, User user);
		Task SignOut();

		/// <summary>
		/// If the user exists it gets it, updates the claims and returns it.
		/// If the user does not exist it creates the user and returns it.
		/// </summary>
		Task<User> SynchronizeUser(IList<Claim> claims, string provider, string providerKey);

		#endregion
	}
}