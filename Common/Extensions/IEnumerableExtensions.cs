using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// 判断IEnumerable是否不为空并且长度大于0
        /// </summary>
        /// <returns></returns>
        public static bool IsNotNullAndCountGtZero<T>(this IEnumerable<T> value)
        {
            return value != null && value.Count() > 0;
        }
    }
}
