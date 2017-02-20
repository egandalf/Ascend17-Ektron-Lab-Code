<%@ WebHandler Language="C#" Class="BlogItemHandler" %>

using System.Web;
using System.Linq;
using System.Text;
using Ektron.Cms.Framework.Content;
using Newtonsoft.Json;
using Lab;

public class BlogItemHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.ContentEncoding = Encoding.Default;

        long id;
        if (string.IsNullOrEmpty(context.Request.QueryString["id"]) || !long.TryParse(context.Request.QueryString["id"], out id))
        {
            context.Response.Write(JsonConvert.SerializeObject(new { Error = "ID is missing or unable to parse." }));
            return;
        }

        var contentManager = new ContentManager();
        var content = contentManager.GetItem(id, true);

        if (content == null || content.FolderId != Constants.Folders.BlogFolderId)
        {
            context.Response.Write(JsonConvert.SerializeObject(new { Error = "Content returned is not part of the Blog folder and cannot be loaded via this handler." }));
            return;
        }

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