using ContactCenter.Web.Pages;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace ContactCenter.Web.Areas.Identity.Pages.Authorize
{
    [Authorize]
    public class IndexModel : SysPageModel
    {
        [Display(Name = "Application")]
        public string ApplicationName { get; set; }
        [BindNever]
        public IEnumerable<KeyValuePair<string, OpenIddictParameter>> Parameters { get; set; }
        [Display(Name = "Scope")]
        public string Scope { get; set; }

        [BindProperty]
        public bool StaySignedIn { get; set; }

        public IActionResult OnGet()
        {
            var request = HttpContext.GetOpenIddictServerRequest();
            Parameters = request.GetParameters();
            string referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrWhiteSpace(referer) && Request.Cookies[nameof(StaySignedIn)] != bool.TrueString)
            {
                var refererUri = new Uri(referer);
                if (refererUri.AbsolutePath != "/Login")
                {
                    var query = new Dictionary<string, string>();
                    foreach (var item in Parameters) query.Add(item.Key, item.Value.Value.ToString());
                    //return LocalRedirect(QueryHelpers.AddQueryString("~/Authorize/Continue", query));
                }
            }
            if (Guid.TryParse(request.ClientId, out Guid clientId))
            {
                return OnPost();
            }
            return RedirectToPage("/AccessDenied");
        }

        public IActionResult OnPost()
        {
            var request = HttpContext.GetOpenIddictServerRequest();
            Parameters = request.GetParameters();
            
            var identity = new ClaimsIdentity(OpenIddict.Server.AspNetCore.OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, Claims.Name, Claims.Role);

            identity.AddClaim(new Claim(Claims.Subject, CurrentUser.Id.ToString())
                   .SetDestinations(Destinations.AccessToken, Destinations.IdentityToken));

            identity.AddClaim(new Claim(Claims.Name, CurrentUser.Name)
                    .SetDestinations(Destinations.AccessToken, Destinations.IdentityToken));

            identity.AddClaim(new Claim(Claims.Email, CurrentUser.Email)
                    .SetDestinations(Destinations.AccessToken, Destinations.IdentityToken));

            identity.AddClaim(new Claim(Claims.Role, CurrentUser.RoleId.ToString())
                    .SetDestinations(Destinations.AccessToken, Destinations.IdentityToken));

            if (StaySignedIn) Response.Cookies.Append(nameof(StaySignedIn), bool.TrueString);

            var principal=new ClaimsPrincipal(identity);
            principal.SetScopes(Scopes.OpenId, Scopes.Email, Scopes.Profile, Scopes.OfflineAccess);
            return SignIn(principal, new AuthenticationProperties(), OpenIddict.Server.AspNetCore.OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
    }
}
