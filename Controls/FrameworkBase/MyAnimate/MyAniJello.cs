
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
    public class MyAniJello : MyAnimateBase
    {
        public MyAniJello(UIElement _element)
            : base("jello", _element)
        { base.AnimateSpeed = 900; }

        public MyAniJello(UIElement _element, Action _completed)
            : base("jello", _element, _completed)
        { base.AnimateSpeed = 900; }

       
        public override MyAnimateBase Animate()
        {
            IsAnimateCompleted = false;
            Element.RenderTransformOrigin = new Point(0.5, 0.5);
            Element.Visibility = Visibility.Visible;

            SkewTransform translation = new SkewTransform(0, 0);
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
                translation = new SkewTransform(1, 1);
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
                    translation = item as SkewTransform;
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
                    translation = new SkewTransform(0,0);
                    translationName = "ayTranslation" + translation.GetHashCode();
                    Win.RegisterName(translationName, translation);
                    tg.Children.Add(translation);
                    Element.RenderTransform = tg;
                }
            }
            #endregion
            var storyboardName = "aystory" + story.GetHashCode();
            Win.Resources.Add(storyboardName, story);
            double danqianX = translation.AngleX;
            double danqianY = translation.AngleY;


            var k2 = new EasingDoubleKeyFrame(danqianX -12.5, TimeSpan.FromMilliseconds(AniTime(0.222)));
            var k2_0 = new EasingDoubleKeyFrame(danqianX +6.25, TimeSpan.FromMilliseconds(AniTime(0.333)));
            var k2_1 = new EasingDoubleKeyFrame(danqianX - 3.125, TimeSpan.FromMilliseconds(AniTime(0.444)));
            var k2_2 = new EasingDoubleKeyFrame(danqianX + 1.5625, TimeSpan.FromMilliseconds(AniTime(0.555)));
            var k2_3 = new EasingDoubleKeyFrame(danqianX -0.78125, TimeSpan.FromMilliseconds(AniTime(0.666)));
            var k2_4 = new EasingDoubleKeyFrame(danqianX + 0.390625, TimeSpan.FromMilliseconds(AniTime(0.777)));
            var k2_5 = new EasingDoubleKeyFrame(danqianX - 0.1953125, TimeSpan.FromMilliseconds(AniTime(0.888)));
            var k2_6 = new EasingDoubleKeyFrame(danqianX, TimeSpan.FromMilliseconds(AniTime(1)));

            var k3 = new EasingDoubleKeyFrame(danqianY - 12.5, TimeSpan.FromMilliseconds(AniTime(0.222)));
            var k3_0 = new EasingDoubleKeyFrame(danqianY + 6.25, TimeSpan.FromMilliseconds(AniTime(0.333)));
            var k3_1 = new EasingDoubleKeyFrame(danqianY - 3.125, TimeSpan.FromMilliseconds(AniTime(0.444)));
            var k3_2 = new EasingDoubleKeyFrame(danqianY + 1.5625, TimeSpan.FromMilliseconds(AniTime(0.555)));
            var k3_3 = new EasingDoubleKeyFrame(danqianY - 0.78125, TimeSpan.FromMilliseconds(AniTime(0.666)));
            var k3_4 = new EasingDoubleKeyFrame(danqianY + 0.390625, TimeSpan.FromMilliseconds(AniTime(0.777)));
            var k3_5 = new EasingDoubleKeyFrame(danqianY - 0.1953125, TimeSpan.FromMilliseconds(AniTime(0.888)));
            var k3_6 = new EasingDoubleKeyFrame(danqianY , TimeSpan.FromMilliseconds(AniTime(1)));



   
            Storyboard.SetTargetName(dauX, translationName);
            Storyboard.SetTargetProperty(dauX, new PropertyPath(SkewTransform.AngleXProperty));
            dauX.KeyFrames.Add(k2);
            dauX.KeyFrames.Add(k2_0);
            dauX.KeyFrames.Add(k2_1);
            dauX.KeyFrames.Add(k2_2);
            dauX.KeyFrames.Add(k2_3);
            dauX.KeyFrames.Add(k2_4);
            dauX.KeyFrames.Add(k2_5);
            dauX.KeyFrames.Add(k2_6);

            story.Children.Add(dauX);

            Storyboard.SetTargetName(dauY, translationName);
            Storyboard.SetTargetProperty(dauY, new PropertyPath(SkewTransform.AngleYProperty));
            dauY.KeyFrames.Add(k3);
            dauY.KeyFrames.Add(k3_0);
            dauY.KeyFrames.Add(k3_1);
            dauY.KeyFrames.Add(k3_2);
            dauY.KeyFrames.Add(k3_3);
            dauY.KeyFrames.Add(k3_4);
            dauY.KeyFrames.Add(k3_5);
            dauY.KeyFrames.Add(k3_6);
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
