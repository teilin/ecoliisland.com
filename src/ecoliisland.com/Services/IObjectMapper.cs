namespace ecoliisland.com.Services
{
    public interface IObjectMapper
    {
        TDest Map<TSource, TDest>(TSource source, TDest dest);
    }
}