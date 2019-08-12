using Financial.Services.Interfaces.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Financial.Presentation.ChatWebServer.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            JsonResult result = new JsonResult(new { ErrorMessage = context.Exception.Message });
            result.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = result;
        }
    }
}

