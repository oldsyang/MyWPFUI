
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
    public class MyAniPulse : MyAnimateBase
    {
        public MyAniPulse(UIElement _element)
            : base("pulse", _element)
        { base.AnimateSpeed = 900; }

        public MyAniPulse(UIElement _element, Action _completed)
            : base("pulse", _element, _completed)
        { base.AnimateSpeed = 900; }

        private double scaleXDiff = 0.14;

        public double ScaleXDiff
        {
            get { return scaleXDiff; }
            set { scaleXDiff = value; }
        }

        private double scaleYDiff = 0.08;

        public double ScaleYDiff
        {
            get { return scaleYDiff; }
            set { scaleYDiff = value; }
        }

        public override MyAnimateBase Animate()
        {
            IsAnimateCompleted = false;
            Element.RenderTransformOrigin = new Point(0.5, 0.5);
            Element.Visibility = Visibility.Visible;

            ScaleTransform translation = new ScaleTransform(1, 1);
            string translationName = "";

            Storyboard story = new Storyboard();
            DoubleAnimationUsingKeyFrames dauX = new DoubleAnimationUsingKeyFrames();
            DoubleAnimationUsingKeyFrames dauY = new DoubleAnimationUsingKeyFrames();
            #region 基本工作，确定类型和name
            //是否存在TranslateTransform
            //动画要的类型是否存在
            //动画要的类型的name是否存在，不存在就注册，结束后取消注册，删除动画
            var ex = Element.RenderTransform;
            if (ex == null || (ex as System.Windows.Media.MatrixTransform) != null)
            {
                var tg = new TransformGroup();
                translation = new ScaleTransform(1, 1);
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
                    translation = item as ScaleTransform;
                    if (translation != null)
                    {
                        break;
                    }
                }
                if (translation != null)
                {

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
                    translation = new ScaleTransform(1, 1);
                    translationName = "ayTranslation" + translation.GetHashCode();
                    Win.RegisterName(translationName, translation);
                    tg.Children.Add(translation);
                    Element.RenderTransform = tg;
                }
            }
            #endregion
            var storyboardName = "aystory" + story.GetHashCode();
            Win.Resources.Add(storyboardName, story);
            double danqianX = translation.ScaleX;
            double danqianY = translation.ScaleY;

            //var k1 = new EasingDoubleKeyFrame(danqianY, TimeSpan.FromMilliseconds(0));
            var k2 = new EasingDoubleKeyFrame(danqianX + ScaleXDiff, TimeSpan.FromMilliseconds(AniTime(0.6)), new ExponentialEase { EasingMode = EasingMode.EaseOut });
            var k3 = new EasingDoubleKeyFrame(danqianX, TimeSpan.FromMilliseconds(AniTime(1)));
            Storyboard.SetTargetName(dauX, translationName);
            Storyboard.SetTargetProperty(dauX, new PropertyPath(ScaleTransform.ScaleXProperty));
            dauX.KeyFrames.Add(k2);
            dauX.KeyFrames.Add(k3);
            story.Children.Add(dauX);

            var k4 = new EasingDoubleKeyFrame(danqianY + ScaleYDiff, TimeSpan.FromMilliseconds(AniTime(0.6)), new ExponentialEase { EasingMode = EasingMode.EaseOut });
            var k5 = new EasingDoubleKeyFrame(danqianY, TimeSpan.FromMilliseconds(AniTime(1)));
            Storyboard.SetTargetName(dauY, translationName);
            Storyboard.SetTargetProperty(dauY, new PropertyPath(ScaleTransform.ScaleYProperty));
            dauY.KeyFrames.Add(k4);
            dauY.KeyFrames.Add(k5);
            story.Children.Add(dauY);


            //<DoubleAnimationUsingKeyFrames Storyboard.TargetProperty="(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)" Storyboard.TargetName="textBlock">
            //    <EasingDoubleKeyFrame KeyTime="0" Value="1"/>
            //    <EasingDoubleKeyFrame KeyTime="0:0:0.6" Value="1.14">
            //        <EasingDoubleKeyFrame.EasingFunction>
            //            <ExponentialEase EasingMode="EaseOut"/>
            //        </EasingDoubleKeyFrame.EasingFunction>
            //    </EasingDoubleKeyFrame>
            //    <EasingDoubleKeyFrame KeyTime="0:0:0.9" Value="1"/>
            //</DoubleAnimationUsingKeyFrames>
            //<DoubleAnimationUsingKeyFrames Storyboard.TargetProperty="(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)" Storyboard.TargetName="textBlock">
            //    <EasingDoubleKeyFrame KeyTime="0" Value="1"/>
            //    <EasingDoubleKeyFrame KeyTime="0:0:0.6" Value="1.08">
            //        <EasingDoubleKeyFrame.EasingFunction>
            //            <ExponentialEase EasingMode="EaseOut"/>
            //        </EasingDoubleKeyFrame.EasingFunction>
            //    </EasingDoubleKeyFrame>
            //    <EasingDoubleKeyFrame KeyTime="0:0:0.9" Value="1"/>
            //</DoubleAnimationUsingKeyFrames>

            story.Completed +=
                   (sndr, evtArgs) =>
                   {
                       try
                       {
                           Win.Resources.Remove(storyboardName);
                           Win.UnregisterName(translationName);

                           dauX.KeyFrames.Clear();
                           dauX = null;
                           dauY.KeyFrames.Clear();
                           dauY = null;
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
