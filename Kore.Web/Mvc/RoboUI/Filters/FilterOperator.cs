namespace Kore.Web.Mvc.RoboUI.Filters
{
    /// <summary>
    /// Operator used in <see cref="T:Kore.Web.Mvc.RoboUI.Filters.FilterDescriptor"/>
    /// </summary>
    public enum FilterOperator
    {
        IsLessThan,
        IsLessThanOrEqualTo,
        IsEqualTo,
        IsNotEqualTo,
        IsGreaterThanOrEqualTo,
        IsGreaterThan,
        StartsWith,
        EndsWith,
        Contains,
        IsContainedIn,
        IsContainedInList,
        DoesNotContain
    }
}