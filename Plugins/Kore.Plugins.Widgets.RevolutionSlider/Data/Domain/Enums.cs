namespace Kore.Plugins.Widgets.RevolutionSlider.Data.Domain
{
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
}