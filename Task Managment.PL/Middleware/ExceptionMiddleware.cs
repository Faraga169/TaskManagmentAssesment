using System.Text.Json;
using Task_Managment.BLL.Exceptions;
using Task_Managment.PL.Errors;

namespace Task_Managment.PL.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next; 
        public ExceptionMiddleware(RequestDelegate next) 
        { 
            _next = next; 
        }
        public async Task InvokeAsync(HttpContext context) 
        { 
            try 
            {
                await _next(context); 
            } 
            catch (Exception ex) { 
                context.Response.ContentType = "application/json"; 
                var response = new ErrorResponse(); 
                switch (ex) { 
                    case BadRequestException: response.StatusCode = StatusCodes.Status400BadRequest; 
                        break; 
                    case UnauthorizedException: response.StatusCode = StatusCodes.Status401Unauthorized; 
                        break; 
                    case ForbiddenException: response.StatusCode = StatusCodes.Status403Forbidden; 
                        break; 
                    case NotFoundException: response.StatusCode = StatusCodes.Status404NotFound; 
                        break; 
                    default: response.StatusCode = StatusCodes.Status500InternalServerError; 
                        break; 
                } 
                response.Message = ex.Message; 
                context.Response.StatusCode = response.StatusCode; 
                await context.Response.WriteAsync(JsonSerializer.Serialize(response)); 
            } 
        }
    }
}
