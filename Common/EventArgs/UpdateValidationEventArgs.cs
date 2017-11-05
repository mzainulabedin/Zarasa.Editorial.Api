
using Microsoft.AspNetCore.Mvc;
using Zarasa.Editorial.Api.Models;

namespace Zarasa.Editorial.Api.Common.EventArgs
{
    public class UpdateValidationEventArgs<T> : System.EventArgs
    {
        private T _Model;
        public UpdateValidationEventArgs(T model, long id){
            this._Model = model;
            this._Id = id;
        }
        public T Model { 
            get { return _Model; } 
            set { _Model = value; } 
        }

         private long  _Id;
         public long Id
         {
             get { return  _Id;}
             set {  _Id = value;}
         }
         

        public IActionResult ActionResult { get; set;}
    }

    

    
}