
using Contensive.BaseClasses;
using Contensive.Models.Db;

namespace Contensive.Addons.Menuing.Models.DbModels {
    /// <summary>
    /// Menu model for aoMenuing addon. Inherits all properties from base Contensive.Models.Db.MenuModel.
    /// All menu fields are now defined in the base platform collection (aoBase51.xml).
    /// </summary>
    public class MenuModel : Contensive.Models.Db.MenuModel {
        //
        //====================================================================================================
        //-- const
        public const string contentName = "menus";
        public const string contentTableName = "ccMenus";
        //
        //====================================================================================================
        public static MenuModel createOrAddDefault(CPBaseClass cp, string instanceGuid) {
            MenuModel result = create<MenuModel>(cp, instanceGuid);
            if (result != null) { return result; }
            result = DbBaseModel.addDefault<MenuModel>(cp);
            result.name = cp.Utils.isGuid(instanceGuid) ? "Menu " + result.id.ToString() : instanceGuid;
            result.ccguid = instanceGuid;
            result.addRootToTier = true;
            result.classItemActive = "";
            result.classItemFirst = "";
            result.classItemHover = "";
            result.classItemLast = "";
            result.classTierAnchor = "";
            result.classTierItem = "";
            result.classTierList = "";
            result.classTopAnchor = "";
            result.classTopItem = "";
            result.classTopList = "";
            result.classTopParentAnchor = "";
            result.classTopParentItem = "";
            result.classTopWrapper = "";
            result.dataToggleTopParentAnchor = "";
            result.depth = 1;
            //
            // -- a copy of the default layout is added during rendering. Verifying  it here would cost a record select
            result.layoutId = 0;
            result.save(cp);
            return result;
        }
    }
}
