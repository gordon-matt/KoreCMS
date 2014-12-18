namespace Kore.Caching
{
    public interface IVolatileToken
    {
        bool IsCurrent { get; }
    }
}