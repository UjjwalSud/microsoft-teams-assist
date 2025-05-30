﻿using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;
using Microsoft.Teams.Assist.Application.Common.Exceptions;
using Microsoft.Teams.Assist.Application.Common.Interfaces;
using System.Net;

namespace Microsoft.Teams.Assist.Infrastructure.Middleware;

internal class ExceptionMiddleware : IMiddleware
{
    private readonly ICurrentUser _currentUser;
    //private readonly IStringLocalizer _t;
    private readonly ISerializerService _jsonSerializer;

    public ExceptionMiddleware(
        ICurrentUser currentUser,
        //IStringLocalizer<ExceptionMiddleware> localizer,
        ISerializerService jsonSerializer)
    {
        _currentUser = currentUser;
       // _t = localizer;
        _jsonSerializer = jsonSerializer;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            string email = _currentUser.GetUserEmail() is string userEmail ? userEmail : "Anonymous";
            var userId = _currentUser.GetUserId();
            var tenantId = _currentUser.GetTenant();//?? string.Empty;
            if (userId != Guid.Empty) LogContext.PushProperty("UserId", userId);
            LogContext.PushProperty("UserEmail", email);
            if (tenantId != -1) LogContext.PushProperty("Tenant", tenantId);
            string errorId = Guid.NewGuid().ToString();
            LogContext.PushProperty("ErrorId", errorId);
            LogContext.PushProperty("StackTrace", exception.StackTrace);
            var errorResult = new ErrorResult
            {
                Source = exception.TargetSite?.DeclaringType?.FullName,
                Exception = exception.Message.Trim() + "\n" + (exception.InnerException != null ? exception.InnerException.Message.Trim() : string.Empty),
                ErrorId = errorId,
                SupportMessage = string.Format("Provide the ErrorId {0} to the support team for further analysis.", errorId)
                //_t["Provide the ErrorId {0} to the support team for further analysis.", errorId]
            };

            if (exception is not CustomException && exception.InnerException != null)
            {
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
            }

            //if (exception is FluentValidation.ValidationException fluentException)
            //{
            //    errorResult.Exception = "One or More Validations failed.";
            //    foreach (var error in fluentException.Errors)
            //    {
            //        errorResult.Messages.Add(error.ErrorMessage);
            //    }
            //}

            switch (exception)
            {
                case CustomException e:
                    errorResult.StatusCode = (int)e.StatusCode;
                    if (e.ErrorMessages is not null)
                    {
                        errorResult.Messages = e.ErrorMessages;
                    }

                    break;

                case KeyNotFoundException:
                    errorResult.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                //case FluentValidation.ValidationException:
                //    errorResult.StatusCode = (int)HttpStatusCode.UpgradeRequired;
                //    break;

                default:
                    errorResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            Log.Error($"{errorResult.Exception} Request failed with Status Code {errorResult.StatusCode} and Error Id {errorId}.");
            var response = context.Response;
            if (!response.HasStarted)
            {
                response.ContentType = "application/json";
                response.StatusCode = errorResult.StatusCode;
                await response.WriteAsync(_jsonSerializer.Serialize(errorResult));
            }
            else
            {
                Log.Warning("Can't write error response. Response has already started.");
            }
        }
    }
}
