
using Contensive.BaseClasses;
using Contensive.Models.Db;
using System;
using System.Collections.Generic;

namespace Contensive.Addons.Menuing.Models.DbModels {
    public class SiteSectionsModel : DbBaseModel
    {
        //
        //====================================================================================================
        //-- const
        public const string contentName = "site sections";
        public const string contentTableName = "ccsitesections";
        //
        public static DbBaseTableMetadataModel tableMetadata { get; } = new DbBaseTableMetadataModel(contentName, contentTableName);
        //
        //====================================================================================================
        // -- instance properties
        public bool blockSection { get; set; }
        public string caption { get; set; }
        public int contentID { get; set; }
        public bool hideMenu { get; set; }
        public string jsEndBody { get; set; }
        public string jsFilename { get; set; }
        public string jsHead { get; set; }
        public string jsOnLoad { get; set; }
        public string menuImageDownFilename { get; set; }
        public string menuImageDownOverFilename { get; set; }
        public string menuImageFilename { get; set; }
        public string menuImageOverFilename { get; set; }
        public int rootPageID { get; set; }
        public int templateID { get; set; }
        //
        //====================================================================================================
        // -- a list of sections you have access to
        // -- all sections without blocking, plus section-groups that you are in the group
        public static List<int> getAllowedSectionIdList(CPBaseClass cp )
        {
            List<int> result = new List<int>();
            try
            {
                string sql = "select sr.sectionId as id"
                    + " from ccSectionBlockRules sr"
                    + " left join ccMemberRules mr on mr.groupId=sr.groupid"
                    + " where mr.memberId=" +  cp.User.Id.ToString() ;
                CPCSBaseClass cs = cp.CSNew();
                if (cs.OpenSQL(sql))
                {
                    do
                    {
                        result.Add(cs.GetInteger("id"));
                        cs.GoNext();
                    } while (cs.OK());
                }
                cs.Close();
            }
            catch (Exception ex)
            {
                cp.Site.ErrorReport(ex);
                throw;
            }
            return result;
        }
    }
}
