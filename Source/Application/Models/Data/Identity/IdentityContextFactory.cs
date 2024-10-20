using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Application.Models.Data.Identity
{
	public class IdentityContextFactory : IDesignTimeDbContextFactory<IdentityContext>
	{
		#region Methods

		public IdentityContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();

			optionsBuilder.UseSqlite("A value that can not be empty just to be able to create/update migrations.");

			return new IdentityContext(optionsBuilder.Options);
		}

		#endregion
	}
}