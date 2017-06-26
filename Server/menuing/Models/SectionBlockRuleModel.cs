
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using Contensive.BaseClasses;

namespace Menuing.Models
{
    public class SectionBlockRuleModel : baseModel
    {
        //
        //====================================================================================================
        //-- const
        public const string contentName = "Section Block Rules";
        public const string contentTableName = "ccSectionBlockRules";
        //
        //====================================================================================================
        // -- instance properties
        public int GroupID { get; set; }
        public int SectionID { get; set; }
        //
        //====================================================================================================
        public static SectionBlockRuleModel @add(CPBaseClass cp)
        {
            return @add<SectionBlockRuleModel>(cp);
        }
        //
        //====================================================================================================
        public static SectionBlockRuleModel create(CPBaseClass cp, int recordId)
        {
            return create<SectionBlockRuleModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static SectionBlockRuleModel create(CPBaseClass cp, string recordGuid)
        {
            return create<SectionBlockRuleModel>(cp, recordGuid);
        }
        //
        //====================================================================================================
        public static SectionBlockRuleModel createByName(CPBaseClass cp, string recordName)
        {
            return createByName<SectionBlockRuleModel>(cp, recordName);
        }
        //
        //====================================================================================================
        public void save(CPBaseClass cp)
        {
            base.save(cp);
        }
        //
        //====================================================================================================
        public static void delete(CPBaseClass cp, int recordId)
        {
            delete<SectionBlockRuleModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static void delete(CPBaseClass cp, string ccGuid)
        {
            delete<SectionBlockRuleModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static List<SectionBlockRuleModel> createList(CPBaseClass cp, string sqlCriteria, string sqlOrderBy = "id")
        {
            return createList<SectionBlockRuleModel>(cp, sqlCriteria, sqlOrderBy);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, int recordId)
        {
            return baseModel.getRecordName<SectionBlockRuleModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, string ccGuid)
        {
            return baseModel.getRecordName<SectionBlockRuleModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static int getRecordId(CPBaseClass cp, string ccGuid)
        {
            return baseModel.getRecordId<SectionBlockRuleModel>(cp, ccGuid);
        }
    }
}
