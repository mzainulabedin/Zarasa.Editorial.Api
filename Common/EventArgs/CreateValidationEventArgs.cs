
using Microsoft.AspNetCore.Mvc;
using Zarasa.Editorial.Api.Models;

namespace Zarasa.Editorial.Api.Common.EventArgs
{
    public class CreateValidationEventArgs1<T> : System.EventArgs
    {
        private T _Model;
        public CreateValidationEventArgs1(T model){
            this._Model = model;
        }
        public T Model { 
            get { return _Model; } 
            set { _Model = value; } 
        }

        public IActionResult ActionResult { get; set;}
    }

    

    
}