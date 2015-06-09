﻿using System;
using System.IO;
using Kore.Web.Mvc.KoreUI.Providers;

namespace Kore.Web.Mvc.KoreUI
{
    public enum PanelSectionType : byte
    {
        Heading,
        Body,
        Footer
    }

    public class PanelSection : IDisposable
    {
        private readonly TextWriter textWriter;
        private readonly IKoreUIProvider provider;

        public PanelSectionType SectionType { get; private set; }

        internal PanelSection(IKoreUIProvider provider, PanelSectionType sectionType, TextWriter writer, string title = null)
        {
            this.provider = provider;
            this.SectionType = sectionType;
            this.textWriter = writer;
            provider.PanelProvider.BeginPanelSection(this.SectionType, this.textWriter, title);
        }

        public void Dispose()
        {
            provider.PanelProvider.EndPanelSection(this.SectionType, this.textWriter);
        }
    }
}