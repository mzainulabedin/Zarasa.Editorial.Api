
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using Zarasa.Editorial.Api.Models;


namespace Zarasa.Editorial.Api.Common.Responses
{
    public class ErrorResponseModel
    {
        public string Message { get; }

        public ErrorResponseModel(string message)
        {
            this.Message = message;
        }
    }
}