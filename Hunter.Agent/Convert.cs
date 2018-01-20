using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunter.Agent
{
    /// <summary>
    /// 类型转换工具类
    /// </summary>
    public static partial class Convert
    {
        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>转换失败返回null</returns>
        public static short? ToShort(object obj)
        {
            if (obj == null)
                return null;
            else if (obj is short)
                return (short)obj;
            return ToShort(obj.ToString());
        }

        /// <summary>
        /// </summary>
        /// <param name="str"></param>
        /// <returns>转换失败返回null</returns>
        public static short? ToShort(this string str)
        {
            if (short.TryParse(str, out short result))
                return result;
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>转换失败返回null</returns>
        public static int? ToInt(object obj)
        {
            if (obj == null)
                return null;
            else if (obj is int result)
                return result;
            return ToInt(obj.ToString());
        }

        /// <summary>
        /// </summary>
        /// <param name="str"></param>
        /// <returns>转换失败返回null</returns>
        public static int? ToInt(this string str)
        {
            if (int.TryParse(str, out int result))
                return result;
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>转换失败返回null</returns>
        public static long? ToLong(object obj)
        {
            if (obj == null)
                return null;
            else if (obj is long)
                return (long)obj;
            return ToLong(obj.ToString());
        }

        /// <summary>
        /// </summary>
        /// <param name="str"></param>
        /// <returns>转换失败返回null</returns>
        public static long? ToLong(this string str)
        {
            if (long.TryParse(str, out long result))
                return result;
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>转换失败返回null</returns>
        public static ushort? ToUShort(object obj)
        {
            if (obj == null)
                return null;
            else if (obj is ushort)
                return (ushort)obj;
            return ToUShort(obj.ToString());
        }

        /// <summary>
        /// </summary>
        /// <param name="str"></param>
        /// <returns>转换失败返回null</returns>
        public static ushort? ToUShort(this string str)
        {
            if (ushort.TryParse(str, out ushort result))
                return result;
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>转换失败返回null</returns>
        public static uint? ToUInt(object obj)
        {
            if (obj == null)
                return null;
            else if (obj is uint)
                return (uint)obj;
            return ToUInt(obj.ToString());
        }

        /// <summary>
        /// </summary>
        /// <param name="str"></param>
        /// <returns>转换失败返回null</returns>
        public static uint? ToUInt(this string str)
        {
            if (uint.TryParse(str, out uint result))
                return result;
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>转换失败返回null</returns>
        public static ulong? ToULong(object obj)
        {
            if (obj == null)
                return null;
            else if (obj is ulong)
                return (ulong)obj;
            return ToULong(obj.ToString());
        }

        /// <summary>
        /// </summary>
        /// <param name="str"></param>
        /// <returns>转换失败返回null</returns>
        public static ulong? ToULong(this string str)
        {
            if (ulong.TryParse(str, out ulong result))
                return result;
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>转换失败返回null</returns>
        public static float? ToFloat(object obj)
        {
            if (obj == null)
                return null;
            else if (obj is float)
                return (float)obj;
            return ToFloat(obj.ToString());
        }

        /// <summary>
        /// </summary>
        /// <param name="str"></param>
        /// <returns>转换失败返回null</returns>
        public static float? ToFloat(this string str)
        {
            if (float.TryParse(str, out float result))
                return result;
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>转换失败返回null</returns>
        public static double? ToDouble(object obj)
        {
            if (obj == null)
                return null;
            else if (obj is double)
                return (double)obj;
            return ToDouble(obj.ToString());
        }

        /// <summary>
        /// </summary>
        /// <param name="str"></param>
        /// <returns>转换失败返回null</returns>
        public static double? ToDouble(this string str)
        {
            if (double.TryParse(str, out double result))
                return result;
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>转换失败返回null</returns>
        public static decimal? ToDecimal(object obj)
        {
            if (obj == null)
                return null;
            else if (obj is decimal @decimal)
                return @decimal;
            else if (obj is double @double)
                return ToDecimal(@double);
            return ToDecimal(obj.ToString());
        }

        /// <summary>
        /// </summary>
        /// <param name="str"></param>
        /// <returns>转换失败返回null</returns>
        public static decimal? ToDecimal(this string str)
        {
            if (decimal.TryParse(str, out decimal result))
                return result;
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static decimal? ToDecimal(this double m)
        {
            return new Decimal(m);
        }

        /// <summary>
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static decimal? ToDecimal(this double? m)
        {
            if (m == null)
                return null;
            return new Decimal(m.Value);
        }

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>转换失败返回null</returns>
        public static char? ToChar(object obj)
        {
            if (obj == null)
                return null;
            else if (obj is char)
                return (char)obj;
            return ToChar(obj.ToString());
        }

        /// <summary>
        /// </summary>
        /// <param name="str"></param>
        /// <returns>转换失败返回null</returns>
        public static char? ToChar(this string str)
        {
            if (char.TryParse(str, out char result))
                return result;
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>转换失败返回null</returns>
        public static bool? ToBool(object obj)
        {
            if (obj == null)
                return null;
            else if (obj is bool)
                return (bool)obj;
            return ToBool(obj.ToString());
        }

        /// <summary>
        /// </summary>
        /// <param name="str"></param>
        /// <returns>转换失败返回null</returns>
        public static bool? ToBool(this string str)
        {
            if (bool.TryParse(str, out bool result))
                return result;
            return null;
        }

        /// <summary>转换失败返回null
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public static System.DateTime? ToDateTime(object obj, bool ticks = false)
        {
            if (obj == null)
                return null;
            else if (obj is System.DateTime result)
                return result;
            else if (ticks && obj is long temp)
                return ToDateTime(temp);
            return ToDateTime(obj.ToString());
        }

        /// <summary>
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public static System.DateTime? ToDateTime(long ticks)
        {
            if (ticks > DateTime.MinValue.Ticks && ticks < DateTime.MaxValue.Ticks)
                return new DateTime(ticks);
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="str"></param>
        /// <returns>转换失败返回null</returns>
        public static System.DateTime? ToDateTime(this string str)
        {
            if (System.DateTime.TryParse(str, out DateTime result))
                return result;
            return null;
        }

        /// <summary> retrun t == null ?  null : t.ToString();
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ToString(this object t)
        {
            if (t == null)
                return null;
            else if (t is string)
                return (string)t;
            return t.ToString();
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T? ToEnum<T>(this string str) where T : struct
        {
            if (String.IsNullOrWhiteSpace(str))
                return null;
            if (Enum.TryParse(str, out T result))
                return result;
            return null;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T? ToEnum<T>(object obj) where T : struct
        {
            if (obj == null)
                return null;
            else if (obj is T)
                return (T)obj;
            return ToEnum<T>(obj.ToString());
        }

       
    }

}