<%@ WebHandler Language="C#" Class="BlogItemNameHandler" %>

using System;
using System.IO;
using System.Text;
using System.Web;
using System.Linq;
using Newtonsoft.Json;
using Lab;
using Ektron.Cms.Content;
using Ektron.Cms.Common;
using Ektron.Cms.Framework.Content;

public class BlogItemNameHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.ContentEncoding = Encoding.Default;

        if (context.Request.HttpMethod != "POST")
        {
            context.Response.Write(JsonConvert.SerializeObject(new { Error = "This endpoint only accepts POST requests." }));
            return;
        }

        string value = null;
        using (var reader = new StreamReader(context.Request.InputStream))
        {
            value = reader.ReadToEnd();
        }

        if (string.IsNullOrEmpty(value))
        {
            context.Response.Write(JsonConvert.SerializeObject(new { Error = "There is no content title value in the request body." }));
            return;
        }

        var contentManager = new ContentManager();
        var criteria = new ContentCriteria();
        criteria.PagingInfo = new Ektron.Cms.PagingInfo(1);
        criteria.AddFilter(ContentProperty.Title, CriteriaFilterOperator.EqualTo, value);
        criteria.AddFilter(ContentProperty.FolderId, CriteriaFilterOperator.EqualTo, Constants.Folders.BlogFolderId);
        var content = contentManager.GetList(criteria).FirstOrDefault();

        if (content == null)
        {
            context.Response.Write(JsonConvert.SerializeObject(new { Error = "The item could not be found." }));
            return;
        }

        content = contentManager.GetItem(content.Id, true);

        var blogSubjectMeta = content.MetaData.FirstOrDefault(m => m.Name == "Blog Categories");

        var result = new BlogArticleResponse
        {
            Id = content.Id,
            Name = content.Title,
            Published = content.DateCreated,
            Body = content.Html,
            Subject = blogSubjectMeta == null ? string.Empty : blogSubjectMeta.Text,
            Teaser = content.Teaser,
            MetaTitle = content.MetaData.First(m => m.Id == Constants.Metadata.MetaTitleId).Text,
            MetaKeywords = content.MetaData.First(m => m.Id == Constants.Metadata.MetaKeywordsId).Text,
            MetaDescription = content.MetaData.First(m => m.Id == Constants.Metadata.MetaDescriptionId).Text
        };

        context.Response.Write(JsonConvert.SerializeObject(result));
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}