
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MyWPFUI.Controls
{
    public class MyAniBounce : MyAnimateBase
    {
        public MyAniBounce(UIElement _element)
            : base("bounce", _element)
        { base.AnimateSpeed = 1000; }

        public MyAniBounce(UIElement _element, Action _completed)
            : base("bounce", _element, _completed)
        { base.AnimateSpeed = 1000; }

        private double upHeight=35;
        /// <summary>
        /// 默认是35
        /// </summary>
        public double UpHeight
        {
            get { return upHeight; }
            set { upHeight = value; }
        }
        

        private int bounces=3;
        /// <summary>
        /// 默认值是3
        /// </summary>
        public int Bounces
        {
            get { return bounces; }
            set { bounces= value; }
        }

        private int bounciness=2;
        /// <summary>
        /// 默认值是2
        /// </summary>
        public int Bounciness
        {
            get { return bounciness; }
            set { bounciness = value; }
        }
        
        public override MyAnimateBase Animate()
        {
            IsAnimateCompleted = false;
            Element.RenderTransformOrigin = new Point(0.5, 0.5);
            Element.Visibility = Visibility.Visible;

            TranslateTransform translation = new TranslateTransform(0, 0);
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
            double danqianY = translation.Y;
            //var k1 = new EasingDoubleKeyFrame(danqianY, TimeSpan.FromMilliseconds(0));
            var k2 = new EasingDoubleKeyFrame((danqianY - UpHeight), TimeSpan.FromMilliseconds(AniTime(0.2)), new PowerEase { EasingMode = EasingMode.EaseOut });
            var k3 = new EasingDoubleKeyFrame(danqianY, TimeSpan.FromMilliseconds(AniTime(1)), new BounceEase { EasingMode = EasingMode.EaseOut, Bounces = this.Bounces, Bounciness = this.Bounciness });

            Storyboard.SetTargetName(dau, translationName);
            Storyboard.SetTargetProperty(dau, new PropertyPath(TranslateTransform.YProperty));

            var storyboardName = "aystory" + story.GetHashCode();
            Win.Resources.Add(storyboardName, story);

            //dau.KeyFrames.Add(k1);
            dau.KeyFrames.Add(k2);
            dau.KeyFrames.Add(k3);

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
