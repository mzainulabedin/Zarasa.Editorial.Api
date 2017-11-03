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

namespace Zarasa.Editorial.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserController : EntityController<User>
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;

            // if (_context.Users.Count() == 0)
            // {
            //     _context.Users.Add(new User { first_name = "Ishtiyaq", last_name = "Ahmed", email = "sahmed@test.com" });
            //     _context.SaveChanges();
            // }
        }  
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _context.Users.Where(x => !x.is_deleted).ToList();
            return EntityListResponse(users);
        }

        [Authorize]
        [HttpGet("{id}", Name = "Get")]
        public IActionResult GetById(long id)
        {
            var user = _context.Users.FirstOrDefault(t => t.id == id);
            if (user == null)
            {
                return NotFound();
            }
            return EntityResponse(user);
        } 

        [HttpPost]
        public IActionResult Create([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return ValidationFailed();
            }
            if(_context.Users.Any(x => x.email == user.email))
            {
                return ValidationFailed("email", "Email can not be duplicate");
            }

            _context.Users.Add(user);
            _context.SaveChanges();

            return EntityResponse(user, "Record Created Successfully.");
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            var existingUser = _context.Users.FirstOrDefault(t => t.id == id);
            if (existingUser == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return ValidationFailed();
            }
            if(_context.Users.Any(x => x.email == user.email && x.id != id))
            {
                return ValidationFailed("email", "Email can not be duplicate");
            }
            existingUser.first_name=user.first_name;
            existingUser.last_name=user.last_name;
            existingUser.email=user.email;
            existingUser.is_active = user.is_active;
            existingUser.updated_at = DateTime.UtcNow;
            _context.Users.Update(existingUser);
            _context.SaveChanges();

            return EntityResponse(existingUser, "Record Updated Successfully.");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var user = _context.Users.FirstOrDefault(t => t.id == id);
            if (user == null)
            {
                return NotFound();
            }

            user.is_deleted = true;
            user.updated_at = DateTime.UtcNow;
            _context.Users.Update(user);
            _context.SaveChanges();

            return EntityResponse(null, "Record Deleted Successfully.");;
        }

    }


}