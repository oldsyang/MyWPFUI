
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
    public class MyAniRubberBand : MyAnimateBase
    {
        public MyAniRubberBand(UIElement _element)
            : base("rubberBand", _element)
        { base.AnimateSpeed = 900; }

        public MyAniRubberBand(UIElement _element, Action _completed)
            : base("rubberBand", _element, _completed)
        { base.AnimateSpeed = 900; }

        private int oscillations=3;
        /// <summary>
        /// 抖动次数
        /// </summary>
        public int Oscillations
        {
            get { return oscillations; }
            set { oscillations = value; }
        }
        
        public override MyAnimateBase Animate()
        {
            IsAnimateCompleted = false;
            Element.RenderTransformOrigin = new Point(0.5, 0.5);
            Element.Visibility = Visibility.Visible;
            ScaleTransform translation = new ScaleTransform(1,1);
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
            var storyboardName = "aystory" + story.GetHashCode();
            Win.Resources.Add(storyboardName, story);
            double danqianX = translation.ScaleX;
            double danqianY = translation.ScaleY;
            #endregion
     


            //var k1 = new EasingDoubleKeyFrame(danqianX, TimeSpan.FromMilliseconds(0));
            var k2 = new EasingDoubleKeyFrame(danqianX + 0.3, TimeSpan.FromMilliseconds(AniTime(0.3)));
            var k3 = new EasingDoubleKeyFrame(danqianX, TimeSpan.FromMilliseconds(AniTime(1)), new ElasticEase { EasingMode = EasingMode.EaseOut, Oscillations = this.Oscillations });
            Storyboard.SetTargetName(dauX, translationName);
            Storyboard.SetTargetProperty(dauX, new PropertyPath(ScaleTransform.ScaleXProperty));

            dauX.KeyFrames.Add(k2);
            dauX.KeyFrames.Add(k3);
            story.Children.Add(dauX);

            var k0 = new EasingDoubleKeyFrame(danqianY - 0.2, TimeSpan.FromMilliseconds(0));
            var k4 = new EasingDoubleKeyFrame(danqianY - 0.2, TimeSpan.FromMilliseconds(AniTime(0.4)));
            var k5 = new EasingDoubleKeyFrame(danqianY, TimeSpan.FromMilliseconds(AniTime(1)), new ElasticEase { EasingMode = EasingMode.EaseOut, Oscillations = this.Oscillations });
            Storyboard.SetTargetName(dauY, translationName);
            Storyboard.SetTargetProperty(dauY, new PropertyPath(ScaleTransform.ScaleYProperty));
            dauY.KeyFrames.Add(k0);
            dauY.KeyFrames.Add(k4);
            dauY.KeyFrames.Add(k5);
            story.Children.Add(dauY);


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
