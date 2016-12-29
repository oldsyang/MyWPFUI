using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MyWPFUI.Controls
{

    public class MyAniDouble : MyAnimateTypeBase
    {
        public MyAniDouble(UIElement _element)
            : base("Double", _element)
        { base.AnimateSpeed = 600; }

        public MyAniDouble(UIElement _element, Action _completed)
            : base("Double", _element, _completed)
        { base.AnimateSpeed = 600; }

        private Double? toDouble;
        public Double? ToDouble
        {
            get { return toDouble; }
            set { toDouble = value; }
        }
        private Double? fromDouble;
        public Double? FromDouble
        {
            get { return fromDouble; }
            set { fromDouble = value; }
        }

        public override void CreateStoryboard()
        {
            DoubleAnimationUsingKeyFrames dau = new DoubleAnimationUsingKeyFrames();

            EasingDoubleKeyFrame fromk = null;
            if (FromDouble.HasValue) {
                fromk=new EasingDoubleKeyFrame(FromDouble.Value, TimeSpan.FromMilliseconds(AniTime(0)));
                dau.KeyFrames.Add(fromk);
            }

            EasingDoubleKeyFrame tok = null;
            if (ToDouble.HasValue)
            {
                tok = new EasingDoubleKeyFrame(ToDouble.Value, TimeSpan.FromMilliseconds(AniTime(1)));
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

            Storyboard.SetTarget(dau, Element);
            Storyboard.SetTargetProperty(dau, AniPropertyPath);
            Story.Children.Add(dau);
        }
    }
}
