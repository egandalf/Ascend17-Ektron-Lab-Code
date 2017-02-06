using Ektron.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab
{
    /// <summary>
    /// Summary description for UserControlBase
    /// </summary>
    public abstract class UserControlBase : System.Web.UI.UserControl
    {
        public ContentData RawContent { get; set; }
    }
}