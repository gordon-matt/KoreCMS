namespace Kore.Plugins.Widgets.RevolutionSlider.Data.Domain
{
    #region Slides

    public enum Transition : byte
    {
        SlideUp = 0,
        SlideDown = 1,
        SlideRight = 2,
        SlideLeft = 3,
        SlideHorizontal = 4,
        SlideVertical = 5,
        BoxSlide = 6,
        SlotSlideHorizontal = 7,
        SlotSlideVertical = 8,
        BoxFade = 9,
        SlotFadeHorizontal = 10,
        SlotFadeVertical = 11,
        FadeFromRight = 12,
        FadeFromLeft = 13,
        FadeFromTop = 14,
        FadeFromBottom = 15,
        FadeToLeftFadeFromRight = 16,
        FadeToRightFadeFromLeft = 17,
        FadeToTopFadeFromBottom = 18,
        FadeToBottomFadeFromTop = 19,
        ParallaxToRight = 20,
        ParallaxToLeft = 21,
        ParallaxToTop = 22,
        ParallaxToBottom = 23,
        ScaleDownFromRight = 24,
        ScaleDownFromLeft = 25,
        ScaleDownFromTop = 26,
        ScaleDownFromBottom = 27,
        ZoomOut = 28,
        ZoomIn = 29,
        SlotZoomHorizontal = 30,
        SlotZoomVertical = 31,
        Fade = 32,
        RandomStatic = 33,
        Random = 34
    }

    public enum BackgroundRepeat : byte
    {
        NoRepeat = 0,
        Repeat = 1,
        RepeatX = 2,
        RepeatY = 3
    }

    public enum BackgroundFit : byte
    {
        Cover = 0,
        Contain = 1,
        Normal = 2,
        Custom = 3
    }

    public enum BackgroundPosition : byte
    {
        LeftTop = 0,
        LeftCenter = 1,
        LeftBottom = 2,
        CenterTop = 3,
        CenterCenter = 4,
        CenterBottom = 5,
        RightTop = 6,
        RightCenter = 7,
        RightBottom = 8
    }

    public enum EasingMethod : byte
    {
        easeInQuad = 0,
        easeOutQuad = 1,
        easeInOutQuad = 2,
        easeInCubic = 3,
        easeOutCubic = 4,
        easeInOutCubic = 5,
        easeInQuart = 6,
        easeOutQuart = 7,
        easeInOutQuart = 8,
        easeInQuint = 9,
        easeOutQuint = 10,
        easeInOutQuint = 11,
        easeInSine = 12,
        easeOutSine = 13,
        easeInOutSine = 14,
        easeInExpo = 15,
        easeOutExpo = 16,
        easeInOutExpo = 17,
        easeInCirc = 18,
        easeOutCirc = 19,
        easeInOutCirc = 20,
        easeInElastic = 21,
        easeOutElastic = 22,
        easeInOutElastic = 23,
        easeInBack = 24,
        easeOutBack = 25,
        easeInOutBack = 26,
        easeInBounce = 27,
        easeOutBounce = 28,
        easeInOutBounce = 29,
    }

    #endregion Slides

    #region Layers

    public enum IncomingAnimation : byte
    {
        ShortFromTop = 0,
        ShortFromBottom = 1,
        ShortFromRight = 2,
        ShortFromLeft = 3,
        LongFromTop = 4,
        LongFromBottom = 5,
        LongFromRight = 6,
        LongFromLeft = 7,
        SkewFromLeft = 8,
        SkewFromRight = 9,
        SkewFromLeftShort = 10,
        SkewFromRightShort = 11,
        Fade = 12,
        RandomRotate = 13
    }

    public enum OutgoingAnimation : byte
    {
        ShortToTop = 0,
        ShortToBottom = 1,
        ShortToRight = 2,
        ShortToLeft = 3,
        LongToTop = 4,
        LongToBottom = 5,
        LongToRight = 6,
        LongToLeft = 7,
        SkewToLeft = 8,
        SkewToRight = 9,
        SkewToLeftShort = 10,
        SkewToRightShort = 11,
        FadeOut = 12,
        RandomRotateOut = 13
    }

    public enum CaptionSplitType : byte
    {
        None = 0,
        Words = 1,
        Chars = 2,
        Lines = 3,
    }

    public enum AspectRatio : byte
    {
        _16x9 = 0,
        _4x3 = 1
    }

    public enum VideoPreloadOption : byte
    {
        None = 0,
        Meta = 1,
        Auto = 2
    }

    public enum VideoType : byte
    {
        Html5 = 0,
        YouTube = 1,
        Vimeo = 2
    }

    public enum VideoLoop : byte
    {
        None = 0,
        Loop = 1,
        LoopAndNoSlideStop = 2
    }

    #endregion Layers
}