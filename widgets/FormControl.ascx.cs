using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ektron.Cms;
using Ektron.Cms.API;
using Ektron.Cms.Common;
using Ektron.Cms.Framework.Content;
using Ektron.Cms.PageBuilder;
using Ektron.Cms.ToolBar;
using Ektron.Cms.Widget;
using System.Web.UI.HtmlControls;

namespace widgets
{
    public partial class WidgetsFormControl : UserControl, IWidget
    {
        #region properties

        // Localizable Strings.
        private const string WidgetTitle = "Form Control Widget";
        private const string InvalidFormId = "Invalid Form Id";
        public string DynamicParameter = "ekfrm";
        private long _formId;

        [WidgetDataMember(0)]
        public long FormId { get { return _formId; } set { _formId = value; } }

        EkRequestInformation _requestInformation;
        Ektron.Cms.PageBuilder.WidgetHost _host;
        Ektron.Cms.PageBuilder.WidgetHost _widgetHost;

        protected string AppPath;
        protected int LangType;
        protected string UniqueId;
		protected long FolderId;
        protected FormData FormSource { get; set; }

        private EkRequestInformation RequestInformation
        {
            get
            {
                if (_requestInformation == null)
                {
                    _requestInformation = ObjectFactory.GetRequestInfoProvider().GetRequestInformation();
                }
                return _requestInformation;
            }
        }

        #endregion

        /// <summary>
        /// Edit Widget Event
        /// </summary>
        /// <param name="settings"></param>
        void EditEvent(string settings)
        {
            try
            {

                string webserviceURL = _requestInformation.SitePath + "widgets/contentblock/CBHandler.ashx";
                // Register JS
                JS.RegisterJSInclude(this, JS.ManagedScript.EktronJS);
                Ektron.Cms.API.JS.RegisterJSInclude(this, Ektron.Cms.API.JS.ManagedScript.EktronClueTipJS);
                JS.RegisterJSInclude(this, JS.ManagedScript.EktronScrollToJS);
                JS.RegisterJSInclude(this, _requestInformation.SitePath + "widgets/contentblock/behavior.js", "ContentBlockWidgetBehaviorJS");

                // Insert CSS Links
                Css.RegisterCss(this, _requestInformation.SitePath + "widgets/contentblock/CBStyle.css", "CBWidgetCSS"); //cbstyle will include the other req'd stylesheets
                Ektron.Cms.Framework.UI.Packages.jQuery.jQueryUI.ThemeRoller.Register(this); //cbstyle will include the other req'd stylesheets

                JS.RegisterJSBlock(this, "Ektron.PFWidgets.ContentBlock.webserviceURL = \"" + webserviceURL + "\"; Ektron.PFWidgets.ContentBlock.setupAll('" + ClientID + "');", "EktronPFWidgetsCBInit");



                ViewSet.SetActiveView(Edit);

                if (FormId > 0)
                {
                    tbData.Text = FormId.ToString();
                    ContentAPI capi = new ContentAPI();
                    long folderid = capi.GetFolderIdForContentId(FormId);
                    tbFolderPath.Text = folderid.ToString();
                    while (folderid != 0)
                    {
                        folderid = capi.GetParentIdByFolderId(folderid);
                        if (folderid > 0) tbFolderPath.Text += "," + folderid.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                errorLb.Text = e.Message + e.Data + e.StackTrace + e.Source + e.ToString();
                _host.Title = _host.Title + " error";
                ViewSet.SetActiveView(View);
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            _host = Ektron.Cms.Widget.WidgetHost.GetHost(this) as Ektron.Cms.PageBuilder.WidgetHost;
            if (_host != null)
            {
                _host.Title = WidgetTitle;
                _host.Edit += new EditDelegate(EditEvent);
                _host.Maximize += new MaximizeDelegate(delegate() { Visible = true; });
                _host.Minimize += new MinimizeDelegate(delegate() { Visible = false; });
                _host.Create += new CreateDelegate(delegate() { EditEvent(""); });
                _host.ExpandOptions = Expandable.ExpandOnEdit;
            }

            this.EnableViewState = false;
            uxFormControl.DefaultFormId = FormId;
            Page.ClientScript.GetPostBackEventReference(SaveButton, "");
            AppPath = RequestInformation.ApplicationPath;
            LangType = RequestInformation.ContentLanguage;
            MainView();
            ViewSet.SetActiveView(View);
            BindCBTypeFilter();

            AddSubmitButton();
        }

        private void AddSubmitButton()
        {
            var controlId = "FormWidgetSubmitControl";
            if (!this.Page.IsPostBack
                && !this.Page.IsCallback
                && !this.Page.Controls.IsReadOnly
                && null == this.Page.FindControl(controlId))
            {
                var ctrl = new HtmlInputButton("submit")
                {
                    ID = controlId,
                    Name = controlId,
                    Value = "submit"
                };
                ctrl.Attributes.CssStyle.Add(HtmlTextWriterStyle.Display, "none");
                this.Page.Form.Controls.Add(ctrl);

                JS.RegisterJSBlock(this, @"
                    var pageRequestManager = Sys.WebForms.PageRequestManager.getInstance();
                    if (null != pageRequestManager)
                    {
                        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(FormWidgetPageLoadedHandler)
                        function FormWidgetPageLoadedHandler(sender, args)
                        {
                            if ('undefined' != typeof($))
                            {
                                $('#" + this.uxFormEditorsMenu.ClientID + @" input[type=""submit""]').not('[onclick]')
                                    .click(function () { $('#" + ctrl.ClientID + @"').click(); });
                            }
                        }
                    }
                ", controlId + "_SubmitScript");
            }
        }

        protected void BindCBTypeFilter()
        {
            var localResourceObject = GetLocalResourceObject("forms");
            if (localResourceObject != null)
                CBTypeFilter.Items.Add(new ListItem(localResourceObject.ToString(), "forms"));
        }

        /// <summary>
        /// Main View (Display)
        /// </summary>
        protected void MainView()
        {
            long formId = -1;
            if (FormId > -1 && !IsPostBack)
            {
                if (this.FormId == 0 && !string.IsNullOrWhiteSpace(DynamicParameter))
                {
                    if (Request[DynamicParameter] != null && long.TryParse(Request[DynamicParameter], out formId))
                    {
                        this.FormId = formId;
                    }
                }
                else if (this.FormId > 0)
                {
                    var formManager = new FormManager();
                    var langId = (!string.IsNullOrEmpty(formManager.RequestInformation.ContentLanguage.ToString(CultureInfo.InvariantCulture)) &&
                                  formManager.RequestInformation.ContentLanguage > 0)
                                     ? formManager.RequestInformation.ContentLanguage
                                     : formManager.RequestInformation.DefaultContentLanguage;
                    FormSource = formManager.GetItem(FormId, langId) ?? new FormData();

                    if (FormSource != null)
                    {
                        if (FormSource.Id > 0)
                        {
                            this.FormId = FormSource.Id;
                        }

                        this.FolderId = FormSource.FolderId;
                    }
                }
            }
            else if (Page.Request["ekfrm"] != null && long.TryParse(Page.Request["ekfrm"], out formId))
            {
                this.FormId = formId;
            }

            if (this.FormId > 0)
            {
                uxFormControl.Visible = true;
                uxFormControl.DefaultFormId = this.FormId;
            }

            uxFormEditorsMenu.ObjectId = FormId;
            uxFormEditorsMenu.EktronOnBeforeDataBind += uxFormEditorsMenu_EktronOnBeforeDataBind;
        }

        void uxFormEditorsMenu_EktronOnBeforeDataBind(object sender, ToolbarData e)
        {
            if (uxFormEditorsMenu.ToolbarData != null)
            {
                uxFormEditorsMenu.ToolbarData.Name = "Form Block";
                if (FormId <= 0)
                {
                    this.uxFormEditorsMenu.ToolbarData.Items.Find(item => item.Href.Contains("type=update")).Enabled = false;
                }
				else
                {
                    this.uxFormEditorsMenu.ToolbarData.Items.Find(item => item.Href.Contains("tab=properties")).Href = string.Format("/WorkArea/cmsform.aspx?action=ViewForm&folder_id={0}&form_id={1}&id={1}&contentid={1}&LangType={2}", this.FolderId, this.FormId, this.LangType);
                }
            }
        }

        /// <summary>
        /// Save Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            long formId = 0;
            if (long.TryParse(tbData.Text, out formId))
            {
                this.FormId = formId;
                _host.SaveWidgetDataMembers();
                MainView();

                ObjectData objectData = new ObjectData();
                objectData.ObjectId = formId;
                objectData.ObjectLanguage = _requestInformation.ContentLanguage;
                objectData.ObjectType = EkEnumeration.CMSObjectTypes.Content;

                if ((Page as PageBuilder) != null)
                {
                    _widgetHost = _host as Ektron.Cms.PageBuilder.WidgetHost;
                    _widgetHost.PBWidgetInfo.Associations.Clear();
                    _widgetHost.PBWidgetInfo.Associations.Add(objectData);
                    _widgetHost.SaveWidgetDataMembers();
                }
                else
                {
                    _host.SaveWidgetDataMembers();
                }

                MainView();
            }
            else
            {
                tbData.Text = "";
                editError.Text = InvalidFormId;
            }

            ViewSet.SetActiveView(View);
        }

        /// <summary>
        /// Cancel Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            MainView();
            ViewSet.SetActiveView(View);
        }
    }
}
