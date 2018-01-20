using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hunter.Agent
{
	/// <summary>
	/// </summary>
	public static class Formater
	{
		/// <summary> yyyy-MM-dd
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static string ToIOSDateString(this DateTime dt)
		{
			return String.Format("{0}-{1}-{2}", dt.Year, dt.Month.ToString().PadLeft(2, '0'), dt.Day.ToString().PadLeft(2, '0'));
		}

		/// <summary> yyyy-MM-dd
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static string ToIOSDateString(this DateTime? dt)
		{
			if (dt.HasValue)
				return ToIOSDateString(dt.Value);
			return null;
		}

        /// <summary> yyyy年MM月dd日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToChineseDateString(this DateTime dt)
        {
            return String.Format("{0}年{1}月{2}日", dt.Year, dt.Month.ToString().PadLeft(2, '0'), dt.Day.ToString().PadLeft(2, '0'));
        }

		/// <summary> HH:mm:ss
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static string ToIOSTimeString(this DateTime dt)
		{
			return String.Format("{0}:{1}:{2}", dt.Hour.ToString().PadLeft(2, '0'), dt.Minute.ToString().PadLeft(2, '0'), dt.Second.ToString().PadLeft(2, '0'));
		}

        /// <summary> HH时mm分ss秒
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToChineseTimeString(this DateTime dt)
        {
            return String.Format("{0}时{1}分{2}秒", dt.Hour.ToString().PadLeft(2, '0'), dt.Minute.ToString().PadLeft(2, '0'), dt.Second.ToString().PadLeft(2, '0'));
        }

		/// <summary> yyyy-MM-dd HH:mm:ss
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static string ToIOSDateTimeString(this DateTime dt)
		{
			return String.Format("{0} {1}", ToIOSDateString(dt), ToIOSTimeString(dt));
		}

		/// <summary> yyyy-MM-dd HH:mm:ss
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static string ToIOSDateTimeString(this DateTime? dt)
		{
			if (dt.HasValue)
				return String.Format("{0} {1}", ToIOSDateString(dt.Value), ToIOSTimeString(dt.Value));
			return null;
		}

        /// <summary> yyyy年MM月dd日HH时mm分ss秒
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToChineseDateTimeString(this DateTime dt)
        {
            return String.Format("{0}{1}", ToChineseDateString(dt), ToChineseTimeString(dt));
        }

		/// <summary> 100,000,000.00
		/// </summary>
		/// <returns></returns>
		public static string ThousansMoney(decimal value)
		{
			return String.Format("{0:N}", value);
		}

		/// <summary> 100,000,000.00
		/// </summary>
		/// <returns></returns>
		public static string ThousansMoney(decimal? value)
		{
			if (value.HasValue)
			{
				return ThousansMoney(value.Value);
			}
			return null;
		}

        /// <summary>
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static String Chinese(decimal number)
        {
            var s = number.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            var d = System.Text.RegularExpressions.Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            var r = System.Text.RegularExpressions.Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());
            if (r.EndsWith("元"))//这两行是我加的
                r += "整";//感觉我拉低了前边代码的逼格……很惭愧
            return r;
        }

        /// <summary>
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="content"></param>
        /// <param name="tab"></param>
        /// <returns></returns>
        public static StringBuilder Information(System.Exception ex, StringBuilder content = null, string tab = "")
        {
            if (content == null)
                content = new StringBuilder();
            if (ex != null)
            {
                content.AppendLine(tab + ex.Message);
                content.AppendLine(tab + ex.Source);
                var stackTrace = ex.StackTrace;
                stackTrace = stackTrace.Replace(Environment.NewLine, Environment.NewLine + tab);
                content.AppendLine(tab + stackTrace);
                if (ex.InnerException != null && ex.InnerException != ex)
                {
                    Information(ex.InnerException, content, "\t\t" + tab);
                }
            }
            return content;
        }

        public static IQueryable<TSource> WhereEQ<TSource>(this IQueryable<TSource> source, string name, object value)
        {
            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(TSource), "m");
            var property = System.Linq.Expressions.Expression.Property(parameter, name);
            var constant = System.Linq.Expressions.Expression.Constant(value);
            var equal = System.Linq.Expressions.Expression.Equal(property, constant);
            var lambda = System.Linq.Expressions.Expression.Lambda<Func<TSource, bool>>(equal, parameter);
            
            return source.Where(lambda);  
        }
    }
}
