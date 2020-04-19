
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Contensive.BaseClasses;
using Contensive.Addons.Menuing.Models;
using Contensive.Addons.Menuing.Models.DbModels;

namespace Contensive.Addons.Menuing.Views {
    /// <summary>
    /// remote method addon that adds a new page and adds it to the menu specified, then forwards the user to edit that page
    /// </summary>
    public class AddMenuPageClass : Contensive.BaseClasses.AddonBaseClass {
        //
        string errMessage = "<div class=\"\"><h4>Page cannot be created</h4><p>There was a problem that prevented the new page from being created. Please use you back button to return to the previous page.</p></div>";
        //
        public override object Execute(Contensive.BaseClasses.CPBaseClass cp) {
            try {
                if ( !cp.User.IsAdmin) { return errMessage;  }
                var page = PageContentModel.add(cp);
                var menuPageRule = MenuPageRuleModel.add(cp);
                menuPageRule.pageId = page.id;
                menuPageRule.menuId = cp.Doc.GetInteger("menuId");
                menuPageRule.save(cp);
                string adminUrl = cp.Site.GetText("adminurl");
                adminUrl = cp.Utils.ModifyLinkQueryString(adminUrl, "af", "4");
                adminUrl = cp.Utils.ModifyLinkQueryString(adminUrl, "cid", cp.Content.GetID("page content").ToString());
                adminUrl = cp.Utils.ModifyLinkQueryString(adminUrl, "id", page.id.ToString());
                cp.Response.Redirect(adminUrl);
                return string.Empty;
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                return errMessage;
            }
        }
    }
}
