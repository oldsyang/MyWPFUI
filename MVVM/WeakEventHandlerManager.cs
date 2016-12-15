using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace MyWPFUI.Controls
{
    /// <summary>
    /// 来源地址：https://github.com/y19890902q/MyWPFUI.git
    /// 最后编辑：yq  2016年12月4日
    /// </summary>
    internal static class WeakEventHandlerManager
    {
        public static void CallWeakReferenceHandlers(object sender, List<WeakReference> handlers)
        {
            if (handlers != null)
            {
                // Take a snapshot of the handlers before we call out to them since the handlers
                // could cause the array to me modified while we are reading it.
                EventHandler[] callees = new EventHandler[handlers.Count];
                int count = 0;

                //Clean up handlers
                count = CleanupOldHandlers(handlers, callees, count);

                // Call the handlers that we snapshotted
                for (int i = 0; i < count; i++)
                {
                    CallHandler(sender, callees[i]);
                }
            }
        }

        private static void CallHandler(object sender, EventHandler eventHandler)
        {
            DispatcherProxy dispatcher = DispatcherProxy.CreateDispatcher();

            if (eventHandler != null)
            {
                if (dispatcher != null && !dispatcher.CheckAccess())
                {
                    dispatcher.BeginInvoke((Action<object, EventHandler>)CallHandler, sender, eventHandler);
                }
                else
                {
                    eventHandler(sender, EventArgs.Empty);
                }
            }
        }

        private class DispatcherProxy
        {
            Dispatcher innerDispatcher;

            private DispatcherProxy(Dispatcher dispatcher)
            {
                innerDispatcher = dispatcher;
            }

            public static DispatcherProxy CreateDispatcher()
            {
                DispatcherProxy proxy = null;
                if (Application.Current == null)
                    return null;

                proxy = new DispatcherProxy(Application.Current.Dispatcher);
                return proxy;
            }

            public bool CheckAccess()
            {
                return innerDispatcher.CheckAccess();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.Windows.Threading.Dispatcher.#BeginInvoke(System.Delegate,System.Windows.Threading.DispatcherPriority,System.Object[])")]
            public DispatcherOperation BeginInvoke(Delegate method, params Object[] args)
            {
                return innerDispatcher.BeginInvoke(method, DispatcherPriority.Normal, args);
            }
        }

        private static int CleanupOldHandlers(List<WeakReference> handlers, EventHandler[] callees, int count)
        {
            for (int i = handlers.Count - 1; i >= 0; i--)
            {
                WeakReference reference = handlers[i];
                EventHandler handler = reference.Target as EventHandler;
                if (handler == null)
                {
                    // Clean up old handlers that have been collected
                    handlers.RemoveAt(i);
                }
                else
                {
                    callees[count] = handler;
                    count++;
                }
            }
            return count;
        }

        public static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler, int defaultListSize)
        {
            if (handlers == null)
            {
                handlers = (defaultListSize > 0 ? new List<WeakReference>(defaultListSize) : new List<WeakReference>());
            }

            handlers.Add(new WeakReference(handler));
        }

        public static void RemoveWeakReferenceHandler(List<WeakReference> handlers, EventHandler handler)
        {
            if (handlers != null)
            {
                for (int i = handlers.Count - 1; i >= 0; i--)
                {
                    WeakReference reference = handlers[i];
                    EventHandler existingHandler = reference.Target as EventHandler;
                    if ((existingHandler == null) || (existingHandler == handler))
                    {
                        // Clean up old handlers that have been collected
                        // in addition to the handler that is to be removed.
                        handlers.RemoveAt(i);
                    }
                }
            }
        }
    }
}
