namespace Ektron.Cms.CampaignManagement.LandingPages.Widgets
{
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;
    using Ektron.Cms.Widget;
    using Ektron.Cms;
    using Ektron.Cms.API;
    using Ektron.Cms.Common;
    using Ektron.Cms.PageBuilder;
    using System.Configuration;
    using Ektron.Cms.Framework.UI;
    using Ektron.Cms.Instrumentation;


    public partial class Heading : System.Web.UI.UserControl, IWidget
    {
        #region properties

        [WidgetDataMember()]
        public string HeadingString { get; set; }

        [GlobalWidgetData("Your Heading")]
        public string DefaulthHeading { get; set; }

        [GlobalWidgetData("Heading Widget")]
        public string WidgetTitle { get; set; }

        #endregion

        #region protected members
        protected string WrapperClientID
        {
            get
            {
                return this.wrapper.ClientID;
            }
        }
        protected string HiddenStaticHtmlClientID
        {
            get
            {
                return this.StaticHtml.ClientID;
            }
        }
        protected string ButtonWrapperClientID
        {
            get
            {
                return this.buttonWrapper.ClientID;
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
            RegisterResources();
            DisplayView();
        }

        private void RegisterResources()
        {
            Packages.Ektron.Namespace.Register(this);
            Packages.EktronCoreJS.Register(this);
            Packages.jQuery.jQueryUI.Complete.Register(this);
            Packages.jQuery.jQueryUI.ThemeRoller.Register(this);
        }

        private void DisplayView()
        {
            this.AlohaJavascriptHandlerBlock.Visible = false;
            this.StaticHtml.Visible = false;
            this.headingLiteral.Text = GetContentString();
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
            this.StaticHtml.Value = this.HeadingString;
            SetView(this.Edit);
        }

        private void Save()
        {
            try
            {
                this.HeadingString = Uri.UnescapeDataString(this.StaticHtml.Value);

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
                return !string.IsNullOrEmpty(this.HeadingString) ? this.HeadingString :
                    this.DefaulthHeading ?? GetLocalResourceObject("DefaulthHeading").ToString();
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
            Save();
            DisplayView();
        }

        protected void CancelClick(object sender, EventArgs e)
        {
            DisplayView();
        }


        #endregion



    }

}