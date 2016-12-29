using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using MyWPFUI.Controls.Models;

namespace MyWPFUI.Controls
{
    public class MyLayerServices
    {
        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <param name="view">要添加的页面</param>
        public static void ShowDialog(UserControl view)
        {
            ShowDialog(null, "", view);
        }
        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <param name="owner">owner</param>
        /// <param name="view">要添加的页面</param>
        public static void ShowDialog(Window owner, UserControl view)
        {
            ShowDialog(owner, "", null);
        }
        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <param name="title">窗口的title</param>
        /// <param name="view">要添加的页面</param>
        public static void ShowDialog(string title, UserControl view)
        {
            ShowDialog(null, title, view);
        }
        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <param name="owner">owner</param>
        /// <param name="title">窗口的title</param>
        /// <param name="view">要添加的页面</param>
        public static void ShowDialog(Window owner, string title, UserControl view)
        {
            ShowDialog<object>(owner, title, view, null);
        }
        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <param name="view">要添加的页面</param>
        /// <param name="option">窗口的设置（new 一个MyLayerOptions）</param>
        public static void ShowDialog(UserControl view, MyLayerOptions option)
        {
            ShowDialog("", view, option);
        }
        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <param name="title">窗口的title</param>
        /// <param name="view">要添加的页面</param>
        /// <param name="option">窗口的设置（new 一个MyLayerOptions）</param>
        public static void ShowDialog(string title, UserControl view, MyLayerOptions option)
        {
            ShowDialog(null, title, view, option);
        }
        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <param name="owner">owner</param>
        /// <param name="view">要添加的页面</param>
        /// <param name="option">窗口的设置（new 一个MyLayerOptions）</param>
        public static void ShowDialog(Window owner, UserControl view, MyLayerOptions option)
        {
            ShowDialog(owner, "", view, option);
        }
        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <param name="owner">owner</param>
        /// <param name="title">窗口的title</param>
        /// <param name="view">要添加的页面</param>
        /// <param name="option">窗口的设置（new 一个MyLayerOptions）</param>
        public static void ShowDialog(Window owner, string title, UserControl view, MyLayerOptions option)
        {
            ShowDialog<object>(owner, title, view, null, option);
        }
        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <typeparam name="TViewModel">要添加的页面的viewmodel的类型</typeparam>
        /// <param name="owner">owner</param>
        /// <param name="title">窗口的title</param>
        /// <param name="view">要添加的页面</param>
        /// <param name="viewmodel">要添加的页面的viewmodel</param>
        /// <param name="option">窗口的设置（new 一个MyLayerOptions）</param>
        public static void ShowDialog<TViewModel>(Window owner, string title, UserControl view, TViewModel viewmodel, MyLayerOptions option)
        {
            ShowDialog<TViewModel>(owner, title, view, viewmodel, null, option, null);
        }
        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <typeparam name="TViewModel">要添加的页面的viewmodel的类型</typeparam>
        /// <param name="title">窗口的title</param>
        /// <param name="view">要添加的页面</param>
        /// <param name="viewmodel">要添加的页面的viewmodel</param>
        public static void ShowDialog<TViewModel>(string title, UserControl view, TViewModel viewmodel)
        {
            ShowDialog<object>(null, title, view, viewmodel);
        }
        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <typeparam name="TViewModel">要添加的页面的viewmodel的类型</typeparam>
        /// <param name="view">要添加的页面</param>
        /// <param name="viewmodel">要添加的页面的viewmodel</param>
        public static void ShowDialog<TViewModel>(UserControl view, TViewModel viewmodel)
        {
            ShowDialog<TViewModel>(null, "", view, viewmodel);
        }

        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <typeparam name="TViewModel">要添加的页面的viewmodel的类型</typeparam>
        /// <param name="owner">owner</param>
        /// <param name="title">窗口的title</param>
        /// <param name="view">要添加的页面</param>
        /// <param name="viewmodel">要添加的页面的viewmodel</param>
        public static void ShowDialog<TViewModel>(Window owner, string title, UserControl view, TViewModel viewmodel)
        {
            ShowDialog<TViewModel>(owner, title, view, viewmodel, null, null, null);
        }
        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <typeparam name="TViewModel">要添加的页面的viewmodel的类型</typeparam>
        /// <param name="view">要添加的页面</param>
        /// <param name="viewmodel">要添加的页面的viewmodel</param>
        /// <param name="onDialogCloseCallBack">关闭窗口后触发的方法（参数为页面的viewmodel）</param>
        public static void ShowDialog<TViewModel>(UserControl view, TViewModel viewmodel, Action<TViewModel> onDialogCloseCallBack)
        {
            ShowDialog<TViewModel>(null, "", view, viewmodel, onDialogCloseCallBack);
        }
        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <typeparam name="TViewModel">要添加的页面的viewmodel的类型</typeparam>
        /// <param name="title">窗口的title</param>
        /// <param name="view">要添加的页面</param>
        /// <param name="viewmodel">要添加的页面的viewmodel</param>
        /// <param name="onDialogCloseCallBack">关闭窗口后触发的方法（参数为页面的viewmodel）</param>
        public static void ShowDialog<TViewModel>(string title, UserControl view, TViewModel viewmodel, Action<TViewModel> onDialogCloseCallBack)
        {
            ShowDialog<TViewModel>(null, title, view, viewmodel, onDialogCloseCallBack);
        }
        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <typeparam name="TViewModel">要添加的页面的viewmodel的类型</typeparam>
        /// <param name="owner">owner</param>
        /// <param name="title">窗口的title</param>
        /// <param name="view">要添加的页面</param>
        /// <param name="viewmodel">要添加的页面的viewmodel</param>
        /// <param name="onDialogCloseCallBack">关闭窗口后触发的方法（参数为页面的viewmodel）</param>
        public static void ShowDialog<TViewModel>(Window owner, string title, UserControl view, TViewModel viewmodel, Action<TViewModel> onDialogCloseCallBack)
        {
            ShowDialog<TViewModel>(owner, title, view, viewmodel, onDialogCloseCallBack, null, null);
        }
        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <typeparam name="TViewModel">要添加的页面的viewmodel的类型</typeparam>
        /// <param name="view">要添加的页面</param>
        /// <param name="viewmodel">要添加的页面的viewmodel</param>
        /// <param name="onDialogCloseCallBack">关闭窗口后触发的方法（参数为页面的viewmodel）</param>
        /// <param name="options">窗口的设置（new 一个MyLayerOptions）</param>
        public static void ShowDialog<TViewModel>(UserControl view, TViewModel viewmodel, Action<TViewModel> onDialogCloseCallBack, MyLayerOptions options)
        {
            ShowDialog<TViewModel>(null, "", view, viewmodel, onDialogCloseCallBack, options, null);
        }
        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <typeparam name="TViewModel">要添加的页面的viewmodel的类型</typeparam>
        /// <param name="title">窗口的title</param>
        /// <param name="view">要添加的页面</param>
        /// <param name="viewmodel">要添加的页面的viewmodel</param>
        /// <param name="onDialogCloseCallBack">关闭窗口后触发的方法（参数为页面的viewmodel）</param>
        /// <param name="options">窗口的设置（new 一个MyLayerOptions）</param>
        public static void ShowDialog<TViewModel>(string title, UserControl view, TViewModel viewmodel, Action<TViewModel> onDialogCloseCallBack, MyLayerOptions options)
        {
            ShowDialog<TViewModel>(null, title, view, viewmodel, onDialogCloseCallBack, options, null);
        }

        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <typeparam name="TViewModel">要添加的页面的viewmodel的类型</typeparam>
        /// <param name="title">窗口的title</param>
        /// <param name="view">要添加的页面</param>
        /// <param name="viewmodel">要添加的页面的viewmodel</param>
        /// <param name="onDialogCloseCallBack">关闭窗口后触发的方法（参数为页面的viewmodel）</param>
        /// <param name="options">窗口的设置（new 一个MyLayerOptions）</param>
        /// <param name="winLoadAction">页面渲染完之后进行的操作，一般是数据加载</param>
        public static void ShowDialog<TViewModel>(string title, UserControl view, TViewModel viewmodel, Action<TViewModel> onDialogCloseCallBack, MyLayerOptions options, Action winLoadAction)
        {
            ShowDialog<TViewModel>(null, title, view, viewmodel, onDialogCloseCallBack, options, winLoadAction);
        }




        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <typeparam name="TViewModel">传入的viewmodel</typeparam>
        /// <param name="owner">窗口的owner</param>
        /// <param name="title">窗口的title</param>
        /// <param name="view">窗口的内容</param>
        /// <param name="viewmodel">窗口的内容的viewmodel</param>
        /// <param name="onDialogCloseCallBack">窗口关闭后的回掉事件</param>
        /// <param name="option">窗口的一些属性设置（主要是动画）</param>
        /// <param name="winLoadAction">窗口加载完页面后执行的操作</param>
        public static void ShowDialog<TViewModel>(Window owner, string title, UserControl view, TViewModel viewmodel, Action<TViewModel> onDialogCloseCallBack, MyLayerOptions option, Action winLoadAction)
        {
            view.DataContext = viewmodel;
            MyLayer messageBox = new MyLayer(owner, view, title, option, true, winLoadAction);
            if (onDialogCloseCallBack != null)
            {
                messageBox.Closed += (sender, e) => onDialogCloseCallBack(viewmodel);
            }
            messageBox.ShowDialog();
        }
        #region 扩展
        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <param name="title">窗口的title</param>
        /// <param name="view">窗口的内容</param>
        /// <param name="request">InteractionRequest类，调用者可执行Requested，即可关闭弹出的窗口</param>
        public static void ShowDialog(string title, System.Windows.Controls.UserControl view, InteractionRequest request)
        {
            ShowDialog<object>(null, title, view, null, null, null, null, request);
        }
        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <typeparam name="TViewModel">传入的viewmodel</typeparam>
        /// <param name="title">窗口的title</param>
        /// <param name="view">窗口的内容</param>
        /// <param name="viewmodel">窗口的内容的viewmodel</param>
        /// <param name="winLoadAction">窗口加载完页面后执行的操作</param>
        /// <param name="request">InteractionRequest类，调用者可执行Requested，即可关闭弹出的窗口</param>
        public static void ShowDialog<TViewModel>(string title, System.Windows.Controls.UserControl view, TViewModel viewmodel, Action winLoadAction, InteractionRequest request)
        {
            ShowDialog<TViewModel>(null, title, view, viewmodel, null, null, winLoadAction, request);
        }
        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <typeparam name="TViewModel">传入的viewmodel</typeparam>
        /// <param name="title">窗口的title</param>
        /// <param name="view">窗口的内容</param>
        /// <param name="viewmodel">窗口的内容的viewmodel</param>
        /// <param name="onDialogCloseCallBack">窗口关闭后的回掉事件</param>
        /// <param name="option">窗口的一些属性设置（主要是动画）</param>
        /// <param name="winLoadAction">窗口加载完页面后执行的操作</param>
        /// <param name="request">InteractionRequest类，调用者可执行Requested，即可关闭弹出的窗口</param>
        public static void ShowDialog<TViewModel>(string title, System.Windows.Controls.UserControl view, TViewModel viewmodel, Action<TViewModel> onDialogCloseCallBack, MyLayerOptions option, Action winLoadAction, InteractionRequest request)
        {
            ShowDialog<TViewModel>(null, title, view, viewmodel, onDialogCloseCallBack, option, winLoadAction, request);
        }
        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <typeparam name="TViewModel">传入的viewmodel</typeparam>
        /// <param name="owner">owner</param>
        /// <param name="title">窗口的title</param>
        /// <param name="view">窗口的内容</param>
        /// <param name="viewmodel">窗口的内容的viewmodel</param>
        /// <param name="onDialogCloseCallBack">窗口关闭后的回掉事件</param>
        /// <param name="option">窗口的一些属性设置（主要是动画）</param>
        /// <param name="winLoadAction">窗口加载完页面后执行的操作</param>
        /// <param name="request">InteractionRequest类，调用者可执行Requested，即可关闭弹出的窗口</param>
        public static void ShowDialog<TViewModel>(Window owner, string title, System.Windows.Controls.UserControl view, TViewModel viewmodel, Action<TViewModel> onDialogCloseCallBack, MyLayerOptions option, Action winLoadAction, InteractionRequest request)
        {
            view.DataContext = viewmodel;
            MyLayer messageBox = new MyLayer(owner, view, title, option, true, winLoadAction);
            if (onDialogCloseCallBack != null)
            {
                messageBox.Closed += (sender, e) => onDialogCloseCallBack(viewmodel);
            }
            if (request != null)
            {
                CallParameterizedMethodAction action = new CallParameterizedMethodAction();
                action.MethodName = "Close";
                action.TargetObject = messageBox;
                InteractionRequestTrigger trigger = new InteractionRequestTrigger();
                trigger.Actions.Add(action);
                trigger.SourceObject = request;
                Interaction.GetTriggers(messageBox).Add(trigger);
            }
            messageBox.Show();
        }
        #endregion
    }
}
