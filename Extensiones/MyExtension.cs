using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using MyWPFUI.Controls;

namespace MyWPFUI.Extensiones
{
    public static class MyExtension
    {
        /// <summary>
        /// 内存回收
        /// </summary>
        [Pure]
        public static void MemoryGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        internal static string ApplicationGlobalPackUriTemplate = null;
        /// <summary>
        /// 使用之前先设置全局模板
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        [Pure]
        public static Uri CreatePackUri(string filename)
        {
            return new Uri(ApplicationGlobalPackUriTemplate + filename, UriKind.RelativeOrAbsolute);
        }
        [Pure]
        public static string CreatePackUriString(string filename)
        {
            return ApplicationGlobalPackUriTemplate + filename;
        }
        [Pure]
        public static List<Guid> ToGuidList(this string ids)
        {
            if (ids.IsNull()) return null;

            return ids.Split(',').Select(x =>
            {
                Guid gid = Guid.Empty;
                Guid.TryParse(x, out gid);
                if (gid != Guid.Empty)
                {
                    return gid;
                }
                return Guid.Empty;
            }

            ).ToList();
        }
        /// <summary>
        /// 默认是 逗号分离
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Pure]
        public static List<string> ToStringList(this string ids)
        {
            if (ids.IsNull()) return null;

            return ids.Split(',').ToList();
        }
        [Pure]
        public static List<string> ToStringList(this string ids, char splitchar)
        {
            if (ids.IsNull()) return null;

            return ids.Split(splitchar).ToList();
        }
        [Pure]
        public static string ToObjectString(this object obj)
        {
            return null == obj ? String.Empty : obj.ToString();
        }
        [Pure]
        public static string ToGuidStringNoSplit(this Guid obj, string addBeforeChar = "", bool isUpperCase = false)
        {
            if (isUpperCase)
            {
                return addBeforeChar + obj.ToObjectString().ToUpper().Replace("-", "");
            }
            else
            {
                return addBeforeChar + obj.ToObjectString().ToLower().Replace("-", "");
            }

        }
        /// <summary>
        /// 切勿转换用于double的字符串 转int类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Pure]
        public static int ToInt(this object obj)
        {
            int ad;
            if (int.TryParse(obj.ToObjectString(), out ad))
            {
                return ad;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 处理，例如03的字符串，即返回3，如果"2"的字符串，那么返回2
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Pure]
        public static int ToInt2(this object obj)
        {
            int ad;
            string ass = obj.ToObjectString();
            if (ass[0] == '0')
            {
                if (int.TryParse(ass[1].ToObjectString(), out ad))
                {
                    return ad;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (int.TryParse(obj.ToObjectString(), out ad))
                {
                    return ad;
                }
                else
                {
                    return 0;
                }
            }


        }
        [Pure]
        public static Guid ToGuid(this object obj)
        {
            Guid gid = Guid.Empty;
            Guid.TryParse(ToObjectString(obj), out gid);
            if (gid != Guid.Empty)
            {
                return gid;
            }
            return Guid.Empty;
        }
        [Pure]
        public static long ToLong(this object obj)
        {
            long ad;
            if (long.TryParse(obj.ToObjectString(), out ad))
            {
                return ad;
            }
            else
            {
                return -1L;
            }
        }
        [Pure]
        public static decimal ToDecimal(this object obj)
        {
            decimal ad;
            if (decimal.TryParse(obj.ToObjectString(), out ad))
            {
                return ad;
            }
            else
            {
                return -1M;
            }

        }
        [Pure]
        public static double ToDouble(this object obj)
        {
            double ad;
            if (double.TryParse(obj.ToObjectString(), out ad))
            {
                return ad;
            }
            else
            {
                return 0.0;
            }


        }
        [Pure]
        public static float ToFloat(this object obj)
        {
            float ad;
            if (float.TryParse(obj.ToObjectString(), out ad))
            {
                return ad;
            }
            else
            {
                return -1;
            }
        }
        [Pure]
        public static DateTime ToDateTime(this object obj)
        {
            try
            {
                DateTime dt = DateTime.Parse(ToObjectString(obj));
                if (dt > DateTime.MinValue && DateTime.MaxValue > dt)
                    return dt;
                return DateTime.Now;
            }
            catch
            { return DateTime.Now; }
        }

        [Pure]
        public static byte ToByteByBool(this object obj)
        {
            string text = ToObjectString(obj).Trim();
            if (text == string.Empty)
                return 0;
            else
            {
                try
                {
                    return (byte)(text.ToLower() == "true" ? 1 : 0);
                }
                catch
                {
                    return 0;
                }
            }
        }
        [Pure]
        public static string GetYYYYMMddDateTime(this DateTime obj)
        {
            return obj.ToString("yyyy-MM-dd HH:mm:ss");
        }

        #region 2016-9-23 22:42:30 增加控件关闭所在窗体
        /// <summary>
        ///  2016-9-23 增加: 控件关闭  所在窗体，调用窗体的Close方法
        /// <param name="element">控件</param>
        public static void CloseParentMyWindow(this UIElement element)
        {
            var a = Window.GetWindow(element) as Window;
            //if (a.IsNotNull()) a.WhenChildCloseTopmostWindow();
        }

        public static void CloseParentMyLayer(this UIElement element)
        {
            var a = Window.GetWindow(element) as MyLayer;
            if (a.IsNotNull()) a.CloseMyLayerTop();
        }

        public static void CloseParentWindow(this UIElement element)
        {
            var a = Window.GetWindow(element);
            if (a.IsNotNull()) a.Close();
        }
        #endregion
        /// <summary>
        ///  2016-8-11 10:00:49
        /// 字符串转 gridlength
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static GridLength ToGridLength(this string str)
        {
            if (str.IndexOf("*") == 0)
            {
                return new GridLength(1, GridUnitType.Star);
            }
            if (str.ToLower().Equals("auto"))
            {
                return GridLength.Auto;
            }
            if (str.IndexOf("*") > -1)
            {
                string _a1 = str.Substring(0, (str.Length - 1));
                return new GridLength(_a1.ToDouble(), GridUnitType.Star);
            }
            return new GridLength(str.ToDouble(), GridUnitType.Pixel);
        }

        public static FontWeight ToFontWeight(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return FontWeights.Normal;
            }
            else
            {
                switch (str)
                {
                    case "bold":
                        return FontWeights.Bold;
                    case "normal":
                        return FontWeights.Normal;
                    case "thin":
                        return FontWeights.Thin;
                    default:
                        return FontWeights.Normal;
                }
            }
        }

        public static Thickness ToThickness(this string str)
        {
            var _1 = str.Split(',');
            return new Thickness(_1[0].ToDouble(), _1[1].ToDouble(), _1[2].ToDouble(), _1[3].ToDouble());
        }

        [Pure]
        public static string GetYYYYMMddDateTime(this DateTime obj, string splitchar)
        {
            return obj.ToString(string.Format("yyyy{0}MM{0}dd HH:mm:ss", splitchar));
        }

        /// <summary>
        /// 1和"true"返回   true，否则返回false
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Pure]
        public static bool ToBoolByByte(this object obj)
        {
            try
            {
                string s = ToObjectString(obj).ToLower();
                return s == "1" || s == "true" ? true : false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 取得byte[]
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Pure]
        public static Byte[] GetByte(this object obj)
        {
            if (!string.IsNullOrEmpty(obj.ToObjectString()))
            {
                return (Byte[])obj;
            }
            else
                return null;
        }

        [Pure]
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        [Pure]
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }
        [Pure]
        public static bool IsEmptyAndNull(this string obj)
        {
            return string.IsNullOrEmpty(obj.ToObjectString());
        }
        [Pure]
        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }
        [Pure]
        public static bool IsNullAndTrimAndEmpty(this object obj)
        {
            if (obj.IsNull()) return true;
            return string.IsNullOrEmpty(obj.ToObjectString().Trim());
        }
        [Pure]
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return str == null || str.Trim().Length == 0;
        }

        [Pure]
        public static bool IsNotNullAndMinusOne(this string obj)
        {
            return obj != null && obj != "-1";
        }
        [Pure]
        public static bool IsNull(this Guid obj)
        {
            return obj == Guid.Empty;
        }
        [Pure]
        public static bool IsNotNull(this Guid obj)
        {
            return obj != Guid.Empty;
        }
        [Pure]
        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }

        ///// <summary>
        ///// 判断文本obj是否为空值。
        ///// </summary>
        ///// <param name="obj">对象。</param>
        ///// <returns>Boolean值。</returns>
        //public static bool IsEmpty(string obj)
        //{
        //    return ToObjectString(obj).Trim() == String.Empty ? true : false;
        //}

        ///// <summary>
        ///// 判断对象是否为正确的日期值。
        ///// </summary>
        ///// <param name="obj">对象。</param>
        ///// <returns>Boolean。</returns>
        //public static bool IsDateTime(object obj)
        //{
        //    try
        //    {
        //        DateTime dt = DateTime.Parse(ToObjectString(obj));
        //        if (dt > DateTime.MinValue && DateTime.MaxValue > dt)
        //            return true;
        //        return false;
        //    }
        //    catch
        //    { return false; }
        //}

        ///// <summary>
        ///// 判断对象是否为正确的Int32值。
        ///// </summary>
        ///// <param name="obj">对象。</param>
        ///// <returns>Int32值。</returns>
        //public static bool IsInt(object obj)
        //{
        //    try
        //    {
        //        int.Parse(ToObjectString(obj));
        //        return true;
        //    }
        //    catch
        //    { return false; }
        //}

        ///// <summary>
        ///// 判断对象是否为正确的Long值。
        ///// </summary>
        ///// <param name="obj">对象。</param>
        ///// <returns>Long值。</returns>
        //public static bool IsLong(object obj)
        //{
        //    try
        //    {
        //        long.Parse(ToObjectString(obj));
        //        return true;
        //    }
        //    catch
        //    { return false; }
        //}

        ///// <summary>
        ///// 判断对象是否为正确的Float值。
        ///// </summary>
        ///// <param name="obj">对象。</param>
        ///// <returns>Float值。</returns>
        //public static bool IsFloat(object obj)
        //{
        //    try
        //    {
        //        float.Parse(ToObjectString(obj));
        //        return true;
        //    }
        //    catch
        //    { return false; }
        //}

        ///// <summary>
        ///// 判断对象是否为正确的Double值。
        ///// </summary>
        ///// <param name="obj">对象。</param>
        ///// <returns>Double值。</returns>
        //public static bool IsDouble(object obj)
        //{
        //    try
        //    {
        //        double.Parse(ToObjectString(obj));
        //        return true;
        //    }
        //    catch
        //    { return false; }
        //}

        ///// <summary>
        ///// 判断对象是否为正确的Decimal值。
        ///// </summary>
        ///// <param name="obj">对象。</param>
        ///// <returns>Decimal值。</returns>
        //public static bool IsDecimal(object obj)
        //{
        //    try
        //    {
        //        decimal.Parse(ToObjectString(obj));
        //        return true;
        //    }
        //    catch
        //    { return false; }
        //}


        #region "全球唯一码GUID"
        /// <summary>
        /// 获取一个全球唯一码GUID字符串,默认全部转换为小写
        /// 时间:2016-6-20 00:17:40
        /// </summary>
        [Pure]
        public static string GetGuid
        {
            get
            {
                return Guid.NewGuid().ToString().ToLower();
            }
        }
        /// <summary>
        /// 创建一个GUID，没有"-"分隔符的
        /// 作者：
        /// 时间:2016-6-20 00:17:40
        /// </summary>
        [Pure]
        public static string GetGuidNoSplit
        {
            get
            {
                return Guid.NewGuid().ToString().Replace("-", "").ToLower();
            }
        }
        #endregion


        public static readonly ImageSourceConverter ImageSourceConverter = new ImageSourceConverter();
        private static readonly WeakReference s_random = new WeakReference(null);
        #region 自动生成日期编号
        /// <summary>
        /// 自动生成编号  201008251145409865
        /// </summary>
        /// <returns></returns>
        [Pure]
        public static string CreateNo()
        {
            string strRandom = Rnd.Next(1000, 10000).ToString(); //生成编号 
            string code = DateTime.Now.ToString("yyyyMMddHHmmss") + strRandom;//形如
            return code;
        }
        #endregion

        #region 生成0-9随机数
        /// <summary>
        /// 生成0-9随机数
        /// </summary>
        /// <param name="codeNum">生成长度</param>
        /// <returns></returns>
        [Pure]
        public static string RndNum(int codeNum)
        {
            StringBuilder sb = new StringBuilder(codeNum);
            for (int i = 1; i < codeNum + 1; i++)
            {
                int t = Rnd.Next(9);
                sb.AppendFormat("{0}", t);
            }
            return sb.ToString();

        }

        public static TResponse UseAndDispose<T, TResponse>(this T source, Func<T, TResponse> func) where T : IDisposable
        {
            using (source)
            {
                return func(source);
            }
        }

        public static string StringFormat(this string source, params object[] args)
        {
            Contract.Requires(source != null);
            Contract.Requires(args != null);
            return string.Format(source, args);
        }

#if SILVERLIGHT
        public static void VerifyAccess(this System.Windows.DependencyObject dependencyObj)
        {
            Contract.Requires(dependencyObj != null);
            if (!dependencyObj.CheckAccess())
            {
                throw new InvalidOperationException("A call was made off the Dispatcher thread.");
            }
        }
#endif

        public static bool NextBool(this Random rnd)
        {
            Contract.Requires(rnd != null);
            return rnd.Next() % 2 == 0;
        }

        public static float NextFloat(this Random rnd, float min = 0, float max = 1)
        {
            Contract.Requires(rnd != null);
            Contract.Requires(max >= min);
            var delta = max - min;
            return (float)rnd.NextDouble() * delta + min;
        }
        #endregion

        #region string转数组[1,2]转 '1','2'
        [Pure]
        public static string ToArrayString(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }
            else
            {
                return "'" + text.Replace(",", "','") + "'";
            }
        }
        #endregion

        #region <<时间扩展>>

        /// <summary>
        /// 例如2012-03-22 12:22:24 可以转换成20120322122224
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToDate2IntString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// 获得本周，上周，下周
        /// 2013年11月6日15:38:54 YANGQUAN写
        /// </summary>
        /// <param name="dts">时间</param>
        /// <param name="day">差天，-7就是上周，7就是下周</param>
        /// <returns></returns>
        public static DateTime[] GetWeekDate(this DateTime dts, int day = 0)
        {
            DateTime dt = dts.AddDays(day);
            DateTime startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));  //本周周一
            DateTime endWeek = startWeek.AddDays(6);  //本周周日
            return new DateTime[] { startWeek, endWeek };
        }

        /// <summary>
        /// 获取开始时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetBeginTime(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
        }
        /// <summary>
        /// 获取结束时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetEndTime(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
        }
        /// <summary>
        /// 获取开始时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime? GetBeginTime(this DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                return dateTime;
            }
            return new Nullable<DateTime>(dateTime.Value.GetBeginTime());
        }
        /// <summary>
        /// 获取结束时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime? GetEndTime(this DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                return dateTime;
            }
            return new Nullable<DateTime>(dateTime.Value.GetEndTime());
        }
        #endregion

        #region <<字符串长度验证扩展>>
        /// <summary>
        /// 判断此字符串是否超过指定长度
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool ValidateLength(this string s, int length)
        {
            if (string.IsNullOrEmpty(s))
            {
                return true;
            }
            return length >= s.Length;
        }
        #endregion


        public static bool ISXP = false;
        public static bool IsXP_OS()
        {
            OperatingSystem os = Environment.OSVersion;
            return (PlatformID.Win32NT == os.Platform && os.Version.Major == 5 && os.Version.Minor == 1);

        }

        /// <summary>
        /// 生成设置范围内的Double的随机数
        /// 例子:_random.AyNextDouble(1.5, 2.5)
        /// 作者：
        /// 添加时间：2016-06-19 21:28:24
        /// </summary>
        /// <param name="random">Random对象</param>
        /// <param name="miniDouble">生成随机数的最大值</param>
        /// <param name="maxiDouble">生成随机数的最小值</param>
        /// <returns>当Random等于NULL的时候返回0;</returns>
        public static double NextDouble(this Random random, double miniDouble, double maxiDouble)
        {
            if (random != null)
            {
                return random.NextDouble() * (maxiDouble - miniDouble) + miniDouble;
            }
            else
            {
                return 0.0d;
            }
        }



        /// <summary>
        /// 指定窗口句柄获得窗体位置
        /// </summary>
        /// <returns></returns>
        public static System.Drawing.Rectangle GetWindowRectHandleByWin(this Window win)
        {
            WindowInteropHelper wndHelper = new WindowInteropHelper(win);
            IntPtr hForeground = wndHelper.Handle;
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle();
            GetWindowRect(hForeground, ref rect);
            return rect;
        }
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetForegroundWindow")]
        public static extern IntPtr GetForegroundWindow();
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetWindowRect")]
        public static extern int GetWindowRect(IntPtr hwnd, ref System.Drawing.Rectangle lpRect);

        public static System.Drawing.Rectangle GetWindowRectangle(this Window w)
        {
            if (w.WindowState == WindowState.Maximized)
            {
                var handle = new System.Windows.Interop.WindowInteropHelper(w).Handle;
                var screen = System.Windows.Forms.Screen.FromHandle(handle);
                return screen.WorkingArea;
            }
            else
            {
                return new System.Drawing.Rectangle(
                    (int)w.Left, (int)w.Top,
                    (int)w.ActualWidth, (int)w.ActualHeight);
            }
        }

        /// <summary>
        /// 根据激活窗口句柄获得窗体位置
        /// </summary>
        /// <returns></returns>
        public static System.Drawing.Rectangle GetWindowRectHandle()
        {
            IntPtr hForeground = GetForegroundWindow();
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle();
            GetWindowRect(hForeground, ref rect);
            return rect;
        }






        #region 2016-8-4 12:48:41 增加  用于创建唯一的随机数种子
        public static Random Rnd
        {
            get
            {
                Contract.Ensures(Contract.Result<Random>() != null);
                var r = (Random)s_random.Target;
                if (r == null)
                {
                    s_random.Target = r = new Random();
                }
                return r;
            }
        }
        #endregion
    }
}
