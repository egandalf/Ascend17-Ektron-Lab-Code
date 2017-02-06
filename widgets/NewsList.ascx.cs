using Ektron.Cms;
using Ektron.Cms.API;
using Ektron.Cms.BusinessObjects.Organization;
using Ektron.Cms.Common;
using Ektron.Cms.Content;
using Ektron.Cms.Framework.Content;
using Ektron.Cms.PageBuilder;
using Ektron.Cms.Widget;
using Lab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using News = Lab.SmartForms.News.root;

public partial class widgets_NewsList : UserControl, IWidget
{
    private IWidgetHost _host;
    private PageBuilder _page = null;
    private string SitePath = "/";

    private Lazy<CommonApi> CommonApi = new Lazy<Ektron.Cms.CommonApi>(() => new Ektron.Cms.CommonApi());
    private Lazy<ContentManager> ContentManager = new Lazy<ContentManager>(() => new ContentManager());
    private Lazy<FolderManager> FolderManager = new Lazy<FolderManager>(() => new FolderManager());
    private Lazy<ContentTypeManager<News>> _contentTypeManager = new Lazy<ContentTypeManager<News>>(() => new ContentTypeManager<News>());

    #region Properties
    [WidgetDataMember(0)]
    public long NewsFolderId { get; set; }
    #endregion Properties

    public widgets_NewsList()
    {
        this.NewsFolderId = 0;
    }

    protected override void OnInit(EventArgs e)
    {
        SitePath = CommonApi.Value.SitePath;
        try
        {
            _host = Ektron.Cms.Widget.WidgetHost.GetHost(this);
            if(_host != null)
            {
                _page = Page as PageBuilder;
                _host.Title = "News List";
                _host.Edit += _host_Edit;
                _host.Maximize += new MaximizeDelegate(() => { Visible = true; });
                _host.Minimize += new MinimizeDelegate(() => { Visible = false; });
                _host.Create += new CreateDelegate(() => { _host_Edit(""); });
            }
        } catch (Exception ex)
        {
            // not used as a widget, do nothing
        }
        uxWidgetView.SetActiveView(uxPublishView);
    }

    protected override void OnPreRender(EventArgs e)
    {
        InitPublishView();
    }

    private void _host_Edit(string settings)
    {
        InitEditView();
        try
        {
            uxFolderSelector.SelectedValue = NewsFolderId.ToString();
        } catch(Exception ex)
        {
            uxFolderSelector.SelectedIndex = 0;
        }
        uxWidgetView.SetActiveView(uxEditView);
    }

    protected void InitEditView()
    {
        uxFolderSelector.Items.Clear();

        var criteria = new FolderCriteria(FolderProperty.FolderName, EkEnumeration.OrderByDirection.Ascending);
        criteria.AddFilter(FolderProperty.ParentId, CriteriaFilterOperator.EqualTo, Constants.Folders.PrimaryContentFolder);
        var folders = FolderManager.Value.GetList(criteria);

        var items = folders.Select(f => new ListItem(f.Name, f.Id.ToString())).ToList();
        items.Insert(0, new ListItem("(Select)", "0"));
        uxFolderSelector.Items.AddRange(items.ToArray());
    }

    protected void InitPublishView()
    {
        long folder = Constants.Folders.NewsFolderId;
        if (NewsFolderId > 0) folder = NewsFolderId;

        var criteria = new ContentMetadataCriteria();
        criteria.AddFilter(ContentProperty.FolderId, CriteriaFilterOperator.EqualTo, folder);
        criteria.AddFilter(ContentProperty.XmlConfigurationId, CriteriaFilterOperator.EqualTo, Constants.SmartForms.NewsTypeId);
        criteria.PagingInfo = new PagingInfo(999);
        var list = _contentTypeManager.Value.GetList(criteria);
        uxNewsList.DataSource = list;
        uxNewsList.DataBind();
    }

    protected void uxSaveButton_Click(object sender, EventArgs e)
    {
        NewsFolderId = long.Parse(uxFolderSelector.SelectedValue);
        _host.SaveWidgetDataMembers();
        uxWidgetView.SetActiveView(uxPublishView);
    }

    protected void uxCancelButton_Click(object sender, EventArgs e)
    {
        uxWidgetView.SetActiveView(uxPublishView);
    }
}