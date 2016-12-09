using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyWPFUI.Controls
{
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
