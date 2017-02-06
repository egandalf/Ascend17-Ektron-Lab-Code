using Ektron.Cms;
using Ektron.Cms.API;
using Ektron.Cms.Widget;
using System;

public partial class widgets_YouTubeVideo : System.Web.UI.UserControl, IWidget
{
    #region properties

    public string appPath;
    public string sitePath;

    [WidgetDataMember("")]
    public string VideoID { get; set; }

    public string MovieSourceURL
    {
        get
        {
            return string.Format("//www.youtube.com/embed/{0}", VideoID);
        }
    }

    [GlobalWidgetData()]
    public string YoutubeAPIKey { get; set; }

    #endregion
    

    IWidgetHost _host;
    protected ContentAPI _ContentAPI = new ContentAPI();


    protected void Page_Init(object sender, EventArgs e)
    {
        _host = Ektron.Cms.Widget.WidgetHost.GetHost(this);
        _host.Title = _ContentAPI.EkMsgRef.GetMessage("lbl youtubevideo widget");
        _host.Edit += new EditDelegate(EditEvent);
        _host.Create += new CreateDelegate(delegate() { EditEvent(""); });

        PreRender += new EventHandler(delegate(object PreRenderSender, EventArgs Evt) { SetOutput(); });

        // Using API to get application and site paths
        var _api = new Ektron.Cms.CommonApi();
        appPath = _api.AppPath;
        sitePath = _api.SitePath;
        
        // Register javascript and style sheet
        JS.RegisterJSInclude(tbData, JS.ManagedScript.EktronJS);
        JS.RegisterJSInclude(tbData, sitePath + "widgets/YouTubeVideo/js/YouTubeVideo.js", "EktronWidgetYouTubeJS");
        Css.RegisterCss(tbData, sitePath + "widgets/YouTubeVideo/css/YouTubeVideo.css", "YouTubecss");
        ViewSet.SetActiveView(View);

        // Localization for buttons
        SaveButton.Text = _ContentAPI.EkMsgRef.GetMessage("btn save");
        CancelButton.Text = _ContentAPI.EkMsgRef.GetMessage("btn cancel");

    }

    void EditEvent(string settings)
    {
        Ektron.Cms.API.JS.RegisterJSBlock(tbData, "Ektron.Widget.YouTubeVideo.AddWidget('" + this.ClientID + "', '" + tbData.ClientID + "', '" + SaveButton.ClientID + "');", "jsblock" + this.ClientID);
        ViewSet.SetActiveView(Edit);
    }

    protected void SetOutput()
    {
        // Display content control if there are has video Id
        var videoIDEmpty = string.IsNullOrEmpty(VideoID);
        phContent.Visible = !videoIDEmpty;
        phHelpText.Visible = videoIDEmpty;

        // Display help text if host is editable
        divHelpText.Visible = _host != null && _host.IsEditable;
    }

    protected void SaveButton_Click(object sender, EventArgs e)
    {
        VideoID = tbData.Text;
        _host.SaveWidgetDataMembers();
        ViewSet.SetActiveView(View);
    }

    protected void CancelButton_Click(object sender, EventArgs e)
    {
        ViewSet.SetActiveView(View);
    }
}








