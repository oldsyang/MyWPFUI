using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace MyWPFUI.Controls
{
    public class MyTime
    {
        /// <summary>
        /// 用于方便定时任务，这里假如3000，那么第3秒执行
        /// </summary>
        /// <param name="millsecond"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static System.Windows.Threading.DispatcherTimer SetInterval(int millsecond, Action action)
        {
            System.Windows.Threading.DispatcherTimer dTimer = new System.Windows.Threading.DispatcherTimer();
            //注：此处 Tick 为 dTimer 对象的事件（ 超过计时器间隔时发生）
            dTimer.Tick += (sender, e) => { action(); };
            dTimer.Interval = new TimeSpan(0, 0, 0, 0, millsecond);
            //启动 DispatcherTimer对象dTime。
            dTimer.Start();
            return dTimer;
        }

        public static System.Windows.Threading.DispatcherTimer SetTimeout(int millsecond, Action action)
        {
            System.Windows.Threading.DispatcherTimer dTimer = new System.Windows.Threading.DispatcherTimer();
            //注：此处 Tick 为 dTimer 对象的事件（ 超过计时器间隔时发生）
            dTimer.Tick += (sender, e) =>
            {
                action();
                System.Windows.Threading.DispatcherTimer ds = sender as System.Windows.Threading.DispatcherTimer;
                if (ds != null)
                {
                    ds.Stop();
                    ds = null;
                }
            };
            dTimer.Interval = new TimeSpan(0, 0, 0, 0, millsecond);
            //启动 DispatcherTimer对象dTime。
            dTimer.Start();
            return dTimer;
        }
    }
}
