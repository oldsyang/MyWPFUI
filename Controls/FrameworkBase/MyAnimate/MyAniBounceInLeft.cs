﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MyWPFUI.Controls
{
    public class MyAniBounceInLeft : MyAnimateBase
    {
        public MyAniBounceInLeft(UIElement _element)
            : base("BounceInLeft", _element)
        { base.AnimateSpeed = 800; }

        public MyAniBounceInLeft(UIElement _element, Action _completed)
            : base("BounceInLeft", _element, _completed)
        {
            base.AnimateSpeed = 800;
        }

        private double oneValue = -1000;

        public double OneValue
        {
            get { return oneValue; }
            set { oneValue = value; }
        }

        private double twoValue = 45;

        public double TwoValue
        {
            get { return twoValue; }
            set { twoValue = value; }
        }

        private double threeValue = -20;

        public double ThreeValue
        {
            get { return threeValue; }
            set { threeValue = value; }
        }

        private double fourValue = 15;

        public double FourValue
        {
            get { return fourValue; }
            set { fourValue = value; }
        }


        public Storyboard story = new Storyboard();

        public override MyAnimateBase Animate()
        {
            IsAnimateCompleted = false;
            Element.RenderTransformOrigin = new Point(0.5, 0.5);
            Element.Visibility = Visibility.Visible;

            TranslateTransform translation = new TranslateTransform(0, 0);
            string translationName = "";

            //Storyboard story = new Storyboard();
            DoubleAnimationUsingKeyFrames dauY = new DoubleAnimationUsingKeyFrames();
            #region 基本工作，确定类型和name
            //是否存在TranslateTransform
            //动画要的类型是否存在
            //动画要的类型的name是否存在，不存在就注册，结束后取消注册，删除动画
            var ex = Element.RenderTransform;
            if (ex == null || (ex as System.Windows.Media.MatrixTransform) != null)
            {
                var tg = new TransformGroup();
                translation = new TranslateTransform(0, 0);
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
                    translation = item as TranslateTransform;
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
                    translation = new TranslateTransform(0, 0);
                    translationName = "ayTranslation" + translation.GetHashCode();
                    Win.RegisterName(translationName, translation);
                    tg.Children.Add(translation);
                    Element.RenderTransform = tg;
                }
            }
            #endregion
            var storyboardName = "aystory" + story.GetHashCode();
            Win.Resources.Add(storyboardName, story);

            var k2 = new EasingDoubleKeyFrame(OneValue, TimeSpan.FromMilliseconds(0));
            var k2_0 = new EasingDoubleKeyFrame(TwoValue, TimeSpan.FromMilliseconds(AniTime(0.6)));
            var k2_1 = new EasingDoubleKeyFrame(ThreeValue, TimeSpan.FromMilliseconds(AniTime(0.75)));
            var k2_2 = new EasingDoubleKeyFrame(FourValue, TimeSpan.FromMilliseconds(AniTime(0.9)));
            var k2_3 = new EasingDoubleKeyFrame(0, TimeSpan.FromMilliseconds(AniTime(1)));


            Storyboard.SetTargetName(dauY, translationName);
            Storyboard.SetTargetProperty(dauY, new PropertyPath(TranslateTransform.XProperty));
            dauY.KeyFrames.Add(k2);
            dauY.KeyFrames.Add(k2_0);
            dauY.KeyFrames.Add(k2_1);
            dauY.KeyFrames.Add(k2_2);
            dauY.KeyFrames.Add(k2_3);
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

                       dauY.KeyFrames.Clear();
                       dauY = null;

                       dauOpacty.KeyFrames.Clear();
                       dauOpacty = null;

                       story = null;
                       base.CallClientCompleted();
                   }
                   catch
                   {

                   }
               };
            story.Begin(); return this;


        }


    }
}
