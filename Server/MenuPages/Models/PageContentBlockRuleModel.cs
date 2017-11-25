
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using Contensive.BaseClasses;

namespace Contensive.Addons.MenuPages.Models
{
    public class PageContentBlockRuleModel : baseModel
    {
        //
        //====================================================================================================
        //-- const
        public const string contentName = "Page Content Block Rules";
        public const string contentTableName = "ccPageContentBlockRules";
        //
        //====================================================================================================
        // -- instance properties
        public int GroupID { get; set; }
        public int RecordID { get; set; }
        //
        //====================================================================================================
        public static PageContentBlockRuleModel @add(CPBaseClass cp)
        {
            return @add<PageContentBlockRuleModel>(cp);
        }
        //
        //====================================================================================================
        public static PageContentBlockRuleModel create(CPBaseClass cp, int recordId)
        {
            return create<PageContentBlockRuleModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static PageContentBlockRuleModel create(CPBaseClass cp, string recordGuid)
        {
            return create<PageContentBlockRuleModel>(cp, recordGuid);
        }
        //
        //====================================================================================================
        public static PageContentBlockRuleModel createByName(CPBaseClass cp, string recordName)
        {
            return createByName<PageContentBlockRuleModel>(cp, recordName);
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
            delete<PageContentBlockRuleModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static void delete(CPBaseClass cp, string ccGuid)
        {
            delete<PageContentBlockRuleModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static List<PageContentBlockRuleModel> createList(CPBaseClass cp, string sqlCriteria, string sqlOrderBy = "id")
        {
            return createList<PageContentBlockRuleModel>(cp, sqlCriteria, sqlOrderBy);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, int recordId)
        {
            return baseModel.getRecordName<PageContentBlockRuleModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, string ccGuid)
        {
            return baseModel.getRecordName<PageContentBlockRuleModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static int getRecordId(CPBaseClass cp, string ccGuid)
        {
            return baseModel.getRecordId<PageContentBlockRuleModel>(cp, ccGuid);
        }
    }
}
