using System.Collections.Concurrent;
using System.Reflection;
using Application.Models.Web.Navigation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Models.Web.Mvc.Extensions
{
	public static class ViewContextExtension
	{
		#region Fields

		private static readonly ConcurrentDictionary<Type, bool> _signInMap = new();
		private static readonly ConcurrentDictionary<Type, bool> _signOutMap = new();

		#endregion

		#region Methods

		private static bool Available<TAttribute>(this ViewContext viewContext, ConcurrentDictionary<Type, bool> map) where TAttribute : Attribute
		{
			ArgumentNullException.ThrowIfNull(viewContext);
			ArgumentNullException.ThrowIfNull(map);

			var model = viewContext.ViewData.Model;

			if(model == null)
				return true;

			var modelType = model.GetType();

			return map.GetOrAdd(modelType, type => GetAttribute<TAttribute>(type) == null);
		}

		private static TAttribute? GetAttribute<TAttribute>(Type type) where TAttribute : Attribute
		{
			return type.GetCustomAttribute<TAttribute>();
		}

		public static bool SignInAvailable(this ViewContext viewContext)
		{
			return viewContext.Available<SignInUnavailableAttribute>(_signInMap);
		}

		public static bool SignOutAvailable(this ViewContext viewContext)
		{
			return viewContext.Available<SignOutUnavailableAttribute>(_signOutMap);
		}

		#endregion
	}
}