using Task_Managment.PL.Middleware;

namespace Task_Managment.PL.Extensions
{
    public static class MiddlewareExtensions
    {
        
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder app) 
        { 
            return app.UseMiddleware<ExceptionMiddleware>(); 
        }
    }
}
