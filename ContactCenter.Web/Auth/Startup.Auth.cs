using ContactCenter.Data;
using ContactCenter.Web;
using ContactCenter.Lib;
using Microsoft.AspNetCore.Identity;
using wCyber.Helpers.Identity.Auth;
using Microsoft.EntityFrameworkCore;
using static OpenIddict.Abstractions.OpenIddictConstants;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;

namespace ContactCenter.Web
{
	internal static class Startup
	{
		public static WebApplicationBuilder ConfigureAuth(this WebApplicationBuilder builder)
		{
			builder.Services.Configure<UserRoleTypeOptions>(o =>
			{
				o.RoleType = typeof(UserRole);
			});

			builder.Services.AddDefaultIdentity<User>()
				.AddUserStore<SysUserStore<User, EDRSMContext>>()
				.AddClaimsPrincipalFactory<SysUserStore<User, EDRSMContext>>()
				.AddDefaultTokenProviders();
			builder.Services.ConfigureApplicationCookie(o =>
			{
				o.ExpireTimeSpan = TimeSpan.FromDays(15);
				//o.LoginPath = new PathString("/Login");
				//o.AccessDeniedPath = new PathString("/AccessDenied");
				o.SlidingExpiration = true;
				o.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
				{
					OnSigningIn = async context => await context.InitUser()
				};
			});

			var SslCertPath = Path.Combine(builder.Environment.ContentRootPath, "SSL", "ccenter.ks");
			var SSlCertPwd = builder.Configuration["IdentitySSLPwd"];


			builder.Services.Configure<IdentityOptions>(options =>
			{
				options.ClaimsIdentity.UserNameClaimType = Claims.Name;
				options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
				options.ClaimsIdentity.RoleClaimType = Claims.Role;
			});

			builder.Services.AddDbContext<Auth.ApplicationStoreContext>(options => options.UseInMemoryDatabase( databaseName:"Db"));
			builder.Services.AddOpenIddict()
			// Register the OpenIddict core components.
			.AddCore(o =>
			{
				o.UseEntityFrameworkCore().UseDbContext<Auth.ApplicationStoreContext>();
				o.ReplaceApplicationManager<Auth.AppStore>();
			})
			// Register the OpenIddict server components.
			.AddServer(options =>
			{
				options.UseDataProtection()
				   //.PreferDefaultAccessTokenFormat()
				   .PreferDefaultAuthorizationCodeFormat()
				   .PreferDefaultDeviceCodeFormat()
				   .PreferDefaultRefreshTokenFormat()
				   .PreferDefaultUserCodeFormat();
				options.DisableAuthorizationStorage();
				options.DisableTokenStorage();
				options.SetAccessTokenLifetime(TimeSpan.FromDays(1));
				options.SetRefreshTokenLifetime(TimeSpan.FromDays(3));
				options.DisableSlidingRefreshTokenExpiration();
				// Enable the authorization, logout, token and userinfo endpoints.
				options

					   .SetTokenEndpointUris("/identity/token")
					   .SetAuthorizationEndpointUris("/identity/authorize")
					   .SetUserinfoEndpointUris("/identity/userinfo");

				// Mark the "email", "profile" and "roles" scopes as supported scopes.
				options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);
				options.AcceptAnonymousClients();
				// Note: the sample uses the code and refresh token flows but you can enable
				// the other flows if you need to support implicit, password or client credentials.
				options.AllowPasswordFlow();
				options.AllowRefreshTokenFlow();
				options.AllowAuthorizationCodeFlow();
				// Register the signing and encryption credentials.
				//options.AddDevelopmentEncryptionCertificate()
				//       .AddDevelopmentSigningCertificate();
				var ssl = new X509Certificate2(SslCertPath, SSlCertPwd, X509KeyStorageFlags.MachineKeySet);
				options.AddEncryptionCertificate(File.OpenRead(SslCertPath), SSlCertPwd, X509KeyStorageFlags.MachineKeySet);
				options.AddSigningKey(new X509SecurityKey(ssl));
				// Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
				options.UseAspNetCore()
					   .EnableTokenEndpointPassthrough()
					   .EnableAuthorizationEndpointPassthrough();
			})

			// Register the OpenIddict validation components.
			.AddValidation(options =>
			{
				// Import the configuration from the local OpenIddict server instance.
				options.UseLocalServer();
				options.UseDataProtection();
				// Register the ASP.NET Core host.
				options.UseAspNetCore();
			});
			
			return builder;
		}
	}
}
