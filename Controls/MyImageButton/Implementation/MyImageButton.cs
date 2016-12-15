using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyWPFUI.Controls
{
    /// <summary>
    /// 来源地址：https://github.com/y19890902q/MyWPFUI.git
    /// 最后编辑：yq  2016年12月4日
    /// </summary>
    public enum MyImageButtonMode
    {
        [Description("横向4图方式")]
        HorizonFour,
        [Description("纵向4图方式")]
        VerticalFour,
        [Description("内容模式")]
        ContentOpacity,
        [Description("自定义模式")]
        Manner
    }
    public class MyImageButton : Button
    {

        /// <summary>
        /// 显示方式
        /// </summary>
        [Description("显示方式")]
        public MyImageButtonMode RenderMode
        {
            get { return (MyImageButtonMode)GetValue(RenderModeProperty); }
            set { SetValue(RenderModeProperty, value); }
        }
        /// <summary>
        /// 显示方式
        /// </summary>
        [Description("显示方式")]
        public static readonly DependencyProperty RenderModeProperty =
            DependencyProperty.Register("RenderMode", typeof(MyImageButtonMode), typeof(MyImageButton), new PropertyMetadata(MyImageButtonMode.HorizonFour));

        /// <summary>
        /// 图片背景
        /// </summary>
        [Description("图片背景")]
        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        /// <summary>
        /// 图片背景
        /// </summary>
        [Description("图片背景")]
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(MyImageButton), new PropertyMetadata(null));


        /// <summary>
        /// 鼠标滑过时的图片背景
        /// </summary>
        [Description("鼠标滑过时的图片背景")]
        public ImageSource HoverIcon
        {
            get { return (ImageSource)GetValue(HoverIconProperty); }
            set { SetValue(HoverIconProperty, value); }
        }

        /// <summary>
        /// 鼠标滑过时的图片背景
        /// </summary>
        [Description("鼠标滑过时的图片背景")]
        public static readonly DependencyProperty HoverIconProperty =
            DependencyProperty.Register("HoverIcon", typeof(ImageSource), typeof(MyImageButton), new PropertyMetadata(null));


        /// <summary>
        /// 鼠标点击时的图片背景
        /// </summary>
        [Description("鼠标点击时的图片背景")]
        public ImageSource PressedIcon
        {
            get { return (ImageSource)GetValue(PressedIconProperty); }
            set { SetValue(PressedIconProperty, value); }
        }
        /// <summary>
        /// 鼠标点击时的图片背景
        /// </summary>
        [Description("鼠标点击时的图片背景")]
        public static readonly DependencyProperty PressedIconProperty =
            DependencyProperty.Register("PressedIcon", typeof(ImageSource), typeof(MyImageButton), new PropertyMetadata(null));


        /// <summary>
        /// 按钮不可用时的图片背景
        /// </summary>
        [Description("按钮不可用时的图片背景")]

        public ImageSource DisabledIcon
        {
            get { return (ImageSource)GetValue(DisabledIconProperty); }
            set { SetValue(DisabledIconProperty, value); }
        }
        /// <summary>
        /// 按钮不可用时的图片背景
        /// </summary>
        [Description("按钮不可用时的图片背景")]
        public static readonly DependencyProperty DisabledIconProperty =
            DependencyProperty.Register("DisabledIcon", typeof(ImageSource), typeof(MyImageButton), new PropertyMetadata(null));



    }
}
