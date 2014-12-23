using System;

namespace Kore.Web.Indexing
{
    [AttributeUsageAttribute(AttributeTargets.Property, AllowMultiple = false)]
    public class SearchableAttribute : Attribute
    {
    }
}