using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace MyWPFUI.Controls
{
    public class KeyDownEventToCommandEx : EventToCommandEx
    {
        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register("Key", typeof(Key), typeof(KeyDownEventToCommandEx), new PropertyMetadata(Key.None));

        public Key Key
        {
            get { return (Key)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        protected override string GetEventName()
        {
            return "KeyUp";
        }

        protected override void OnEvent(EventArgs eventArgs)
        {
            if (this.Command == null)
                return;

            KeyEventArgs keyArgs = eventArgs as KeyEventArgs;
            if (keyArgs == null)
                return;

            if(keyArgs.Key == this.Key)
            {
                base.OnEvent(eventArgs);
            }
        }
    }
}
