//using Kore.DI;

namespace Kore.Data.EntityFramework
{
    public interface IEntityTypeConfiguration
    {
        bool IsEnabled { get; }
    }
}