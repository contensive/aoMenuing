
using Contensive.Models.Db;

namespace Contensive.Addons.Menuing.Models.DbModels {
    public class GroupModel : DbBaseModel {
        //
        //====================================================================================================
        //-- const
        public const string contentName = "groups";
        public const string contentTableName = "ccgroups";
        //
        public static DbBaseTableMetadataModel tableMetadata { get; } = new DbBaseTableMetadataModel(contentName, contentTableName);
        //
        //====================================================================================================
        // -- instance properties
        public bool allowBulkEmail { get; set; }
        public string caption { get; set; }
        public string copyFilename { get; set; }
        public bool publicJoin { get; set; }
    }
}
