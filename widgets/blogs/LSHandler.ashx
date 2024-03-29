<%@ WebHandler Language="C#" Class="LSHandler" %>

using System;
using System.Web;
using System.Text;
using Ektron.Cms;
using System.Collections.Generic;
using Ektron.Cms.API.Search;
using Ektron.Cms.WebSearch.SearchData;
using Ektron.Cms.WebSearch;
using Ektron.Cms.API.Content;
using Ektron.Cms.API;
using Ektron.Newtonsoft.Json;

#region RequestObject
[JsonObject(MemberSerialization.OptIn)]
public class Request
{
    [JsonProperty("action")]
    public string action = "";
    [JsonProperty("filter")]
    public string filter = "";
    [JsonProperty("page")]
    public int page = 0;
    [JsonProperty("searchText")]
    public string searchText = "";
    [JsonProperty("objectType")]
    public string objectType = "";
    [JsonProperty("objectID")]
    public long objectID = 0;
}
#endregion

#region ResponseObjects
[JsonObject(MemberSerialization.OptIn)]
public class DirectoryList
{
    [JsonProperty("subdirectories")]
    public List<DirectoryResult> SubDirectories = new List<DirectoryResult>();
}

[JsonObject(MemberSerialization.OptIn)]
public class ContentList
{
    [JsonProperty("contents")]
    public List<ContentResult> Contents = new List<ContentResult>();
    [JsonProperty("numpages")]
    public int Pages = 0;
    [JsonProperty("numitems")]
    public int Items = 0;
    [JsonProperty("paginglinks")]
    public string PagingLinks = "";
}

[JsonObject(MemberSerialization.OptIn)]
public class DirectoryResult
{
    [JsonProperty("name")]
    public string Name = "";
    [JsonProperty("id")]
    public long id = 0;
    [JsonProperty("haschildren")]
    public bool HasChildren = false;
    
    [JsonProperty("foldertype")]
    public int FolderType = 0;
}

[JsonObject(MemberSerialization.OptIn)]
public class ContentResult
{
    [JsonProperty("title")]
    public string Title = "";
    [JsonProperty("id")]
    public long Id = 0;
    [JsonProperty("folderid")]
    public long FolderID = 0;
}

[JsonObject(MemberSerialization.OptIn)]
public class ContentDetail
{
    [JsonProperty("title")]
    public string Title = "";
    [JsonProperty("id")]
    public long Id = 0;
    [JsonProperty("folderid")]
    public long FolderID = 0;
    [JsonProperty("summary")]
    public long Summary = 0;
    [JsonProperty("link")]
    public long link = 0;
}

[JsonObject(MemberSerialization.OptIn)]
public class Jsonexception
{
    [JsonProperty("message")]
    public string message = "";
    [JsonProperty("innerMessage")]
    public string innerMessage = "";
}
#endregion

public class LSHandler : IHttpHandler {
    private int contentPageSize = 8;
    private Request request;
    private Ektron.Cms.ContentAPI capi;
    HttpContext _context;

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        context.Response.Buffer = false;
        
        capi = new Ektron.Cms.ContentAPI();
        _context = context;
        
        try
        {
            string response = "";
            if (context.Request["request"] != null)
            {
                request = (Request)JsonConvert.DeserializeObject(context.Request["request"], typeof(Request));

                switch (request.action)
                {
                    
                    case "getchildfolders":
                        response = getchildfolders();
                        break;
                                       
                }
            }
            else if (context.Request["detail"] != null)
            {
                response = getcontenttip();

            }
            context.Response.Write(response);
        }
        catch(Exception e)
        {
            Jsonexception ex = new Jsonexception();
            ex.message = e.Message;
            if (e.InnerException != null) ex.innerMessage = e.InnerException.Message;

            context.Response.Write(JsonConvert.SerializeObject(ex));
        }
        context.Response.End();
    }

   
    public string getchildfolders()
    {
        DirectoryList directoryInfo = new DirectoryList();

        long folid = 0;
        if (request.objectType == "folder" && request.objectID > -1)
        {
            //Folder fol = new Folder();
            FolderData[] fd = capi.GetChildFolders(request.objectID, false, Ektron.Cms.Common.EkEnumeration.FolderOrderBy.Name);
            
            if (fd != null && fd.Length > 0)
            {
                foreach (FolderData f in fd)
                {
                    DirectoryResult mytd = new DirectoryResult();
                    mytd.Name = f.Name;
                    mytd.id = f.Id;
                    mytd.HasChildren = f.HasChildren;
                    mytd.FolderType = f.FolderType; 
                    directoryInfo.SubDirectories.Add(mytd);
                }
            }
            return JsonConvert.SerializeObject(directoryInfo);
        }
        return "";
    }
   
    
    public string getcontenttip()
    {
        Folder fol = new Folder();
        try
        {
            long objid;
            StringBuilder sb = new StringBuilder();
            if (long.TryParse(_context.Request["detail"], out objid))
            {
                //get whatever it is and render the summary
                Ektron.Cms.Common.ContentBase cb = capi.EkContentRef.LoadContent(objid, false);
                if (cb != null)
                {
                    sb.Append("<span>Content Details:</span><table><tr><td>");
                    sb.Append("Last Modified:");
                    sb.Append("</td><td>");
                    sb.Append(cb.DisplayDateModified);
                    sb.Append("</td></tr><tr><td>");
                    sb.Append("Last Editor:");
                    sb.Append("</td><td>");
                    sb.Append(cb.LastEditorFname + " " + cb.LastEditorLname);
                    sb.Append("</td></tr><tr><td colspan=\"2\">");
                    sb.Append("Teaser:");
                    sb.Append("<br />");
                    sb.Append(cb.Teaser);
                    sb.Append("</td></tr></table>");
                }
            }
            return sb.ToString();
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

     public bool IsReusable
    {
        get {
            return false;
        }
    }

}