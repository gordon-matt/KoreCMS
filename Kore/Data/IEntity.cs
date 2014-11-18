namespace Kore.Data
{
    public interface IEntity
    {
        //object Id { get; set; }

        object[] KeyValues { get; }
    }

    //public interface IEntity<TKey> where TKey : struct
    //{
    //    TKey Id { get; set; }
    //}
}