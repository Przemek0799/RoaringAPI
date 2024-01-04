namespace RoaringAPI.Interface
{
    public interface ITokenService
    {
        Task<string> GetAccessTokenAsync();
    }
}