using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Treatment.Monitor.BusinessLogic.ServiceAction;
using Treatment.Monitor.Extensions;
using Treatment.Monitor.Models;

namespace Treatment.Monitor.Filters
{
    public class ServiceActionFilter : IActionFilter
    {
                public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is not ObjectResult result || result.Value == null)
            {
                return;
            }

            context.Result = result.Value switch
            {
                IServiceActionResult<int> intValue => GetActionResult(intValue),
                IServiceActionResult<bool> boolValue => GetActionResult(boolValue),
                IServiceActionResult<object> objectValue => GetActionResult(objectValue),
                IServiceActionResult value => GetActionResult(value),
                _ => context.Result
            };
        }

        private static IActionResult GetActionResult(IServiceActionResult serviceActionResult) =>
            new ObjectResult(new ErrorResult(serviceActionResult.ErrorMessages).ToJson())
            {
                StatusCode = GetStatusCodeForResultType(serviceActionResult.ResultType)
            };

        private static IActionResult GetActionResult<T>(IServiceActionResult<T> serviceActionResult) =>
            new ObjectResult(serviceActionResult.GetData()
                             ?? new ErrorResult(serviceActionResult.ErrorMessages).ToJson())
            {
                StatusCode = GetStatusCodeForResultType(serviceActionResult.ResultType)
            };

        private static int GetStatusCodeForResultType(ServiceActionResultType serviceActionResultType) =>
            serviceActionResultType switch
            {
                ServiceActionResultType.Created => (int) HttpStatusCode.Created,
                ServiceActionResultType.Success => (int) HttpStatusCode.OK,
                ServiceActionResultType.ForbiddenAccess => (int) HttpStatusCode.Forbidden,
                ServiceActionResultType.UnauthorizedAccess => (int) HttpStatusCode.Unauthorized,
                ServiceActionResultType.InvalidDataOrOperation => (int) HttpStatusCode.BadRequest,
                ServiceActionResultType.ObjectNotFound => (int) HttpStatusCode.NotFound,
                _ => throw new InvalidOperationException("Unsupported operation type")
            };

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            context.Result = context.ModelState.ToBadResponseResult();
        }
    }
}