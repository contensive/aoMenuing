
using Contensive.Addons.Menuing.Models.DbModels;
using System;

namespace Contensive.Addons.Menuing.Views {
    /// <summary>
    /// remote method addon that adds a new page and adds it to the menu specified, then forwards the user to edit that page
    /// </summary>
    public class AddMenuPageClass : Contensive.BaseClasses.AddonBaseClass {
        //
        readonly string errMessage = "<div class=\"\"><h4>Page cannot be created</h4><p>There was a problem that prevented the new page from being created. Please use you back button to return to the previous page.</p></div>";
        //
        public override object Execute(Contensive.BaseClasses.CPBaseClass cp) {
            try {
                if ( !cp.User.IsAdmin) { return errMessage;  }
                //
                var page = Contensive.Models.Db.DbBaseModel.addDefault<PageContentModel>(cp);
                var menuPageRule = Contensive.Models.Db.DbBaseModel.addDefault<MenuPageRuleModel>(cp);
                menuPageRule.pageId = page.id;
                menuPageRule.menuId = cp.Doc.GetInteger("menuId");
                menuPageRule.save(cp);
                string adminUrl = cp.Site.GetText("adminurl");
                adminUrl = cp.Utils.ModifyLinkQueryString(adminUrl, "af", "4");
                adminUrl = cp.Utils.ModifyLinkQueryString(adminUrl, "cid", cp.Content.GetID("page content").ToString());
                adminUrl = cp.Utils.ModifyLinkQueryString(adminUrl, "id", page.id.ToString());
                //
                // -- after editing, return to the page that called this addon
                adminUrl = cp.Utils.ModifyLinkQueryString(adminUrl, "editreferer", cp.Request.Referer);
                cp.Response.Redirect(adminUrl);
                return string.Empty;
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
    }
}
