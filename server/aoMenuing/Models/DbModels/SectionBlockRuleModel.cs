
using Contensive.Models.Db;

namespace Contensive.Addons.Menuing.Models.DbModels {
    public class SectionBlockRuleModel : DbBaseModel
    {
        //
        //====================================================================================================
        //-- const
        public const string contentName = "Section Block Rules";
        public const string contentTableName = "ccSectionBlockRules";
        //
        public static DbBaseTableMetadataModel tableMetadata { get; } = new DbBaseTableMetadataModel(contentName, contentTableName);
        //
        //====================================================================================================
        // -- instance properties
        public int groupID { get; set; }
        public int sectionID { get; set; }
    }
}
