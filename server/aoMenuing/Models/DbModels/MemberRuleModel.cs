
using Contensive.Models.Db;
using System;

namespace Contensive.Addons.Menuing.Models.DbModels {
    public class MemberRuleModel : DbBaseModel
    {
        //
        //====================================================================================================
        //-- const
        public const string contentName = "member rules";
        public const string contentTableName = "ccmemberrules";
        //
        public static DbBaseTableMetadataModel tableMetadata { get; } = new DbBaseTableMetadataModel(contentName, contentTableName);
        //
        //====================================================================================================
        // -- instance properties
        public DateTime dateExpires { get; set; }
        public int groupID { get; set; }
        public int memberID { get; set; }
    }
}
