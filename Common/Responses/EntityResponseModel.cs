
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using Zarasa.Editorial.Api.Models;

namespace Zarasa.Editorial.Api.Common.Responses
{
    public class EntityResponseModel<T> where T : Entity
    {
        public string Message { get; }

        public T Data { get; }

        public EntityResponseModel(T data, string message)
        {
            this.Message = message;
            this.Data = data;
        }
    }
}