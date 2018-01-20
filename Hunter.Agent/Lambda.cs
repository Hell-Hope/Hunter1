using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hunter.Agent
{
    public class Lambda
    {

        public static System.Reflection.MethodInfo EFLike { get; private set; }

        /// <summary> 等于
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static System.Linq.Expressions.Expression<Func<TSource, bool>> PropertyEqual<TSource>(string name, object value)
        {
            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(TSource), "m");
            var property = System.Linq.Expressions.Expression.Property(parameter, name);
            var constant = System.Linq.Expressions.Expression.Constant(value);
            var equal = System.Linq.Expressions.Expression.Equal(property, constant);
            var lambda = System.Linq.Expressions.Expression.Lambda<Func<TSource, bool>>(equal, parameter);
            return lambda;
        }

        /// <summary> 大于
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static System.Linq.Expressions.Expression<Func<TSource, bool>> PropertyGreaterThan<TSource>(string name, object value)
        {
            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(TSource), "m");
            var property = System.Linq.Expressions.Expression.Property(parameter, name);
            var constant = System.Linq.Expressions.Expression.Constant(value);
            var equal = System.Linq.Expressions.Expression.GreaterThan(property, constant);
            var lambda = System.Linq.Expressions.Expression.Lambda<Func<TSource, bool>>(equal, parameter);
            return lambda;
        }

        /// <summary> 大于或等于
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static System.Linq.Expressions.Expression<Func<TSource, bool>> PropertyGreaterThanOrEqual<TSource>(string name, object value)
        {
            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(TSource), "m");
            var property = System.Linq.Expressions.Expression.Property(parameter, name);
            var constant = System.Linq.Expressions.Expression.Constant(value);
            var equal = System.Linq.Expressions.Expression.GreaterThanOrEqual(property, constant);
            var lambda = System.Linq.Expressions.Expression.Lambda<Func<TSource, bool>>(equal, parameter);
            return lambda;
        }

        /// <summary> 小于
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static System.Linq.Expressions.Expression<Func<TSource, bool>> PropertyLessThan<TSource>(string name, object value)
        {
            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(TSource), "m");
            var property = System.Linq.Expressions.Expression.Property(parameter, name);
            var constant = System.Linq.Expressions.Expression.Constant(value);
            var equal = System.Linq.Expressions.Expression.LessThan(property, constant);
            var lambda = System.Linq.Expressions.Expression.Lambda<Func<TSource, bool>>(equal, parameter);
            return lambda;
        }

        /// <summary> 小于或等于
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static System.Linq.Expressions.Expression<Func<TSource, bool>> PropertyLessThanOrEqual<TSource>(string name, object value)
        {
            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(TSource), "m");
            var property = System.Linq.Expressions.Expression.Property(parameter, name);
            var constant = System.Linq.Expressions.Expression.Constant(value);
            var equal = System.Linq.Expressions.Expression.LessThanOrEqual(property, constant);
            var lambda = System.Linq.Expressions.Expression.Lambda<Func<TSource, bool>>(equal, parameter);
            return lambda;
        }

        public static System.Linq.Expressions.Expression<Func<TSource, object>> PropertyOrder<TSource>(string name)
        {
            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(TSource), "m");
            var property = System.Linq.Expressions.Expression.Property(parameter, name);
            var lambda = System.Linq.Expressions.Expression.Lambda<Func<TSource, object>>(property, parameter);
            return lambda;
        }
    }
}
