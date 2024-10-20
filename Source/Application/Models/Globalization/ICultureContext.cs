using System.Globalization;

namespace Application.Models.Globalization
{
	public interface ICultureContext
	{
		#region Properties

		CultureInfo CurrentCulture { get; }
		CultureInfo CurrentUiCulture { get; }
		CultureInfo MasterCulture { get; }
		CultureInfo MasterUiCulture { get; }

		#endregion
	}
}