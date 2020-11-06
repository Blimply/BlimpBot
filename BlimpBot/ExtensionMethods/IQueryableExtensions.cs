using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlimpBot.ExtensionMethods
{
    public static class IQueryableExtensions
    {
        public static T RandomRow<T>(this IQueryable<T> query)
        {
            var countOfRows = query.Count();
            var random = new Random(); //We don't care too much about potential duplicates for short times
            return query.Skip(random.Next(0, countOfRows)).FirstOrDefault();
        }
    }
}
