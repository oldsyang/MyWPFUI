
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace MyWPFUI.Controls
{
    public sealed class SetTargetPropertyBehavior : Behavior<DependencyObject>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            if (!string.IsNullOrEmpty(TargetProperty))
            {
                this.ValueChanged += pcn_ValueChanged;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (!string.IsNullOrEmpty(TargetProperty))
            {
                this.ValueChanged -= pcn_ValueChanged;
            }
        }

        private void pcn_ValueChanged(object sender, EventArgs e)
        {
            if (TargetObject == null)
                return;
            PropertyInfo p = TargetObject.GetType().GetProperty(TargetProperty, BindingFlags.Public | BindingFlags.Instance);
            if (p != null)
            {
                p.SetValue(TargetObject, this.BindableValue, null);
            }
        }

        public static DependencyProperty PropertyProperty = DependencyProperty.Register("TargetProperty", typeof(string), 
            typeof(SetTargetPropertyBehavior), new PropertyMetadata(null));

        public string TargetProperty
        {
            get { return (string)GetValue(PropertyProperty); }
            set { SetValue(PropertyProperty, value); }
        }

        public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register("TargetObject", typeof(object), 
            typeof(SetTargetPropertyBehavior), new PropertyMetadata(null));

        [DefaultValue(null)]
        [Bindable(true)]
        public object TargetObject
        {
            get
            {
                return base.GetValue(SetTargetPropertyBehavior.TargetObjectProperty);
            }
            set
            {
                base.SetValue(SetTargetPropertyBehavior.TargetObjectProperty, value);
            }
        }

        public event EventHandler ValueChanged;

        public static readonly DependencyProperty BindableValueProperty = DependencyProperty.Register("BindableValue",
            typeof(object), typeof(SetTargetPropertyBehavior), 
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, SetTargetPropertyBehavior.OnBindableValueChanged)
          );

        private static void OnBindableValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetTargetPropertyBehavior notifier = (SetTargetPropertyBehavior)d;
            if (null != notifier && null != notifier.ValueChanged)
                notifier.ValueChanged(notifier, EventArgs.Empty);
        }

        [Bindable(true)]
        public object BindableValue
        {
            get
            {
                return (object)this.GetValue(BindableValueProperty);
            }
            set
            {
                this.SetValue(BindableValueProperty, value);
            }
        }
    }
}
