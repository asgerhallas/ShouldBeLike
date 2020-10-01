using System.Collections.Generic;
using System.Linq;

namespace ShouldBeLike
{
    public static class LinqEx
    {
        public static IEnumerable<T> SkipUntil<T>(this IEnumerable<T> self, T obj) => 
            self.SkipWhile(item => !Equals(item, obj));
    }
}