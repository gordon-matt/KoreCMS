namespace Kore.Events.Handlers
{
    public interface IEntityModifiedEventHandler<T> : IEventHandler
    {
        void Deleted(T entity);

        void Inserted(T entity);

        void Updated(T entity);
    }
}