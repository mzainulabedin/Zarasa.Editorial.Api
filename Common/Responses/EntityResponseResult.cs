using System.Collections.Generic;
using Zarasa.Editorial.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Zarasa.Editorial.Api.Common.Responses
{
    public class EntityResponseResult<T> : ObjectResponseResult where T : Entity
    {
        
        public EntityResponseResult(T data, string message)
            : base(new EntityResponseModel<T>(data, message)) => StatusCode = StatusCodes.Status200OK;

        public EntityResponseResult(IEnumerable<T> data, string message)
            : base(new EntityResponseList<T>(data, message)) => StatusCode = StatusCodes.Status200OK;

        public EntityResponseResult(IEnumerable<T> data, string message, int pageNumber, int pageSize, long totalRecords)
            : base(new EntityResponseList<T>(data, message, pageNumber, pageSize, totalRecords)) => StatusCode = StatusCodes.Status200OK;
    }
}