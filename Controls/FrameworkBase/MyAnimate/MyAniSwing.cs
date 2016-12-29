
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
    public class MyAniSwing : MyAnimateBase
    {
        public MyAniSwing(UIElement _element)
            : base("swing", _element)
        { base.AnimateSpeed = 1000; }

        public MyAniSwing(UIElement _element, Action _completed)
            : base("swing", _element, _completed)
        {
            base.AnimateSpeed = 1000;
        }


        public override MyAnimateBase Animate()
        {
            IsAnimateCompleted = false;
            Element.RenderTransformOrigin = new Point(0.5, 0);
            Element.Visibility = Visibility.Visible;

            RotateTransform translation = new RotateTransform();
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
                translation = new RotateTransform();
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
                    translation = item as RotateTransform;
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
                    translation = new RotateTransform();
                    translationName = "ayTranslation" + translation.GetHashCode();
                    Win.RegisterName(translationName, translation);
                    tg.Children.Add(translation);
                    Element.RenderTransform = tg;
                }
            }
            #endregion

            double angle = translation.Angle;
            var k2 = new EasingDoubleKeyFrame(angle + 15, TimeSpan.FromMilliseconds(AniTime(0.2)));
            var k2_1 = new EasingDoubleKeyFrame(angle - 10, TimeSpan.FromMilliseconds(AniTime(0.4)));
            var k2_2 = new EasingDoubleKeyFrame(angle + 5, TimeSpan.FromMilliseconds(AniTime(0.6)));
            var k2_3 = new EasingDoubleKeyFrame(angle - 5, TimeSpan.FromMilliseconds(AniTime(0.8)));
            var k2_4 = new EasingDoubleKeyFrame(angle, TimeSpan.FromMilliseconds(AniTime(1)));

            Storyboard.SetTargetName(dau, translationName);
            Storyboard.SetTargetProperty(dau, new PropertyPath(RotateTransform.AngleProperty));

            var storyboardName = "aystory" + story.GetHashCode();
            Win.Resources.Add(storyboardName, story);

            dau.KeyFrames.Add(k2);
            dau.KeyFrames.Add(k2_1);
            dau.KeyFrames.Add(k2_2);
            dau.KeyFrames.Add(k2_3);
            dau.KeyFrames.Add(k2_4);
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
