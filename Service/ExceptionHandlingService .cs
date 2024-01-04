using RoaringAPI.Interface;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RoaringAPI.Service
{
    public class ExceptionHandlingService : IExceptionHandlingService
    {
        private readonly ILogger<ExceptionHandlingService> _logger;

        public ExceptionHandlingService(ILogger<ExceptionHandlingService> logger)
        {
            _logger = logger;
        }

        public async Task HandleExceptionAsync(Exception exception, string customMessage)
        {
            _logger.LogError(exception, "Exception: {CustomMessage}", customMessage);
        }
    }
}
