
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contensive.Addons.Menuing.Views {
    /// <summary>
    /// remote method addon that saves the menu item order called from drag-drop stop event
    /// </summary>
    public class SaveNavbarNavSortableRemote : Contensive.BaseClasses.AddonBaseClass {
        //
        public override object Execute(Contensive.BaseClasses.CPBaseClass cp) {
            try {
                if (!cp.User.IsAdmin) { return string.Empty; }
                //
                // -- split comma list
                List<string> argList = cp.Doc.GetText("sortlist").Split(',').ToList();
                
                if (argList.Count == 0) { return string.Empty; }
                //
                // -- first element of the list is the menuId prefixed with either 'menu' or 'mavbarNav'
                int menuId = cp.Utils.EncodeInteger(argList.First().Replace("menu", ""));
                if (menuId == 0) { menuId = cp.Utils.EncodeInteger(argList.First().Replace("navbarNav", "")); }
                if (menuId == 0) { return string.Empty; }
                //
                // -- iterate through menu-page-rules and set sort-order to the order they appear here
                cp.Db.ExecuteNonQuery("update ccmenupagerules set sortorder=null where menuId=" + menuId);
                int ptr = 0;
                foreach (var arg in argList.Skip(1)) {
                    int pageId = cp.Utils.EncodeInteger(arg.Replace("m" + menuId + "p", ""));
                    if (pageId > 0) {
                        cp.Db.ExecuteNonQuery("update ccmenupagerules set sortorder='" + ptr.ToString("0000") + "' where (menuId=" + menuId + ")and(pageid=" + pageId + ")");
                        ptr++;
                    }
                }
                //
                // -- remove any deleted menu items
                cp.Db.ExecuteNonQuery("delete from ccmenupagerules where (sortorder=null)and(menuId=" + menuId + ")");
                //
                // -- clear cache
                cp.Cache.invalidateTableDependencyKey("ccmenupagerules");
                return string.Empty;
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
    }
}
