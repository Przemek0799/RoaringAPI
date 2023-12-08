
namespace RoaringAPI.Interface
{
    public interface IExceptionHandlingService
    {
        Task HandleExceptionAsync(Exception exception, string customMessage);
    }
}
