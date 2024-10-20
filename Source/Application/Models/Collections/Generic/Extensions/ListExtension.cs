namespace Application.Models.Collections.Generic.Extensions
{
	public static class ListExtension
	{
		#region Methods

		public static void Sort<T>(this IList<T> list, Comparison<T> comparison)
		{
			list.Sort(concreteList => { concreteList.Sort(comparison); });
		}

		public static void Sort<T>(this IList<T> list, IComparer<T> comparer)
		{
			list.Sort(concreteList => { concreteList.Sort(comparer); });
		}

		private static void Sort<T>(this IList<T> list, Action<List<T>> action)
		{
			ArgumentNullException.ThrowIfNull(list);

			var emptyAndFill = false;

			if(list is not List<T> concreteList)
			{
				concreteList = [.. list];
				emptyAndFill = true;
			}

			action(concreteList);

			if(!emptyAndFill)
				return;

			list.Clear();

			foreach(var item in concreteList)
			{
				list.Add(item);
			}
		}

		#endregion
	}
}