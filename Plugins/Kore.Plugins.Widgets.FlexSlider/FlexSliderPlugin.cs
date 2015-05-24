﻿using Kore.Plugins.Widgets.FlexSlider.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.FlexSlider
{
    public class FlexSliderPlugin : BasePlugin
    {
        public override void Install()
        {
            base.Install();
            InstallLanguagePack<LanguagePackInvariant>();
        }

        public override void Uninstall()
        {
            base.Uninstall();
            UninstallLanguagePack<LanguagePackInvariant>();
        }
    }
}