using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MyWPFUI.Controls
{
    public class MyAniSlideOutUp : MyAnimateBase
    {
        public MyAniSlideOutUp(UIElement _element)
            : base("SlideOutUp", _element)
        { base.AnimateSpeed = 300; }

        public MyAniSlideOutUp(UIElement _element, Action _completed)
            : base("SlideOutUp", _element, _completed)
        {
            base.AnimateSpeed = 300;
        }


        private double fromDistance = 0;

        public double FromDistance
        {
            get { return fromDistance; }
            set { fromDistance = value; }
        }
        private double toDistance = 0;

        public double ToDistance
        {
            get { return toDistance; }
            set { toDistance = value; }
        }

        private FillBehavior aniEndBehavior = FillBehavior.Stop;

        public FillBehavior AniEndBehavior
        {
            get { return aniEndBehavior; }
            set { aniEndBehavior = value; }
        }

        private IEasingFunction easingFunction;

        public IEasingFunction EasingFunction
        {
            get { return easingFunction; }
            set { easingFunction = value; }
        }

        private bool opacityNeed = true;

        public bool OpacityNeed
        {
            get { return opacityNeed; }
            set { opacityNeed = value; }
        }


        public Storyboard story = new Storyboard();

        public override MyAnimateBase Animate()
        {
            IsAnimateCompleted = false;
            Element.RenderTransformOrigin = new Point(0.5, 0.5);
            Element.Visibility = Visibility.Visible;
            TranslateTransform translation = new TranslateTransform(0, 0);
            string translationName = "";

             story = new Storyboard();
            DoubleAnimationUsingKeyFrames dau = new DoubleAnimationUsingKeyFrames();
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
                    translation = new TranslateTransform(0, 0);
                    translationName = "ayTranslation" + translation.GetHashCode();
                    Win.RegisterName(translationName, translation);
                    tg.Children.Add(translation);
                    Element.RenderTransform = tg;
                }
            }
            #endregion

            if (ToDistance == 0)
            {
                ToDistance = Element.RenderSize.Height * (-1.5);
            }


            var k2 = new EasingDoubleKeyFrame(FromDistance, TimeSpan.FromMilliseconds(AniTime(0)));
            var k3 = new EasingDoubleKeyFrame(ToDistance, TimeSpan.FromMilliseconds(AniTime(1)));
            if (EasingFunction != null)
            {
                k3.EasingFunction = EasingFunction;
            }

            Storyboard.SetTargetName(dau, translationName);
            Storyboard.SetTargetProperty(dau, new PropertyPath(TranslateTransform.YProperty));
            dau.FillBehavior = AniEndBehavior;

            var storyboardName = "aystory" + story.GetHashCode();
            Win.Resources.Add(storyboardName, story);


            dau.KeyFrames.Add(k2);
            dau.KeyFrames.Add(k3);
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
                         catch { }
                       
                     };
            story.Begin();return this;

        }


    }
}
