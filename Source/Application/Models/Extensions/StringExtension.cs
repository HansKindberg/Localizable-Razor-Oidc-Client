using System.Text.RegularExpressions;

namespace Application.Models.Extensions
{
	public static class StringExtension
	{
		#region Fields

		private const RegexOptions _likeRegexOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase;

		#endregion

		#region Methods

		public static bool Like(this string? value, string? pattern)
		{
			if(value == null)
				return pattern == null;

			if(pattern == null)
				return false;

			pattern = "^" + Regex.Escape(pattern).Replace("\\*", ".*") + "$";

			return Regex.IsMatch(value, pattern, _likeRegexOptions);
		}

		#endregion
	}
}