using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MyWPFUI.Extensiones
{
    public static class EnumExtension
    {
        public static List<T> EnumToList<T>(this Enum tEnum)
        {
            return new List<T>((T[])Enum.GetValues(typeof(T)));
        }
        public static Dictionary<T, string> EnumToDictionaryWithDescription<T>(this Enum tEnum)
        {
            Dictionary<T, string> dic = new Dictionary<T, string>();
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                dic.Add(value, "");
                object[] objAttrs = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objAttrs.Length > 0)
                {
                    DescriptionAttribute descAttr = objAttrs[0] as DescriptionAttribute;
                    if (descAttr != null)
                        dic[value] = descAttr.Description;
                }
            }
            return dic;
        }
        public static Dictionary<T, int> EnumToDictionaryWithValue<T>(this Enum tEnum)
        {
            Dictionary<T, int> dic = new Dictionary<T, int>();
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                dic.Add(value, Convert.ToInt32(value));
            }
            return dic;
        }
        public static Dictionary<T, Dictionary<int, string>> EnumToDictionaryWithDesValue<T>(this Enum tEnum)
        {
            Dictionary<T, Dictionary<int, string>> dic = new Dictionary<T, Dictionary<int, string>>();
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                Dictionary<int, string> childdic = new Dictionary<int, string>();
                dic.Add(value, childdic);
                var childkey = Convert.ToInt32(value);
                childdic.Add(childkey, "");
                object[] objAttrs = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objAttrs.Length > 0)
                {
                    DescriptionAttribute descAttr = objAttrs[0] as DescriptionAttribute;
                    if (descAttr != null)
                        childdic[childkey] = descAttr.Description;
                }
            }
            return dic;
        }
    }
}
