
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
    /// <summary>
    /// 支持animatespeed
    /// </summary>
    public class MyAniTada : MyAnimateBase
    {
        public MyAniTada(UIElement _element)
            : base("tada", _element)
        {
            base.AnimateSpeed = 1100;
        }

        public MyAniTada(UIElement _element, Action _completed)
            : base("tada", _element, _completed)
        {
            base.AnimateSpeed = 1100;
        }

        private double tadaScale = 0.1;

        public double TadaScale
        {
            get { return tadaScale; }
            set { tadaScale = value; }
        }



        public override MyAnimateBase Animate()
        {
            IsAnimateCompleted = false;
            Element.Visibility = Visibility.Visible;
            Element.RenderTransformOrigin = new Point(0.5, 0.5);

            RotateTransform translation = new RotateTransform();
            ScaleTransform translationScale = new ScaleTransform(1, 1);

            string translationName = "";
            string translationScaleName = "";

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

                translationScaleName = "ayTranslation" + translationScale.GetHashCode();
                Win.RegisterName(translationScaleName, translationScale);
                tg.Children.Add(translationScale);

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
                foreach (var item in tg.Children)
                {
                    translationScale = item as ScaleTransform;
                    if (translationScale != null)
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

                if (translationScale != null)
                {

                    var tex = translationScale.GetValue(FrameworkElement.NameProperty);
                    if (tex != null && tex.ToString() != "")
                    {
                        translationScaleName = tex.ToString();
                    }
                    else
                    {
                        translationScaleName = "ayTranslation" + translationScale.GetHashCode();
                        Win.RegisterName(translationScaleName, translationScale);
                    }
                }
                else
                {
                    translationScale = new ScaleTransform(1, 1);
                    translationScaleName = "ayTranslation" + translationScale.GetHashCode();
                    Win.RegisterName(translationScaleName, translationScale);
                    tg.Children.Add(translationScale);
                    Element.RenderTransform = tg;
                }
            }
            #endregion

            translation.CenterX = 0.5;
            translation.CenterY = 0;
            double angle = translation.Angle;
            var k2 = new EasingDoubleKeyFrame(angle - 3, TimeSpan.FromMilliseconds(AniTime(0.1)));
            var k2_1 = new EasingDoubleKeyFrame(angle - 3, TimeSpan.FromMilliseconds(AniTime(0.2)));
            var k2_2 = new EasingDoubleKeyFrame(angle + 3, TimeSpan.FromMilliseconds(AniTime(0.3)));
            var k2_3 = new EasingDoubleKeyFrame(angle + 3, TimeSpan.FromMilliseconds(AniTime(0.5)));
            var k2_4 = new EasingDoubleKeyFrame(angle + 3, TimeSpan.FromMilliseconds(AniTime(0.7)));
            var k2_5 = new EasingDoubleKeyFrame(angle + 3, TimeSpan.FromMilliseconds(AniTime(0.9)));
            var k2_6 = new EasingDoubleKeyFrame(angle - 3, TimeSpan.FromMilliseconds(AniTime(0.4)));
            var k2_7 = new EasingDoubleKeyFrame(angle - 3, TimeSpan.FromMilliseconds(AniTime(0.6)));
            var k2_8 = new EasingDoubleKeyFrame(angle - 3, TimeSpan.FromMilliseconds(AniTime(0.8)));
            var k2_9 = new EasingDoubleKeyFrame(angle, TimeSpan.FromMilliseconds(AnimateSpeed));

            Storyboard.SetTargetName(dau, translationName);
            Storyboard.SetTargetProperty(dau, new PropertyPath(RotateTransform.AngleProperty));

            var storyboardName = "aystory" + story.GetHashCode();
            Win.Resources.Add(storyboardName, story);

            dau.KeyFrames.Add(k2);
            dau.KeyFrames.Add(k2_1);
            dau.KeyFrames.Add(k2_2);
            dau.KeyFrames.Add(k2_3);
            dau.KeyFrames.Add(k2_4);
            dau.KeyFrames.Add(k2_5);
            dau.KeyFrames.Add(k2_6);
            dau.KeyFrames.Add(k2_7);
            dau.KeyFrames.Add(k2_8);
            dau.KeyFrames.Add(k2_9);

            story.Children.Add(dau);


            DoubleAnimationUsingKeyFrames dauScaleX = new DoubleAnimationUsingKeyFrames();
            DoubleAnimationUsingKeyFrames dauScaleY = new DoubleAnimationUsingKeyFrames();
            double scaleX = translationScale.ScaleX;
            double scaleY = translationScale.ScaleY;
            double s1 = scaleX + tadaScale;
            double s2 = scaleX - tadaScale;

            double s3 = scaleY + tadaScale;
            double s4 = scaleY - tadaScale;

            var k3 = new EasingDoubleKeyFrame(s2, TimeSpan.FromMilliseconds(AniTime(0.1)));
            var k3_1 = new EasingDoubleKeyFrame(s2, TimeSpan.FromMilliseconds(AniTime(0.2)));
            var k3_2 = new EasingDoubleKeyFrame(s1, TimeSpan.FromMilliseconds(AniTime(0.3)));
            var k3_3 = new EasingDoubleKeyFrame(s1, TimeSpan.FromMilliseconds(AniTime(0.5)));
            var k3_4 = new EasingDoubleKeyFrame(s1, TimeSpan.FromMilliseconds(AniTime(0.7)));
            var k3_5 = new EasingDoubleKeyFrame(s1, TimeSpan.FromMilliseconds(AniTime(0.9)));
            var k3_6 = new EasingDoubleKeyFrame(s1, TimeSpan.FromMilliseconds(AniTime(0.4)));
            var k3_7 = new EasingDoubleKeyFrame(s1, TimeSpan.FromMilliseconds(AniTime(0.6)));
            var k3_8 = new EasingDoubleKeyFrame(s1, TimeSpan.FromMilliseconds(AniTime(0.8)));
            var k3_9 = new EasingDoubleKeyFrame(scaleX, TimeSpan.FromMilliseconds(AnimateSpeed));


            var k4 = new EasingDoubleKeyFrame(s4, TimeSpan.FromMilliseconds(AniTime(0.1)));
            var k4_1 = new EasingDoubleKeyFrame(s4, TimeSpan.FromMilliseconds(AniTime(0.2)));
            var k4_2 = new EasingDoubleKeyFrame(s3, TimeSpan.FromMilliseconds(AniTime(0.3)));
            var k4_3 = new EasingDoubleKeyFrame(s3, TimeSpan.FromMilliseconds(AniTime(0.5)));
            var k4_4 = new EasingDoubleKeyFrame(s3, TimeSpan.FromMilliseconds(AniTime(0.7)));
            var k4_5 = new EasingDoubleKeyFrame(s3, TimeSpan.FromMilliseconds(AniTime(0.9)));
            var k4_6 = new EasingDoubleKeyFrame(s3, TimeSpan.FromMilliseconds(AniTime(0.4)));
            var k4_7 = new EasingDoubleKeyFrame(s3, TimeSpan.FromMilliseconds(AniTime(0.6)));
            var k4_8 = new EasingDoubleKeyFrame(s3, TimeSpan.FromMilliseconds(AniTime(0.8)));
            var k4_9 = new EasingDoubleKeyFrame(scaleY, TimeSpan.FromMilliseconds(AnimateSpeed));


            Storyboard.SetTargetName(dauScaleX, translationScaleName);
            Storyboard.SetTargetProperty(dauScaleX, new PropertyPath(ScaleTransform.ScaleXProperty));
            Storyboard.SetTargetName(dauScaleY, translationScaleName);
            Storyboard.SetTargetProperty(dauScaleY, new PropertyPath(ScaleTransform.ScaleYProperty));

            dauScaleX.KeyFrames.Add(k3);
            dauScaleX.KeyFrames.Add(k3_1);
            dauScaleX.KeyFrames.Add(k3_2);
            dauScaleX.KeyFrames.Add(k3_3);
            dauScaleX.KeyFrames.Add(k3_4);
            dauScaleX.KeyFrames.Add(k3_5);
            dauScaleX.KeyFrames.Add(k3_6);
            dauScaleX.KeyFrames.Add(k3_7);
            dauScaleX.KeyFrames.Add(k3_8);
            dauScaleX.KeyFrames.Add(k3_9);

            story.Children.Add(dauScaleX);


            dauScaleY.KeyFrames.Add(k4);
            dauScaleY.KeyFrames.Add(k4_1);
            dauScaleY.KeyFrames.Add(k4_2);
            dauScaleY.KeyFrames.Add(k4_3);
            dauScaleY.KeyFrames.Add(k4_4);
            dauScaleY.KeyFrames.Add(k4_5);
            dauScaleY.KeyFrames.Add(k4_6);
            dauScaleY.KeyFrames.Add(k4_7);
            dauScaleY.KeyFrames.Add(k4_8);
            dauScaleY.KeyFrames.Add(k4_9);

            story.Children.Add(dauScaleY);


            story.Completed +=
                     (sndr, evtArgs) =>
                     {
                         try
                         {
                             Win.Resources.Remove(storyboardName);
                             Win.UnregisterName(translationName);
                             Win.UnregisterName(translationScaleName);
                             dau.KeyFrames.Clear();
                             dauScaleX.KeyFrames.Clear();
                             dauScaleY.KeyFrames.Clear();
                             dauScaleX = null;
                             dauScaleY = null;
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
