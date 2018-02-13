
using Microsoft.AspNetCore.Mvc;
using Zarasa.Editorial.Api.Models;
using System.Linq;

namespace Zarasa.Editorial.Api.Common.EventArgs
{
    public class SearchEventArgs<T> : System.EventArgs
    {
        private IQueryable<T> _Query;
        private string _SearchString;
        private string _OrderBy;
        private string _OrderByDirection = "asc";
        public SearchEventArgs(IQueryable<T> query, string searchString, string orderBy, string orderByDirection){
            this._Query = query;
            this._SearchString = searchString;
            this._OrderBy = orderBy;
            this._OrderByDirection = orderByDirection;
        }
        public IQueryable<T> Query { 
            get { return _Query; } 
            set { _Query = value; } 
        }

        public IActionResult ActionResult { get; set;}
        public string SearchString { get => _SearchString; set => _SearchString = value; }
        public string OrderBy { get => _OrderBy; set => _OrderBy = value; }
        public string OrderByDirection { get => _OrderByDirection; set => _OrderByDirection = value; }
    }

    

    
}