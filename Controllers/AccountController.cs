using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Zarasa.Editorial.Api.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections;
using JWT;
using JWT.Serializers;
using JWT.Algorithms;
using Microsoft.Extensions.Options;
using System;
using Zarasa.Editorial.Api.Data;
using Zarasa.Editorial.Api.Common.Responses;
// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Zarasa.Editorial.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : EntityController<User>
    {
        private readonly ApplicationDbContext _context;
        // private readonly UserManager<User> _userManager;
        // private readonly SignInManager<User> _signInManager;
        private readonly JWTSettings _options;

        public AccountController(
            ApplicationDbContext context,
        //   UserManager<User> userManager,
        //   SignInManager<User> signInManager,
          IOptions<JWTSettings> optionsAccessor)
        {
            _context = context;
            // _userManager = userManager;
            // _signInManager = signInManager;
            _options = optionsAccessor.Value;
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] Credentials Credentials)
        {
            if (ModelState.IsValid)
            {
                var exists = _context.Users.Any(x => !x.is_deleted && x.email == Credentials.username && x.password == Credentials.password);
                if (exists)
                {
                    var user = _context.Users.Where(x => !x.is_deleted && x.email == Credentials.username).FirstOrDefault();
                    var response = new Dictionary<string, object>
                    {
                        // { "access_token", GetAccessToken(Credentials.email) },
                        { "access_token", GetIdToken(user) }
                    };
                    return ObjectResponse(response, "Sign In successfully");
                }
                return ErrorResponse("Incorrect username or password");
            }
            return ValidationFailed();
        }

        private string GetIdToken(User user)
        {
            var payload = new Dictionary<string, object>
            {
                { "id", user.id },
                // { "sub", user.email },
                { "email", user.email },
                // { "emailConfirmed", user.EmailConfirmed },
            };
            return GetToken(payload);
        }

        private string GetAccessToken(string Email)
        {
            var payload = new Dictionary<string, object>
            {
                // { "sub", Email },
                { "email", Email }
            };
            return GetToken(payload);
        }

        private string GetToken(Dictionary<string, object> payload)
        {
            var secret = _options.SecretKey;

            payload.Add("iss", _options.Issuer);
            payload.Add("aud", _options.Audience);
            payload.Add("nbf", ConvertToUnixTimestamp(DateTime.Now));
            payload.Add("iat", ConvertToUnixTimestamp(DateTime.Now));
            payload.Add("exp", ConvertToUnixTimestamp(DateTime.Now.AddDays(7)));

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder.Encode(payload, secret);
        }

        private JsonResult Errors(IdentityResult result)
        {
            var items = result.Errors
                .Select(x => x.Description)
                .ToArray();
            return new JsonResult(items) { StatusCode = 400 };
        }

        private JsonResult Error(string message)
        {
            return new JsonResult(message) { StatusCode = 400 };
        }

        private static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}
