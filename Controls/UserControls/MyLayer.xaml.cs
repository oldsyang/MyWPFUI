using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Expression.Interactivity.Layout;
using MyWPFUI.Controls.Models;

namespace MyWPFUI.Controls
{
    /// <summary>
    /// MyLayer.xaml 的交互逻辑
    /// </summary>
    public partial class MyLayer : Window
    {
        public MyLayer()
        {
            InitializeComponent();
            MyTime.SetInterval(1000, () =>
            {
                this.WindowState = WindowState.Maximized;
            });
      
        }

        private MyLayerOptions _options;
        bool isDragDropInEffect = false;
        Point pos = new Point();
        void Element_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragDropInEffect)
            {
                FrameworkElement currEle = sender as FrameworkElement;
                double xPos = e.GetPosition(null).X - pos.X + currEle.Margin.Left;
                double yPos = e.GetPosition(null).Y - pos.Y + currEle.Margin.Top;
                currEle.Margin = new Thickness(xPos, yPos, 0, 0);
                pos = e.GetPosition(null);
            }
        }
        void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            FrameworkElement fEle = sender as FrameworkElement;
            isDragDropInEffect = true;
            pos = e.GetPosition(null);
            fEle.CaptureMouse();
            fEle.Cursor = Cursors.Arrow;
        }
        void Element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragDropInEffect)
            {
                FrameworkElement ele = sender as FrameworkElement;
                isDragDropInEffect = false;
                ele.ReleaseMouseCapture();
            }
        }
        private void SetMyLayerBase(Window owner, object content, string title, MyLayerOptions options, bool isDiaglog, Action action)
        {
            if (options == null)
            {
                options = MYUI.DefaultAyLayerOptions;
            }
            _options = options;
            this.Title = title ?? "";
            //this.Topmost = true;
            //this.ShowInTaskbar = true;
            //if (owner != null)
            //{
            //    Owner = owner;
            //}
            Owner = owner ?? Application.Current.MainWindow;
            //userPresenter.Content = content;
            var fram = content as FrameworkElement;
            if (fram != null)
            {
                userPresenter.Width = fram.Width;
                userPresenter.Height = fram.Height;
            }
            if (options.CanDrag)
            {
                MouseDragElementBehavior m = new MouseDragElementBehavior();
                m.Attach(body);
                //body.MouseMove += new MouseEventHandler(Element_MouseMove);
                //body.MouseLeftButtonDown += new MouseButtonEventHandler(Element_MouseLeftButtonDown);
                //body.MouseLeftButtonUp += new MouseButtonEventHandler(Element_MouseLeftButtonUp);
            }
            if (options.IsContainsTitleBar)
            {
                d.Height = Extensiones.ToGridLength("42.00");
                Border b = new Border();
                if (options.LayerCornerRadius.HasValue)
                {
                    b.Margin = new Thickness(-1, -1, -1, 0);
                    b.CornerRadius = new CornerRadius(options.LayerCornerRadius.Value.TopLeft, options.LayerCornerRadius.Value.TopRight, 0, 0);
                }

                b.HorizontalAlignment = HorizontalAlignment.Stretch;
                b.VerticalAlignment = VerticalAlignment.Stretch;
                //b.SetResourceReference(Border.BackgroundProperty, Colors.CadetBlue);
                b.Background = SolidColorBrushConverter.From16JinZhi("#F0F1F2");
                Grid g = new Grid();
                if (!string.IsNullOrWhiteSpace(title))
                {
                    TextBlock tb = new TextBlock();
                    tb.Width = 200;
                    tb.VerticalAlignment = VerticalAlignment.Center;
                    tb.HorizontalAlignment = HorizontalAlignment.Left;
                    tb.Margin = new Thickness(10, 0, 0, 0);
                    tb.TextWrapping = TextWrapping.NoWrap;
                    tb.FontSize = 14;
                    tb.TextTrimming = TextTrimming.CharacterEllipsis;
                    //tb.Foreground = SolidColorBrushConverter.From16JinZhi("#99FFFFFF");
                    Binding binding = new Binding { Path = new PropertyPath("Title"), Source = this, Mode = BindingMode.TwoWay };
                    tb.SetBinding(TextBlock.TextProperty, binding);
                    g.Children.Add(tb);
                }
                MyImageButton ab = new MyImageButton();
                ab.RenderMode = MyImageButtonMode.HorizonFour;
                ab.SnapsToDevicePixels = true;
                ab.Margin = new Thickness(0, 0, 10, 0);
                ab.Height = ab.Width = 26;
                ab.VerticalAlignment = VerticalAlignment.Center;
                ab.HorizontalAlignment = HorizontalAlignment.Right;
                ab.Padding = new Thickness(0);
                ab.Icon = new BitmapImage(new Uri("pack://application:,,,/MyWPFUI;component/Resources/Images/closebtn.png"));
                ab.Click += closewindow_Click;
                ab.VerticalContentAlignment = VerticalAlignment.Center;
                ab.HorizontalContentAlignment = HorizontalAlignment.Center;
                //ab.Foreground = SolidColorBrushConverter.From16JinZhi("#99FFFFFF");
                //ab.Content = "关闭";
                g.Children.Add(ab);
                b.Child = g;
                bodyConent.Children.Add(b);
            }
            Console.WriteLine("开始触发options.MaskBrush");
            if (options.MaskBrush.IsNotNull())
            {
                layoutMain.Background = options.MaskBrush;
            }
            if (isDiaglog)
            {
                if (options.MaskBrush.IsNull())
                {
                    layoutMain.Background = SolidColorBrushConverter.From16JinZhi("#05FFFFFF");
                }
                layoutMain.MouseLeftButtonDown += LayoutMain_MouseLeftButtonDown;
            }
            Console.WriteLine("开始触发options.IsShowLayerBorder");


            if (options.IsShowLayerBorder)
            {
                body.SetResourceReference(Border.BorderBrushProperty, "Ay.Brush14");

                if (options.LayerBorderThickness.HasValue)
                {
                    body.BorderThickness = options.LayerBorderThickness.Value;
                }
                else
                {
                    body.BorderThickness = new Thickness(1);
                }
            }
            Console.WriteLine("开始触发options.LayerCornerRadius");
            if (options.LayerCornerRadius.HasValue)
            {
                body.CornerRadius = options.LayerCornerRadius.Value;
            }
            options.LayerBackground.Freeze();
            body.Background = options.LayerBackground;
            Console.WriteLine("开始触发options.ShowAnimateIndex");
            if (options.ShowAnimateIndex == 0)
            {
                ShowShadow(options);
                body.Visibility = Visibility.Visible;

                this.ContentRendered += (e, f) =>
                {
                    userPresenter.Content = content;
                    if (action != null)
                        MyTime.SetTimeout(20, action);
                };
            }
            else if (options.ShowAnimateIndex == 1)
            {
                var sc = new MyAniScale(body, () =>
                {
                    ShowShadow(options);
                });
                sc.AnimateSpeed = 750;
                sc.ScaleXFrom = 0;
                sc.ScaleYFrom = 0;
                sc.ScaleXTo = 1;
                sc.ScaleYTo = 1;
                sc.EasingFunction = new System.Windows.Media.Animation.CubicEase { EasingMode = EasingMode.EaseOut };
                sc.Animate().End();
                sc.story.Completed += (c, d) =>
                {
                    userPresenter.Content = content;
                    if (action != null)
                        MyTime.SetTimeout(20, action);
                };
            }
            else if (options.ShowAnimateIndex == 2)
            {
                var sc = new MyAniSlideInDown(body, () =>
                {
                    ShowShadow(options);
                });
                sc.AnimateSpeed = 750;
                sc.FromDistance = -4000;
                sc.OpacityNeed = false;
                sc.EasingFunction = new System.Windows.Media.Animation.CubicEase { EasingMode = EasingMode.EaseOut };
                sc.Animate().End();
                sc.story.Completed += (c, d) =>
                {
                    userPresenter.Content = content;
                    if (action != null)
                        MyTime.SetTimeout(20, action);
                };
            }
            else if (options.ShowAnimateIndex == 3)
            {
                var sc = new MyAniSlideInUp(body, () =>
                {

                    ShowShadow(options);
                });
                sc.AnimateSpeed = 750;
                sc.FromDistance = 4000;
                sc.OpacityNeed = false;
                sc.EasingFunction = new System.Windows.Media.Animation.CubicEase { EasingMode = EasingMode.EaseOut };
                sc.Animate().End();

                sc.story.Completed += (c, d) =>
                {
                    userPresenter.Content = content;
                    if (action != null)
                        MyTime.SetTimeout(20, action);
                };
            }
            else if (options.ShowAnimateIndex == 4)
            {
                var sc = new MyAniSlideInLeft(body, () =>
                {
                    ShowShadow(options);
                });
                sc.AnimateSpeed = 750;
                sc.FromDistance = -4000;
                sc.OpacityNeed = false;
                sc.EasingFunction = new System.Windows.Media.Animation.CubicEase { EasingMode = EasingMode.EaseOut };
                sc.Animate().End();

                sc.story.Completed += (c, d) =>
                {
                    userPresenter.Content = content;
                    if (action != null)
                        MyTime.SetTimeout(20, action);
                };
            }
            else if (options.ShowAnimateIndex == 5)
            {
                var sc = new MyAniSlideInRight(body, () =>
                {

                    ShowShadow(options);
                });
                sc.AnimateSpeed = 750;
                sc.FromDistance = 4000;
                sc.OpacityNeed = false;
                sc.EasingFunction = new System.Windows.Media.Animation.CubicEase { EasingMode = EasingMode.EaseOut };
                sc.Animate().End();

                sc.story.Completed += (c, d) =>
                {
                    userPresenter.Content = content;
                    if (action != null)
                        MyTime.SetTimeout(20, action);
                };
            }
            else if (options.ShowAnimateIndex == 6)
            {
                var sc = new MyAniBounceInDown(body, () =>
                {

                    ShowShadow(options);
                });
                sc.Animate().End();

                sc.story.Completed += (c, d) =>
                {
                    userPresenter.Content = content;
                    if (action != null)
                        MyTime.SetTimeout(20, action);
                };
            }
            else if (options.ShowAnimateIndex == 7)
            {
                var sc = new MyAniBounceInUp(body, () =>
                {

                    ShowShadow(options);
                });
                sc.Animate().End();

                sc.story.Completed += (c, d) =>
                {
                    userPresenter.Content = content;
                    if (action != null)
                        MyTime.SetTimeout(20, action);
                };
            }
            else if (options.ShowAnimateIndex == 8)
            {
                var sc = new MyAniBounceInLeft(body, () =>
                {

                    ShowShadow(options);
                });
                sc.Animate().End();

                sc.story.Completed += (c, d) =>
                {
                    userPresenter.Content = content;
                    if (action != null)
                        MyTime.SetTimeout(20, action);
                };
            }
            else if (options.ShowAnimateIndex == 9)
            {
                var sc = new MyAniBounceInRight(body, () =>
                {
                    ShowShadow(options);
                });
                sc.Animate().End();

                sc.story.Completed += (c, d) =>
                {
                    userPresenter.Content = content;
                    if (action != null)
                        MyTime.SetTimeout(20, action);
                };
            }
            else if (options.ShowAnimateIndex == 10)
            {
                var sc = new MyAniRotateIn(body, () =>
                {

                    ShowShadow(options);
                });
                sc.AnimateSpeed = 750;
                sc.EasingFunction = new System.Windows.Media.Animation.CubicEase { EasingMode = EasingMode.EaseOut };
                sc.Animate().End();

                sc.story.Completed += (c, d) =>
                {
                    userPresenter.Content = content;
                    if (action != null)
                        MyTime.SetTimeout(20, action);
                };
            }
            else if (options.ShowAnimateIndex == 11)
            {
                var sc = new MyAniBounceIn(body, () =>
                {
                    ShowShadow(options);
                });
                sc.AnimateSpeed = 750;
                sc.Animate().End();

                sc.story.Completed += (c, d) =>
                {
                    userPresenter.Content = content;
                    if (action != null)
                        MyTime.SetTimeout(20, action);
                };
            }
            //else if (options.ShowAnimateIndex == 12)
            //{
            //    var sc = new MyAniSlideOutDown(body, () =>
            //    {
            //        ShowShadow(options);
            //    });
            //    sc.AnimateSpeed = 750;
            //    sc.Animate().End();

            //    sc.story.Completed += (c, d) =>
            //    {
            //        userPresenter.Content = content;
            //        if (action != null)
            //            MyTime.SetTimeout(20, action);
            //    };
            //}
            //else if (options.ShowAnimateIndex == 13)
            //{
            //    var sc = new MyAniSlideOutRight(body, () =>
            //    {
            //        ShowShadow(options);
            //    });
            //    sc.AnimateSpeed = 750;
            //    sc.Animate().End();

            //    sc.story.Completed += (c, d) =>
            //    {
            //        userPresenter.Content = content;
            //        if (action != null)
            //            MyTime.SetTimeout(20, action);
            //    };
            //}
            //else if (options.ShowAnimateIndex == 14)
            //{
            //    var sc = new MyAniSlideOutUp(body, () =>
            //    {
            //        ShowShadow(options);
            //    });
            //    sc.AnimateSpeed = 750;
            //    sc.Animate().End();

            //    sc.story.Completed += (c, d) =>
            //    {
            //        userPresenter.Content = content;
            //        if (action != null)
            //            MyTime.SetTimeout(20, action);
            //    };
            //}
            //else if (options.ShowAnimateIndex == 15)
            //{
            //    var sc = new MyAniSlideOutLeft(body, () =>
            //    {
            //        ShowShadow(options);
            //    });
            //    sc.AnimateSpeed = 750;
            //    sc.Animate().End();

            //    sc.story.Completed += (c, d) =>
            //    {
            //        userPresenter.Content = content;
            //        if (action != null)
            //            MyTime.SetTimeout(20, action);
            //    };
            //}
        }

        public void CloseMyLayerTop()
        {
            //var win = this.Owner as AyWindow;
            //if (win != null)
            //{
            //    win.WhenChildCloseTopmostWindow();
            //    this.Close();
            //}
            //else
            //{
            this.Close();
            //}
        }
        private void closewindow_Click(object sender, RoutedEventArgs e)
        {
            CloseMyLayerTop();
        }
        MyAniSwing ani = null;
        private void LayoutMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var _ayhit = e.OriginalSource as Grid;
            if (_ayhit != null)
            {
                if (_ayhit.Name != null && _ayhit.Name.Equals("layoutMain"))
                {
                    if (ani == null)
                    {
                        ani = new MyAniSwing(body);
                        ani.Animate();
                    }
                    else if (ani.IsAnimateCompleted)
                    {
                        ani.Animate();
                    }
                }
            }


            e.Handled = true;
        }
        private void ShowShadow(MyLayerOptions options)
        {
            if (options.HasShadow)
            {
                DropShadowEffect de = new DropShadowEffect();
                //de.BlurRadius = options.ShadowRadius;
                de.Color = options.ShadowColor;
                de.ShadowDepth = options.ShadowDepth;
                body.Effect = de;
                de.BlurRadius = options.ShadowRadius;
                de.Opacity = 0.3;
                MyAniDouble _1 = new MyAniDouble(body);
                _1.AniPropertyPath = new PropertyPath("(FrameworkElement.Effect).(DropShadowEffect.BlurRadius)");
                _1.FromDouble = 0;
                _1.ToDouble = options.ShadowRadius;
                _1.AniEasingMode = 2;
                _1.AnimateSpeed = 200;
                _1.Animate().End();

            }
        }
        public MyLayer(Window owner, object content, string title, MyLayerOptions options, bool isdialog) : this(owner, content, title, options, isdialog, null)
        {
        }
        public MyLayer(Window owner, object content, string title, MyLayerOptions options, bool isdialog, Action action)
        {
            InitializeComponent();
            //this.SourceInitialized += new EventHandler(win_SourceInitialized);
            SetMyLayerBase(owner, content, title, options, isdialog, action);
        }
        void win_SourceInitialized(object sender, EventArgs e)
        {
            //System.IntPtr handle = (new WinInterop.WindowInteropHelper(this)).Handle;
            //WinInterop.HwndSource.FromHwnd(handle).AddHook(new WinInterop.HwndSourceHook(WindowProc));
        }
        public static void Show(object content)
        {
            Show(null, content);
        }

        public static void Show(Window owner, object content)
        {
            Show(owner, content, null);
        }
        public static void Show(Window owner, object content, string title)
        {
            Show(owner, content, title, null);
        }
        public static void Show(Window owner, object content, string title, MyLayerOptions options)
        {
            var messageBox = new MyLayer(owner, content, title, options, false);
            messageBox.Show();
        }


        public static void ShowDialog(object content)
        {
            ShowDialog(null, content);
        }
        public static void ShowDialog(Window owner, object content)
        {
            ShowDialog(owner, content, null);
        }
        public static void ShowDialog(Window owner, object content, string title)
        {
            Console.WriteLine("开始触发");
            ShowDialog(owner, content, title, null);
        }
        public static void ShowDialog(Window owner, object content, string title, MyLayerOptions options)
        {
            var messageBox = new MyLayer(owner, content, title, options, true);
            messageBox.Show();
        }
        private static void ShowDialogCore(Window view)
        {
            Window activeWindow = null;

            if (view.Owner == null && Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    foreach (Window win in Application.Current.Windows)
                    {
                        if (win.IsActive)
                        {
                            activeWindow = win;
                            break;
                        }
                    }

                    if (activeWindow == null)
                    {
                        if (Application.Current.Windows.Count > 0)
                        {
                            activeWindow = Application.Current.Windows[Application.Current.Windows.Count - 1];
                        }
                    }
                    if (view != activeWindow)
                        view.Owner = activeWindow;
                }), null);
            }
            else
            {
            }
        }
    }
}
