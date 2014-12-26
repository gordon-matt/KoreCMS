using System;

namespace Kore.Web.Indexing
{
    //TODO: This is currently not used anymore because of KorePageType.PopulateDocumentIndex()
    //  Might be better to add some properties to this attribute and use it instead of the
    //  aforementioned method
    [AttributeUsageAttribute(AttributeTargets.Property, AllowMultiple = false)]
    public class SearchableAttribute : Attribute
    {
    }
}