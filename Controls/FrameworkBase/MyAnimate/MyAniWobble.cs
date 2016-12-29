
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
    public class MyAniWobble : MyAnimateBase
    {
        public MyAniWobble(UIElement _element)
            : base("wobble", _element)
        {
            base.AnimateSpeed = 1200;
        }

        public MyAniWobble(UIElement _element, Action _completed)
            : base("wobble", _element, _completed)
        {
            base.AnimateSpeed = 1200;

        }




        public override MyAnimateBase Animate()
        {
            IsAnimateCompleted = false;
            Element.Visibility = Visibility.Visible;
            Element.RenderTransformOrigin = new Point(0.5, 0.5);

            RotateTransform translation = new RotateTransform();
            TranslateTransform translationTranslate = new TranslateTransform(0, 0);

            string translationName = "";
            string translationTranslateName = "";

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

                translationTranslateName = "ayTranslation" + translationTranslate.GetHashCode();
                Win.RegisterName(translationTranslateName, translationTranslate);
                tg.Children.Add(translationTranslate);

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
                    translationTranslate = item as TranslateTransform;
                    if (translationTranslate != null)
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
                    translation = new RotateTransform();
                    translationName = "ayTranslation" + translation.GetHashCode();
                    Win.RegisterName(translationName, translation);
                    tg.Children.Add(translation);
                    Element.RenderTransform = tg;
                }

                if (translationTranslate != null)
                {

                    var tex = translationTranslate.GetValue(FrameworkElement.NameProperty);
                    if (tex != null && tex.ToString() != "")
                    {
                        translationTranslateName = tex.ToString();
                    }
                    else
                    {
                        translationTranslateName = "ayTranslation" + translationTranslate.GetHashCode();
                        Win.RegisterName(translationTranslateName, translationTranslate);
                    }
                }
                else
                {
                    translationTranslate = new TranslateTransform(0, 0);
                    translationTranslateName = "ayTranslation" + translationTranslate.GetHashCode();
                    Win.RegisterName(translationTranslateName, translationTranslate);
                    tg.Children.Add(translationTranslate);
                    Element.RenderTransform = tg;
                }
            }
            #endregion

            translation.CenterX = 0.5;
            translation.CenterY = 1;

            double angle = translation.Angle;
            var k2 = new EasingDoubleKeyFrame(angle - 5, TimeSpan.FromMilliseconds(AniTime(0.15)));
            var k2_1 = new EasingDoubleKeyFrame(angle + 3, TimeSpan.FromMilliseconds(AniTime(0.3)));
            var k2_2 = new EasingDoubleKeyFrame(angle - 3, TimeSpan.FromMilliseconds(AniTime(0.45)));
            var k2_3 = new EasingDoubleKeyFrame(angle + 2, TimeSpan.FromMilliseconds(AniTime(0.6)));
            var k2_4 = new EasingDoubleKeyFrame(angle - 1, TimeSpan.FromMilliseconds(AniTime(0.75)));
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
            dau.KeyFrames.Add(k2_9);

            story.Children.Add(dau);



            DoubleAnimationUsingKeyFrames dauTranslateX = new DoubleAnimationUsingKeyFrames();
            double lateX = translationTranslate.X;
            double elementWidth = Element.RenderSize.Width;
            double s1 = lateX + (-0.25 * elementWidth);
            double s2 = lateX + (0.2 * elementWidth);

            double s3 = lateX + (-0.15 * elementWidth);
            double s4 = lateX + (0.1 * elementWidth);
            double s5 = lateX + (-0.05 * elementWidth);

            var k3 = new EasingDoubleKeyFrame(s1, TimeSpan.FromMilliseconds(AniTime(0.15)));
            var k3_1 = new EasingDoubleKeyFrame(s2, TimeSpan.FromMilliseconds(AniTime(0.3)));
            var k3_2 = new EasingDoubleKeyFrame(s3, TimeSpan.FromMilliseconds(AniTime(0.45)));
            var k3_3 = new EasingDoubleKeyFrame(s4, TimeSpan.FromMilliseconds(AniTime(0.6)));
            var k3_4 = new EasingDoubleKeyFrame(s5, TimeSpan.FromMilliseconds(AniTime(0.75)));
            var k3_9 = new EasingDoubleKeyFrame(lateX, TimeSpan.FromMilliseconds(AnimateSpeed));

            Storyboard.SetTargetName(dauTranslateX, translationTranslateName);
            Storyboard.SetTargetProperty(dauTranslateX, new PropertyPath(TranslateTransform.XProperty));

            dauTranslateX.KeyFrames.Add(k3);
            dauTranslateX.KeyFrames.Add(k3_1);
            dauTranslateX.KeyFrames.Add(k3_2);
            dauTranslateX.KeyFrames.Add(k3_3);
            dauTranslateX.KeyFrames.Add(k3_4);
            dauTranslateX.KeyFrames.Add(k3_9);

            story.Children.Add(dauTranslateX);




            story.Completed +=
                     (sndr, evtArgs) =>
                     {
                         try
                         {
                             Win.Resources.Remove(storyboardName);
                             Win.UnregisterName(translationName);
                             Win.UnregisterName(translationTranslateName);

                             dau.KeyFrames.Clear();
                             dauTranslateX.KeyFrames.Clear();

                             dauTranslateX = null;
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
