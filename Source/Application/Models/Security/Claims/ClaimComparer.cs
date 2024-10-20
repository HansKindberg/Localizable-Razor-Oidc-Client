using System.Security.Claims;

namespace Application.Models.Security.Claims
{
	public class ClaimComparer : IComparer<Claim>
	{
		#region Properties

		public static ClaimComparer Instance { get; } = new();

		#endregion

		#region Methods

		public int Compare(Claim? x, Claim? y)
		{
			if(x == null)
				return y == null ? 0 : -1;

			return y == null ? 1 : string.Compare(x.Type, y.Type, StringComparison.OrdinalIgnoreCase);
		}

		#endregion
	}
}