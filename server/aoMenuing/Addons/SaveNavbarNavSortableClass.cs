
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Contensive.BaseClasses;
using Contensive.Addons.Menuing.Models;
using Contensive.Addons.Menuing.Models.DbModels;

namespace Contensive.Addons.Menuing.Views {
    /// <summary>
    /// remote method addon that saves the menu item order called from drag-drop stop event
    /// </summary>
    public class SaveNavbarNavSortableClass : Contensive.BaseClasses.AddonBaseClass {
        //
        public override object Execute(Contensive.BaseClasses.CPBaseClass cp) {
            try {
                if ( !cp.User.IsAdmin) { return string.Empty;  }
                List<string> argList = cp.Doc.GetText("sortlist").Split(',').ToList();
                if (argList.Count > 0) {
                    int menuId = cp.Utils.EncodeInteger(argList.First().Replace("navbarNav", ""));
                    if(menuId>0) {
                        cp.Db.ExecuteNonQuery("update ccmenupagerules set sortorder=null where menuId=" + menuId);
                        int ptr = 0;
                        foreach ( var arg in argList.Skip(1)) {
                            int pageId = cp.Utils.EncodeInteger(  arg.Replace("m" + menuId + "p", ""));
                            if(pageId>0) {
                                cp.Db.ExecuteNonQuery("update ccmenupagerules set sortorder='" + ptr.ToString("0000") + "' where (menuId=" + menuId + ")and(pageid=" + pageId + ")");
                                ptr++;
                            }
                        }
                        cp.Db.ExecuteNonQuery("delete from ccmenupagerules where (sortorder=null)and(menuId=" + menuId + ")");
                        //
                        // -- clear cache
                        cp.Cache.invalidateTableDependencyKey("ccMenuPageRules");
                    }
                }
                return string.Empty;
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                return string.Empty;
            }
        }
    }
}
