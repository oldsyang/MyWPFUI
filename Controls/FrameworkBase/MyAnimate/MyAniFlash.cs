
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MyWPFUI.Controls
{

    public class MyAniFlash : MyAnimateBase
    {
        public MyAniFlash(UIElement _element)
            : base("flash", _element)
        { base.AnimateSpeed = 500; }

        public MyAniFlash(UIElement _element, Action _completed)
            : base("flash", _element, _completed)
        { base.AnimateSpeed = 500; }

        private int flashCount = 2;
        /// <summary>
        /// -1代表一直闪，>0的值
        /// </summary>
        public int FlashCount
        {
            get { return flashCount; }
            set
            {
                if (value == 0)
                {
                    throw new Exception("闪烁次数不能为0");
                }
                flashCount = value;
            }
        }

        Storyboard story = new Storyboard();
        DoubleAnimationUsingKeyFrames dau = new DoubleAnimationUsingKeyFrames();
        public override MyAnimateBase Animate()
        {
            IsAnimateCompleted = false;
            Element.Visibility = Visibility.Visible;

            if (story == null) { story = new Storyboard(); }
    
            //var k1 = new EasingDoubleKeyFrame(1, TimeSpan.FromMilliseconds(0));
            var k2 = new EasingDoubleKeyFrame(0, TimeSpan.FromMilliseconds(AniTime(0.5)));
            var k3 = new EasingDoubleKeyFrame(1, TimeSpan.FromMilliseconds(AniTime(1)));
            //dau.KeyFrames.Add(k1);
            dau.KeyFrames.Add(k2);
            dau.KeyFrames.Add(k3);
            if (FlashCount < 0)
            {
                dau.RepeatBehavior = RepeatBehavior.Forever;
            }
            else
            {
                dau.RepeatBehavior = new RepeatBehavior(FlashCount);
            }
            story.Children.Add(dau);

            //Storyboard.SetTargetName(dau, tex.ToString());
            Storyboard.SetTarget(dau, Element);
            Storyboard.SetTargetProperty(dau, new PropertyPath(UIElement.OpacityProperty));

            story.Completed += (sndr, evtArgs) =>
            {
                try
                {
                    dau.KeyFrames.Clear();
                    dau = null;
                    story = null;
                    base.CallClientCompleted();
                }
                catch
                {

                }
            };
            story.Begin();return this;
            //Element.BeginAnimation(UIElement.OpacityProperty,dau);
        }

        public override void Stop()
        {
            if (story != null)
            {
                story.Stop();
            }
        }
        public override void Destroy()
        {
            if (story != null && FlashCount <0)
            {
                story.Stop();
                story.Children.Clear();
                try
                {

                    dau.KeyFrames.Clear();
                    dau = null;
                    story.Children.Clear();
                    story = null;
                    base.CallClientCompleted();
                }
                catch
                {

                }
            }
        }

    }
}
