
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MyWPFUI.Controls
{
    public class MyAniRotate : MyAnimateBase
    {
        public MyAniRotate(UIElement _element)
            : base("Rotate", _element)
        { base.AnimateSpeed = 350; }

        public MyAniRotate(UIElement _element, Action _completed)
            : base("Rotate", _element, _completed)
        {
            base.AnimateSpeed = 350;
        }

        private double? rotateAngleAdd;

        public double? RotateAngleAdd
        {
            get { return rotateAngleAdd; }
            set { rotateAngleAdd = value; }
        }


        private double? rotateAngleTo;

        public double? RotateAngleTo
        {
            get { return rotateAngleTo; }
            set { rotateAngleTo = value; }
        }



        private RepeatBehavior? aniRepeatBehavior;

        public RepeatBehavior? AniRepeatBehavior
        {
            get { return aniRepeatBehavior; }
            set { aniRepeatBehavior = value; }
        }

        private bool? aniAutoReverse;

        public bool? AniAutoReverse
        {
            get { return aniAutoReverse; }
            set { aniAutoReverse = value; }
        }

        private IEasingFunction easingFunction;

        public IEasingFunction EasingFunction
        {
            get { return easingFunction; }
            set { easingFunction = value; }
        }

        Storyboard story = new Storyboard();
        DoubleAnimationUsingKeyFrames dau = new DoubleAnimationUsingKeyFrames();
        string storyboardName = "";
        string translationName = "";

        public override MyAnimateBase Animate()
        {
            IsAnimateCompleted = false;
            Element.RenderTransformOrigin = new Point(0.5, 0.5);
            Element.Visibility = Visibility.Visible;
            RotateTransform translation = new RotateTransform();
         
           
        
            #region 基本工作，确定类型和name
            //是否存在TranslateTransform
            //动画要的类型是否存在
            //动画要的类型的name是否存在，不存在就注册，结束后取消注册，删除动画
            var ex = Element.RenderTransform;
            if (ex == null || (ex as System.Windows.Media.MatrixTransform) != null)
            {
                var tg = new TransformGroup();
                translation = new RotateTransform();
                translationName = "ayTranslation" + translation.GetHashCode();
                Win.RegisterName(translationName, translation);
                tg.Children.Add(translation);
                Element.RenderTransform = tg;
            }
            else
            {
                var tg = ex as TransformGroup;
                foreach (var item in tg.Children)
                {
                    translation = item as RotateTransform;
                    if (translation != null)
                    {
                        break;
                    }
                }
                if (translation != null)
                {
                    //当前Y值
                    var tex = translation.GetValue(FrameworkElement.NameProperty);
                    if (tex != null && tex.ToString() != "")
                    {
                        translationName = tex.ToString();
                    }
                    else
                    {
                        translationName = "ayTranslation" + translation.GetHashCode();
                        Win.RegisterName(translationName, translation);
                    }
                }
                else
                {
                    translation = new RotateTransform();
                    translationName = "ayTranslation" + translation.GetHashCode();
                    Win.RegisterName(translationName, translation);
                    tg.Children.Add(translation);
                    Element.RenderTransform = tg;
                }
            }
            #endregion
            double angle = translation.Angle;
            //如果旋转角度和最终的是一致的就不执行下面代码
            if (RotateAngleTo.HasValue && RotateAngleTo==angle)
            {
                return this;
            }
            EasingDoubleKeyFrame k2 = null;
            if (RotateAngleAdd.HasValue)
            {
                k2 = new EasingDoubleKeyFrame(RotateAngleAdd.Value + angle, TimeSpan.FromMilliseconds(AniTime(1)));
            }
            else if (RotateAngleTo.HasValue)
            {
                k2 = new EasingDoubleKeyFrame(RotateAngleTo.Value, TimeSpan.FromMilliseconds(AniTime(1)));
            }

            if (!RotateAngleAdd.HasValue && !RotateAngleTo.HasValue)
            {
                k2 = new EasingDoubleKeyFrame(RotateAngleAdd.Value + 360, TimeSpan.FromMilliseconds(AniTime(1)));
            }
            if (EasingFunction != null)
            {
                k2.EasingFunction = EasingFunction;
            }

            Storyboard.SetTargetName(dau, translationName);
            Storyboard.SetTargetProperty(dau, new PropertyPath(RotateTransform.AngleProperty));

            if (AniRepeatBehavior.HasValue)
            {
                story.RepeatBehavior = AniRepeatBehavior.Value;
            }

            if (AniAutoReverse.HasValue)
            {
                story.AutoReverse = AniAutoReverse.Value;
            }

            storyboardName = "aystory" + story.GetHashCode();
            Win.Resources.Add(storyboardName, story);

            dau.KeyFrames.Add(k2);

            story.Children.Add(dau);



            story.Completed +=
                     (sndr, evtArgs) =>
                     {
                         try
                         {
                             Win.Resources.Remove(storyboardName);
                             Win.UnregisterName(translationName);

                             dau.KeyFrames.Clear();
                             dau = null;
                             story.Children.Clear();
                             story = null;
                             base.CallClientCompleted();
                         }
                         catch
                         {

                         }
                     };
            story.Begin(); return this;

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
            if (story != null && AniRepeatBehavior==RepeatBehavior.Forever)
            {
                story.Stop();
                story.Children.Clear();
           
                try
                {
                    Win.Resources.Remove(storyboardName);
                    Win.UnregisterName(translationName);
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
