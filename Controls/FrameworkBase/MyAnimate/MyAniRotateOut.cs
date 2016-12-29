
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
    public class MyAniRotateOut : MyAnimateBase
    {
        public MyAniRotateOut(UIElement _element)
            : base("RotateOut", _element)
        { base.AnimateSpeed = 350; }

        public MyAniRotateOut(UIElement _element, Action _completed)
            : base("RotateOut", _element, _completed)
        {
            base.AnimateSpeed = 350;
        }
        private IEasingFunction easingFunction;

        public IEasingFunction EasingFunction
        {
            get { return easingFunction; }
            set { easingFunction = value; }
        }

        private double rotateAngle=200;

        public double RotateAngle
        {
            get { return rotateAngle; }
            set { rotateAngle = value; }
        }

        private bool opacityNeed = true;

        public bool OpacityNeed
        {
            get { return opacityNeed; }
            set { opacityNeed = value; }
        }


        public override MyAnimateBase Animate()
        {
            IsAnimateCompleted = false;
            Element.RenderTransformOrigin = new Point(0.5, 0.5);
            Element.Visibility = Visibility.Visible;

            RotateTransform translation = new RotateTransform();
            string translationName = "";

            Storyboard story = new Storyboard();
            DoubleAnimationUsingKeyFrames dau = new DoubleAnimationUsingKeyFrames();
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
            var k2_0 = new EasingDoubleKeyFrame(RotateAngle, TimeSpan.FromMilliseconds(AniTime(0)));
            var k2 = new EasingDoubleKeyFrame(angle, TimeSpan.FromMilliseconds(AniTime(1)));
            if (EasingFunction != null)
            {
                if (k2_0 != null) k2_0.EasingFunction = EasingFunction;
                if (k2 != null) k2.EasingFunction = EasingFunction;
            }

            Storyboard.SetTargetName(dau, translationName);
            Storyboard.SetTargetProperty(dau, new PropertyPath(RotateTransform.AngleProperty));

            var storyboardName = "aystory" + story.GetHashCode();
            Win.Resources.Add(storyboardName, story);

            dau.KeyFrames.Add(k2_0);
            dau.KeyFrames.Add(k2);
            story.Children.Add(dau);

            if (OpacityNeed)
            {
                DoubleAnimationUsingKeyFrames dauOpacty = new DoubleAnimationUsingKeyFrames();
                var k6 = new EasingDoubleKeyFrame(1, TimeSpan.FromMilliseconds(AniTime(0)));
                var k6_1 = new EasingDoubleKeyFrame(0, TimeSpan.FromMilliseconds(AniTime(1)));

                dauOpacty.KeyFrames.Add(k6);
                dauOpacty.KeyFrames.Add(k6_1);
                Storyboard.SetTarget(dauOpacty, Element);
                dauOpacty.FillBehavior = FillBehavior.Stop;
                Storyboard.SetTargetProperty(dauOpacty, new PropertyPath(UIElement.OpacityProperty));
                story.Children.Add(dauOpacty);
            }



            story.Completed +=
                     (sndr, evtArgs) =>
                     {
                         try
                         {
                             Element.Visibility = Visibility.Collapsed;

                             Win.Resources.Remove(storyboardName);
                             Win.UnregisterName(translationName);

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

        }
    }
}
