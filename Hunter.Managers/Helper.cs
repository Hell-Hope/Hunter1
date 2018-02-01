using System;
using System.Collections.Generic;

namespace Hunter.Managers
{
    using MongoDB.Driver;

    public static class Helper
    {

        public static IFindFluent<TDocument, TProjection> Pagination<TDocument, TProjection, Condtion>(this IFindFluent<TDocument, TProjection> findFluent, Models.PageParam<Condtion> pageParam)
        {
            var temp = findFluent;
            if (pageParam.Index > 1)
                temp = findFluent.Skip((pageParam.Index - 1) * pageParam.Size);
            temp = findFluent.Limit(pageParam.Size);
            return temp;
        }

        public static IFindFluent<TDocument, TProjection> Sort<TDocument, TProjection, Condtion>(this IFindFluent<TDocument, TProjection> findFluent, Models.PageParam<Condtion> pageParam)
        {
            var temp = findFluent;
            if (pageParam.Sort != null)
            {
                var sort = new SortDefinitionBuilder<TDocument>();
                if (pageParam.Sort.Order == Models.Order.Ascending)
                {
                    temp = temp.Sort(sort.Ascending(pageParam.Sort.Field));
                }
                else if (pageParam.Sort.Order == Models.Order.Descending)
                {
                    temp = temp.Sort(sort.Descending(pageParam.Sort.Field));
                }
            }
            return temp;
        }

        public static string FormatQueryString(string str)
        {
            if (str == null)
                return String.Empty;
            return str.Replace("\\", "\\\\");
        }
    }
}

namespace System.Linq
{
    using Hunter.Entities;

    public static class Helper
    {
        public static Node GetStartNode(this IEnumerable<Node> that)
        {
            return that.Where(m => m.IsStartType).FirstOrDefault();
        }

        public static IEnumerable<Line> ByFrom(this IEnumerable<Line> that, string from)
        {
            return that.Where(m => m.From == from);
        }

    }
}
