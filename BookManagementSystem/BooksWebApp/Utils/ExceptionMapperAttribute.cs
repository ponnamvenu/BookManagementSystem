using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BooksWebApp.Utils
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class ExceptionMapperAttribute : Attribute, IExceptionFilter
    {
        public Type ExceptionType { get; set; }
        public int HttpStatus { get; set; }

        public void OnException(ExceptionContext context)
        {
            //if exception that is throw is of type or subtype of ExceptionType mentioned in the filter
            if (ExceptionType.IsAssignableFrom(context.Exception.GetType()))
            {
                var result = new ObjectResult(context.Exception.Message)
                {
                    StatusCode = HttpStatus
                };

                context.Result = result; //setting context.Result will prevent MVC from processing it.
            }
        }
    }
}