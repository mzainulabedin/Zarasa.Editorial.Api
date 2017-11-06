using System.Linq;
using System.Collections.Generic;
using Zarasa.Editorial.Api.Common.Responses;
using Zarasa.Editorial.Api.Common.Validation;
using Zarasa.Editorial.Api.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zarasa.Editorial.Api.Data;
using Microsoft.EntityFrameworkCore;
using Zarasa.Editorial.Api.Common.EventArgs;
using System;
using Zarasa.Editorial.Api.Repository;

namespace Zarasa.Editorial.Api.Controllers
{
    public delegate void CreatingValidationEventHandler<T>(CreateValidationEventArgs1<T> e);
    public delegate void UpdatingValidationEventHandler<T>(UpdateValidationEventArgs<T> e);
    
    public abstract class EntityController<T> : BaseContoller where T : Entity
    {
        protected abstract EntityRepository<T> GetRepository();

        private readonly ApplicationDbContext _context;

        public EntityController(ApplicationDbContext context)
        {
            _context = context;
        } 

        protected ApplicationDbContext getContext() {
            return _context;
        }

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
             return new EntityResponseResult<T>(data, message, pageNumber, pageSize, totalRecords);
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

        public virtual IActionResult GetAll(int? page, int? size)
        {
            long count;
            var data =  GetRepository().Get(page, ref size, out count);
            return EntityListResponse(data, pageNumber: page==null?0:page.Value, pageSize:size==null?0:size.Value, totalRecords: count);
        }

        public virtual IActionResult GetById(long id)
        {
            var entity = GetRepository().Get(id);
            if (entity == null)
            {
                return NotFound();
            }
            return EntityResponse(entity);
        } 

        [HttpPost]
        public virtual IActionResult Create([FromBody] T newEntity)
        {
            IActionResult result = null;
            try{
                if (newEntity == null)
                {
                    result = BadRequest();
                }
                if (result==null && !ModelState.IsValid)
                {
                    result = ValidationFailed();
                }
                if (result==null)
                {
                    var validationEventArgs = new CreateValidationEventArgs1<T>(newEntity);
                    OnCreatingValidation(validationEventArgs);
                    result = validationEventArgs.ActionResult;
                }            
                if (result==null)
                {
                    var entity = GetRepository().Create(newEntity, long.Parse(GetCurrentUserId()));
                    result = EntityResponse(entity, "Record Created Successfully.");
                }
            } catch(Exception e){
                result = ErrorResponse(e.Message);
            }
            return result;
        }

        public virtual IActionResult Update(long id, [FromBody] T updatedEntity)
        {
            IActionResult result = null;
            try{
                if (updatedEntity == null)
                {
                    result =  BadRequest();
                }
                
                if (result == null && !ModelState.IsValid)
                {
                    return ValidationFailed();
                }
                if (result==null)
                {
                    var validationEventArgs = new UpdateValidationEventArgs<T>(updatedEntity, id);
                    OnUpdatingValidation(validationEventArgs);
                    result = validationEventArgs.ActionResult;
                }  
                if(result == null){
                    var entity = GetRepository().Update(id, updatedEntity, long.Parse(GetCurrentUserId()));

                    result = EntityResponse(entity, "Record Updated Successfully.");
                }
            } catch (Exception e){
                result = ErrorResponse(e.Message);
            }
            return result;
        }

        
        public virtual IActionResult Delete(long id)
        {
            IActionResult result = null;
            try{
                if(result == null) {
                    GetRepository().Delete(id);
                    result = EntityResponse(null, "Record Deleted Successfully.");
                }
            } catch (Exception e){
                result = ErrorResponse(e.Message);
            }
            return result;
        }

        public event CreatingValidationEventHandler<T> CreatingValidation;

        protected virtual void OnCreatingValidation(CreateValidationEventArgs1<T> e)
        {
            CreatingValidationEventHandler<T> handler = CreatingValidation;
            if (handler != null)
            {
                handler(e);
            }
        }

        public event UpdatingValidationEventHandler<T> UpdatingValidation;

        protected virtual void OnUpdatingValidation(UpdateValidationEventArgs<T> e)
        {
            UpdatingValidationEventHandler<T> handler = UpdatingValidation;
            if (handler != null)
            {
                handler(e);
            }
        }
    }
}