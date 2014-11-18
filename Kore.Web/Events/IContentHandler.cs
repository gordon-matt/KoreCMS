//using Kore.DI;

namespace Kore.Web.Events
{
    public interface IContentHandler //: IDependency
    {
    }

    public interface IContentHandler<T> : IContentHandler
    {
        void Creating(CreateContentContext<T> context);

        void Created(CreateContentContext<T> context);

        void Updating(UpdateContentContext<T> context);

        void Updated(UpdateContentContext<T> context);

        void Removing(RemoveContentContext<T> context);

        void Removed(RemoveContentContext<T> context);
    }

    public abstract class ContentContextBase<T>
    {
        public T ContentItem { get; protected set; }

        protected ContentContextBase(T contentItem)
        {
            ContentItem = contentItem;
        }
    }

    public class CreateContentContext<T> : ContentContextBase<T>
    {
        public CreateContentContext(T contentItem)
            : base(contentItem)
        {
        }
    }

    public class UpdateContentContext<T> : ContentContextBase<T>
    {
        public UpdateContentContext(T contentItem)
            : base(contentItem)
        {
        }
    }

    public class RemoveContentContext<T> : ContentContextBase<T>
    {
        public RemoveContentContext(T contentItem)
            : base(contentItem)
        {
        }
    }
}