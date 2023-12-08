using RoaringAPI.Interface;
using System;
using System.Threading.Tasks;

namespace RoaringAPI.Service
{
    public class ExceptionHandlingService : IExceptionHandlingService
    {
        public async Task HandleExceptionAsync(Exception exception, string customMessage)
        {
            // Log the exception or perform any other centralized exception handling here
            Console.WriteLine($"Exception: {customMessage}. Details: {exception.Message}");
        }
    }
}
