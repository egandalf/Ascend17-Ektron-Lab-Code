using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lab;
using Ektron.Cms.BusinessObjects.Organization;
using Ektron.Cms.Organization;
using Ektron.Cms.Framework.Content;

public partial class Components_Controls_Menu : System.Web.UI.UserControl
{
    private long _menuId = 0;
    private Lazy<MenuManager> MenuManager = new Lazy<MenuManager>(() => new MenuManager());
    private Lazy<ContentManager> ContentManager = new Lazy<ContentManager>(() => new ContentManager());

    public long MenuId
    {
        get { return _menuId; }
        set { _menuId = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        var menuId = Constants.Menus.MainMenu;
        if (MenuId > 0) menuId = MenuId;

        var menu = MenuManager.Value.GetTree(menuId);
        var menuItems = menu.Items.Select(m => new {
            Text = m.Text,
            Link = m.Href,
            Selected = IsSelected(m)
        });
        uxMenu.DataSource = menuItems;
        uxMenu.DataBind();
    }

    private bool IsSelected(IMenuItemData item)
    {
        var contentId = HttpContext.Current.Request.QueryString.GetContentId();
        if (item.ItemId == contentId) return true;

        if (string.Compare(item.Href.Trim('/'), HttpContext.Current.Request.Path.Trim('/'), true) == 0) return true;

        var path = HttpContext.Current.Request.RawUrl;
        if (path.StartsWith(item.Href, StringComparison.CurrentCultureIgnoreCase)) return true;

        var submenu = item as SubMenuData;
        if(submenu != null)
        {
            var content = ContentManager.Value.GetItem(contentId);
            if (submenu.AssociatedFolders.Contains(content.FolderId)) return true;
        }

        return false;
    }
}