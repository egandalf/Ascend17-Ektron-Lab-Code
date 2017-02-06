using Lab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using News = Lab.SmartForms.News.root;

public partial class Components_Controls_News : UserControlBase
{
    private Lazy<ContentTypeManager<News>> NewsManager = new Lazy<ContentTypeManager<News>>(() => new ContentTypeManager<News>());
    protected void Page_Load(object sender, EventArgs e)
    {
        if (RawContent != null && RawContent.XmlConfiguration.Id == Constants.SmartForms.NewsTypeId)
        {
            uxEditor.ObjectId = RawContent.Id;
            uxEditor.FolderId = RawContent.FolderId;

            var newsContent = NewsManager.Value.Make(RawContent);

            uxHeadline.Text = newsContent.SmartForm.Headline;
            if (!string.IsNullOrEmpty(newsContent.SmartForm.Subhead))
            {
                uxSubhead.Visible = true;
                uxSubhead.InnerText = newsContent.SmartForm.Subhead;
            }
            uxByline.Text = newsContent.SmartForm.Byline;
            uxSource.Text = newsContent.SmartForm.Source;
            uxPubDate.Text = DateTime.Parse(newsContent.SmartForm.PublicationDate).ToLongDateString();
            uxPubBody.Text = newsContent.SmartForm.Release.Any.GetRichString();
            uxContactName.Text = newsContent.SmartForm.Contact.Name;
            uxContactEmail.Text = newsContent.SmartForm.Contact.Email;
            uxContactPhone.Text = newsContent.SmartForm.Contact.Phone;
        }
    }
}