using System.Globalization;

namespace Application.Models.Globalization
{
	public class CultureContext(CultureInfo masterCulture, CultureInfo masterUiCulture) : ICultureContext
	{
		#region Properties

		public CultureInfo CurrentCulture => CultureInfo.CurrentCulture;
		public CultureInfo CurrentUiCulture => CultureInfo.CurrentUICulture;
		public CultureInfo MasterCulture { get; } = masterCulture ?? throw new ArgumentNullException(nameof(masterCulture));
		public CultureInfo MasterUiCulture { get; } = masterUiCulture ?? throw new ArgumentNullException(nameof(masterUiCulture));

		#endregion
	}
}