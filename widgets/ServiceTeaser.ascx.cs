using Ektron.Cms;
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
using Service = Lab.SmartForms.Service.root;

public partial class widgets_ServiceTeaser : UserControl, IWidget
{
    private IWidgetHost _host;
    private PageBuilder _page = null;
    private string SitePath = "/";

    private Lazy<CommonApi> CommonApi = new Lazy<Ektron.Cms.CommonApi>(() => new Ektron.Cms.CommonApi());
    private Lazy<ContentManager> ContentManager = new Lazy<ContentManager>(() => new ContentManager());
    private ContentTypeManager<Service> _contentTypeManager = new ContentTypeManager<Service>();

    #region Properties
    [WidgetDataMember(0)]
    public long ServiceId { get; set; }
    #endregion Properties

    public widgets_ServiceTeaser()
    {
        this.ServiceId = 0;
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
                _host.Title = "Service Teaser";
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
        if(ServiceId <= 0 && (_page != null && _page.Status != Mode.Editing))
        {
            this.Visible = false;
        }
        else
        {
            InitPublishView();
        }
    }

    private void _host_Edit(string settings)
    {
        InitEditView();
        try
        {
            uxServiceSelector.SelectedValue = ServiceId.ToString();
        } catch(Exception ex)
        {
            uxServiceSelector.SelectedIndex = 0;
        }
        uxWidgetView.SetActiveView(uxEditView);
    }

    protected void InitEditView()
    {
        uxServiceSelector.Items.Clear();

        var criteria = new ContentCriteria();
        criteria.AddFilter(Ektron.Cms.Common.ContentProperty.FolderId, Ektron.Cms.Common.CriteriaFilterOperator.EqualTo, Lab.Constants.Folders.ServicesFolderId);
        criteria.AddFilter(Ektron.Cms.Common.ContentProperty.XmlConfigurationId, Ektron.Cms.Common.CriteriaFilterOperator.EqualTo, Lab.Constants.SmartForms.ServiceTypeId);
        criteria.PagingInfo = new Ektron.Cms.PagingInfo(999);
        var allServices = ContentManager.Value.GetList(criteria);

        if (!allServices.Any()) return;

        var items = allServices.Select(c => new ListItem(c.Title, c.Id.ToString())).ToList();
        items.Insert(0, new ListItem("(Select)", "0"));
        uxServiceSelector.Items.AddRange(items.ToArray());
    }

    protected void InitPublishView()
    {
        if (ServiceId <= 0)
        {
            return;
        }

        var service = _contentTypeManager.GetItem(ServiceId);
        uxServiceImage.ImageUrl = service.SmartForm.Graphic.img.src;
        uxServiceImage.AlternateText = service.SmartForm.Graphic.img.alt;

        uxServiceDescription.Text = string.Join(" ", service.SmartForm.Description.Any.GetText().Split(' ').Take(100).ToArray());

        uxServiceHeading.Text = service.Content.Title;
        uxServicePrice.Text = service.SmartForm.CostPerHour;

        uxServiceLink.HRef = service.Content.Quicklink;
        uxServiceLink.Title = service.Content.Title;

        //link.NavigateUrl = cta.SmartForm.LandingPage.a.href;
        //link.Text = cta.SmartForm.LandingPage.a.Any[0].InnerText;
    }

    protected void uxSaveButton_Click(object sender, EventArgs e)
    {
        ServiceId = long.Parse(uxServiceSelector.SelectedValue);
        _host.SaveWidgetDataMembers();
        uxWidgetView.SetActiveView(uxPublishView);
    }

    protected void uxCancelButton_Click(object sender, EventArgs e)
    {
        uxWidgetView.SetActiveView(uxPublishView);
    }
}