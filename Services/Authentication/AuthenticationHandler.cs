using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using System.Security.Claims;
using System.Diagnostics;

namespace VortexCore.Services.Authentication
{
    public static class VortexClaims
    {
        public static IEnumerable<Claim> GetClaims(UserRecord user)
        {
            var claims = new List<Claim>()
            {
                VortexClaim(VortexClaimTypes.Name, user),
                VortexClaim(VortexClaimTypes.Email, user),
                VortexClaim(VortexClaimTypes.EmailVerified, user),
                VortexClaim(VortexClaimTypes.PhoneNumber, user),
                VortexClaim(VortexClaimTypes.PhotoUrl, user),
                VortexClaim(VortexClaimTypes.Uid, user),
                VortexClaim(VortexClaimTypes.Country, user),
                VortexClaim(VortexClaimTypes.Locality, user),
                VortexClaim(VortexClaimTypes.Role, user)
            };
            

            return claims;
        }

        public static Claim VortexClaim(VortexClaimTypes.VortexClaimTypeRepresent claimType, UserRecord user)
        {
            Claim claim = null;
            if (claimType.Custom)
            {
                if (user.CustomClaims.GetValueOrDefault(claimType.Type) != null)
                {
                    claim = new Claim(claimType.AssociatedType, user.CustomClaims[claimType.Type].ToString());
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (user.GetType().GetProperty(claimType.Type).GetValue(user) != null)
                {
                    claim = new Claim(claimType.AssociatedType, user.GetType().GetProperty(claimType.Type).GetValue(user).ToString());
                }
                else
                {
                    return null;
                }
            }
            return claim;
            
        }

        public class VortexClaimTypes
        {
            public class VortexClaimTypeRepresent
            {
                public string Type { get; set; }
                public string AssociatedType { get; set; }
                public bool Custom { get; set; } = false;
            }

            public static readonly VortexClaimTypeRepresent Name = new VortexClaimTypeRepresent { Type = "DisplayName", AssociatedType = ClaimTypes.Name };
            public static readonly VortexClaimTypeRepresent Email = new VortexClaimTypeRepresent { Type = "Email", AssociatedType = ClaimTypes.Email };
            public static readonly VortexClaimTypeRepresent EmailVerified = new VortexClaimTypeRepresent { Type = "EmailVerified", AssociatedType = "EmailVerified" };
            public static readonly VortexClaimTypeRepresent PhotoUrl = new VortexClaimTypeRepresent { Type = "PhotoUrl", AssociatedType = "PhotoUrl" };
            public static readonly VortexClaimTypeRepresent PhoneNumber = new VortexClaimTypeRepresent { Type = "PhoneNumber", AssociatedType = ClaimTypes.MobilePhone };
            public static readonly VortexClaimTypeRepresent Uid = new VortexClaimTypeRepresent { Type = "Uid", AssociatedType = "Uid" };
            public static readonly VortexClaimTypeRepresent Role = new VortexClaimTypeRepresent { Type = "Role", AssociatedType = ClaimTypes.Role, Custom = true };
            public static readonly VortexClaimTypeRepresent Locality = new VortexClaimTypeRepresent { Type = "Locality", AssociatedType = ClaimTypes.Locality, Custom = true };
            public static readonly VortexClaimTypeRepresent Country = new VortexClaimTypeRepresent { Type = "Country", AssociatedType = ClaimTypes.Country, Custom = true };
        }
    }
    

    public class FirebaseAuthenticationOptions : AuthenticationSchemeOptions
    {
        public static string Scheme = "FirebaseAuthScheme";
        public FirebaseAuthenticationOptions() : base()
        {
        }
    }

    internal class FirebaseAuthenticationHandler : AuthenticationHandler<FirebaseAuthenticationOptions>
    {
        public FirebaseAuthenticationHandler(IOptionsMonitor<FirebaseAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
            
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Stopwatch sw = new Stopwatch();


            sw.Start();
            string tokenKey = Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(tokenKey))
            {
                tokenKey = tokenKey.Replace("Bearer ", null);
            }
            else
            {
                Request.Cookies.TryGetValue("access_token", out tokenKey);
                if (tokenKey == null)
                {
                    return AuthenticateResult.Fail("No token");
                }
            }

            sw.Stop();
            sw.Reset();

            sw.Start();

            var fireBaseUser = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(tokenKey);
            if (fireBaseUser == null) return AuthenticateResult.Fail("Token incorrect");

            sw.Stop();
            sw.Reset();

            try
            {
                sw.Start();
                var user = await FirebaseAuth.DefaultInstance.GetUserAsync(fireBaseUser.Uid);
                sw.Stop();
                sw.Reset();

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(VortexClaims.GetClaims(user), FirebaseAuthenticationOptions.Scheme));
                var ticket = new AuthenticationTicket(claimsPrincipal,
                    new AuthenticationProperties { IsPersistent = false }, FirebaseAuthenticationOptions.Scheme
                );

                if (ticket != null) return AuthenticateResult.Success(ticket);
                return AuthenticateResult.Fail("User empty");
            } catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex);
            }
            



            //var token = context.HttpContext.Request.Cookies["access_token"];
            //if (token == null) context.Fail("No token");
            //var fireBaseUser = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
            //if (fireBaseUser == null) context.Fail("Token incorrect");
            //var user = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.GetUserAsync(fireBaseUser.Uid);
            //var claims = user.CustomClaims.Select(x => new Claim(x.Key, x.Value.ToString())).ToList();
            ////claims.Add(new Claim(ClaimTypes.Name, user.DisplayName));
            //claims.Add(new Claim(ClaimTypes.Email, user.Email));
            ////var indentityBuilder = new 
            ////IdentityUser identity = new IdentityUser
            ////{
            ////    UserName = user.DisplayName,
            ////    Email = user.Email,
            ////    EmailConfirmed = user.EmailVerified,
            ////    Id = user.Uid,
            ////    PhoneNumber = user.PhoneNumber,
            ////};
            //context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            //context.Success();
        }
    }


}
