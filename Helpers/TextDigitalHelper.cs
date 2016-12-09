using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyWPFUI.Controls
{
    public static class TextDigitalHelper
    {


        public static readonly DependencyProperty InputTextTypeProperty = DependencyProperty.RegisterAttached(
            "InputTextType", typeof(TextInputType), typeof(TextDigitalHelper),
            new PropertyMetadata(default(TextInputType), new PropertyChangedCallback(InputTextTypeChangedCallBack)));

        private static void InputTextTypeChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var host = d as TextBox;
            if (host != null)
            {
                var property = (TextInputType)host.GetValue(InputTextTypeProperty);
                switch (property)
                {
                    case TextInputType.Default:
                        break;
                    case TextInputType.InputMethodEnabled:
                        InputMethod.SetIsInputMethodEnabled(host, false);//关闭输入法
                        InputMethod.SetPreferredImeState(host, InputMethodState.Off);//关闭输入法
                        DataObject.AddPastingHandler(host, TestPastSettingMaskChinese);
                        break;
                    case TextInputType.Digital:
                        InputMethod.SetIsInputMethodEnabled(host, false);
                        InputMethod.SetPreferredImeState(host, InputMethodState.Off);
                        DataObject.AddPastingHandler(host, TestPastSetting);
                        host.PreviewKeyDown += host_DigitalPreviewKeyDown;
                        host.PreviewTextInput += host_PreviewTextInput;
                        break;
                    case TextInputType.DigitalWithMinus:
                        InputMethod.SetIsInputMethodEnabled(host, false);
                        InputMethod.SetPreferredImeState(host, InputMethodState.Off);
                        DataObject.AddPastingHandler(host, TestPastSetting);
                        host.PreviewKeyDown += host_DigitalWithMinusPreviewKeyDown;
                        host.PreviewTextInput += host_DigitalWithMinusPreviewTextInput;
                        break;
                    case TextInputType.DigitalWithOemPeriod:
                        InputMethod.SetIsInputMethodEnabled(host, false);
                        InputMethod.SetPreferredImeState(host, InputMethodState.Off);
                        DataObject.AddPastingHandler(host, TestPastSetting);
                        host.PreviewKeyDown += host_PreviewKeyDown;
                        host.PreviewTextInput += host_PreviewTextInputWithOemPeriod;
                        break;
                    case TextInputType.DigitalWithMinusAndOemPeriod:
                        InputMethod.SetIsInputMethodEnabled(host, false);
                        InputMethod.SetPreferredImeState(host, InputMethodState.Off);
                        DataObject.AddPastingHandler(host, TestPastSetting);
                        host.PreviewKeyDown += host_DigitalWithMinusAndOemPeriod;
                        host.PreviewTextInput += host_DigitalWithMinusAndOemPeriodPreviewTextInput;
                        break;
                    case TextInputType.Letter:
                        InputMethod.SetIsInputMethodEnabled(host, false);
                        InputMethod.SetPreferredImeState(host, InputMethodState.Off);
                        DataObject.AddPastingHandler(host, TestPastSettingOnlyEn);
                        host.PreviewTextInput += host_PreviewTextInputOnlyEnglish;
                        host.PreviewKeyDown += host_PreviewKeyDownOnlyEn;
                        break;

                    default:
                        break;
                }
            }
        }

        private static void host_DigitalPreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private static void TestPastSettingOnlyEn(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var str = (string)e.DataObject.GetData(typeof(string));
                int val;
                if (!IsAllEnglishCharacters(str))
                {
                    e.CancelCommand();
                }
            }
        }
        private static void TestPastSettingMaskChinese(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var str = (string)e.DataObject.GetData(typeof(string));
                int val;
                if (IsChineseCharacters(str))
                {
                    e.CancelCommand();
                }
            }
        }

        private static void TestPastSetting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var str = (string)e.DataObject.GetData(typeof(string));
                int val;
                if (!IsNumberic(str))
                {
                    e.CancelCommand();
                }
            }
        }

        static void host_PreviewTextInputWithOemPeriod(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (e.Text.Contains("."))
            {
                e.Handled = false;
            }
            else
            {
                var text = sender as TextBox;
                if (text != null && !string.IsNullOrEmpty(text.Text))
                {
                    //var count = (long)text.GetValue(TextDigitalHelper.CountProperty);
                    //e.Handled = (text.Text.Length - text.SelectionLength) + 1 > count;
                    //if (e.Handled == true) return;
                    if (text.Text.Contains("."))
                    {
                        var number = (long)text.GetValue(TextDigitalHelper.UnitsProperty);
                        //var number = 
                        var periodLength = text.Text.Substring(text.Text.IndexOf(".") + 1).Length;
                        if (periodLength >= number && text.CaretIndex >= (text.Text.IndexOf(".") + 1) && text.SelectionLength == 0)
                        {
                            e.Handled = true;
                            return;
                        }
                    }

                }
                e.Handled = !IsNumberic(e.Text);
            }
        }
        /// <summary>
        /// 带符号的输入数字PreviewTextInput事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void host_DigitalWithMinusPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (e.Text.Contains("-"))
            {
                e.Handled = false;
            }
            else
            {
                if (!IsNumberic(e.Text))
                {
                    e.Handled = true;
                    return;
                }
                var text = sender as TextBox;
                if (text != null && !string.IsNullOrEmpty(text.Text))
                {
                    var count = (long)text.GetValue(TextDigitalHelper.CountProperty);
                    if (text.Text.Contains("-"))
                        e.Handled = (text.Text.Length - text.SelectionLength) > count;
                    else
                        e.Handled = (text.Text.Length - text.SelectionLength) + 1 > count;

                }
            }
        }

        /// <summary>
        /// 带符号的包含小数点的数字PreviewTextInput事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void host_DigitalWithMinusAndOemPeriodPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (e.Text.Contains("-") || e.Text.Contains("."))
            {
                e.Handled = false;
            }
            else
            {
                var text = sender as TextBox;
                if (text != null && !string.IsNullOrEmpty(text.Text))
                {
                    if (text.Text.Contains("."))
                    {
                        var number = (long)text.GetValue(TextDigitalHelper.UnitsProperty);
                        var periodLength = text.Text.Substring(text.Text.IndexOf(".") + 1).Length;
                        if (periodLength >= number && text.CaretIndex >= (text.Text.IndexOf(".") + 1) && text.SelectionLength == 0)
                        {
                            e.Handled = true;
                            return;
                        }
                    }

                }
                e.Handled = !IsNumberic(e.Text);
            }
        }

        private static void host_DigitalWithMinusAndOemPeriod(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Subtract || e.Key == Key.Decimal)
            {
                if (txt != null && (txt.Text.Length > 0 && e.Key == Key.Subtract && txt.SelectionLength == 0 && txt.SelectionStart != 0))
                {
                    e.Handled = true;
                    return;
                }
                if (txt != null && (txt.Text.Contains("-") && e.Key == Key.Subtract && txt.SelectionLength == 0))
                {
                    e.Handled = true;
                    return;
                }

                if (txt != null && (txt.SelectionLength > 0 && txt.SelectionLength < txt.Text.Length && e.Key == Key.Subtract))
                {
                    if (txt.SelectionStart != 0)
                    {
                        e.Handled = true;
                        return;
                    }
                }

                if (txt != null && (txt.Text.Length == 0 && e.Key == Key.Decimal))
                {
                    e.Handled = true;
                    return;
                }

                if (txt != null && (txt.Text.Contains(".") && e.Key == Key.Decimal && txt.SelectionLength == 0))
                {
                    e.Handled = true;
                    return;
                }

                if (txt != null && txt.SelectionLength > 0 && txt.SelectionLength <= txt.Text.Length && e.Key == Key.Decimal)
                {
                    if (txt.SelectionLength == txt.Text.Length)
                    {
                        e.Handled = true;
                        return;
                    }
                    var firstStr = txt.Text.Substring(0, txt.SelectionStart);
                    if (firstStr.Contains(".") || firstStr.Contains("-"))
                    {
                        e.Handled = true;
                        return;
                    }
                }

                e.Handled = false;
            }
            else if (((e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.OemMinus || e.Key == Key.OemPeriod) &&
                     e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
            {
                if (txt != null && (txt.Text.Length > 0 && e.Key == Key.OemMinus && txt.SelectionLength == 0 && txt.SelectionStart != 0))
                {
                    e.Handled = true;
                    return;
                }
                if (txt != null && (txt.Text.Contains("-") && e.Key == Key.OemMinus && txt.SelectionLength == 0))
                {
                    e.Handled = true;
                    return;
                }

                if (txt != null && (txt.SelectionLength > 0 && txt.SelectionLength < txt.Text.Length && e.Key == Key.OemMinus))
                {
                    if (txt.SelectionStart != 0)
                    {
                        e.Handled = true;
                        return;
                    }
                }
                if (txt != null && (txt.Text.Length == 0 && e.Key == Key.OemPeriod))
                {
                    e.Handled = true;
                    return;
                }
                if (txt != null && (txt.Text.Contains(".") && e.Key == Key.OemPeriod && txt.SelectionLength == 0))
                {
                    e.Handled = true;
                    return;
                }
                if (txt != null && txt.SelectionLength > 0 && txt.SelectionLength <= txt.Text.Length && e.Key == Key.OemPeriod)
                {
                    if (txt.SelectionLength == txt.Text.Length)
                    {
                        e.Handled = true;
                        return;
                    }
                    var firstStr = txt.Text.Substring(0, txt.SelectionStart);
                    if (firstStr.Contains(".") || firstStr.Contains("-"))
                    {
                        e.Handled = true;
                        return;
                    }
                }
                e.Handled = false;
            }
            else if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            //KeyConverter keyConverter = new KeyConverter();
            //var str = keyConverter.ConvertToString(e.Key);
            //e.Handled = !IsNumberic(str);
        }

        /// <summary>
        /// 有符号的数字PreviewKeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void host_DigitalWithMinusPreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Subtract)
            {
                if (txt != null && (txt.Text.Length > 0 && e.Key == Key.Subtract && txt.SelectionLength == 0 && txt.SelectionStart != 0))
                {
                    e.Handled = true;
                    return;
                }
                if (txt != null && (txt.Text.Contains("-") && e.Key == Key.Subtract && txt.SelectionLength == 0))
                {
                    e.Handled = true;
                    return;
                }

                if (txt != null && (txt.SelectionLength > 0 && txt.SelectionLength < txt.Text.Length && e.Key == Key.Subtract))
                {
                    if (txt.SelectionStart != 0)
                    {
                        e.Handled = true;
                        return;
                    }
                }
                e.Handled = false;
            }
            else if (((e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.OemMinus) &&
                     e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
            {
                if (txt != null && (txt.Text.Length > 0 && e.Key == Key.OemMinus && txt.SelectionLength == 0 && txt.SelectionStart != 0))
                {
                    e.Handled = true;
                    return;
                }
                if (txt != null && (txt.Text.Contains("-") && e.Key == Key.OemMinus && txt.SelectionLength == 0))
                {
                    e.Handled = true;
                    return;
                }

                if (txt != null && (txt.SelectionLength > 0 && txt.SelectionLength < txt.Text.Length && e.Key == Key.OemMinus))
                {
                    if (txt.SelectionStart != 0)
                    {
                        e.Handled = true;
                        return;
                    }
                }
                e.Handled = false;
            }
            else if (e.Key == Key.Space)
            {
                e.Handled = true;
            }


            //KeyConverter keyConverter = new KeyConverter();
            //var str = keyConverter.ConvertToString(e.Key);
            //e.Handled = !IsNumberic(str);
        }
        static void host_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!IsNumberic(e.Text))
            {
                e.Handled = true;
                return;
            }
            var text = sender as TextBox;
            if (text != null && !string.IsNullOrEmpty(text.Text))
            {
                var count = (long)text.GetValue(TextDigitalHelper.CountProperty);
                e.Handled = (text.Text.Length - text.SelectionLength) + 1 > count;
            }
        }
        static void host_PreviewTextInputOnlyEnglish(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsAllEnglishCharacters(e.Text);
        }

        /// <summary>
        /// 待小数点输入的数字PreviewKeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void host_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Decimal)
            {
                if (txt != null && (txt.Text.Length == 0 && e.Key == Key.Decimal))
                {
                    e.Handled = true;
                    return;
                }

                if (txt != null && (txt.Text.Contains(".") && e.Key == Key.Decimal && txt.SelectionLength == 0))
                {
                    e.Handled = true;
                    return;
                }

                if (txt != null && txt.SelectionLength > 0 && txt.SelectionLength <= txt.Text.Length && e.Key == Key.Decimal)
                {
                    if (txt.SelectionLength == txt.Text.Length)
                    {
                        e.Handled = true;
                        return;
                    }
                    if (txt.Text.Substring(0, txt.SelectionStart).Contains("."))
                    {
                        e.Handled = true;
                        return;
                    }
                }
                e.Handled = false;
            }
            else if (((e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.OemPeriod) &&
                     e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
            {
                if (txt != null && (txt.Text.Length == 0 && e.Key == Key.OemPeriod))
                {
                    e.Handled = true;
                    return;
                }
                if (txt != null && (txt.Text.Contains(".") && e.Key == Key.OemPeriod && txt.SelectionLength == 0))
                {
                    e.Handled = true;
                    return;
                }
                if (txt != null && txt.SelectionLength > 0 && txt.SelectionLength <= txt.Text.Length && e.Key == Key.OemPeriod)
                {
                    if (txt.SelectionLength == txt.Text.Length)
                    {
                        e.Handled = true;
                        return;
                    }
                    if (txt.Text.Substring(0, txt.SelectionStart).Contains("."))
                    {
                        e.Handled = true;
                        return;
                    }
                }
                e.Handled = false;
            }
            else if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            //KeyConverter keyConverter = new KeyConverter();
            //var str = keyConverter.ConvertToString(e.Key);
            //e.Handled = !IsNumberic(str);
        }
        private static void host_PreviewKeyDownOnlyEn(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private static bool IsNumberic(string str)
        {
            if (string.IsNullOrEmpty(str)) return false;
            foreach (var c in str)
            {
                if (!char.IsDigit(c))
                    return false;
                //if (c < '0' || c > '9') return false;
            }
            return true;
        }

        /// <summary>
        /// 用ASCII码判断中文字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static bool IsChineseChara(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] > 127)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 用汉字的 UNICODE 编码范围判断中文字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static bool IsChineseCharac(string str)
        {
            char[] c = str.ToCharArray();

            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] >= 0x4e00 && c[i] <= 0x9fbb)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 用正则表达式判断中文字符
        ///全部是中文的表达式： "^[\u4e00-\u9fa5]+$"
        /// 包含中文字符串的表达式："[\u4e00-\u9fa5]"
        /// 中文字符开头的表达式："^[\u4e00-\u9fa5]"
        /// 中文字符结尾："[\u4e00-\u9fa5]$"
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static bool IsChineseCharacters(string str)
        {
            return Regex.IsMatch(str.ToString(), @"[\u4e00-\u9fa5]");
        }

        /// <summary>
        /// 正则判断是否是英文字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static bool IsAllEnglishCharacters(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (Regex.IsMatch(str[i].ToString(), @"^[A-Za-z]*$"))
                {

                }
                else
                {
                    return false;
                }
            }
            return true;
        }



        public static TextInputType GetInputTextType(DependencyObject d)
        {
            return (TextInputType)d.GetValue(InputTextTypeProperty);
        }

        public static void SetInputTextType(DependencyObject d, TextInputType value)
        {
            d.SetValue(InputTextTypeProperty, value);
        }

        public static long GetUnits(DependencyObject d)
        {
            return (long)d.GetValue(UnitsProperty);
        }

        public static void SetUnits(DependencyObject d, long value)
        {
            d.SetValue(UnitsProperty, value);
        }
        public static readonly DependencyProperty UnitsProperty = DependencyProperty.RegisterAttached(
       "Units", typeof(long), typeof(TextDigitalHelper),
       new PropertyMetadata(default(long), new PropertyChangedCallback(UnitsChangedCallBack)));


        public static long GetCount(DependencyObject d)
        {
            return (long)d.GetValue(CountProperty);
        }

        public static void SetCount(DependencyObject d, long value)
        {
            d.SetValue(CountProperty, value);
        }
        public static readonly DependencyProperty CountProperty = DependencyProperty.RegisterAttached(
       "Count", typeof(long), typeof(TextDigitalHelper),
       new PropertyMetadata(default(long), new PropertyChangedCallback(CountChangedCallBack)));

        private static void CountChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //long i;
            //var result = long.TryParse(e.NewValue.ToString(), out i);
            //if (result)
            //    Count = long.Parse(e.NewValue.ToString());
        }

        //private static int Number = 2;
        //private static long Count = 999999999999999999;
        private static void UnitsChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //int i;
            //var result = int.TryParse(e.NewValue.ToString(), out i);
            //if (result)
            //    Number = int.Parse(e.NewValue.ToString());
        }
    }

    public enum TextInputType
    {
        /// <summary>
        /// 都可以输入
        /// </summary>
        [Description("都可以输入")]
        Default,
        /// <summary>
        /// 关闭输入法
        /// </summary>
        [Description("关闭输入法")]
        InputMethodEnabled,
        /// <summary>
        /// 只能输入字母
        /// </summary>
        [Description("只能输入字母")]
        Letter,
        /// <summary>
        /// 只能输入数字
        /// </summary>
        [Description("只能输入数字")]
        Digital,
        /// <summary>
        /// 只能输入数字,和小数
        /// </summary>
        [Description("只能输入数字,和小数")]
        DigitalWithOemPeriod,
        /// <summary>
        /// 有符号的数字
        /// </summary>
        [Description("有符号的数字")]
        DigitalWithMinus,
        /// <summary>
        /// 有符号的数字，同时支持小数点
        /// </summary>
        [Description("有符号的数字，同时支持小数点")]
        DigitalWithMinusAndOemPeriod
    }
}
