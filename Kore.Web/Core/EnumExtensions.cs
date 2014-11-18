using System;
using System.Collections;
using System.Linq;
using System.Web.Mvc;

namespace Kore.Web
{
    public static class EnumExtensions
    {
        //Why are these versions here with "Type"? Why did they replace the generic versions?

        public static SelectList ToSelectList(this Type type)
        {
            return ToSelectList(type, null);
        }

        public static SelectList ToSelectList(this Type type, object selectedValue, string emptyText = null)
        {
            if (!type.IsEnum)
            {
                throw new NotSupportedException("The type must be is enum type.");
            }

            var array = Enum.GetValues(type);
            int order;
            var values = (from object e in array
                          select new
                          {
                              Id = e.ConvertTo<int>(),
                              Name = Kore.EnumExtensions.GetDisplayName(e, out order),
                              Order = order
                          }).ToList();

            values = values
                .Where(x => x.Order != -1)
                .OrderBy(x => x.Order)
                .ToList();

            if (emptyText != null)
            {
                values.Insert(0, new { Id = -1, Name = emptyText, Order = -1 });
            }

            return new SelectList(values, "Id", "Name", selectedValue);
        }

        public static SelectList ToSelectList<T>() where T : struct
        {
            return ToSelectList<T>(null);
        }

        public static SelectList ToSelectList<T>(object selectedValue, string emptyText = null) where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new NotSupportedException("You must specify an enum type");
            }

            int order;
            var values = (from T e in Kore.EnumExtensions.GetValues<T>()
                          select new
                          {
                              Id = e.ConvertTo<int>(),
                              Name = Kore.EnumExtensions.GetDisplayName(e, out order),
                              Order = order
                          }).ToList();

            values = values
                .Where(x => x.Order != -1)
                .OrderBy(x => x.Order)
                .ToList();

            if (emptyText != null)
            {
                values.Insert(0, new { Id = -1, Name = emptyText, Order = -1 });
            }

            return new SelectList(values, "Id", "Name", selectedValue);
        }

        public static MultiSelectList ToMultiSelectList(this Type type)
        {
            return ToMultiSelectList(type, null);
        }

        public static MultiSelectList ToMultiSelectList(this Type type, IEnumerable selectedValues, string emptyText = null)
        {
            if (!type.IsEnum)
            {
                throw new NotSupportedException("The type must be is enum type.");
            }

            var array = Enum.GetValues(type);
            int order;
            var values = (from object e in array
                          select new
                          {
                              Id = e.ConvertTo<int>(),
                              Name = Kore.EnumExtensions.GetDisplayName(e, out order),
                              Order = order
                          }).ToList();

            values = values
                .Where(x => x.Order != -1)
                .OrderBy(x => x.Order)
                .ToList();

            if (emptyText != null)
            {
                values.Insert(0, new { Id = -1, Name = emptyText, Order = -1 });
            }

            return new MultiSelectList(values, "Id", "Name", selectedValues);
        }

        public static MultiSelectList ToMultiSelectList<T>() where T : struct
        {
            return ToMultiSelectList<T>(null);
        }

        public static MultiSelectList ToMultiSelectList<T>(IEnumerable selectedValues, string emptyText = null) where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new NotSupportedException("You must specify an enum type");
            }

            int order;
            var values = (from T e in Kore.EnumExtensions.GetValues<T>()
                          select new
                          {
                              Id = e.ConvertTo<int>(),
                              Name = Kore.EnumExtensions.GetDisplayName(e, out order),
                              Order = order
                          }).ToList();

            values = values
                .Where(x => x.Order != -1)
                .OrderBy(x => x.Order)
                .ToList();

            if (emptyText != null)
            {
                values.Insert(0, new { Id = -1, Name = emptyText, Order = -1 });
            }

            return new MultiSelectList(values, "Id", "Name", selectedValues);
        }
    }
}