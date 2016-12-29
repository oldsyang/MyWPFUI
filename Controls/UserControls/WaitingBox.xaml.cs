using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Cache;
using System.Security.Policy;
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
    /// WaitingBox.xaml 的交互逻辑
    /// </summary>
    public partial class WaitingBox : UserControl,INotifyPropertyChanged
    {
        public WaitingBox()
        {
            InitializeComponent();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyname)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler!=null)
                handler(this,new PropertyChangedEventArgs(propertyname));
        }
        private ImageSource _imageSource=new BitmapImage(new Uri("/MyWPFUI;component/Resources/Images/icon-circle.png", UriKind.RelativeOrAbsolute));

        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }
    }
}
