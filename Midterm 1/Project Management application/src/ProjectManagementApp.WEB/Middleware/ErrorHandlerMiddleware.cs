using Microsoft.AspNetCore.Http;
using ProjectManagementApp.BLL.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApi.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case UserNotFoundException userNotFound:
                    case TeamNotFoundException teamNotFound:
                    case ProjectNotFoundException projectNotFound:
                    case TaskNotFoundException taskNotFound:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case UserExistException userExistException:
                    case TeamExistException teamExistException:
                    case ProjectExistException projectExistException:
                    case TaskExistException taskExistException:
                        response.StatusCode = (int)HttpStatusCode.Conflict;
                        break;
                    case UnauthorizedUserException unauthorizedUserException:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}