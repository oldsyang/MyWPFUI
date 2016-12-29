
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MyWPFUI.Controls
{
    /// <summary>
    /// 2016-7-4 10:55:23 version:2.0
    /// </summary>
    public abstract class MyAnimateBase : IDisposable
    {

        public MyAnimateBase(UIElement _element)
        {

            this.Element = _element;

        }

        public MyAnimateBase(UIElement _element, Action _completed)
        {

            this.Completed = _completed;
            this.Element = _element;
        }
        public MyAnimateBase(string _name, UIElement _element)
        {

            this.AnimateName = _name;
            this.Element = _element;
        }
        public MyAnimateBase(string _name, UIElement _element, Action _completed)
        {

            this.AnimateName = _name;
            this.Completed = _completed;
            this.Element = _element;
        }
        public Action Completed { get; set; }

        private Window win;

        internal Window Win
        {
            get
            {
                if (win != null)
                {
                    return win;
                }
                else
                {
                    win = Window.GetWindow(Element);
                    return win;
                }
            }
        }
        #region 2016-9-22 01:23:15 是否完成
        private bool isAnimateCompleted = true;
        /// <summary>
        /// 动画是否完成
        /// </summary>
        public bool IsAnimateCompleted
        {
            get { return isAnimateCompleted; }
            set { isAnimateCompleted = value; }
        }

        #endregion

        private UIElement element;

        public UIElement Element
        {
            get { return element; }
            set { element = value; }
        }

        private string animateName;

        public string AnimateName
        {
            get { return animateName; }
            set { animateName = value; }
        }

        public virtual MyAnimateBase Animate() { return this; }

        public virtual void Stop() { }

        public virtual void Destroy() { }

        public virtual void CallClientCompleted()
        {
            IsAnimateCompleted = true;
            if (Completed != null)
            {
                Completed();
            }
        }

        private double animateSpeed = 1200;

        public double AnimateSpeed
        {
            get { return animateSpeed; }
            set { animateSpeed = value; }
        }

        public virtual double AniTime(double percent)
        {
            return AnimateSpeed * percent;
        }


        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //Completed = null;
            }
        }

        ~MyAnimateBase()
        {
            this.Dispose(false);
        }
    }
    public static class MyAnimateBaseExt
    {
        public static void End(this MyAnimateBase ani)
        {

            MyTime.SetTimeout(((int)ani.AnimateSpeed + 1000), () =>
              {
                  var d = ani as MyAnimateTypeBase;
                  if (d != null)
                  {
                      d.RaiseStoryComplete();
                  }
                  ani.Stop();
                  ani.Destroy();
                  ani.Completed = null;

                  GC.SuppressFinalize(ani);
                  ani = null;
              });
        }
    }


}
