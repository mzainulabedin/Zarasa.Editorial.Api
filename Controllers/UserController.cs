using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Zarasa.Editorial.Api.Models;
using Zarasa.Editorial.Api.Data;

using Zarasa.Editorial.Api.Common.Responses;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Zarasa.Editorial.Api.Common.EventArgs;
using Zarasa.Editorial.Api.Repository;

namespace Zarasa.Editorial.Api.Controllers
{
    
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : EntityController<User>
    {
        private readonly UserRepository _repository;
        public UserController(ApplicationDbContext context) : base(context)
        {
            _repository = new UserRepository(context);
        }

        // protected override DbSet<User> GetDbSet() => getContext().Users;

        protected override EntityRepository<User> GetRepository() => _repository;

        [HttpGet]
        public override IActionResult GetAll() => base.GetAll();

        [HttpGet("{id}", Name = "Get")]
        public override IActionResult GetById(long id) => base.GetById(id);

        [HttpPost]
        public override IActionResult Create([FromBody] User entity) => base.Create(entity);
        

        protected override void OnCreatingValidation(CreateValidationEventArgs1<User> e){
            if(_repository.EmailExists(e.Model.email))
            {
                e.ActionResult = ValidationFailed("email", "Email can not be duplicate");
            }
        }

        [HttpPut("{id}")]
        public override IActionResult Update(long id, [FromBody] User updatedEntity) => base.Update(id, updatedEntity);
        
        
        protected override void OnUpdatingValidation(UpdateValidationEventArgs<Models.User> e){
            if(_repository.EmailExists(e.Model.email, e.Id))
            {
                e.ActionResult =  ValidationFailed("email", "Email can not be duplicate");
            }
        }

        [HttpDelete("{id}")]
        public override IActionResult Delete(long id) => base.Delete(id);

    }


}