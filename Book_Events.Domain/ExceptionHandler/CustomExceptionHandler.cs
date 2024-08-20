using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Events.Domain.ExceptionHandler
{
    public class CustomExceptionHandlerAttribute : ExceptionFilterAttribute, IExceptionFilter
    {
    //    private readonly ILogger<CustomExceptionHandlerAttribute> _logger;

    //    public CustomExceptionHandlerAttribute(ILogger<CustomExceptionHandlerAttribute> logger)
    //    {
    //        _logger = logger;
    //    }

        public CustomExceptionHandlerAttribute()
        {
            
        }

        public override void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled && context.Exception is ArgumentException)
            {
                //_logger.LogError("Error: {Message}", context.Exception.Message);
                context.Result = new RedirectToActionResult("Error", "Event", null);
                context.ExceptionHandled = true;
            }
        }
    }
}
