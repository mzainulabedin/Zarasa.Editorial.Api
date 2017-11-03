using System.Linq;
using System.Collections.Generic;
using Zarasa.Editorial.Api.Common.Responses;
using Zarasa.Editorial.Api.Common.Validation;
using Zarasa.Editorial.Api.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Zarasa.Editorial.Api.Controllers
{
    public abstract class EntityController<T> : BaseContoller where T : Entity
    {

        protected ValidationFailedResult ValidationFailed(){
            return new ValidationFailedResult(ModelState);
        }

        protected ValidationFailedResult ValidationFailed(string key, string errorMessage){
            ModelState.AddModelError(key, errorMessage);
            return new ValidationFailedResult(ModelState);
        }

        protected EntityResponseResult<T> EntityResponse(T data, string message = "Record fetched successfully."){
             return new EntityResponseResult<T>(data, message);
        }

        protected EntityResponseResult<T> EntityListResponse(IEnumerable<T> data, string message = "Record fetched successfully.", int pageNumber = 0, int pageSize = 0, long totalRecords = 0){
             return new EntityResponseResult<T>(data, message);
        }

        protected ErrorResponseResult ErrorResponse(string errorMessage, int statusCode = StatusCodes.Status400BadRequest){
            return new ErrorResponseResult(errorMessage, statusCode);
        }

        protected ObjectResponseResult ObjectResponse(object data, string message){
            return new ObjectResponseResult(data, message);
        }

        protected string GetCurrentUserId()
        {
            string id = null;
            if(HttpContext != null && HttpContext.User != null && HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated){
                id = HttpContext.User.Claims.ToList().Find(x => x.Type == "id").Value;
            }
            return id;
        }
    }
}