﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTP_Client_Asp_Server.ConsoleClass
{
    public static class CommandInvoker
    {
        public static object Invoke(this Delegate @delegate, object[] parameters)
        {
            // Invoke command and wait if it returns a task value.
            var returnValue = @delegate.DynamicInvoke(parameters);

            if (returnValue is Task task)
            {
                var resultProperty = task.GetType().GetProperty("Result");
                return resultProperty.PropertyType.ToString() == "System.Threading.Tasks.VoidTaskResult"
                    ? ""
                    : (resultProperty?.GetValue(task));
            }

            return returnValue;
        }
    }
}