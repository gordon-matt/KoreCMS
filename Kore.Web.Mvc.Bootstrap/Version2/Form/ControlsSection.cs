﻿using System;
using System.IO;

namespace Kore.Web.Mvc.Bootstrap.Version2
{
    public class ControlsSection : IDisposable
    {
        private readonly TextWriter textWriter;

        internal ControlsSection(TextWriter writer)
        {
            this.textWriter = writer;
            this.textWriter.Write(@"<div class=""controls"">");
        }

        public void Dispose()
        {
            this.textWriter.Write("</div>");
        }
    }
}