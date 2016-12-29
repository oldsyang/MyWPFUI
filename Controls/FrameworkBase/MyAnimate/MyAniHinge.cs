
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
    public class MyAniHinge : MyAnimateBase
    {
        public MyAniHinge(UIElement _element)
            : base("hinge", _element)
        {
            base.AnimateSpeed = 2000;
        }

        public MyAniHinge(UIElement _element, Action _completed)
            : base("hinge", _element, _completed)
        {
            base.AnimateSpeed =2000;

        }




        public override MyAnimateBase Animate()
        {
            IsAnimateCompleted = false;
            Element.RenderTransformOrigin = new Point(0,0);
            Element.Visibility = Visibility.Visible;

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


            var k2 = new EasingDoubleKeyFrame(80, TimeSpan.FromMilliseconds(AniTime(0.2)));
            var k2_2 = new EasingDoubleKeyFrame(40, TimeSpan.FromMilliseconds(AniTime(0.4)));
            var k2_1 = new EasingDoubleKeyFrame(75, TimeSpan.FromMilliseconds(AniTime(0.6)));
            var k2_3 = new EasingDoubleKeyFrame(60, TimeSpan.FromMilliseconds(AniTime(0.8)));
            var k2_4 = new EasingDoubleKeyFrame(0, TimeSpan.FromMilliseconds(AniTime(1)), new CircleEase() { EasingMode = EasingMode.EaseOut });

            Storyboard.SetTargetName(dau, translationName);
            Storyboard.SetTargetProperty(dau, new PropertyPath(RotateTransform.AngleProperty));
            dau.FillBehavior = FillBehavior.Stop;
            var storyboardName = "aystory" + story.GetHashCode();
            Win.Resources.Add(storyboardName, story);

            dau.KeyFrames.Add(k2);
            dau.KeyFrames.Add(k2_1);
            dau.KeyFrames.Add(k2_2);
            dau.KeyFrames.Add(k2_3);
            dau.KeyFrames.Add(k2_4);


            story.Children.Add(dau);



            DoubleAnimationUsingKeyFrames dauTranslateY = new DoubleAnimationUsingKeyFrames();
            var k3 = new EasingDoubleKeyFrame(0, TimeSpan.FromMilliseconds(AniTime(0.8)));
            var k3_1 = new EasingDoubleKeyFrame(700, TimeSpan.FromMilliseconds(AniTime(1)));

            Storyboard.SetTargetName(dauTranslateY, translationTranslateName);
            Storyboard.SetTargetProperty(dauTranslateY, new PropertyPath(TranslateTransform.YProperty));

            dauTranslateY.KeyFrames.Add(k3);
            dauTranslateY.KeyFrames.Add(k3_1);
            story.Children.Add(dauTranslateY);
            dauTranslateY.FillBehavior = FillBehavior.Stop;


            DoubleAnimationUsingKeyFrames dauOpacty = new DoubleAnimationUsingKeyFrames();
            var k4 = new EasingDoubleKeyFrame(1, TimeSpan.FromMilliseconds(AniTime(0.8)));
            var k4_0 = new EasingDoubleKeyFrame(0, TimeSpan.FromMilliseconds(AniTime(1)));

            dauOpacty.KeyFrames.Add(k4);
            dauOpacty.KeyFrames.Add(k4_0);

            Storyboard.SetTarget(dauOpacty, Element);
            dauOpacty.FillBehavior = FillBehavior.Stop;
            Storyboard.SetTargetProperty(dauOpacty, new PropertyPath(UIElement.OpacityProperty));
            story.Children.Add(dauOpacty);


            //PointAnimationUsingKeyFrames porender = new PointAnimationUsingKeyFrames();
            //var k5 = new EasingPointKeyFrame(new Point(0, 0), TimeSpan.FromMilliseconds(0));
            //var k5_0 = new EasingPointKeyFrame(new Point(0, 0), TimeSpan.FromMilliseconds(AniTime(0.8)));
            ////var k5_1 = new EasingPointKeyFrame(new Point(0.4, 0.9), TimeSpan.FromMilliseconds(AniTime(0.8001)));
            //porender.KeyFrames.Add(k5);
            //porender.KeyFrames.Add(k5_0);
            ////porender.KeyFrames.Add(k5_1);

            //Storyboard.SetTarget(porender, Element);
            //dauOpacty.FillBehavior = FillBehavior.Stop;
            //Storyboard.SetTargetProperty(porender, new PropertyPath(UIElement.RenderTransformOriginProperty));
            //story.Children.Add(porender);

            story.Completed +=
                     (sndr, evtArgs) =>
                     {
                         try
                         {
                             Element.Visibility = Visibility.Collapsed;

                             Win.Resources.Remove(storyboardName);
                             Win.UnregisterName(translationName);
                             Win.UnregisterName(translationTranslateName);

                             dau.KeyFrames.Clear();
                             dauTranslateY.KeyFrames.Clear();

                             dauTranslateY = null;
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
