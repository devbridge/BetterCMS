/*
    Copyright 2010 CodePlex Foundation, Inc.
    Copyright 2007-2010 Eric Hexter, Jeffrey Palermo
    For Licence, see http://www.codeplex.com/MVCContrib/license

    Modified by Devbridge Group: updated source code to support ASP.NET 5 MVC6
*/

using System.IO;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewEngines;

namespace BetterCms.Core.Mvc.Grid
{
    /// <summary>
    /// Context class used when rendering the grid.
    /// </summary>
    public class RenderingContext
    {
        public TextWriter Writer { get; private set; }
        public ViewContext ViewContext { get; private set; }
        public ICompositeViewEngine CompositeViewEngine { get; private set; }

        public RenderingContext(TextWriter writer, ViewContext viewContext, ICompositeViewEngine compositeViewEngine)
        {
            Writer = writer;
            ViewContext = viewContext;
            CompositeViewEngine = compositeViewEngine;
        }
    }
}