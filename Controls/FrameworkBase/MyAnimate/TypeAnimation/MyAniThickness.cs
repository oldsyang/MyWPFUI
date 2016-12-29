using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MyWPFUI.Controls
{
    public class MyAniThickness : MyAnimateTypeBase
    {
        public MyAniThickness(UIElement _element)
            : base("Thickness", _element)
        { base.AnimateSpeed = 600; }

        public MyAniThickness(UIElement _element, Action _completed)
            : base("Thickness", _element, _completed)
        { base.AnimateSpeed = 600; }


        private Thickness? toThickness;

        public Thickness? ToThickness
        {
            get { return toThickness; }
            set { toThickness = value; }
        }

        private Thickness? fromThickness;

        public Thickness? FromThickness
        {
            get { return fromThickness; }
            set { fromThickness = value; }
        }


        public override void CreateStoryboard()
        {
            ThicknessAnimationUsingKeyFrames dau = new ThicknessAnimationUsingKeyFrames();

            EasingThicknessKeyFrame fromk = null;
            if (FromThickness.HasValue)
            {
                fromk = new EasingThicknessKeyFrame(FromThickness.Value, TimeSpan.FromMilliseconds(AniTime(0)));
                dau.KeyFrames.Add(fromk);
            }
            EasingThicknessKeyFrame tok = null;
            if (ToThickness.HasValue)
            {
                tok = new EasingThicknessKeyFrame(ToThickness.Value, TimeSpan.FromMilliseconds(AniTime(1)));
                dau.KeyFrames.Add(tok);
            }


            if (AniEasingFunction != null)
            {
                if (fromk != null) fromk.EasingFunction = AniEasingFunction;
                if (tok != null) tok.EasingFunction = AniEasingFunction;
            }
            else if (CirDefault != null)
            {
                if (fromk != null) fromk.EasingFunction = CirDefault;
                if (tok != null) tok.EasingFunction = CirDefault;
            }


            Storyboard.SetTarget(dau, Element);
            SetPropertyPath(FrameworkElement.MarginProperty);
            Storyboard.SetTargetProperty(dau, AniPropertyPath);
            Story.Children.Add(dau);
        }
    }
}
