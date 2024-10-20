using System.Security.Claims;
using Application.Models.Collections.Generic.Extensions;
using Application.Models.Data.Identity;
using Application.Models.Extensions;
using Application.Models.Logging.Extensions;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using ClaimComparer = Application.Models.Security.Claims.ClaimComparer;

namespace Application.Models.Web.Identity
{
	public class IdentityFacade(ILoggerFactory loggerFactory, SignInManager<User> signInManager, UserManager userManager) : IIdentityFacade
	{
		#region Fields

		private readonly ILogger _logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger<IdentityFacade>();
		private readonly SignInManager<User> _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
		private readonly UserManager _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

		#endregion

		#region Methods

		private static bool ClaimsAreEqual(IList<Claim>? firstClaims, IList<Claim>? secondClaims)
		{
			if(firstClaims == null)
				return secondClaims == null;

			if(secondClaims == null)
				return false;

			if(firstClaims.Count != secondClaims.Count)
				return false;

			foreach(var firstClaim in firstClaims)
			{
				if(!secondClaims.Any(secondClaim => string.Equals(firstClaim.Type, secondClaim.Type, StringComparison.OrdinalIgnoreCase) && string.Equals(firstClaim.Value, secondClaim.Value, StringComparison.Ordinal)))
					return false;
			}

			return true;
		}

		private static string CreateErrorMessage(IdentityResult identityResult)
		{
			ArgumentNullException.ThrowIfNull(identityResult);

			return string.Join(" ", identityResult.Errors.Select(identityError => identityError.Description));
		}

		public void CreateFirstUserIfNotExist(string email, string password, string userName)
		{
			ArgumentNullException.ThrowIfNull(email);
			ArgumentNullException.ThrowIfNull(password);
			ArgumentNullException.ThrowIfNull(userName);

			if(this._userManager.Users.Any())
				return;

			var autoSaveChanges = this._userManager.Store.AutoSaveChanges;
			this._userManager.Store.AutoSaveChanges = true;

			try
			{
				var user = new User
				{
					Email = email,
					UserName = userName
				};

				var result = this._userManager.CreateAsync(user, password).Result;

				if(!result.Succeeded)
					throw new InvalidOperationException($"Could not create user {userName.ToStringRepresentation()}.");
			}
			finally
			{
				this._userManager.Store.AutoSaveChanges = autoSaveChanges;
			}
		}

		public async Task<IList<UserModel>> FindUsers(string userName)
		{
			/*
				This is not the correct way to do it. We loop all users and check if they should be included in the search-result. If we have a lot of users it will take time. We should handle it with SQL and "WHERE column LIKE '%text%'" syntax instead.
			*/

			var users = new List<UserModel>();

			foreach(var user in this._userManager.Users)
			{
				if(!user.UserName.Like(userName))
					continue;

				var model = new UserModel
				{
					Email = user.Email,
					Subject = user.Id,
					UserName = user.UserName
				};

				foreach(var claim in await this._userManager.GetClaimsAsync(user))
				{
					if(claim.Type.Equals(JwtClaimTypes.Email, StringComparison.OrdinalIgnoreCase))
						model.Email = claim.Value;

					if(claim.Type.Equals(JwtClaimTypes.PreferredUserName, StringComparison.OrdinalIgnoreCase))
						model.PreferredUserName = claim.Value;
				}

				users.Add(model);
			}

			users.Sort((first, second) => string.Compare(first.UserName, second.UserName, StringComparison.Ordinal));

			return users;
		}

		private async Task SaveClaims(IList<Claim> claims, User user)
		{
			var logPrefix = $"{this.GetType().FullName}.{nameof(this.SaveClaims)}:";
			this._logger.LogDebugIfEnabled($"{logPrefix} user-id = {user.Id.ToStringRepresentation()}, starting...");

			ArgumentNullException.ThrowIfNull(claims);
			ArgumentNullException.ThrowIfNull(user);

			try
			{
				await Task.CompletedTask;

				claims.Sort(ClaimComparer.Instance);

				var i = 0;
				var userClaimsToRemove = new List<IdentityUserClaim<string>>();

				foreach(var userClaim in this._userManager.Store.Context.UserClaims.Where(claim => claim.UserId == user.Id).OrderBy(claim => claim.Id))
				{
					if(claims.Count < i + 1)
					{
						userClaimsToRemove.Add(userClaim);
					}
					else
					{
						var claim = claims[i];
						const StringComparison comparison = StringComparison.OrdinalIgnoreCase;

						if(!string.Equals(claim.Type, userClaim.ClaimType, comparison) || !string.Equals(claim.Value, userClaim.ClaimValue, comparison))
						{
							this._logger.LogDebugIfEnabled($"{logPrefix} changing claim with id {userClaim.Id.ToStringRepresentation()} from type {userClaim.ClaimType.ToStringRepresentation()} to type {claim.Type.ToStringRepresentation()} and from value {userClaim.ClaimValue.ToStringRepresentation()} to value {claim.Value.ToStringRepresentation()}.");

							userClaim.ClaimType = claim.Type;
							userClaim.ClaimValue = claim.Value;
						}
					}

					i++;
				}

				if(userClaimsToRemove.Count > 0)
				{
					this._logger.LogDebugIfEnabled($"{logPrefix} removing {userClaimsToRemove.Count} claims with id's: {string.Join(", ", userClaimsToRemove.Select(userClaim => userClaim.Id))}");
					this._userManager.Store.Context.UserClaims.RemoveRange(userClaimsToRemove);
				}
				else if(claims.Count > i)
				{
					foreach(var claim in claims.Skip(i))
					{
						var claimToAdd = new IdentityUserClaim<string>
						{
							ClaimType = claim.Type,
							ClaimValue = claim.Value,
							UserId = user.Id
						};

						this._logger.LogDebugIfEnabled($"{logPrefix} adding claim with type {claim.Type.ToStringRepresentation()} and value {claim.Value.ToStringRepresentation()}.");
						await this._userManager.Store.Context.UserClaims.AddAsync(claimToAdd);
					}
				}

				var savedChanges = await this._userManager.Store.Context.SaveChangesAsync();
				this._logger.LogDebugIfEnabled($"{logPrefix} saved changes = {savedChanges}");
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException("Could not save claims for user.", exception);
			}
		}

		public async Task<bool> SignIn(string password, string userName)
		{
			var result = await this._signInManager.PasswordSignInAsync(userName, password, false, false);

			return result.Succeeded;
		}

		public async Task SignIn(IList<Claim> additionalClaims, AuthenticationProperties authenticationProperties, User user)
		{
			await this._signInManager.SignInWithClaimsAsync(user, authenticationProperties, additionalClaims);
		}

		public async Task SignOut()
		{
			await this._signInManager.SignOutAsync();
		}

		public async Task<User> SynchronizeUser(IList<Claim> claims, string provider, string providerKey)
		{
			ArgumentNullException.ThrowIfNull(claims);
			ArgumentNullException.ThrowIfNull(provider);
			ArgumentNullException.ThrowIfNull(providerKey);

			var user = await this._userManager.FindByLoginAsync(provider, providerKey);
			var userClaims = (user != null ? await this._userManager.GetClaimsAsync(user) : null) ?? [];

			if(user != null && ClaimsAreEqual(claims, userClaims))
				return user;

			var autoSaveChanges = this._userManager.Store.AutoSaveChanges;
			this._userManager.Store.AutoSaveChanges = true;

			try
			{
				if(user == null)
				{
					user = new User
					{
						//UserName = Guid.NewGuid().ToString()
						UserName = $"{claims.First(claim => claim.Type.Equals(JwtClaimTypes.PreferredUserName, StringComparison.OrdinalIgnoreCase)).Value.Replace(" ", string.Empty)}-{providerKey.Replace("-", string.Empty)}@{provider}"
					};

					var identityResult = await this._userManager.CreateAsync(user);

					if(!identityResult.Succeeded)
						throw new InvalidOperationException($"Could not create user: {CreateErrorMessage(identityResult)}");

					identityResult = await this._userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerKey, provider));

					if(!identityResult.Succeeded)
						throw new InvalidOperationException($"Could not add login for user: {CreateErrorMessage(identityResult)}");
				}

				await this.SaveClaims(claims, user);

				return user;
			}
			finally
			{
				this._userManager.Store.AutoSaveChanges = autoSaveChanges;
			}
		}

		#endregion
	}
}