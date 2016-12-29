using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MyWPFUI.Controls
{

    public class MyAniColor : MyAnimateTypeBase
    {
        public MyAniColor(UIElement _element)
            : base("Color", _element)
        { base.AnimateSpeed = 600; }

        public MyAniColor(UIElement _element, Action _completed)
            : base("Color", _element, _completed)
        { base.AnimateSpeed = 600; }

        private Color? toColor;
        public Color? ToColor
        {
            get { return toColor; }
            set { toColor = value; }
        }
        private Color? fromColor;
        public Color? FromColor
        {
            get { return fromColor; }
            set { fromColor = value; }
        }

        public override void CreateStoryboard()
        {
            ColorAnimationUsingKeyFrames dau = new ColorAnimationUsingKeyFrames();

            EasingColorKeyFrame fromk = null;
            if (FromColor.HasValue) {
                fromk=new EasingColorKeyFrame(FromColor.Value, TimeSpan.FromMilliseconds(AniTime(0)));
                dau.KeyFrames.Add(fromk);
            }

            EasingColorKeyFrame tok = null;
            if (ToColor.HasValue)
            {
                tok = new EasingColorKeyFrame(ToColor.Value, TimeSpan.FromMilliseconds(AniTime(1)));
                dau.KeyFrames.Add(tok);
            }


            if (AniEasingFunction != null)
            {
                if (fromk!=null) fromk.EasingFunction = AniEasingFunction;
                if (tok!=null) tok.EasingFunction = AniEasingFunction;
            }
            else if (CirDefault != null)
            {
                if (fromk != null) fromk.EasingFunction = CirDefault;
                if (tok != null) tok.EasingFunction = CirDefault;
            }

            //Storyboard.SetTargetName(dau, ElementName);
            Storyboard.SetTarget(dau, Element);
            Storyboard.SetTargetProperty(dau, AniPropertyPath);
            Story.Children.Add(dau);
        }
    }
}
