using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace MyWPFUI.Controls.Models
{
    public class MyLayerOptions
    {
        private bool canDrag = true;
        [Description("是否可移动")]
        public bool CanDrag
        {
            get { return canDrag; }
            set { canDrag = value; }
        }
        private string _layerId;
        /// <summary>
        /// 层Id
        /// </summary>
        public string LayerId
        {
            get { return _layerId; }
            set { _layerId = value; }
        }
        private bool isShowTitleBar = true;
        /// <summary>
        /// 是否包含标题栏
        /// </summary>
        public bool IsContainsTitleBar
        {
            get { return isShowTitleBar; }
            set { isShowTitleBar = value; }
        }
        private Brush maskBrush = null;
        /// <summary>
        /// 遮盖画刷
        /// </summary>
        public Brush MaskBrush
        {
            get { return maskBrush; }
            set { maskBrush = value; }
        }

        private bool hasShadow = true;
        /// <summary>
        /// 是否有阴影
        /// </summary>
        public bool HasShadow
        {
            get { return hasShadow; }
            set { hasShadow = value; }
        }

        private double _width;
        /// <summary>
        /// 宽度
        /// </summary>
        public double Width
        {
            get { return _width; }
            set { _width = value; }
        }
        private double _height;
        /// <summary>
        /// 高度
        /// </summary>
        public double Height
        {
            get { return _height; }
            set { _height = value; }
        }
        private double _minWidth;
        /// <summary>
        /// 最小宽度
        /// </summary>
        public double MinWidth
        {
            get { return _minWidth; }
            set { _minWidth = value; }
        }
        private double _minHeight;
        /// <summary>
        /// 最小高度
        /// </summary>
        public double MinHeight
        {
            get { return _minHeight; }
            set { _minHeight = value; }
        }
        private double _maxWidth;
        /// <summary>
        /// 最大宽度
        /// </summary>
        public double MaxWidth
        {
            get { return _maxWidth; }
            set { _maxWidth = value; }
        }
        private double _maxHeight;
        /// <summary>
        /// 最大高度
        /// </summary>
        public double MaxHeight
        {
            get { return _maxHeight; }
            set { _maxHeight = value; }
        }
        /// <summary>
        /// 填入1-15，目前15种开场动画
        /// </summary>
        public int ShowAnimateIndex
        {
            get { return (int)this.AnimationType; }
        }

        public AnimationType AnimationType { get; set; }
        private double shadowRadius = 27.00;

        public double ShadowRadius
        {
            get { return shadowRadius; }
            set { shadowRadius = value; }
        }
        private Color shodowColor = SolidColorBrushConverter.ToColor("#C6030303");

        public Color ShadowColor
        {
            get { return shodowColor; }
            set { shodowColor = value; }
        }
        private double shadowDepth = 0;

        public double ShadowDepth
        {
            get { return shadowDepth; }
            set { shadowDepth = value; }
        }
        private bool isShowLayerBorder = false;

        public bool IsShowLayerBorder
        {
            get { return isShowLayerBorder; }
            set { isShowLayerBorder = value; }
        }
        private Thickness? layerBorderThickness;

        public Thickness? LayerBorderThickness
        {
            get { return layerBorderThickness; }
            set { layerBorderThickness = value; }
        }
        public CornerRadius? LayerCornerRadius { get; set; }

        private Brush layerBackground = new SolidColorBrush(Colors.White);

        public Brush LayerBackground
        {
            get { return layerBackground; }
            set { layerBackground = value; }
        }
    }

    public class MYUI
    {
        private static readonly object sync = new object();
        public static MyLayerOptions _defaultAyLayerOptions;
        public static MyLayerOptions DefaultAyLayerOptions
        {
            get
            {
                if (_defaultAyLayerOptions == null)
                {
                    lock (sync)
                    {
                        if (_defaultAyLayerOptions == null)
                        {
                            _defaultAyLayerOptions = new MyLayerOptions();
                            _defaultAyLayerOptions.CanDrag = true;
                            _defaultAyLayerOptions.HasShadow = true;
                        }
                    }
                }
                return _defaultAyLayerOptions;
            }
        }
    }

    public enum AnimationType
    {
        [Description("正常")]
        Normal=0,
        [Description("由远到近")]
        FromFarToNear = 1,
        [Description("从上侧进入")]
        InFormUp = 2,
        [Description("从下侧进入")]
        InFormDown = 3,
        [Description("从左侧进入")]
        InFormLeft = 4,
        [Description("从右侧进入")]
        InFormRight = 5,
        [Description("从上侧进入(带回弹)")]
        InFormUpWithBounce = 6,
        [Description("从下侧进入(带回弹)")]
        InFormDownWithBounce = 7,
        [Description("从左侧进入(带回弹)")]
        InFormLeftWithBounce = 8,
        [Description("从右侧进入(带回弹)")]
        InFormRightWithBounce = 9,
        [Description("背景由浅到深的旋转入场")]
        Rotate=10,
        [Description("背景由浅到深的弹跳")]
        Bounce = 11,

    }
}
