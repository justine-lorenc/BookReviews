using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace BookReviews.Web.Utilities
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType().GetMember(enumValue.ToString()).First()
                .GetCustomAttribute<DisplayAttribute>()?.GetName();
        }

        public static int GetDisplayOrder(this Enum enumValue)
        {
            return enumValue.GetType().GetMember(enumValue.ToString()).First()
                .GetCustomAttribute<DisplayAttribute>()?.GetOrder() ?? 0;
        }

        public static List<Enum> GetValuesInOrder(Type enumType)
        {
            var orderList = new List<Tuple<int, Enum>>();
            Array values = Enum.GetValues(enumType);

            foreach (Enum value in values)
            {
                var tuple = new Tuple<int, Enum>(value.GetDisplayOrder(), value);
                orderList.Add(tuple);
            }

            List<Enum> orderedValues = orderList.OrderBy(x => x.Item1).Select(x => x.Item2).ToList();

            return orderedValues;
        }
    }
}