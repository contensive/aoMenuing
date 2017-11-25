
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using Contensive.BaseClasses;

namespace Contensive.Addons.MenuSections.Models
{
    public class DynamicMenuSectionRuleModel : baseModel
    {
        //
        //====================================================================================================
        //-- const
        //------ set content name
        public const string contentName = "DynamicMenu Section Rules";
        //------ set to tablename for the primary content (used for cache names)
        public const string contentTableName = "ccDynamicMenuSectionRules";
        //
        //====================================================================================================
        // -- instance properties
        public int DynamicMenuID { get; set; }
        public int SectionID { get; set; }
        //
        //====================================================================================================
        public static DynamicMenuSectionRuleModel @add(CPBaseClass cp)
        {
            return @add<DynamicMenuSectionRuleModel>(cp);
        }
        //
        //====================================================================================================
        public static DynamicMenuSectionRuleModel create(CPBaseClass cp, int recordId)
        {
            return create<DynamicMenuSectionRuleModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static DynamicMenuSectionRuleModel create(CPBaseClass cp, string recordGuid)
        {
            return create<DynamicMenuSectionRuleModel>(cp, recordGuid);
        }
        //
        //====================================================================================================
        public static DynamicMenuSectionRuleModel createByName(CPBaseClass cp, string recordName)
        {
            return createByName<DynamicMenuSectionRuleModel>(cp, recordName);
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
            delete<DynamicMenuSectionRuleModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static void delete(CPBaseClass cp, string ccGuid)
        {
            delete<DynamicMenuSectionRuleModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static List<DynamicMenuSectionRuleModel> createList(CPBaseClass cp, string sqlCriteria, string sqlOrderBy = "id")
        {
            return createList<DynamicMenuSectionRuleModel>(cp, sqlCriteria, sqlOrderBy);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, int recordId)
        {
            return baseModel.getRecordName<DynamicMenuSectionRuleModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, string ccGuid)
        {
            return baseModel.getRecordName<DynamicMenuSectionRuleModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static int getRecordId(CPBaseClass cp, string ccGuid)
        {
            return baseModel.getRecordId<DynamicMenuSectionRuleModel>(cp, ccGuid);
        }
    }
}
