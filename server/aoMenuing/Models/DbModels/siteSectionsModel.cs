
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using Contensive.BaseClasses;

namespace Contensive.Addons.Menuing.Models.DbModels
{
    public class SiteSectionsModel : BaseModel
    {
        //
        //====================================================================================================
        //-- const
        public const string contentName = "site sections";
        public const string contentTableName = "ccsitesections";
        //
        //====================================================================================================
        // -- instance properties
        public bool BlockSection { get; set; }
        public string Caption { get; set; }
        public int ContentID { get; set; }
        public bool HideMenu { get; set; }
        public string JSEndBody { get; set; }
        public string JSFilename { get; set; }
        public string JSHead { get; set; }
        public string JSOnLoad { get; set; }
        public string MenuImageDownFilename { get; set; }
        public string menuImageDownOverFilename { get; set; }
        public string MenuImageFilename { get; set; }
        public string MenuImageOverFilename { get; set; }
        public int RootPageID { get; set; }
        public int TemplateID { get; set; }
        //
        //====================================================================================================
        public static SiteSectionsModel @add(CPBaseClass cp)
        {
            return @add<SiteSectionsModel>(cp);
        }
        //
        //====================================================================================================
        public static SiteSectionsModel create(CPBaseClass cp, int recordId)
        {
            return create<SiteSectionsModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static SiteSectionsModel create(CPBaseClass cp, string recordGuid)
        {
            return create<SiteSectionsModel>(cp, recordGuid);
        }
        //
        //====================================================================================================
        public static SiteSectionsModel createByName(CPBaseClass cp, string recordName)
        {
            return createByName<SiteSectionsModel>(cp, recordName);
        }
        //
        //====================================================================================================
        public new void save(CPBaseClass cp)
        {
            base.save(cp);
        }
        //
        //====================================================================================================
        public static void delete(CPBaseClass cp, int recordId)
        {
            delete<SiteSectionsModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static void delete(CPBaseClass cp, string ccGuid)
        {
            delete<SiteSectionsModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static List<SiteSectionsModel> createList(CPBaseClass cp, string sqlCriteria, string sqlOrderBy = "id")
        {
            return createList<SiteSectionsModel>(cp, sqlCriteria, sqlOrderBy);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, int recordId)
        {
            return BaseModel.getRecordName<SiteSectionsModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, string ccGuid)
        {
            return BaseModel.getRecordName<SiteSectionsModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static int getRecordId(CPBaseClass cp, string ccGuid)
        {
            return BaseModel.getRecordId<SiteSectionsModel>(cp, ccGuid);
        }
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
            }
            return result;
        }
    }
}
