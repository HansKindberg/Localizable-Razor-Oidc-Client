using System.IdentityModel.Tokens.Jwt;
using Application.Models.DependencyInjection.Extensions;
using Application.Models.Web.Builder.Extensions;

var builder = WebApplication.CreateBuilder(args);

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.Add(builder.Configuration, builder.Environment);

var application = builder.Build();

application.Use(application.Environment);

application.MapRazorPages()
	.RequireAuthorization();

application.Run();