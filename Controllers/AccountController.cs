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
using Microsoft.EntityFrameworkCore;
using Zarasa.Editorial.Api.Repository;
using Zarasa.Editorial.Api.Helper;
// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Zarasa.Editorial.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : EntityController<User>
    {
        
        private readonly JWTSettings _options;
        private readonly UserRepository _repository;

        public AccountController(ApplicationDbContext context, IOptions<JWTSettings> optionsAccessor)
            : base(context) { 
                _options = optionsAccessor.Value; 
            _repository = new UserRepository(context);
        }

        // protected override DbSet<User> GetDbSet() => getContext().Users;
        protected override EntityRepository<User> GetRepository() => _repository;

        // [HttpGet]
        // public override IActionResult GetAll() => base.GetAll();

        // [HttpGet("{id}", Name = "Get")]
        // public override IActionResult GetById(long id) => base.GetById(id);

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] Credentials Credentials)
        {
            if (ModelState.IsValid)
            {
                var exists = _repository.CredentialExists(Credentials.username, Credentials.password);
                if (exists)
                {
                    var user = _repository.GetByEmail(Credentials.username);
                    var jwt = new JWTHelper(_options);
                    var response = new Dictionary<string, object>
                    {
                        // { "access_token", GetAccessToken(Credentials.email) },
                        { "access_token", jwt.GetIdToken(user) }
                    };
                    return ObjectResponse(response, "Sign In successfully");
                }
                return ErrorResponse("Incorrect username or password");
            }
            return ValidationFailed();
        }

        

        // private JsonResult Errors(IdentityResult result)
        // {
        //     var items = result.Errors
        //         .Select(x => x.Description)
        //         .ToArray();
        //     return new JsonResult(items) { StatusCode = 400 };
        // }

        // private JsonResult Error(string message)
        // {
        //     return new JsonResult(message) { StatusCode = 400 };
        // }

        
    }
}
