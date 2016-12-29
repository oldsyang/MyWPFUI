using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace MyWPFUI.Controls
{
    public class EventToCommandEx : System.Windows.Interactivity.EventTrigger
    {
        //public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(EventToCommandEx), new PropertyMetadata(null, CommandChangedCallback));
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(EventToCommandEx), null);
        public static readonly DependencyProperty CommandParameter1Property = DependencyProperty.Register("CommandParameter1", typeof(object), typeof(EventToCommandEx), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, CommandParameter1ChangedCallback));
        public static readonly DependencyProperty CommandParameter2Property = DependencyProperty.Register("CommandParameter2", typeof(object), typeof(EventToCommandEx), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, CommandParameter2ChangedCallback));
        public static readonly DependencyProperty CommandParameter3Property = DependencyProperty.Register("CommandParameter3", typeof(object), typeof(EventToCommandEx), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, CommandParameter3ChangedCallback));
        public static readonly DependencyProperty PassEventSenderToCommandProperty = DependencyProperty.Register("PassEventSenderToCommand", typeof(bool), typeof(EventToCommandEx), new PropertyMetadata(false));
        public static readonly DependencyProperty PassEventArgsToCommandProperty = DependencyProperty.Register("PassEventArgsToCommand", typeof(bool), typeof(EventToCommandEx), new PropertyMetadata(false));
        public static readonly DependencyProperty PassRoutEventArgsToCommandProperty = DependencyProperty.Register("PassRoutEventArgsToCommand", typeof(bool), typeof(EventToCommandEx), new PropertyMetadata(false));

        private static void CommandParameter1ChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var invokeCommand = d as EventToCommandEx;
            if (invokeCommand != null)
                invokeCommand.SetValue(CommandParameter1Property, e.NewValue);


            UIElement element = invokeCommand.Source as UIElement;
            if (element != null)
            {
                CommandActionExParameter param = new CommandActionExParameter();
                param.Parameter1 = invokeCommand.CommandParameter1;
                param.Parameter2 = invokeCommand.CommandParameter2;
                param.Parameter3 = invokeCommand.CommandParameter3;

                element.IsEnabled = invokeCommand.Command.CanExecute(param);
            }
        }
        private static void CommandParameter2ChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var invokeCommand = d as EventToCommandEx;
            if (invokeCommand != null)
                invokeCommand.SetValue(CommandParameter2Property, e.NewValue);


            UIElement element = invokeCommand.Source as UIElement;
            if (element != null)
            {
                CommandActionExParameter param = new CommandActionExParameter();
                param.Parameter1 = invokeCommand.CommandParameter1;
                param.Parameter2 = invokeCommand.CommandParameter2;
                param.Parameter3 = invokeCommand.CommandParameter3;

                element.IsEnabled = invokeCommand.Command.CanExecute(param);
            }
        }

        private static void CommandParameter3ChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var invokeCommand = d as EventToCommandEx;
            if (invokeCommand != null)
                invokeCommand.SetValue(CommandParameter3Property, e.NewValue);


            UIElement element = invokeCommand.Source as UIElement;
            if (element != null)
            {
                CommandActionExParameter param = new CommandActionExParameter();
                param.Parameter1 = invokeCommand.CommandParameter1;
                param.Parameter2 = invokeCommand.CommandParameter2;
                param.Parameter3 = invokeCommand.CommandParameter3;

                element.IsEnabled = invokeCommand.Command.CanExecute(param);
            }
        }

        //private static void CommandChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var invokeCommand = d as EventToCommandEx;
        //    if (invokeCommand != null)
        //        invokeCommand.SetValue(CommandProperty, e.NewValue);
        //}

        public ICommand Command
        {
            get { return GetValue(CommandProperty) as ICommand; }
            set { SetValue(CommandProperty, value); }
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

        public bool PassRoutEventArgsToCommand
        {
            get { return (bool)GetValue(PassRoutEventArgsToCommandProperty); }
            set { SetValue(PassRoutEventArgsToCommandProperty, value); }
        }



        protected override void OnEvent(EventArgs eventArgs)
        {
            if (this.Command == null)
                return;

            CommandActionExParameter param = new CommandActionExParameter();
            param.Parameter1 = CommandParameter1;
            param.Parameter2 = CommandParameter2;
            param.Parameter3 = CommandParameter3;

            if (PassEventArgsToCommand)
            {
                param.EventArgs = eventArgs;
            }
            if (PassEventSenderToCommand)
            {
                param.EventSender = this.Source;
            }
            if (PassRoutEventArgsToCommand)
            {
                param.RoutedEventArgs = eventArgs as RoutedEventArgs;
            }

            if (this.Command.CanExecute(param))
            {
                this.Command.Execute(param);
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            if (this.Command != null)
            {
                UIElement element = this.Source as UIElement;
                if (element != null)
                {
                    CommandActionExParameter param = new CommandActionExParameter();
                    param.Parameter1 = CommandParameter1;
                    param.Parameter2 = CommandParameter2;
                    param.Parameter3 = CommandParameter3;

                    element.IsEnabled = this.Command.CanExecute(param);
                    this.Command.CanExecuteChanged += Command_CanExecuteChanged;
                }
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (this.Command != null)
            {
                this.Command.CanExecuteChanged -= Command_CanExecuteChanged;
            }
        }

        private void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            UIElement element = this.Source as UIElement;
            if (element != null)
            {
                CommandActionExParameter param = new CommandActionExParameter();
                param.Parameter1 = CommandParameter1;
                param.Parameter2 = CommandParameter2;
                param.Parameter3 = CommandParameter3;

                element.IsEnabled = this.Command.CanExecute(param);
            }
        }


    }

}
