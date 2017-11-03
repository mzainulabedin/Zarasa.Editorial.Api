using System.Collections.Generic;
using Zarasa.Editorial.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Zarasa.Editorial.Api.Common.Responses
{
    public class ErrorResponseResult : ObjectResult
    {
        
        public ErrorResponseResult(string message, int statusCode = StatusCodes.Status400BadRequest)
            : base(new ErrorResponseModel(message)) => StatusCode = statusCode;

    }
}