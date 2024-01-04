namespace RoaringAPI.Interface
{
    public interface ICacheService
    {
        void Add<TItem>(string key, TItem item, TimeSpan duration);
        TItem Get<TItem>(string key);
        bool TryGetValue<TItem>(string key, out TItem value);
    }
}
