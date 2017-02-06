using Lab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Components_Controls_Html : UserControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(RawContent != null && RawContent.XmlConfiguration.Id == 0)
        {
            uxEditor.FolderId = RawContent.FolderId;
            uxEditor.ObjectId = RawContent.Id;

            uxHeading.Text = RawContent.Title;
            uxBody.Text = RawContent.Html;
        }
    }
}