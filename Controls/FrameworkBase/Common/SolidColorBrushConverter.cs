using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace MyWPFUI.Controls
{
    /// <summary>
    /// 用户画刷辅助类
    /// </summary>
    public class SolidColorBrushConverter
    {

        public static System.Windows.Media.Brush From16JinZhi(string color)
        {
            BrushConverter converter = new BrushConverter();
            return (System.Windows.Media.Brush)converter.ConvertFromString(color);
        }


        public static System.Windows.Media.Color ToColor(string colorName)
        {
            if (colorName.StartsWith("#"))
                colorName = colorName.Replace("#", string.Empty);
            int v = int.Parse(colorName, System.Globalization.NumberStyles.HexNumber);
            return new System.Windows.Media.Color()
            {
                A = Convert.ToByte((v >> 24) & 255),
                R = Convert.ToByte((v >> 16) & 255),
                G = Convert.ToByte((v >> 8) & 255),
                B = Convert.ToByte((v >> 0) & 255)
            };
        }
    }
}
