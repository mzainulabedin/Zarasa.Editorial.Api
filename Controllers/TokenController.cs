using Microsoft.AspNetCore.Mvc;

namespace Zarasa.Editorial.Api.Controllers
{
    public class TokenController : BaseContoller
    {
        [Route("/token")]
        [HttpPost]        
        public IActionResult Create(string username, string password)
        {
            if (IsValidUserAndPasswordCombination(username, password))
                return new ObjectResult(GenerateToken(username));
            return BadRequest();
        }

        private bool IsValidUserAndPasswordCombination(string username, string password){
            return true;
        }

        private string GenerateToken(string username){
            return "TOKEN";
        }
    }
}