using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyWPFUI.Controls
{
    /// <summary>
    /// 来源地址：https://github.com/y19890902q/MyWPFUI.git
    /// 最后编辑：yq  2016年12月4日
    /// </summary>
    public class DelegateCommand<T> : DelegateCommandBase
    {
        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, (o) => true)
        {
        }

        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            : base((o) => executeMethod((T)o), (o) => canExecuteMethod((T)o))
        {
            if (executeMethod == null || canExecuteMethod == null)
            {
                throw new ArgumentNullException("executeMethod", "DelegateCommandDelegatesCannotBeNull");
            }

            Type genericType = typeof(T);

            // DelegateCommand allows object or Nullable<>.  
            // note: Nullable<> is a struct so we cannot use a class constraint.
            if (genericType.IsValueType)
            {
                if ((!genericType.IsGenericType) || (!typeof(Nullable<>).IsAssignableFrom(genericType.GetGenericTypeDefinition())))
                {
                    throw new InvalidCastException("DelegateCommandInvalidGenericPayloadType");
                }
            }
        }

        public bool CanExecute(T parameter)
        {
            return base.CanExecute(parameter);
        }

        public void Execute(T parameter)
        {
            base.Execute(parameter);
        }
    }

    public class DelegateCommand : DelegateCommandBase
    {
        public DelegateCommand(Action executeMethod)
            : this(executeMethod, () => true)
        {
        }

        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : base((o) => executeMethod(), (o) => canExecuteMethod())
        {
            if (executeMethod == null || canExecuteMethod == null)
            {
                throw new ArgumentNullException("executeMethod", "DelegateCommandDelegatesCannotBeNull");
            }
        }

        public void Execute()
        {
            Execute(null);
        }

        public bool CanExecute()
        {
            return CanExecute(null);
        }
    }
}
