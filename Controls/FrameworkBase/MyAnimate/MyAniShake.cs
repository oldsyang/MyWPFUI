
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
    public class MyAniShake : MyAnimateBase
    {
        public MyAniShake(UIElement _element)
            : base("shake", _element)
        {
            base.AnimateSpeed = 320;
        }

        public MyAniShake(UIElement _element, Action _completed)
            : base("shake", _element, _completed)
        {
            base.AnimateSpeed = 320;
        }
        private double moveWidth=12;

        public double MoveWidth
        {
            get { return moveWidth; }
            set { moveWidth = value; }
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
            double danqianX = translation.X;
            var k2 = new EasingDoubleKeyFrame((danqianX - MoveWidth), TimeSpan.FromMilliseconds(AniTime(0.25)));
            var k2_1 = new EasingDoubleKeyFrame(danqianX, TimeSpan.FromMilliseconds(AniTime(0.5)));
            var k2_2 = new EasingDoubleKeyFrame((danqianX + MoveWidth), TimeSpan.FromMilliseconds(AniTime(0.75)));
            var k2_3 = new EasingDoubleKeyFrame(danqianX, TimeSpan.FromMilliseconds(AniTime(1)));
         
            Storyboard.SetTargetName(dau, translationName);
            Storyboard.SetTargetProperty(dau, new PropertyPath(TranslateTransform.XProperty));

            var storyboardName = "aystory" + story.GetHashCode();
            Win.Resources.Add(storyboardName, story);

            dau.KeyFrames.Add(k2);
            dau.KeyFrames.Add(k2_1);
            dau.KeyFrames.Add(k2_2);
            dau.KeyFrames.Add(k2_3);;
            dau.RepeatBehavior = new RepeatBehavior(3);
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
