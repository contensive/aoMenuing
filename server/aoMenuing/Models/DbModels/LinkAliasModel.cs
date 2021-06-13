
using Contensive.Models.Db;

namespace Contensive.Addons.Menuing.Models.DbModels {
    public class LinkAliasModel : DbBaseModel {
        //
        //====================================================================================================
        //-- const
        public const string contentName = "Link Aliases";
        public const string contentTableName = "ccLinkAliases";
        //
        public static DbBaseTableMetadataModel tableMetadata { get; } = new DbBaseTableMetadataModel(contentName, contentTableName);
        //
        //====================================================================================================
        // -- instance properties
        public int pageID { get; set; }
        public string queryStringSuffix { get; set; }
    }
}
