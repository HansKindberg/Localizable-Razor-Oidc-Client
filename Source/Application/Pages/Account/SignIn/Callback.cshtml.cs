using System.Security.Claims;
using Application.Models.Extensions;
using Application.Models.Logging.Extensions;
using Application.Models.Web;
using Application.Models.Web.Identity;
using Application.Models.Web.Mvc.Extensions;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application.Pages.Account.SignIn
{
	[AllowAnonymous]
	public class Callback(IIdentityFacade identity, ILoggerFactory loggerFactory) : PageModel
	{
		#region Fields

		private readonly IIdentityFacade _identity = identity ?? throw new ArgumentNullException(nameof(identity));
		private readonly ILogger _logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger<Callback>();

		#endregion

		#region Methods

		private static List<Claim> CreateAdditionalClaims(AuthenticateResult authenticateResult, string scheme)
		{
			var claims = new List<Claim>();

			var authenticationMethodClaim = GetClaim(authenticateResult, JwtClaimTypes.AuthenticationMethod);
			claims.Add(new Claim(authenticationMethodClaim.Type, authenticationMethodClaim.Value, authenticationMethodClaim.ValueType));

			claims.Add(new Claim(JwtClaimTypes.IdentityProvider, scheme));

			var sessionIdClaim = GetClaim(authenticateResult, JwtClaimTypes.SessionId);
			claims.Add(new Claim(sessionIdClaim.Type, sessionIdClaim.Value, sessionIdClaim.ValueType));

			return claims;
		}

		private static List<Claim> CreateClaims(AuthenticateResult authenticateResult)
		{
			var claims = new List<Claim>();

			var nameClaim = GetClaim(authenticateResult, JwtClaimTypes.Name);
			claims.Add(new Claim(JwtClaimTypes.PreferredUserName, nameClaim.Value, nameClaim.ValueType));

			var emailClaim = GetClaim(authenticateResult, JwtClaimTypes.Email);
			claims.Add(new Claim(emailClaim.Type, emailClaim.Value, emailClaim.ValueType));

			return claims;
		}

		private static Claim GetClaim(AuthenticateResult authenticateResult, string claimType)
		{
			return authenticateResult.Principal?.Claims.FirstOrDefault(claim => string.Equals(claim.Type, claimType, StringComparison.OrdinalIgnoreCase)) ?? throw new InvalidOperationException($"No {claimType.ToStringRepresentation()}-claim was found.");
		}

		public async Task<IActionResult> OnGet()
		{
			var authenticateResult = await this.HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);

			if(!authenticateResult.Succeeded)
				throw new InvalidOperationException($"Authentication error for authentication-scheme {IdentityConstants.ExternalScheme.ToStringRepresentation()}: {authenticateResult.Failure}");

			if(authenticateResult.Principal == null)
				throw new InvalidOperationException($"The principal of the authentication-result is null for authentication-scheme {IdentityConstants.ExternalScheme.ToStringRepresentation()}.");

			var returnUrl = this.Url.ResolveAndValidateReturnUrl(this._logger, authenticateResult.Properties?.Items[QueryStringKeys.ReturnUrl]);

			var scheme = authenticateResult.Properties?.Items[QueryStringKeys.Scheme];

			if(scheme == null)
				throw new InvalidOperationException($"The properties of the authentication-result does not have a {QueryStringKeys.Scheme.ToStringRepresentation()}-item.");

			if(!string.Equals(scheme, OpenIdConnectDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
				throw new InvalidOperationException($"The authentication-scheme {scheme.ToStringRepresentation()} is invalid.");

			this._logger.LogDebugIfEnabled($"Claims received for authentication-scheme {IdentityConstants.ExternalScheme.ToStringRepresentation()}: {string.Join(", ", authenticateResult.Principal.Claims.Select(claim => claim.Type + " = " + claim.Value))}");

			var subjectClaim = GetClaim(authenticateResult, JwtClaimTypes.Subject);
			var subject = subjectClaim.Value;

			var claims = CreateClaims(authenticateResult);
			var additionalClaims = CreateAdditionalClaims(authenticateResult, scheme);

			// If the external provider issued an id_token, we'll keep it for sign out.
			var authenticationProperties = new AuthenticationProperties();
			var idToken = authenticateResult.Properties?.GetTokenValue(OidcConstants.TokenTypes.IdentityToken);
			if(idToken != null)
				authenticationProperties.StoreTokens([new AuthenticationToken { Name = OidcConstants.TokenTypes.IdentityToken, Value = idToken }]);

			var user = await this._identity.SynchronizeUser(claims, scheme.ToLowerInvariant(), subject);

			await this._identity.SignIn(additionalClaims, authenticationProperties, user);

			await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			return this.Redirect(returnUrl);
		}

		#endregion
	}
}