namespace UnitTests.Helpers
{
	public static class LocalizationHelper
	{
		#region Fields

		public static readonly string[] Cultures = ["sv-SE", "de-DE", "en-001", "fi-FI", "fr-FR"];
		public static readonly string DefaultCurrentCulture = Cultures.First();
		public static readonly string[] UiCultures = Cultures.Select(culture => culture.Split('-').First()).ToArray();
		public static readonly string DefaultCurrentUiCulture = UiCultures.First();
		public static readonly string MasterCulture = Cultures.First();
		public static readonly string MasterUiCulture = UiCultures.First();

		#endregion
	}
}