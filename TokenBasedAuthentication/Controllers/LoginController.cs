using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TokenBasedAuthentication.Models;

namespace TokenBasedAuthentication.Controllers
{
    [RoutePrefix("api/Login")]
    public class LoginController : ApiController
    {
        public const string UserName = "Sameep";
        public const string Password = "sameep123";

        [AllowAnonymous]
        [HttpGet]
        [Route("ForAll")]
        public IHttpActionResult ForAll()
        {
            return Ok("Login Screen - Now Server Time is :" + DateTime.Now.ToString());
        }


        [Authorize(Message="Unauthorized!")]
        [HttpGet]
        [Route("ForAuthorized")]
        public IHttpActionResult ForAuthorized()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return Ok("Hello " + identity.Name);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("ForAdmin")]
        public IHttpActionResult ForAdmin()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims.Where(x => x.Type == ClaimTypes.Role).Select(y => y.Value);
            return Ok("Hello " + identity.Name + " Role: " + string.Join(",", roles.ToList()));
        }

        [HttpPost]
        [Route("ValidateUser")]
        public IHttpActionResult ValidateUser([FromBody]JObject param)
        {
            bool isValid = false;
            string Token = string.Empty;
            dynamic userObj = JObject.Parse(param.ToString());
            UserLogin objUserLogin = userObj.ToObject<UserLogin>();
            if (objUserLogin.UserName == UserName && objUserLogin.Password == Password)
            {
                isValid = true;
                Token = GetToken(objUserLogin.UserName, objUserLogin.Password);
            }
            return Ok(new { isValid = isValid, token = Token });
        }

        public static string GetToken(string UserName, string Password)
        {
            //Generate Owin OAuth Token
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            string baseUrl = "http://localhost:2853/token";
            HttpResponseMessage response = httpClient.PostAsync(baseUrl, new StringContent("grant_type=password&username=" + UserName + "&password=" + Password)).Result;
            string content = response.Content.ReadAsStringAsync().Result;
            UserResponse userResult = JsonConvert.DeserializeObject<UserResponse>(content);
            return userResult.access_token;
        }

    }
}
