using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_Core_ServerTests.Infrastructure;

public static class ResultExtensions
{
    public static T OKResponseToType<T>(ActionResult<T> actionResult) where T : class
    {
        var result = actionResult.Result as OkObjectResult;
        return result.Value as T;
    }
}
