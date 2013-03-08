using System.Collections.Generic;

using HtmlAgilityPack;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    public static class HtmlDocumentExtensions
    {
        /// <summary>
        /// Wraps the root text nodes with specified tag.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="nodeName">Name of the node.</param>
        public static void WrapRootTextNodes(this HtmlDocument document, string nodeName = "bcms")
        {
            WrapRootTextNodes(document.DocumentNode, document, nodeName);
        }

        /// <summary>
        /// Wraps the root text nodes with specified tag.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="doc">The doc.</param>
        /// <param name="nodeName">Name of the node.</param>
        private static void WrapRootTextNodes(HtmlNode node, HtmlDocument doc, string nodeName)
        {
            var replacements = new Dictionary<HtmlNode, HtmlNode>();
            foreach (HtmlNode subnode in node.ChildNodes)
            {
                if (subnode.NodeType == HtmlNodeType.Text)
                {
                    var newNode = doc.CreateElement(nodeName);
                    newNode.AppendChild(subnode);

                    replacements.Add(subnode, newNode);
                }
            }
            foreach (var pair in replacements)
            {
                node.ReplaceChild(pair.Value, pair.Key);
            }
        }
    }
}