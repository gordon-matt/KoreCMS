﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kore.Web.Mvc.KoreUI
{
    public interface IToolbarProvider
    {
        string ToolbarTag { get; }

        void BeginToolbar(Toolbar toolbar);

        void BeginButtonGroup(TextWriter writer);

        void EndButtonGroup(TextWriter writer);

        void AddButton(TextWriter writer, string text, State state, string onClick = null, object htmlAttributes = null);
    }
}
