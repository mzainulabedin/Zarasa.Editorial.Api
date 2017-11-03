
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using Zarasa.Editorial.Api.Models;
using System.Collections;

namespace Zarasa.Editorial.Api.Common.Responses
{
    public class EntityResponseList<T> : ObjectResponseModel where T : Entity
    {
        public int PageNumber { get; }

        public int PageSize { get; }

        public long TotalRecords { get; }

        public new IEnumerable<T> Data { get; }

        public EntityResponseList(IEnumerable<T> data, string message, int pageNumber, int pageSize, long totalRecords)
            :base(data, message)
        {
            this.Data = data;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalRecords = totalRecords;
        }

        public EntityResponseList(IEnumerable<T> data, string message):this(data, message, 0, 0, 0)
        {
        }
    }
}