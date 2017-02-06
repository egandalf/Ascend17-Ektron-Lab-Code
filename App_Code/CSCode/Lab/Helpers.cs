using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace Lab
{
    /// <summary>
    /// Summary description for Helpers
    /// </summary>
    public static class Helpers
    {
        public static long GetContentId(this NameValueCollection pair)
        {
            long id = 0;
            if (!string.IsNullOrEmpty(pair["id"]) && long.TryParse(pair["id"], out id)) return id;
            else if (!string.IsNullOrEmpty(pair["pageid"]) && long.TryParse(pair["pageid"], out id)) return id;
            else if (!string.IsNullOrEmpty(pair["ekfrm"]) && long.TryParse(pair["ekfrm"], out id)) return id;
            return id;
        }

        public static string GetRichString(this XmlNode[] nodeArray)
        {
            return (nodeArray != null && nodeArray.Length > 0) ? nodeArray.Aggregate(new StringBuilder(), (sb, node) => sb.AppendLine(node.OuterXml), sb => sb.ToString()) : string.Empty;
        }

        public static string GetText(this XmlNode[] nodeArray)
        {
            return (nodeArray != null && nodeArray.Any()) ? nodeArray.Aggregate(new StringBuilder(), (sb, node) => sb.Append(node.InnerText), sb => sb.ToString()) : string.Empty;
        }
    }
}