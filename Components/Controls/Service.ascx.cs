using Ektron.Cms;
using Lab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Service = Lab.SmartForms.Service.root;

public partial class Components_Controls_Service : UserControlBase
{
    private Lazy<ContentTypeManager<Service>> ServiceManager = new Lazy<ContentTypeManager<Service>>(() => new ContentTypeManager<Service>()); 
    protected void Page_Load(object sender, EventArgs e)
    {
        if(RawContent != null && RawContent.XmlConfiguration.Id == Constants.SmartForms.ServiceTypeId)
        {
            var serviceContent = ServiceManager.Value.Make(RawContent);

            uxHeading.Text = serviceContent.Content.Title;
            uxPrice.Text = string.Format("${0}/hr", serviceContent.SmartForm.CostPerHour);
            uxBody.Text = serviceContent.SmartForm.Description.Any.GetRichString();
            uxServiceImage.ImageUrl = serviceContent.SmartForm.Graphic.img.src;
            uxServiceImage.AlternateText = serviceContent.SmartForm.Graphic.img.alt;
        }
    }
}