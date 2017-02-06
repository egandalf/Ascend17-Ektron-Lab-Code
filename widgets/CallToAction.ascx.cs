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
using CallToAction = Lab.SmartForms.CallToAction.root;

public partial class widgets_CallToAction : UserControl, IWidget
{
    private IWidgetHost _host;
    private PageBuilder _page = null;
    private string SitePath = "/";

    private Lazy<CommonApi> CommonApi = new Lazy<Ektron.Cms.CommonApi>(() => new Ektron.Cms.CommonApi());
    private Lazy<ContentManager> ContentManager = new Lazy<ContentManager>(() => new ContentManager());
    private ContentTypeManager<CallToAction> _contentTypeManager = new ContentTypeManager<CallToAction>();

    #region Properties
    [WidgetDataMember(0)]
    public long CallToActionId { get; set; }
    #endregion Properties

    public widgets_CallToAction()
    {
        this.CallToActionId = 0;
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
                _host.Title = "Call to Action";
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
        if(CallToActionId <= 0 && (_page != null && _page.Status != Mode.Editing))
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
            uxCtaSelector.SelectedValue = CallToActionId.ToString();
        } catch(Exception ex)
        {
            uxCtaSelector.SelectedIndex = 0;
        }
        uxWidgetView.SetActiveView(uxEditView);
    }

    protected void InitEditView()
    {
        uxCtaSelector.Items.Clear();

        var criteria = new ContentCriteria();
        criteria.AddFilter(Ektron.Cms.Common.ContentProperty.FolderId, Ektron.Cms.Common.CriteriaFilterOperator.EqualTo, Lab.Constants.Folders.CtaFolderId);
        criteria.AddFilter(Ektron.Cms.Common.ContentProperty.XmlConfigurationId, Ektron.Cms.Common.CriteriaFilterOperator.EqualTo, Lab.Constants.SmartForms.CtaTypeId);
        criteria.PagingInfo = new Ektron.Cms.PagingInfo(999);
        var allCtas = ContentManager.Value.GetList(criteria);

        if (!allCtas.Any()) return;

        var items = allCtas.Select(c => new ListItem(c.Title, c.Id.ToString())).ToList();
        items.Insert(0, new ListItem("(Select)", "0"));
        uxCtaSelector.Items.AddRange(items.ToArray());
    }

    protected void InitPublishView()
    {
        var panel = uxPublishView.FindControl("uxCtaPanel") as Panel;
        var image = panel.FindControl("uxCtaImage") as Image;
        //var heading = panel.FindControl("uxCtaHeading") as Literal;
        var text = panel.FindControl("uxCtaText") as Literal;
        var link = panel.FindControl("uxCtaLink") as HyperLink;

        if (CallToActionId <= 0)
        {
            image.ImageUrl = "";
            image.AlternateText = "";
            link.NavigateUrl = "";
            link.Text = "";
            return;
        }

        var cta = _contentTypeManager.GetItem(CallToActionId);
        image.ImageUrl = cta.SmartForm.Graphic.img.src;
        image.AlternateText = cta.SmartForm.Graphic.img.alt;

        text.Text = cta.SmartForm.Text;

        link.NavigateUrl = cta.SmartForm.LandingPage.a.href;
        link.Text = cta.SmartForm.LandingPage.a.Any[0].InnerText;
    }

    protected void uxSaveButton_Click(object sender, EventArgs e)
    {
        CallToActionId = long.Parse(uxCtaSelector.SelectedValue);
        _host.SaveWidgetDataMembers();
        uxWidgetView.SetActiveView(uxPublishView);
    }

    protected void uxCancelButton_Click(object sender, EventArgs e)
    {
        uxWidgetView.SetActiveView(uxPublishView);
    }
}