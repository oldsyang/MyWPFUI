using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace MyWPFUI.Controls
{
    /// <summary>
    /// 来源地址：https://github.com/y19890902q/MyWPFUI.git
    /// 最后编辑：yq  2016年12月4日
    /// </summary>
    public class MyTextBox : TextBox
    {
        static MyTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyTextBox), new FrameworkPropertyMetadata(typeof(MyTextBox)));
            TextProperty.OverrideMetadata(
          typeof(MyTextBox),
          new FrameworkPropertyMetadata(new PropertyChangedCallback(TextPropertyChanged)));
        }
        private static void TextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var itb = sender as MyTextBox;
            bool actuallyHasText = itb != null && itb.Text.Length > 0;
            if (itb != null && actuallyHasText != itb.HasText)
            {
                itb.SetValue(HasTextPropertyKey, actuallyHasText);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //_tootipPopup = GetTemplateChild("Part_ToopTipPopup") as Popup;
            //rightcp = GetTemplateChild("rightcp") as ContentPresenter;
        }
        #region 常用属性和附加属性
        public static readonly DependencyProperty IsErrorProperty = DependencyProperty.Register(
      "IsError", typeof(bool), typeof(MyTextBox), new PropertyMetadata(default(bool)));

        public bool IsError
        {
            get { return (bool)GetValue(IsErrorProperty); }
            set { SetValue(IsErrorProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof(CornerRadius), typeof(MyTextBox), new PropertyMetadata(default(CornerRadius)));

        /// <summary>
        /// 圆角设置
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public static void SetCornerRadius(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }

        public static object GetCornerRadius(DependencyObject obj)
        {
            return (VerticalAlignment)obj.GetValue(CornerRadiusProperty);
        }
        private static readonly DependencyPropertyKey HasTextPropertyKey = DependencyProperty.RegisterReadOnly(
"HasText", typeof(bool), typeof(MyTextBox), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty HasTextProperty = HasTextPropertyKey.DependencyProperty;

        public bool HasText
        {
            get { return (bool)GetValue(HasTextProperty); }
            //set { SetValue(HasTextProperty, value); }
        }
        public static void SetWidth(DependencyObject obj, object value)
        {
            obj.SetValue(WidthProperty, value);
        }

        public static object GetWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(WidthProperty);
        }
        public static void SetHeight(DependencyObject obj, object value)
        {
            obj.SetValue(HeightProperty, value);
        }
        public static object GetHeight(DependencyObject obj)
        {
            return (double)obj.GetValue(HeightProperty);
        }
        #endregion

        #region 左侧和右侧内容
        public static readonly DependencyProperty RightContentProperty = DependencyProperty.Register(
             "RightContent", typeof(object), typeof(MyTextBox), new PropertyMetadata(default(object)));

        /// <summary>
        /// 右侧信息
        /// </summary>
        public object RightContent
        {
            get { return (object)GetValue(RightContentProperty); }
            set { SetValue(RightContentProperty, value); }
        }

        public static readonly DependencyProperty LeftContentProperty = DependencyProperty.Register(
            "LeftContent", typeof(object), typeof(MyTextBox), new PropertyMetadata(default(object)));

        /// <summary>
        /// 左侧信息
        /// </summary>
        public object LeftContent
        {
            get { return (object)GetValue(LeftContentProperty); }
            set { SetValue(LeftContentProperty, value); }
        }
        #endregion

        #region 控制输入类型
        public static readonly DependencyProperty TextInputTypeProperty = DependencyProperty.Register(
          "TextInputType", typeof(TextInputType), typeof(MyTextBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(TextInputCallBack))
          {
              DefaultValue = default(TextInputType),
              BindsTwoWayByDefault = true,
              DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
          });

        private static void TextInputCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var text = d as MyTextBox;
            if (text != null)
            {
                if (e.OldValue != null && !e.OldValue.Equals(e.NewValue))
                {
                    var newValue = (TextInputType)e.NewValue;
                    d.SetValue(TextDigitalHelper.InputTextTypeProperty, newValue);
                }
            }
        }
        /// <summary>
        /// 控制输入类型
        /// </summary>
        [Description("控制输入类型")]
        public TextInputType TextInputType
        {
            get { return (TextInputType)GetValue(TextInputTypeProperty); }
            set { SetValue(TextInputTypeProperty, value); }
        }

        public static readonly DependencyProperty UnitsProperty = DependencyProperty.Register(
    "Units", typeof(long), typeof(MyTextBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(UnitsCallBack))
    {
        DefaultValue = default(long),
        BindsTwoWayByDefault = true,
        DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
    });

        private static void UnitsCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var text = d as MyTextBox;
            if (text != null)
            {
                if (e.OldValue != null && !e.OldValue.Equals(e.NewValue))
                {
                    var newValue = (long)e.NewValue;
                    d.SetValue(TextDigitalHelper.UnitsProperty, newValue);
                }
            }
        }

        [Description("小数位数")]
        public long Units
        {
            get { return (long)GetValue(UnitsProperty); }
            set { SetValue(UnitsProperty, value); }
        }


        public static readonly DependencyProperty IntCountProperty = DependencyProperty.Register(
"IntCount", typeof(long), typeof(MyTextBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(IntCountCallBack))
{
    DefaultValue = default(long),
    BindsTwoWayByDefault = true,
    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
});

        private static void IntCountCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var text = d as MyTextBox;
            if (text != null)
            {
                if (e.OldValue != null && !e.OldValue.Equals(e.NewValue))
                {
                    var newValue = (long)e.NewValue;
                    d.SetValue(TextDigitalHelper.CountProperty, newValue);
                }
            }
        }

        [Description("输入纯数字的个数")]
        public long IntCount
        {
            get { return (long)GetValue(IntCountProperty); }
            set { SetValue(IntCountProperty, value); }
        }
        #endregion

        #region 提示文字设置
        public static readonly DependencyProperty MaskProperty = DependencyProperty.Register(
          "Mask", typeof(object), typeof(MyTextBox), new PropertyMetadata(default(object)));
        /// <summary>
        /// 提示阴影
        /// </summary>
        public object Mask
        {
            get { return (object)GetValue(MaskProperty); }
            set { SetValue(MaskProperty, value); }
        }

        public static void SetMask(DependencyObject obj, object value)
        {
            obj.SetValue(MaskProperty, value);
        }

        public static object GetMask(DependencyObject obj)
        {
            return (object)obj.GetValue(MaskProperty);
        }

        public static void SetMaskForeground(DependencyObject obj, object value)
        {
            obj.SetValue(MaskProperty, value);
        }

        public static object GetMaskForeground(DependencyObject obj)
        {
            return (object)obj.GetValue(MaskProperty);
        }
        public static readonly DependencyProperty MaskForegroundProperty = DependencyProperty.Register(
        "MaskForeground", typeof(Brush), typeof(MyTextBox), new PropertyMetadata(default(Brush)));
        /// <summary>
        /// 提示阴影字体颜色
        /// </summary>
        public Brush MaskForeground
        {
            get { return (Brush)GetValue(MaskForegroundProperty); }
            set { SetValue(MaskForegroundProperty, value); }
        }

        public static readonly DependencyProperty MaskVerticalAlignmentProperty = DependencyProperty.Register(
            "MaskVerticalAlignment", typeof(VerticalAlignment), typeof(MyTextBox), new PropertyMetadata(default(VerticalAlignment)));

        /// <summary>
        /// 提示内容垂直设置
        /// </summary>
        public VerticalAlignment MaskVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(MaskVerticalAlignmentProperty); }
            set { SetValue(MaskVerticalAlignmentProperty, value); }
        }

        public static void SetMaskVerticalAlignment(DependencyObject obj, VerticalAlignment value)
        {
            obj.SetValue(MaskVerticalAlignmentProperty, value);
        }

        public static object GetMaskVerticalAlignment(DependencyObject obj)
        {
            return (VerticalAlignment)obj.GetValue(MaskVerticalAlignmentProperty);
        }

        public static readonly DependencyProperty MaskHorizontalAlignmentProperty = DependencyProperty.Register(
            "MaskHorizontalAlignment", typeof(HorizontalAlignment), typeof(MyTextBox), new PropertyMetadata(default(HorizontalAlignment)));

        /// <summary>
        /// 提示内容水平设置
        /// </summary>
        public HorizontalAlignment MaskHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(MaskHorizontalAlignmentProperty); }
            set { SetValue(MaskHorizontalAlignmentProperty, value); }
        }

        public static void SetMaskHorizontalAlignment(DependencyObject obj, HorizontalAlignment value)
        {
            obj.SetValue(MaskHorizontalAlignmentProperty, value);
        }

        public static object GetMaskHorizontalAlignment(DependencyObject obj)
        {
            return (HorizontalAlignment)obj.GetValue(MaskHorizontalAlignmentProperty);
        }

        public static readonly DependencyProperty MaskFontSizeProperty = DependencyProperty.Register(
            "MaskFontSize", typeof(double), typeof(MyTextBox), new PropertyMetadata(default(double)));

        /// <summary>
        /// 提示内容大小
        /// </summary>
        public double MaskFontSize
        {
            get { return (double)GetValue(MaskFontSizeProperty); }
            set { SetValue(MaskFontSizeProperty, value); }
        }
        public static readonly DependencyProperty MaskOpacityProperty = DependencyProperty.Register(
    "MaskOpacity", typeof(double), typeof(MyTextBox), new PropertyMetadata(default(double)));

        public double MaskOpacity
        {
            get { return (double)GetValue(MaskOpacityProperty); }
            set { SetValue(MaskOpacityProperty, value); }
        }

        #endregion

        #region 搜索模式(项目扩展)
        public static readonly DependencyProperty IsSearchStyleProperty = DependencyProperty.Register(
          "IsSearchStyle", typeof(bool), typeof(MyTextBox), new PropertyMetadata(default(bool), new PropertyChangedCallback(Ceshi)));

        private static void Ceshi(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        public event RoutedEventHandler SearchClick
        {
            add { AddHandler(SearchClickEvent, value); }
            remove { RemoveHandler(SearchClickEvent, value); }
        }
        public static readonly RoutedEvent SearchClickEvent = EventManager.RegisterRoutedEvent("SearchClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MyTextBox));

        public bool IsSearchStyle
        {
            get { return (bool)GetValue(IsSearchStyleProperty); }
            set { SetValue(IsSearchStyleProperty, value); }
        }

        public static readonly DependencyProperty SearchCommandProperty = DependencyProperty.Register(
            "SearchCommand", typeof(ICommand), typeof(MyTextBox), new PropertyMetadata(default(ICommand)));

        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        public static readonly DependencyProperty KeyEnterCommandProperty = DependencyProperty.Register(
            "KeyEnterCommand", typeof(ICommand), typeof(MyTextBox), new PropertyMetadata(default(ICommand)));

        public ICommand KeyEnterCommand
        {
            get { return (ICommand)GetValue(KeyEnterCommandProperty); }
            set { SetValue(KeyEnterCommandProperty, value); }
        }
        #endregion

        #region 设置获取焦点时，全选中内容
        public static readonly DependencyProperty IsFocusSelectAllProperty = DependencyProperty.Register(
           "IsFocusSelectAll", typeof(bool), typeof(MyTextBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(IsFocusSelectAllCallBack))
           {
               DefaultValue = default(bool),
               BindsTwoWayByDefault = true,
               DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
           });

        private static void IsFocusSelectAllCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var text = d as TextBox;
            if (text != null && (bool)e.NewValue)
            {
                text.GotFocus -= EventSetter_GotFocus;
                text.PreviewMouseDown -= EventSetter_PreviewMouseDown;
                text.LostFocus -= EventSetter_LostFocus;
                text.GotFocus += EventSetter_GotFocus;
                text.PreviewMouseDown += EventSetter_PreviewMouseDown;
                text.LostFocus += EventSetter_LostFocus;
            }
        }
        private static void EventSetter_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var text = sender as TextBox;
            text.Focus();
            e.Handled = true;
        }

        private static void EventSetter_GotFocus(object sender, RoutedEventArgs e)
        {
            var text = sender as TextBox;
            text.SelectAll();
            text.PreviewMouseDown -= new MouseButtonEventHandler(EventSetter_PreviewMouseDown);
        }

        private static void EventSetter_LostFocus(object sender, RoutedEventArgs e)
        {
            var text = sender as TextBox;
            text.PreviewMouseDown += new MouseButtonEventHandler(EventSetter_PreviewMouseDown);
        }

        public static void SetIsFocusSelectAll(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFocusSelectAllProperty, value);
        }

        public static bool GetIsFocusSelectAll(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFocusSelectAllProperty);
        }
        #endregion

        #region 其他辅助
        public static readonly DependencyProperty PassEventSenderToCommandProperty = DependencyProperty.Register("PassEventSenderToCommand", typeof(bool), typeof(MyTextBox), new PropertyMetadata(false));
        public static readonly DependencyProperty PassEventArgsToCommandProperty = DependencyProperty.Register("PassEventArgsToCommand", typeof(bool), typeof(MyTextBox), new PropertyMetadata(false));
        public static readonly DependencyProperty CommandParameter1Property = DependencyProperty.Register("CommandParameter1", typeof(object), typeof(MyTextBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, CommandParameter1ChangedCallback));
        public static readonly DependencyProperty CommandParameter2Property = DependencyProperty.Register("CommandParameter2", typeof(object), typeof(MyTextBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, CommandParameter2ChangedCallback));
        public static readonly DependencyProperty CommandParameter3Property = DependencyProperty.Register("CommandParameter3", typeof(object), typeof(MyTextBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, CommandParameter3ChangedCallback));

        public bool PassEventSenderToCommand
        {
            get { return (bool)GetValue(PassEventSenderToCommandProperty); }
            set { SetValue(PassEventSenderToCommandProperty, value); }
        }
        public bool PassEventArgsToCommand
        {
            get { return (bool)GetValue(PassEventArgsToCommandProperty); }
            set { SetValue(PassEventArgsToCommandProperty, value); }
        }

        public object CommandParameter1
        {
            get { return GetValue(CommandParameter1Property); }
            set { SetValue(CommandParameter1Property, value); }
        }
        public object CommandParameter2
        {
            get { return GetValue(CommandParameter2Property); }
            set { SetValue(CommandParameter2Property, value); }
        }
        public object CommandParameter3
        {
            get { return GetValue(CommandParameter3Property); }
            set { SetValue(CommandParameter3Property, value); }
        }
        private static void CommandParameter1ChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var invokeCommand = d as MyTextBox;
            if (invokeCommand != null)
                invokeCommand.SetValue(CommandParameter1Property, e.NewValue);
        }
        private static void CommandParameter2ChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var invokeCommand = d as MyTextBox;
            if (invokeCommand != null)
                invokeCommand.SetValue(CommandParameter2Property, e.NewValue);

        }

        private static void CommandParameter3ChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var invokeCommand = d as MyTextBox;
            if (invokeCommand != null)
                invokeCommand.SetValue(CommandParameter3Property, e.NewValue);
        }
        #endregion
    }
}
