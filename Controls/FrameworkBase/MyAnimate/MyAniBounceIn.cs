﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MyWPFUI.Controls
{
    public class MyAniBounceIn : MyAnimateBase
    {
        public MyAniBounceIn(UIElement _element)
            : base("BounceIn", _element)
        { base.AnimateSpeed = 800; }

        public MyAniBounceIn(UIElement _element, Action _completed)
            : base("BounceIn", _element, _completed)
        {
            base.AnimateSpeed = 800;
        }

        public Storyboard story = new Storyboard();

        public override MyAnimateBase Animate()
        {
            IsAnimateCompleted = false;
            Element.RenderTransformOrigin = new Point(0.5, 0.5);
            Element.Visibility = Visibility.Visible;

            ScaleTransform translation = new ScaleTransform(1, 1);
            string translationName = "";

            //Storyboard story = new Storyboard();
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

            
            var k2 = new EasingDoubleKeyFrame(0.3, TimeSpan.FromMilliseconds(0));
            var k2_0 = new EasingDoubleKeyFrame(1.1, TimeSpan.FromMilliseconds(AniTime(0.2)));
            var k2_1 = new EasingDoubleKeyFrame(0.9, TimeSpan.FromMilliseconds(AniTime(0.4)));
            var k2_2 = new EasingDoubleKeyFrame(1.03, TimeSpan.FromMilliseconds(AniTime(0.6)));
            var k2_3 = new EasingDoubleKeyFrame(0.97, TimeSpan.FromMilliseconds(AniTime(0.8)));
            var k2_4 = new EasingDoubleKeyFrame(1, TimeSpan.FromMilliseconds(AniTime(1)));
                   

            Storyboard.SetTargetName(dauX, translationName);
            Storyboard.SetTargetProperty(dauX, new PropertyPath(ScaleTransform.ScaleXProperty));
            dauX.KeyFrames.Add(k2);
            dauX.KeyFrames.Add(k2_0);
            dauX.KeyFrames.Add(k2_1);
            dauX.KeyFrames.Add(k2_2);
            dauX.KeyFrames.Add(k2_3);
            dauX.KeyFrames.Add(k2_4);
            story.Children.Add(dauX);



            Storyboard.SetTargetName(dauY, translationName);
            Storyboard.SetTargetProperty(dauY, new PropertyPath(ScaleTransform.ScaleYProperty));
            dauY.KeyFrames.Add(k2);
            dauY.KeyFrames.Add(k2_0);
            dauY.KeyFrames.Add(k2_1);
            dauY.KeyFrames.Add(k2_2);
            dauY.KeyFrames.Add(k2_3);
            dauY.KeyFrames.Add(k2_4);
            story.Children.Add(dauY);

            DoubleAnimationUsingKeyFrames dauOpacty = new DoubleAnimationUsingKeyFrames();
            var k3 = new EasingDoubleKeyFrame(0, TimeSpan.FromMilliseconds(AniTime(0)));
            var k3_0 = new EasingDoubleKeyFrame(1, TimeSpan.FromMilliseconds(AniTime(0.6)));
            var k3_1 = new EasingDoubleKeyFrame(1, TimeSpan.FromMilliseconds(AniTime(1)));

            dauOpacty.KeyFrames.Add(k3);
            dauOpacty.KeyFrames.Add(k3_0);
            dauOpacty.KeyFrames.Add(k3_1);
            Storyboard.SetTarget(dauOpacty, Element);
            dauOpacty.FillBehavior = FillBehavior.Stop;
            Storyboard.SetTargetProperty(dauOpacty, new PropertyPath(UIElement.OpacityProperty));
            story.Children.Add(dauOpacty);

            story.Completed +=
               (sndr, evtArgs) =>
               {
                   try
                   {
                       Element.Opacity = 1;
                       Win.Resources.Remove(storyboardName);
                       Win.UnregisterName(translationName);

                       dauX.KeyFrames.Clear();
                       dauX = null;
                       dauY.KeyFrames.Clear();
                       dauY = null;

                       dauOpacty.KeyFrames.Clear();
                       dauOpacty = null;

                       story = null;
                       base.CallClientCompleted();
                   }
                   catch 
                   {
                       
                       throw;
                   }
               };
            story.Begin();return this;


        }


    }
}
