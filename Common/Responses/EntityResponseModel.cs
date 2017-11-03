
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using Zarasa.Editorial.Api.Models;

namespace Zarasa.Editorial.Api.Common.Responses
{
    public class EntityResponseModel<T> : ObjectResponseModel where T : Entity
    {
        public new T Data { get; }

        public EntityResponseModel(T data, string message)
            : base(data, message) => this.Data = data;
        
    }
}