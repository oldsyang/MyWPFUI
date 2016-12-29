﻿using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MyWPFUI.Controls
{

    public class MyAniSize : MyAnimateTypeBase
    {
        public MyAniSize(UIElement _element)
            : base("Size", _element)
        { base.AnimateSpeed = 600; }

        public MyAniSize(UIElement _element, Action _completed)
            : base("Size", _element, _completed)
        { base.AnimateSpeed = 600; }

        private Size? toSize;
        public Size? ToSize
        {
            get { return toSize; }
            set { toSize = value; }
        }
        private Size? fromSize;
        public Size? FromSize
        {
            get { return fromSize; }
            set { fromSize = value; }
        }

        public override void CreateStoryboard()
        {
            SizeAnimationUsingKeyFrames dau = new SizeAnimationUsingKeyFrames();

            EasingSizeKeyFrame fromk = null;
            if (FromSize.HasValue) {
                fromk=new EasingSizeKeyFrame(FromSize.Value, TimeSpan.FromMilliseconds(AniTime(0)));
                dau.KeyFrames.Add(fromk);
            }

            EasingSizeKeyFrame tok = null;
            if (ToSize.HasValue)
            {
                tok = new EasingSizeKeyFrame(ToSize.Value, TimeSpan.FromMilliseconds(AniTime(1)));
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
