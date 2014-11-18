namespace Kore.Web.Mvc.RoboUI
{
    public interface IDescriptor
    {
        void Deserialize(string source);

        string Serialize();
    }
}