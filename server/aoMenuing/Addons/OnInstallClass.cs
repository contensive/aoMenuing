
using Contensive.Addons.Menuing.Controllers;
using Contensive.Addons.Menuing.Models.DbModels;
using Contensive.Addons.Menuing.Models.ViewModels;
using Contensive.BaseClasses;
using System;

namespace Contensive.Addons.Menuing.Views {
    /// <summary>
    /// installation options not possible with collection addon
    /// </summary>
    public class OnInstallClass : AddonBaseClass {
        //
        // ====================================================================================================
        //
        public override object Execute(Contensive.BaseClasses.CPBaseClass cp) {
            try {
                // 
                // -- some features of mening content moved from base collection to menuing collection, to deprecate
                int menuingContentId = cp.Content.GetID("menuing");
                cp.Db.ExecuteNonQuery("update ccfields set isBaseField=0 where name='Depth' and  contentid=" + menuingContentId.ToString());
                cp.Db.ExecuteNonQuery("update ccfields set isBaseField=0 where name='addRootToTier' and  contentid=" + menuingContentId.ToString());
                cp.Db.ExecuteNonQuery("update ccfields set isBaseField=0 where name='includeBlockedFlyoutPages' and  contentid=" + menuingContentId.ToString());
                //
                cp.Db.ExecuteNonQuery("update ccfields set isBaseField=0,EditTab='Legacy' where name='classtopparentitem' and contentid=" + menuingContentId.ToString());
                cp.Db.ExecuteNonQuery("update ccfields set isBaseField=0,EditTab='Legacy' where name='classtopanchor' and contentid=" + menuingContentId.ToString());
                cp.Db.ExecuteNonQuery("update ccfields set isBaseField=0,EditTab='Legacy' where name='classtopparentanchor' and contentid=" + menuingContentId.ToString());
                cp.Db.ExecuteNonQuery("update ccfields set isBaseField=0,EditTab='Legacy' where name='datatoggletopparentanchor' and contentid=" + menuingContentId.ToString());
                cp.Db.ExecuteNonQuery("update ccfields set isBaseField=0,EditTab='Legacy' where name='classtieranchor' and contentid=" + menuingContentId.ToString());
                cp.Db.ExecuteNonQuery("update ccfields set isBaseField=0,EditTab='Legacy' where name='classTopWrapper' and contentid=" + menuingContentId.ToString());
                cp.Db.ExecuteNonQuery("update ccfields set isBaseField=0,EditTab='Legacy' where name='classTopList' and contentid=" + menuingContentId.ToString());
                cp.Db.ExecuteNonQuery("update ccfields set isBaseField=0,EditTab='Legacy' where name='classTopItem' and contentid=" + menuingContentId.ToString());
                cp.Db.ExecuteNonQuery("update ccfields set isBaseField=0,EditTab='Legacy' where name='classItemActive' and contentid=" + menuingContentId.ToString());
                cp.Db.ExecuteNonQuery("update ccfields set isBaseField=0,EditTab='Legacy' where name='classTierList' and contentid=" + menuingContentId.ToString());
                cp.Db.ExecuteNonQuery("update ccfields set isBaseField=0,EditTab='Legacy' where name='classTierItem' and contentid=" + menuingContentId.ToString());
                cp.Db.ExecuteNonQuery("update ccfields set isBaseField=0,EditTab='Legacy' where name='classItemFirst' and contentid=" + menuingContentId.ToString());
                cp.Db.ExecuteNonQuery("update ccfields set isBaseField=0,EditTab='Legacy' where name='classItemLast' and contentid=" + menuingContentId.ToString());
                cp.Db.ExecuteNonQuery("update ccfields set isBaseField=0,EditTab='Legacy' where name='classItemHover' and contentid=" + menuingContentId.ToString());
                // 
                // -- if editing enabled, add the link and wrapperwrapper
                return null;
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
    }
}
