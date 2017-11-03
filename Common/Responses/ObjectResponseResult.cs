using System.Collections.Generic;
using Zarasa.Editorial.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Zarasa.Editorial.Api.Common.Responses
{
    public class ObjectResponseResult : ObjectResult
    {
        
        public ObjectResponseResult(object data, string message)
            : base(new ObjectResponseModel(data, message)) => StatusCode = StatusCodes.Status200OK;

        public ObjectResponseResult(ObjectResponseModel objectResponseModel)
            : base(objectResponseModel) => StatusCode = StatusCodes.Status200OK;
    }
}