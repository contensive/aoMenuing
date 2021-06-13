
using Contensive.Models.Db;

namespace Contensive.Addons.Menuing.Models.DbModels {
    public class DynamicMenuSectionRuleModel : DbBaseModel {
        //
        //====================================================================================================
        //-- const
        //------ set content name
        public const string contentName = "DynamicMenu Section Rules";
        //------ set to tablename for the primary content (used for cache names)
        public const string contentTableName = "ccDynamicMenuSectionRules";
        //
        public static DbBaseTableMetadataModel tableMetadata { get; } = new DbBaseTableMetadataModel(contentName, contentTableName);
        //
        //====================================================================================================
        // -- instance properties
        public int dynamicMenuID { get; set; }
        public int sectionID { get; set; }
    }
}
