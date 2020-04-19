
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
    public class MemberRuleModel : BaseModel
    {
        //
        //====================================================================================================
        //-- const
        public const string contentName = "member rules";
        public const string contentTableName = "ccmemberrules";
        //
        //====================================================================================================
        // -- instance properties
        public DateTime DateExpires { get; set; }
        public int GroupID { get; set; }
        public int MemberID { get; set; }
        //
        //====================================================================================================
        public static MemberRuleModel @add(CPBaseClass cp)
        {
            return @add<MemberRuleModel>(cp);
        }
        //
        //====================================================================================================
        public static MemberRuleModel create(CPBaseClass cp, int recordId)
        {
            return create<MemberRuleModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static MemberRuleModel create(CPBaseClass cp, string recordGuid)
        {
            return create<MemberRuleModel>(cp, recordGuid);
        }
        //
        //====================================================================================================
        public static MemberRuleModel createByName(CPBaseClass cp, string recordName)
        {
            return createByName<MemberRuleModel>(cp, recordName);
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
            delete<MemberRuleModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static void delete(CPBaseClass cp, string ccGuid)
        {
            delete<MemberRuleModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static List<MemberRuleModel> createList(CPBaseClass cp, string sqlCriteria, string sqlOrderBy = "id")
        {
            return createList<MemberRuleModel>(cp, sqlCriteria, sqlOrderBy);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, int recordId)
        {
            return BaseModel.getRecordName<MemberRuleModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, string ccGuid)
        {
            return BaseModel.getRecordName<MemberRuleModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static int getRecordId(CPBaseClass cp, string ccGuid)
        {
            return BaseModel.getRecordId<MemberRuleModel>(cp, ccGuid);
        }
    }
}
