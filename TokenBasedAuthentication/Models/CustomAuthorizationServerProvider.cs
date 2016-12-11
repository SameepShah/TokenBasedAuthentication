using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security.OAuth;

namespace TokenBasedAuthentication.Models
{
    public class CustomAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated(); //Validated the Client
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //Validate user and Generate Token
            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            //if (context.UserName == "Sameep" && context.Password == "sameep123")
            //{
            //    identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
            //    identity.AddClaim(new Claim("username", "admin"));
            //    identity.AddClaim(new Claim(ClaimTypes.Name, "Sameep Shah"));
            //    context.Validated(identity);
            //} 
            //else if (context.UserName == "ajay" && context.Password == "ajay123")
            //{
                identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
                identity.AddClaim(new Claim("username", context.UserName));
                //identity.AddClaim(new Claim(ClaimTypes.Name, "Ajay Patel"));
                context.Validated(identity);
            //}
            //else
            //{
            //    context.SetError("invalid_grant", "Username or Password Incorrect");
            //    return;
            //}
            //return base.GrantResourceOwnerCredentials(context);
        }

        
    }
}