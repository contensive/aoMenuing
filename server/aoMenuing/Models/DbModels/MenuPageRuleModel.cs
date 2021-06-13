
using Contensive.Models.Db;

namespace Contensive.Addons.Menuing.Models.DbModels {
    public class MenuPageRuleModel : DbBaseModel {
        //
        //====================================================================================================
        //-- const
        //------ set content name
        public const string contentName = "Menu Page Rules";
        //------ set to tablename for the primary content (used for cache names)
        public const string contentTableName = "ccmenupagerules";
        //
        public static DbBaseTableMetadataModel tableMetadata { get; } = new DbBaseTableMetadataModel(contentName, contentTableName);
        //
        //====================================================================================================
        // -- instance properties
        public int menuId { get; set; }
        public int pageId { get; set; }
    }
}
