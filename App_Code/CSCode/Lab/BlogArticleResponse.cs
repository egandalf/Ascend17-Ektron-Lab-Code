using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab
{
    /// <summary>
    /// Summary description for BlogArticleResponse
    /// </summary>
    public class BlogArticleResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime Published { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string Teaser { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }

        public BlogArticleResponse()
        {
            Published = DateTime.MinValue;
        }
    }
}