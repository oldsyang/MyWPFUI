using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyWPFUI.Controls
{
    /// <summary>
    /// 来源地址：https://github.com/y19890902q/MyWPFUI.git
    /// 最后编辑：yq  2016年12月4日
    /// </summary>
    public class CommandActionExParameter
    {
        public object Parameter1 { get; set; }
        public object Parameter2 { get; set; }
        public object Parameter3 { get; set; }
        public object EventSender { get; set; }
        public EventArgs EventArgs { get; set; }
        public RoutedEventArgs RoutedEventArgs { get; set; }
    }
}
