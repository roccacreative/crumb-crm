using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrumbCRM.Web.Helpers
{
    public static class HtmlHelper
    {
        public static string CleanHtml(string html)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            doc.DocumentNode.Descendants()
                            .Where(n => n.Name == "script" || n.Name == "style")
                            .ToList()
                            .ForEach(n => n.Remove());

            return doc.DocumentNode.OuterHtml.ToString();
        }

        public static string StripHtml(string html)
        {
            if (!string.IsNullOrEmpty(html))
            {
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);
                return doc.DocumentNode.InnerText;
            }

            return html;
        }

    }

}