namespace Ektron.Cms.CampaignManagement.LandingPages.Widgets
{
    using Ektron.Cms.Framework.UI;
    using Ektron.Cms.Instrumentation;
    using Ektron.Cms.Interfaces.Context;
    using Ektron.Cms.Widget;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class StaticContentBlock : System.Web.UI.UserControl, IWidget
    {
        #region properties
        [WidgetDataMember()]
        public string ContentString { get; set; }

        [GlobalWidgetData()]
        public string DefaultContent { get; set; }

        [GlobalWidgetData("Static Content Widget")]
        public string WidgetTitle { get; set; }

        #endregion

        #region protected members
        protected string AlohaContentControlClientID
        {
            get
            {
                return this.Aloha.ContentControl.ClientID;
                //return String.Empty;
            }
        }
        protected string HiddenStaticHtmlClientId
        {
            get
            {
                return this.StaticHtml.ClientID;
            }
        }
        #endregion

        #region private members
        private IWidgetHost Host;
        private ILogWriter Logger;
        #endregion
        protected void Page_Init(object sender, EventArgs e)
        {

            this.Logger = ObjectFactory.Get<ILogWriter>();

            this.Host = Ektron.Cms.Widget.WidgetHost.GetHost(this);
            this.Host.Title = this.WidgetTitle;
            this.Host.Edit += new EditDelegate(EditEvent);
            this.Host.Create += new CreateDelegate(() => { EditEvent(string.Empty); });
            SetLocalResourceValues();
            RegisterResources();
            DisplayView();
        }

        private void RegisterResources()
        {
            var cmsContext = ObjectFactory.Get<ICmsContextService>();
            Css.Register(this, cmsContext.SitePath + "/widgets/StaticContentBlock/css/staticcontentblock.css");
            Css.Register(this, cmsContext.SitePath + "/Workarea/csslib/ektron.workarea.css");
            Packages.Ektron.Namespace.Register(this);
        }

        private void SetLocalResourceValues()
        {
            try
            {
                this.Save.Text = GetLocalResourceObject("SaveButton").ToString();
                this.Cancel.Text = GetLocalResourceObject("CancelButton").ToString();
            }
            catch (Exception ex)
            {
                if (this.Logger != null)
                {
                    this.Logger.WriteError(ex);
                }
                throw;
            }
        }

        private void DisplayView()
        {
            this.AlohaJavascriptHandlerBlock.Visible = false;
            this.StaticHtml.Visible = false;
            this.DisplayContent.InnerHtml = GetContentString();
            SetView(this.Display);
        }

        private void SetView(View view)
        {
            this.Views.SetActiveView(view);
        }

        private void EditEvent(string settings)
        {
            this.AlohaJavascriptHandlerBlock.Visible = true;
            this.StaticHtml.Visible = true;
            this.Aloha.ContentControl.InnerHtml = GetContentString();

            SetView(this.Edit);
        }

        private void SaveContentString()
        {
            try
            {
                this.ContentString = Uri.UnescapeDataString(this.StaticHtml.Value);

                this.Host.SaveWidgetDataMembers();
            }
            catch (Exception ex)
            {
                if (this.Logger != null)
                {
                    this.Logger.WriteError(ex);
                }
                throw;
            }
        }

        private string GetContentString()
        {
            try
            {
                return !string.IsNullOrEmpty(this.ContentString) ? this.ContentString :
                    this.DefaultContent ?? GetLocalResourceObject("DefaultContent").ToString();
            }
            catch (Exception ex)
            {
                if (this.Logger != null)
                {
                    this.Logger.WriteError(ex);
                }
                throw;
            }
        }

        #region page element events
        protected void SaveClick(object sender, EventArgs e)
        {
            SaveContentString();
            DisplayView();
        }

        protected void CancelClick(object sender, EventArgs e)
        {
            DisplayView();
        }


        #endregion
    }
}