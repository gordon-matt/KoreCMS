using System;
using System.Collections.Generic;
using System.Linq;

namespace Kore.Web.Mvc.RoboUI.Filters
{
    public static class FilterExtensions
    {
        #region RemoveFilterDescriptorByPrefix

        public static void RemoveFilterDescriptorByPrefix(this IList<IFilterDescriptor> filters, string prefix)
        {
            for (var i = filters.Count - 1; i >= 0; i--)
            {
                var descriptor = filters[i];
                var compositeFilterDescriptor = descriptor as CompositeFilterDescriptor;
                if (compositeFilterDescriptor != null)
                {
                    RemoveFilterDescriptorByPrefix(compositeFilterDescriptor, prefix);
                    if (compositeFilterDescriptor.FilterDescriptors.Count == 0)
                    {
                        filters.Remove(descriptor);
                    }
                    continue;
                }

                var filterDescriptor = descriptor as FilterDescriptor;
                if (filterDescriptor != null)
                {
                    if (filterDescriptor.Member.StartsWith(prefix + "."))
                    {
                        filters.Remove(descriptor);
                    }
                }
            }
        }

        private static void RemoveFilterDescriptorByPrefix(CompositeFilterDescriptor descriptors, string prefix)
        {
            for (var i = descriptors.FilterDescriptors.Count - 1; i >= 0; i--)
            {
                var descriptor = descriptors.FilterDescriptors[i];
                var compositeFilterDescriptor = descriptor as CompositeFilterDescriptor;
                if (compositeFilterDescriptor != null)
                {
                    RemoveFilterDescriptorByPrefix(compositeFilterDescriptor, prefix);
                    if (compositeFilterDescriptor.FilterDescriptors.Count == 0)
                    {
                        descriptors.FilterDescriptors.Remove(descriptor);
                    }
                    continue;
                }

                var filterDescriptor = descriptor as FilterDescriptor;
                if (filterDescriptor != null)
                {
                    if (filterDescriptor.Member.StartsWith(prefix + "."))
                    {
                        descriptors.FilterDescriptors.Remove(descriptor);
                    }
                }
            }
        }

        #endregion RemoveFilterDescriptorByPrefix

        #region RemoveFilterDescriptorPrefix

        public static void RemoveFilterDescriptorPrefix(this IList<IFilterDescriptor> filters)
        {
            for (var i = filters.Count - 1; i >= 0; i--)
            {
                var descriptor = filters[i];
                var compositeFilterDescriptor = descriptor as CompositeFilterDescriptor;
                if (compositeFilterDescriptor != null)
                {
                    RemoveFilterDescriptorPrefix(compositeFilterDescriptor);
                    if (compositeFilterDescriptor.FilterDescriptors.Count == 0)
                    {
                        filters.Remove(descriptor);
                    }
                    continue;
                }

                var filterDescriptor = descriptor as FilterDescriptor;
                if (filterDescriptor != null)
                {
                    if (filterDescriptor.Member.Contains("."))
                    {
                        filterDescriptor.Member = filterDescriptor.Member.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries).Last();
                    }
                }
            }
        }

        private static void RemoveFilterDescriptorPrefix(CompositeFilterDescriptor descriptors)
        {
            for (var i = descriptors.FilterDescriptors.Count - 1; i >= 0; i--)
            {
                var descriptor = descriptors.FilterDescriptors[i];
                var compositeFilterDescriptor = descriptor as CompositeFilterDescriptor;
                if (compositeFilterDescriptor != null)
                {
                    RemoveFilterDescriptorPrefix(compositeFilterDescriptor);
                    if (compositeFilterDescriptor.FilterDescriptors.Count == 0)
                    {
                        descriptors.FilterDescriptors.Remove(descriptor);
                    }
                    continue;
                }

                var filterDescriptor = descriptor as FilterDescriptor;
                if (filterDescriptor != null)
                {
                    if (filterDescriptor.Member.Contains("."))
                    {
                        filterDescriptor.Member = filterDescriptor.Member.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries).Last();
                    }
                }
            }
        }

        #endregion RemoveFilterDescriptorPrefix

        #region RemoveFilterDescriptor

        public static void RemoveFilterDescriptor(this IList<IFilterDescriptor> filters, string memberName)
        {
            for (var i = filters.Count - 1; i >= 0; i--)
            {
                var descriptor = filters[i];
                var compositeFilterDescriptor = descriptor as CompositeFilterDescriptor;
                if (compositeFilterDescriptor != null)
                {
                    FindInCompositeFilterDescriptor(compositeFilterDescriptor, memberName);
                    if (compositeFilterDescriptor.FilterDescriptors.Count == 0)
                    {
                        filters.Remove(descriptor);
                    }
                    continue;
                }

                var filterDescriptor = descriptor as FilterDescriptor;
                if (filterDescriptor != null)
                {
                    if (filterDescriptor.Member == memberName)
                    {
                        filters.Remove(descriptor);
                    }
                }
            }
        }

        private static void FindInCompositeFilterDescriptor(CompositeFilterDescriptor descriptors, string memberName)
        {
            for (var i = descriptors.FilterDescriptors.Count - 1; i >= 0; i--)
            {
                var descriptor = descriptors.FilterDescriptors[i];
                var compositeFilterDescriptor = descriptor as CompositeFilterDescriptor;
                if (compositeFilterDescriptor != null)
                {
                    FindInCompositeFilterDescriptor(compositeFilterDescriptor, memberName);
                    if (compositeFilterDescriptor.FilterDescriptors.Count == 0)
                    {
                        descriptors.FilterDescriptors.Remove(descriptor);
                    }
                    continue;
                }

                var filterDescriptor = descriptor as FilterDescriptor;
                if (filterDescriptor != null)
                {
                    if (filterDescriptor.Member == memberName)
                    {
                        descriptors.FilterDescriptors.Remove(descriptor);
                    }
                }
            }
        }

        #endregion RemoveFilterDescriptor

        #region IsContainsFilterDescriptorPrefix

        public static bool IsContainsFilterDescriptorPrefix(this IList<IFilterDescriptor> filters, string prefix)
        {
            for (var i = filters.Count - 1; i >= 0; i--)
            {
                var descriptor = filters[i];
                var compositeFilterDescriptor = descriptor as CompositeFilterDescriptor;
                if (compositeFilterDescriptor != null)
                {
                    var value = IsContainsFilterDescriptorPrefix(compositeFilterDescriptor, prefix);
                    if (value)
                    {
                        return true;
                    }
                    continue;
                }

                var filterDescriptor = descriptor as FilterDescriptor;
                if (filterDescriptor != null)
                {
                    if (filterDescriptor.Member.Contains(prefix + "."))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool IsContainsFilterDescriptorPrefix(CompositeFilterDescriptor descriptors, string prefix)
        {
            for (var i = descriptors.FilterDescriptors.Count - 1; i >= 0; i--)
            {
                var descriptor = descriptors.FilterDescriptors[i];
                var compositeFilterDescriptor = descriptor as CompositeFilterDescriptor;
                if (compositeFilterDescriptor != null)
                {
                    var value = IsContainsFilterDescriptorPrefix(compositeFilterDescriptor, prefix);
                    if (value)
                    {
                        return true;
                    }
                    continue;
                }

                var filterDescriptor = descriptor as FilterDescriptor;
                if (filterDescriptor != null)
                {
                    if (filterDescriptor.Member.Contains(prefix + "."))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion IsContainsFilterDescriptorPrefix

        #region GetFilterDescriptionValue

        public static object GetFilterDescriptionValue(this IList<IFilterDescriptor> filters, string name)
        {
            for (var i = filters.Count - 1; i >= 0; i--)
            {
                var descriptor = filters[i];
                var compositeFilterDescriptor = descriptor as CompositeFilterDescriptor;
                if (compositeFilterDescriptor != null)
                {
                    return GetFilterDescriptionValue(compositeFilterDescriptor, name);
                }

                var filterDescriptor = descriptor as FilterDescriptor;
                if (filterDescriptor != null)
                {
                    if (filterDescriptor.Member == name)
                    {
                        return filterDescriptor.Value;
                    }
                }
            }
            return null;
        }

        private static object GetFilterDescriptionValue(CompositeFilterDescriptor descriptors, string name)
        {
            for (var i = descriptors.FilterDescriptors.Count - 1; i >= 0; i--)
            {
                var descriptor = descriptors.FilterDescriptors[i];
                var compositeFilterDescriptor = descriptor as CompositeFilterDescriptor;
                if (compositeFilterDescriptor != null)
                {
                    return GetFilterDescriptionValue(compositeFilterDescriptor, name);
                }

                var filterDescriptor = descriptor as FilterDescriptor;
                if (filterDescriptor != null)
                {
                    if (filterDescriptor.Member == name)
                    {
                        return filterDescriptor.Value;
                    }
                }
            }

            return null;
        }

        #endregion GetFilterDescriptionValue
    }
}