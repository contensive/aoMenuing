
using Contensive.Models.Db;

namespace Contensive.Addons.Menuing.Models.DbModels {
    public class PageContentBlockRuleModel : DbBaseModel
    {
        //
        //====================================================================================================
        //-- const
        public const string contentName = "Page Content Block Rules";
        public const string contentTableName = "ccPageContentBlockRules";
        //
        public static DbBaseTableMetadataModel tableMetadata { get; } = new DbBaseTableMetadataModel(contentName, contentTableName);
        //
        //====================================================================================================
        // -- instance properties
        public int groupID { get; set; }
        public int recordID { get; set; }
    }
}
