using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ContactCenter.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ContactCenter.Data;
using Microsoft.EntityFrameworkCore;
using wCyber.Helpers.Identity.Data;
using System.Net;

namespace ContactCenter.Web
{
    public static class AuthExtensions
    {
        public static bool IsAdmin(this ClaimsPrincipal user) => user.IsInRole(nameof(UserRole.ADMIN));
        public static bool IsAgent(this ClaimsPrincipal user) => user.IsInRole(nameof(UserRole.AGENT));

        public static string GetActivationLink(this User user, HttpRequest Request)
            => Request.Scheme + "://" + Request.Host + "/Identity/Account/Activate?code=" + WebUtility.UrlEncode(Convert.ToBase64String(user.Id.ToByteArray()));

        public static async Task InitUser(this CookieSigningInContext context)
        {
            var userId = Guid.Parse(context.Principal.FindFirst(ClaimTypes.NameIdentifier).Value);
            var db = context.HttpContext.RequestServices.GetService<EDRSMContext>();
            var user = await db.Users
                .Include(c => c.Agent)
                .FirstOrDefaultAsync(c => c.Id == userId);
            var claims = new List<Claim>();

            if (user.IsAgent)
            {
                claims.Add(new Claim(ClaimsHelper.AgentStatusClaim, user.Agent?.StatusId.ToString()));
            }
            if (claims.Any()) context.Principal.AddIdentity(new ClaimsIdentity(claims));
        }

        public static AgentStatus AgentStatus(this ClaimsPrincipal user)
           => (AgentStatus)int.Parse(user.FindFirstValue(ClaimsHelper.AgentStatusClaim) ?? "0");

        public static bool IsOnline(this ClaimsPrincipal user) => user.AgentStatus() == Lib.AgentStatus.ONLINE;

    }
}
