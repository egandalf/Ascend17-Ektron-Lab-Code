using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lab;
using Ektron.Cms.Framework.Content;
using System.Web.UI.HtmlControls;

public partial class Components_Masters_Bootstrap : System.Web.UI.MasterPage
{
    private Lazy<ContentManager> ContentManager = new Lazy<ContentManager>(() => new ContentManager(Ektron.Cms.Framework.ApiAccessMode.Admin));

    protected void Page_Load(object sender, EventArgs e)
    {
        long id =Request.QueryString.GetContentId();
        if (id == 0) SetDefaults();
        else SetContentMetadata(id);
    }

    private void SetDefaults()
    {
        PlaceTitleTag("Unidentified Page" + Constants.Strings.TitleSuffix);
    }

    private void SetContentMetadata(long id)
    {
        var content = ContentManager.Value.GetItem(id, true);
        if (content == null) SetDefaults();
        else
        {
            var metadata = content.MetaData;

            var titleMeta = metadata.FirstOrDefault(m => m.Id == Constants.Metadata.MetaTitleId);
            if (titleMeta != null)
            {
                var title = !string.IsNullOrEmpty(titleMeta.Text) ? titleMeta.Text : content.Title;
                title = title + Constants.Strings.TitleSuffix;
                PlaceTitleTag(title);
            }

            var keywordMeta = metadata.FirstOrDefault(m => m.Id == Constants.Metadata.MetaKeywordsId);
            if (keywordMeta != null) PlaceKeywords(keywordMeta.Text);

            var descriptionMeta = metadata.FirstOrDefault(m => m.Id == Constants.Metadata.MetaDescriptionId);
            if (descriptionMeta != null) PlaceDescription(descriptionMeta.Text);
        }
    }

    private void PlaceDescription(string text)
    {
        var descriptionTag = new HtmlGenericControl("meta");
        descriptionTag.Attributes.Add("name", "description");
        descriptionTag.Attributes.Add("content", text);
        Page.Header.Controls.AddAt(0, descriptionTag);
    }

    private void PlaceKeywords(string text)
    {
        var keywordsTag = new HtmlGenericControl("meta");
        keywordsTag.Attributes.Add("name", "keywords");
        keywordsTag.Attributes.Add("content", text);
        Page.Header.Controls.AddAt(0, keywordsTag);
    }

    private void PlaceTitleTag(string title)
    {
        var titleTag = new HtmlGenericControl("title");
        titleTag.InnerText = title;
        Page.Header.Controls.AddAt(0, titleTag);
    }
}
