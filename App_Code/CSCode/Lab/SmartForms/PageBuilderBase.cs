using Ektron.Cms.PageBuilder;
using System.Web.UI.WebControls;

namespace Lab
{
    /// <summary>
    /// Summary description for PageBuilderBase
    /// </summary>
    public abstract class PageBuilderBase : PageBuilder
    {
        public override void Error(string message)
        {
            jsAlert(message);
        }
        public override void Notify(string message)
        {
            jsAlert(message);
        }
        public void jsAlert(string message)
        {
            Literal lit = new Literal();
            lit.Text = "<script type=\"\"language =\"\">{0}</script>";
            lit.Text = string.Format(lit.Text, "alert('" + message + "');");
            Form.Controls.Add(lit);
        }
    }
}