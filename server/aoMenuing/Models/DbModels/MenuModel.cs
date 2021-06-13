
using Contensive.BaseClasses;
using Contensive.Models.Db;

namespace Contensive.Addons.Menuing.Models.DbModels {
    public class MenuModel : DesignBlockBase.Models.Db.SettingsBaseModel {
        //
        //====================================================================================================
        //-- const
        public const string contentName = "menus";
        public const string contentTableName = "ccMenus";
        //
        public static DbBaseTableMetadataModel tableMetadata { get; } = new DbBaseTableMetadataModel(contentName, contentTableName);
        //
        //====================================================================================================
        //// -- instance properties
        //public string classFlyoutParent { get; set; }
        //public string classItemActive { get; set; }
        public string classItemFirst { get; set; }
        //public string classItemHover { get; set; }
        public string classItemLast { get; set; }
        public string classTierItem { get; set; }
        public string classTierAnchor { get; set; }
        public string classTierList { get; set; }
        public string classTopItem { get; set; }
        public string classTopParentItem { get; set; }
        public string classTopAnchor { get; set; }
        public string classTopParentAnchor { get; set; }
        public string dataToggleTopParentAnchor { get; set; }
        public string classTopList { get; set; }
        public string classTopWrapper { get; set; }
        public string classItemActive { get; set; }
        public string classItemHover { get; set; }
        public int depth { get; set; }
        public bool addRootToTier { get; set; }
        public bool includeBlockedFlyoutPages { get; set; }
        /// <summary>
        /// The layout assigned to this instance of the menu. If missing, a copy of the default is assigned to it.
        /// </summary>
        public int layoutId { get; set; }
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
