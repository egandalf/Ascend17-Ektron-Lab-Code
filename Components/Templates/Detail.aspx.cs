using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lab;
using Ektron.Cms.Framework.Content;
using System.Reflection;
using Ektron.Cms;

public partial class Components_Templates_Detail : System.Web.UI.Page
{
    private Lazy<ContentManager> contentManager = new Lazy<ContentManager>(() => new ContentManager());

    private ContentData _content;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString.GetContentId() <= 0) return;

        _content = contentManager.Value.GetItem(Request.QueryString.GetContentId());
        if (_content == null) return;

        var xmlConfig = _content.XmlConfiguration.Id;
        switch (xmlConfig)
        {
            case Constants.SmartForms.ServiceTypeId:
                LoadControl("Service");
                break;
            case Constants.SmartForms.NewsTypeId:
                LoadControl("News");
                break;
            case Constants.EktronContentTypes.WebEvent:
                LoadControl("Event");
                break;
            default:
                LoadControl("Html");
                break;
        }
    }

    private void LoadControl(string controlName)
    {
        var controlPath = string.Format("~/Components/Controls/{0}.ascx", controlName);
        Control ctrl = Page.LoadControl(controlPath);
        PropertyInfo prop = ctrl.GetType().GetProperty("RawContent");
        prop.SetValue(ctrl, _content);
        uxControlHolder.Controls.Add(ctrl);
    }
}