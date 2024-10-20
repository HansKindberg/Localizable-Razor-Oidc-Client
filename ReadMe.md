# Localizable-Razor-Oidc-Client

## 1 IdP

- [demo.duendesoftware.com](https://demo.duendesoftware.com)
- Client id: **interactive.confidential**

## 2 Localization

Even if https://demo.duendesoftware.com does not implement localization we can see the passed ui_locales paremeter in the url to it:

- https://demo.duendesoftware.com/Account/Login?ReturnUrl=...%26ui_locales%3Dsv%26...

Packages:

- https://www.nuget.org/packages/RegionOrebroLan.Localization
- https://github.com/RegionOrebroLan/.NET-Localization-Extensions

We use a culture-selector in the UI. The culture-selector only renders UI-cultures.

We use

- [Application.Models.Web.Localization.Routing.RouteDataRequestCultureProvider](/Source/Application/Models/Web/Localization/Routing/RouteDataRequestCultureProvider.cs)

as primary [Microsoft.AspNetCore.Localization.IRequestCultureProvider](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.localization.irequestcultureprovider) instead of [Microsoft.AspNetCore.Localization.Routing.RouteDataRequestCultureProvider](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.localization.routing.routedatarequestcultureprovider).

Some of the the localization/translation files, [Texts.de.xml](/Source/Application/Resources/Localization/Texts.de.xml), [Texts.fi.xml](/Source/Application/Resources/Localization/Texts.fi.xml) and [Texts.fr.xml](/Source/Application/Resources/Localization/Texts.fr.xml), are translated by https://chatgpt.com/:

*Can you please translate this xml-file to Finnish:*

	<cultures>
		<culture name="en">
			<hello>Hello</hello>
		</culture>
	</cultures>

Answer: *Here is the XML file translated to Finnish:*

	<cultures>
		<culture name="fi">
			<hello>Hei</hello>
		</culture>
	</cultures>

## 3 Development

### 3.1 Migrations

- [Migrations with Multiple Providers](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/providers)
- [Design-time DbContext Creation](https://learn.microsoft.com/en-us/ef/core/cli/dbcontext-creation)
- [IDesignTimeDbContextFactory&lt;TContext&gt; Interface](https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.design.idesigntimedbcontextfactory-1)
- [Entity Framework Core tools reference - Package Manager Console in Visual Studio](https://learn.microsoft.com/en-us/ef/core/cli/powershell)

We use [IDesignTimeDbContextFactory&lt;TContext&gt; Interface](https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.design.idesigntimedbcontextfactory-1) for our migration creation.

To avoid analysis errors on our generated migrations we have the following in [.editorconfig](/.editorconfig):

	[**Application/Models/Data/**/Migrations/*.cs]
	dotnet_analyzer_diagnostic.severity = none

We might want to create/recreate migrations. If we can accept data-loss we can recreate the migrations otherwhise we will have to update them.

The NuGet-package "Microsoft.EntityFrameworkCore.Tools" is required for the commands below.

Copy all the commands below and run them in the Package Manager Console for the affected database-context.

If you want more migration-information you can add the -Verbose parameter:

	Add-Migration TheMigration -Context TheDatabaseContext -OutputDir Data/Migrations -Project Application -Verbose;

**Important!**: Before running the commands below you need to ensure that the "Application"-project is set as startup-project.

When you run the migration commands below you may get logs in the Package Manager Console. You can ignore the following one:

	Host terminated unexpectedly.
	Microsoft.Extensions.Hosting.HostAbortedException: The host was aborted.
	   at Microsoft.Extensions.Hosting.HostFactoryResolver.HostingListener.ThrowHostAborted()
	   at Microsoft.Extensions.Hosting.HostFactoryResolver.HostingListener.OnNext(KeyValuePair`2 value)
	   at System.Diagnostics.DiagnosticListener.Write(String name, Object value)
	   at Microsoft.Extensions.Hosting.HostBuilder.ResolveHost(IServiceProvider serviceProvider, DiagnosticListener diagnosticListener)
	   at Microsoft.Extensions.Hosting.HostApplicationBuilder.Build()
	   at Microsoft.AspNetCore.Builder.WebApplicationBuilder.Build()
	   at Program.<Main>$(String[] args) in {PATH}\Source\Application\Program.cs:line nn

The important lines in the Package Manager Console for a successful migration creation is:

	Build started...
	Build succeeded.
	...

	To undo this action, use Remove-Migration.

We use suffixed migration-names to make it easier to know what the rows in the *__EFMigrationsHistory*-table relates to. Every time we run a migration update, we need to increment the suffix.

So if the last update was this (just an example):

	Add-Migration TheMigration2 -Context TheDatabaseContext -OutputDir Data/Migrations -Project Application;

The new update should be this:

	Add-Migration TheMigration3 -Context TheDatabaseContext -OutputDir Data/Migrations -Project Application;

#### 3.1.1 Identity

##### 3.1.1.1 Create migrations

	Write-Host "Removing migrations...";
	Remove-Migration -Context IdentityContext -Force -Project Application;
	Write-Host "Removing current migration-directories...";
	Remove-Item "Source/Application/Models/Data/Identity/Migrations" -ErrorAction Ignore -Force -Recurse;
	Write-Host "Creating migrations...";
	Add-Migration Identity0 -Context IdentityContext -OutputDir Models/Data/Identity/Migrations -Project Application;
	Write-Host "Finnished";

##### 3.1.1.2 Update migrations

	Write-Host "Updating migrations...";
	Add-Migration Identity1 -Context IdentityContext -OutputDir Models/Data/Identity/Migrations -Project Application;
	Write-Host "Finnished";

### 3.2 Sqlite

If you want to look in the database use **DB Browser for SQLite**. Download:

- https://sqlitebrowser.org/dl/