using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyWPFUI.Controls
{
    /// <summary>
    /// 来源地址：https://github.com/y19890902q/MyWPFUI.git
    /// 最后编辑：yq  2016年12月4日
    /// </summary>
    public partial class AutoCompleteTextBox : UserControl
    {
        public AutoCompleteTextBox()
        {
            InitializeComponent();
            _delayTime = 100;
            _keypressTimer = new System.Timers.Timer();
            _keypressTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            SearchText.PreviewKeyDown += SearchText_PreviewKeyDown;
            SearchText.PreviewMouseLeftButtonDown += SearchText_PreviewMouseLeftButtonDown;
            SearchText.LostFocus += SearchText_LostFocus;
        }

        #region Property
        private System.Timers.Timer _keypressTimer;
        private bool _insertText;
        private int _delayTime;
        private delegate void TextChangedCallback();
        /// <summary>
        ///要查询的字段（搜索的字段）
        /// </summary>
        private PropertyInfo _property;
        /// <summary>
        /// 搜索的模式
        /// </summary>
        public SelectedMode SelectedMode { get; set; }
        /// <summary>
        /// 数据源
        /// </summary>
        [Description("数据源")]
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemsSourceProperty =
            ItemsControl.ItemsSourceProperty.AddOwner(
                typeof(AutoCompleteTextBox),
                new UIPropertyMetadata(null, OnItemsSourceChanged));

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteTextBox actb = d as AutoCompleteTextBox;
            if (actb == null) return;

            if (e.NewValue != null)
            {
                if (actb.SourceListBox == null) return;
                actb._property = null;
                actb.SourceListBox.ItemsSource = (IEnumerable)e.NewValue;
                ICollectionView view = CollectionViewSource.GetDefaultView(e.NewValue);
                view.Filter += actb.Filter;
                if (actb.SourceListBox.Items.Count == 0) actb.InternalClosePopup();
                var modeTypes = e.NewValue.GetType().GetGenericArguments();
                if (modeTypes.Length > 0)
                {
                    if (!string.IsNullOrWhiteSpace(actb.DisplayMemberPath))
                    {
                        actb._property = modeTypes[0].GetProperty(actb.DisplayMemberPath);
                    }
                }
                actb.InternalClosePopup();
            }

            if (e.OldValue != null)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(e.OldValue);
                view.Filter -= actb.Filter;
            }
        }

        /// <summary>
        /// 选中项
        /// </summary>
        [Description("选中项")]
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            "SelectedItem", typeof(object), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(SelectedItemCallBack))
            {
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                DefaultValue = default(object)
            });

        private static void SelectedItemCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as AutoCompleteTextBox;
            if (control != null)
            {
                var selecteditem = e.NewValue;
                if (selecteditem == null)
                {
                    if (!string.IsNullOrEmpty(control.SearchText.Text))
                    {
                        control._insertText = true;
                        control.SearchText.Text = string.Empty;
                        control.SearchText.CaretIndex = control.SearchText.Text.Length;
                    }
                }
                else
                {
                    string newvalue = "";
                    if (control._property != null)
                    {
                        newvalue = control._property.GetValue(selecteditem, null).ToString();
                    }
                    else
                    {
                        newvalue = selecteditem.ToString();
                    }
                    if (control.SearchText.Text != newvalue)
                    {
                        control._insertText = true;
                        control.SearchText.Text = newvalue;
                        control.SearchText.CaretIndex = control.SearchText.Text.Length;
                    }

                }
            }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        "Text", typeof(string), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata()
        {
            BindsTwoWayByDefault = true,
            DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            DefaultValue = default(string)
        });

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private string _displayMemberPath;
        /// <summary>
        /// 要显示的字段
        /// </summary>
        public string DisplayMemberPath
        {
            get { return _displayMemberPath; }
            set
            {
                _displayMemberPath = value;
                SourceListBox.DisplayMemberPath = _displayMemberPath;
            }
        }
        private int _maxLength;
        public int MaxLength
        {
            get { return _maxLength; }
            set
            {
                _maxLength = value;
                SearchText.MaxLength = value;
            }
        }
        #endregion

        #region Private
        private void InternalClosePopup()
        {
            if (popup != null)
            {
                popup.IsOpen = false;
            }
        }
        private void InternalOpenPopup()
        {
            popup.IsOpen = false;
            popup.IsOpen = true;
            if (SourceListBox != null) SourceListBox.SelectedIndex = -1;
        }
        private bool Filter(object obj)
        {
            string value;
            if (_property != null)
                value = _property.GetValue(obj, null).ToString();
            else
                value = obj.ToString();

            if (SelectedMode == SelectedMode.None)
            {
                return value.IndexOf(SearchText.Text, StringComparison.CurrentCultureIgnoreCase) >= 0;
            }
            if (SelectedMode == SelectedMode.FirstSpelling)
            {
                return value.IndexOf(SearchText.Text, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                 value.toFirstPingying().IndexOf(SearchText.Text, StringComparison.CurrentCultureIgnoreCase) >= 0;
            }


            return false;
        }
        private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            _keypressTimer.Stop();
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                new TextChangedCallback(TextChanged));
        }
        private void TextChanged()
        {
            if (this.ItemsSource != null)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(this.ItemsSource);
                view.Filter += Filter;
                if (SourceListBox.Items.Count == 0)
                    InternalClosePopup();
                else
                    InternalOpenPopup();
            }
        }
        #endregion

        #region MouserClick
        private void Popup_OnClosed(object sender, EventArgs e)
        {
            popup.IsOpen = false;
        }
        private void SearchText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (SourceListBox != null && SourceListBox.ItemsSource != null && popup.IsOpen)
                {
                    if (SourceListBox.SelectedIndex > 0)
                    {
                        SourceListBox.SelectedIndex--;
                        e.Handled = true;
                        SourceListBox.ScrollIntoView(SourceListBox.SelectedItem);
                    }

                }
            }
            else if (e.Key == Key.Down)
            {
                if (SourceListBox != null && SourceListBox.ItemsSource != null && popup.IsOpen)
                {
                    if (SourceListBox.SelectedIndex < SourceListBox.Items.Count)
                    {
                        SourceListBox.SelectedIndex++;
                        e.Handled = true;
                        SourceListBox.ScrollIntoView(SourceListBox.SelectedItem);
                    }
                }
            }
            else if (e.Key == Key.Enter)
            {
                if (SourceListBox != null && SourceListBox.ItemsSource != null && SourceListBox.SelectedItem != null &&
                    SourceListBox.SelectedIndex >= 0 && popup.IsOpen)
                {
                    if (this.SelectedItem != SourceListBox.SelectedItem)
                        this.SelectedItem = SourceListBox.SelectedItem;
                    else
                    {
                        string newvalue = "";
                        if (_property != null)
                        {
                            newvalue = _property.GetValue(SourceListBox.SelectedItem, null).ToString();
                        }
                        else
                        {
                            newvalue = SourceListBox.SelectedItem.ToString();
                        }
                        if (!SearchText.Text.Equals(newvalue))
                        {
                            _insertText = true;
                            SearchText.Text = newvalue;
                            SearchText.CaretIndex = SearchText.Text.Length;
                        }
                    }
                    popup.IsOpen = false;
                }
                else
                {

                }
            }
        }

        private void SourceListBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("SourceListBox_PreviewMouseUp");
            Console.WriteLine(SourceListBox.SelectedIndex);
            var currentItem = (sender as ListBoxItem).DataContext;
            if (SourceListBox != null && SourceListBox.ItemsSource != null && currentItem != null)
            {
                if (this.SelectedItem != currentItem)
                    this.SelectedItem = currentItem;
                else
                {
                    string newvalue = "";
                    if (_property != null)
                    {
                        newvalue = _property.GetValue(currentItem, null).ToString();
                    }
                    else
                    {
                        newvalue = currentItem.ToString();
                    }
                    if (!SearchText.Text.Equals(newvalue))
                    {
                        _insertText = true;
                        SearchText.Text = newvalue;
                        SearchText.CaretIndex = SearchText.Text.Length;

                    }
                }
                popup.IsOpen = false;
            }
            else
            {

            }
        }

        private void SearchText_PreviewMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            MyTime.SetTimeout(10, () =>
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(this.ItemsSource);
                if (view.Filter != null)
                    view.Filter -= Filter;
                view.Filter += (obj) =>
                {
                    return true;
                };
                if (SourceListBox.Items.Count == 0)
                    InternalClosePopup();
                else
                    InternalOpenPopup();
            });

        }
        private void SearchText_LostFocus(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = false;
        }
        private void SearchText_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Text = SearchText.Text;
            if (_insertText == true) _insertText = false;

            else
            {
                if (_delayTime > 0)
                {
                    _keypressTimer.Interval = _delayTime;
                    _keypressTimer.Start();
                }
                else TextChanged();
            }
        }
        #endregion
    }
    /// <summary>
    /// 查询模式
    /// </summary>
    public enum SelectedMode
    {
        None,//不匹配拼音，查询速度最快，数据量在50万以内
        FirstSpelling,//匹配汉字首字母，数据量在10万以内
    }
}
