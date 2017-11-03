
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using Zarasa.Editorial.Api.Models;

namespace Zarasa.Editorial.Api.Common.Responses
{
    public class ObjectResponseModel
    {
        public string Message { get; }

        public object Data { get; }

        public ObjectResponseModel(object data, string message)
        {
            this.Message = message;
            this.Data = data;
        }
    }
}