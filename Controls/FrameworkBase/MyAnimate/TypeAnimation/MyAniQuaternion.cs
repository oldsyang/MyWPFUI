using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace MyWPFUI.Controls
{

    public class MyAniQuaternion : MyAnimateTypeBase
    {
        public MyAniQuaternion(UIElement _element)
            : base("Quaternion", _element)
        { base.AnimateSpeed = 600; }

        public MyAniQuaternion(UIElement _element, Action _completed)
            : base("Quaternion", _element, _completed)
        { base.AnimateSpeed = 600; }

        private Quaternion? toQuaternion;
        public Quaternion? ToQuaternion
        {
            get { return toQuaternion; }
            set { toQuaternion = value; }
        }
        private Quaternion? fromQuaternion;
        public Quaternion? FromQuaternion
        {
            get { return fromQuaternion; }
            set { fromQuaternion = value; }
        }

        public override void CreateStoryboard()
        {
            QuaternionAnimationUsingKeyFrames dau = new QuaternionAnimationUsingKeyFrames();

            EasingQuaternionKeyFrame fromk = null;
            if (FromQuaternion.HasValue) {
                fromk=new EasingQuaternionKeyFrame(FromQuaternion.Value, TimeSpan.FromMilliseconds(AniTime(0)));
                dau.KeyFrames.Add(fromk);
            }

            EasingQuaternionKeyFrame tok = null;
            if (ToQuaternion.HasValue)
            {
                tok = new EasingQuaternionKeyFrame(ToQuaternion.Value, TimeSpan.FromMilliseconds(AniTime(1)));
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
