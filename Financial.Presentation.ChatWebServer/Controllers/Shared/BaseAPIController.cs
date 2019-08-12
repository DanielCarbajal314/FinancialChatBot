using Financial.Presentation.ChatWebServer.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financial.Presentation.ChatWebServer.Controllers.Shared
{
    [ServiceFilter(typeof(ExceptionFilter))] 
    public class BaseAPIController : ControllerBase
    {
    }
}
