using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using wCyber.Helpers.Web;

namespace ContactCenter.Web.Auth
{
    public class AppStore : OpenIddictApplicationManager<OpenIddictEntityFrameworkCoreApplication>
    {
        public AppStore(IOpenIddictApplicationCache<OpenIddictEntityFrameworkCoreApplication> cache, ILogger<OpenIddictApplicationManager<OpenIddictEntityFrameworkCoreApplication>> logger, IOptionsMonitor<OpenIddictCoreOptions> options, IOpenIddictApplicationStoreResolver resolver) : base(cache, logger, options, resolver)
        {
        }

        public override async ValueTask<OpenIddictEntityFrameworkCoreApplication> FindByClientIdAsync(string identifier, CancellationToken cancellationToken = default)
        {
            var res = new OpenIddictEntityFrameworkCoreApplication
            {
                ClientId = identifier,
                ClientSecret = await ObfuscateClientSecretAsync("AQAAAAEAACcQAAAAEF+zC7ac3gOcoLIjGnpCZivlYODRBqVSOsBs9YuJYDwdtEaEa/aOJvfkRcVd3FkuVw==", cancellationToken),
                Id = identifier,
                RedirectUris = "[\"https://localhost:5011/signin-oidc\",\"https://127.0.0.1:5011/signin-oidc\"]",
                Permissions = new List<string>
    {
        OpenIddictConstants.Permissions.Endpoints.Authorization,
        OpenIddictConstants.Permissions.Endpoints.Device,
        OpenIddictConstants.Permissions.Endpoints.Token,
        OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
        OpenIddictConstants.Permissions.GrantTypes.DeviceCode,
        OpenIddictConstants.Permissions.GrantTypes.Password,
        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
        OpenIddictConstants.Permissions.Scopes.Email,
        OpenIddictConstants.Permissions.Scopes.Profile,
        OpenIddictConstants.Permissions.Scopes.Roles,

        OpenIddictConstants.Permissions.ResponseTypes.Code
    }.ToJsonString()
            };
            return res;
        }
        public override ValueTask<bool> ValidateRedirectUriAsync(OpenIddictEntityFrameworkCoreApplication application, string address, CancellationToken cancellationToken = default)
        {
            return new ValueTask<bool>(true);
        }

        public override ValueTask<bool> HasPermissionAsync(OpenIddictEntityFrameworkCoreApplication application, string permission, CancellationToken cancellationToken = default)
        {
            var res = base.HasPermissionAsync(application, permission, cancellationToken);
            return res;
        }

        public override ValueTask<bool> HasRequirementAsync(OpenIddictEntityFrameworkCoreApplication application, string requirement, CancellationToken cancellationToken = default)
        {
            var res = base.HasRequirementAsync(application, requirement, cancellationToken);
            return res;
        }

        public override ValueTask<bool> ValidateClientSecretAsync(OpenIddictEntityFrameworkCoreApplication application, string secret, CancellationToken cancellationToken = default)
        {
            var res = base.ValidateClientSecretAsync(application, secret, cancellationToken);
            return res;
        }
    }


}
