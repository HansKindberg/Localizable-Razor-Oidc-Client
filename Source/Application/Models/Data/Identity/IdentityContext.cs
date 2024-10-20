using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Data.Identity
{
	public class IdentityContext(DbContextOptions<IdentityContext> options) : IdentityDbContext<User, Role, string>(options)
	{
		#region Methods

		protected override void OnModelCreating(ModelBuilder builder)
		{
			ArgumentNullException.ThrowIfNull(builder);

			base.OnModelCreating(builder);

			foreach(var entityType in builder.Model.GetEntityTypes())
			{
				foreach(var property in entityType.GetProperties())
				{
					if(property.ClrType == typeof(string))
						property.SetCollation("NOCASE");
				}
			}
		}

		#endregion
	}
}