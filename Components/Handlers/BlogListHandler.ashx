<%@ WebHandler Language="C#" Class="BlogListHandler" %>

using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Ektron.Cms;
using Ektron.Cms.Common;
using Ektron.Cms.Content;
using Ektron.Cms.Framework.Content;
using Newtonsoft.Json;
using Lab;

public class BlogListHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";

        var contentManager = new ContentManager();
        var criteria = new ContentCriteria();
        criteria.AddFilter(ContentProperty.FolderId, CriteriaFilterOperator.EqualTo, Constants.Folders.BlogFolderId);
        criteria.OrderByDirection = EkEnumeration.OrderByDirection.Descending;
        criteria.OrderByField = ContentProperty.DateCreated;
        criteria.PagingInfo = new PagingInfo(9999);
        criteria.ReturnMetadata = true;

        var contentList = contentManager.GetList(criteria);
        

        // returning limited data as metadata isn't needed for listing the items.
        var result = contentList.Select(c => new BlogArticleResponse
        {
            Id = c.Id,
            Name = c.Title,
            Published = c.DateCreated,
            Subject = c.MetaData.Any(m => m.Name == "Blog Categories") ? c.MetaData.First(m => m.Name == "Blog Categories").Text : string.Empty,
            Teaser = c.Teaser
        });

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