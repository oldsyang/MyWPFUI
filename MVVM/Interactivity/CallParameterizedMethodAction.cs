using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace MyWPFUI.Controls
{
    public class CallParameterizedMethodAction : TriggerAction<DependencyObject>
    {
        private class MethodDescriptor
		{
			public MethodInfo MethodInfo
			{
				get;
				private set;
			}
            //public bool HasParameters
            //{
            //    get
            //    {
            //        return this.Parameters.Length > 0;
            //    }
            //}
            //public int ParameterCount
            //{
            //    get
            //    {
            //        return this.Parameters.Length;
            //    }
            //}
			public ParameterInfo[] Parameters
			{
				get;
				private set;
			}
            //public Type SecondParameterType
            //{
            //    get
            //    {
            //        if (this.Parameters.Length >= 2)
            //        {
            //            return this.Parameters[1].ParameterType;
            //        }
            //        return null;
            //    }
            //}
			public MethodDescriptor(MethodInfo methodInfo, ParameterInfo[] methodParams)
			{
				this.MethodInfo = methodInfo;
				this.Parameters = methodParams;
			}
		}
		private List<CallParameterizedMethodAction.MethodDescriptor> methodDescriptors;
        public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register("TargetObject", typeof(object), typeof(CallParameterizedMethodAction), new PropertyMetadata(new PropertyChangedCallback(CallParameterizedMethodAction.OnTargetObjectChanged)));
        public static readonly DependencyProperty MethodNameProperty = DependencyProperty.Register("MethodName", typeof(string), typeof(CallParameterizedMethodAction), new PropertyMetadata(new PropertyChangedCallback(CallParameterizedMethodAction.OnMethodNameChanged)));
        public static readonly DependencyProperty MethodParametersProperty = DependencyProperty.Register("MethodParameters", typeof(object[]), typeof(CallParameterizedMethodAction), new PropertyMetadata());
        public object TargetObject
		{
			get
			{
				return base.GetValue(CallParameterizedMethodAction.TargetObjectProperty);
			}
			set
			{
				base.SetValue(CallParameterizedMethodAction.TargetObjectProperty, value);
			}
		}
		public string MethodName
		{
			get
			{
				return (string)base.GetValue(CallParameterizedMethodAction.MethodNameProperty);
			}
			set
			{
				base.SetValue(CallParameterizedMethodAction.MethodNameProperty, value);
			}
		}
        public object[] MethodParameters
        {
            get
            {
                return (object[])base.GetValue(CallParameterizedMethodAction.MethodParametersProperty);
            }
            set
            {
                base.SetValue(CallParameterizedMethodAction.MethodParametersProperty, value);
            }
        }
		private object Target
		{
			get
			{
				return this.TargetObject ?? base.AssociatedObject;
			}
		}
        public CallParameterizedMethodAction()
		{
            this.methodDescriptors = new List<CallParameterizedMethodAction.MethodDescriptor>();
		}
		protected override void Invoke(object parameter)
		{
            if (base.AssociatedObject != null)
			{
                CallParameterizedMethodAction.MethodDescriptor methodDescriptor = this.FindBestMethod(parameter);
				if (methodDescriptor != null)
				{
					ParameterInfo[] parameters = methodDescriptor.Parameters;
					if (parameters.Length == 0)
					{
						methodDescriptor.MethodInfo.Invoke(this.Target, null);
						return;
					}
                    else
                    {
                        methodDescriptor.MethodInfo.Invoke(this.Target, MethodParameters);
                    }
                    //if (parameters.Length == 2 && base.AssociatedObject != null && parameter != null && parameters[0].ParameterType.IsAssignableFrom(base.AssociatedObject.GetType()) && parameters[1].ParameterType.IsAssignableFrom(parameter.GetType()))
                    //{
                    //    methodDescriptor.MethodInfo.Invoke(this.Target, new object[]
                    //    {
                    //        base.AssociatedObject,
                    //        parameter
                    //    });
                    //    return;
                    //}
				}
				else
				{
					if (this.TargetObject != null)
					{
						throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Method not found.", new object[]
						{
							this.MethodName,
							this.TargetObject.GetType().Name
						}));
					}
				}
			}
		}
		protected override void OnAttached()
		{
			base.OnAttached();
			this.UpdateMethodInfo();
		}
		protected override void OnDetaching()
		{
			this.methodDescriptors.Clear();
			base.OnDetaching();
		}
        private CallParameterizedMethodAction.MethodDescriptor FindBestMethod(object parameter)
		{
            for (int i = 0; i < this.methodDescriptors.Count; i++)
            {
                if (MethodParameters == null)
                {
                    if (methodDescriptors[i].Parameters.Length == 0)
                    {
                        return this.methodDescriptors[i];
                    }
                    else
                    {
                        continue;
                    }
                }
                if (MethodParameters.Length != methodDescriptors[i].Parameters.Length)
                {
                    continue;
                }
                bool flag = true;
                for (int j = 0; j < this.methodDescriptors[i].Parameters.Length; j++)
                {
                    if (MethodParameters[j] != null && this.methodDescriptors[i].Parameters[j].ParameterType != MethodParameters[j].GetType())
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    return this.methodDescriptors[i];
                }
            }

            return null;

            //if (parameter != null)
            //{
            //    parameter.GetType();
            //}
            //return this.methodDescriptors.FirstOrDefault();
		}
		private void UpdateMethodInfo()
		{
			this.methodDescriptors.Clear();
			if (this.Target != null && !string.IsNullOrEmpty(this.MethodName))
			{
				Type type = this.Target.GetType();
				MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public);
				for (int i = 0; i < methods.Length; i++)
				{
					MethodInfo methodInfo = methods[i];
					if (this.IsMethodValid(methodInfo))
					{
						ParameterInfo[] parameters = methodInfo.GetParameters();
                        if (CallParameterizedMethodAction.AreMethodParamsValid(parameters))
						{
                            this.methodDescriptors.Add(new CallParameterizedMethodAction.MethodDescriptor(methodInfo, parameters));
						}
					}
				}
                //this.methodDescriptors = this.methodDescriptors.OrderByDescending(delegate(CallParameterizedMethodAction.MethodDescriptor methodDescriptor)
                //{
                //    int num = 0;
                //    if (methodDescriptor.HasParameters)
                //    {
                //        Type type2 = methodDescriptor.SecondParameterType;
                //        while (type2 != typeof(EventArgs))
                //        {
                //            num++;
                //            type2 = type2.BaseType;
                //        }
                //    }
                //    return methodDescriptor.ParameterCount + num;
                //}).ToList<CallParameterizedMethodAction.MethodDescriptor>();
			}
		}
		private bool IsMethodValid(MethodInfo method)
		{
			return string.Equals(method.Name, this.MethodName, StringComparison.Ordinal) && !(method.ReturnType != typeof(void));
		}
		private static bool AreMethodParamsValid(ParameterInfo[] methodParams)
		{
            //if (methodParams.Length == 2)
            //{
            //    if (methodParams[0].ParameterType != typeof(object))
            //    {
            //        return false;
            //    }
            //    if (!typeof(EventArgs).IsAssignableFrom(methodParams[1].ParameterType))
            //    {
            //        return false;
            //    }
            //}
            //else
            //{
            //    if (methodParams.Length != 0)
            //    {
            //        return false;
            //    }
            //}
			return true;
		}

		private static void OnMethodNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
            CallParameterizedMethodAction callMethodAction = (CallParameterizedMethodAction)sender;
			callMethodAction.UpdateMethodInfo();
		}

		private static void OnTargetObjectChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
            CallParameterizedMethodAction callMethodAction = (CallParameterizedMethodAction)sender;
			callMethodAction.UpdateMethodInfo();
		}
        
    }
}
