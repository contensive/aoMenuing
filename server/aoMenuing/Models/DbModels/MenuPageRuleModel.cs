
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
    public class MenuPageRuleModel : BaseModel
    {
        //
        //====================================================================================================
        //-- const
        //------ set content name
        public const string contentName = "Menu Page Rules";
        //------ set to tablename for the primary content (used for cache names)
        public const string contentTableName = "ccmenupagerules";
        //
        //====================================================================================================
        // -- instance properties
        public int menuId { get; set; }
        public int pageId { get; set; }
        //
        //====================================================================================================
        public static MenuPageRuleModel @add(CPBaseClass cp)
        {
            return @add<MenuPageRuleModel>(cp);
        }
        //
        //====================================================================================================
        public static MenuPageRuleModel create(CPBaseClass cp, int recordId)
        {
            return create<MenuPageRuleModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static MenuPageRuleModel create(CPBaseClass cp, string recordGuid)
        {
            return create<MenuPageRuleModel>(cp, recordGuid);
        }
        //
        //====================================================================================================
        public static MenuPageRuleModel createByName(CPBaseClass cp, string recordName)
        {
            return createByName<MenuPageRuleModel>(cp, recordName);
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
            delete<MenuPageRuleModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static void delete(CPBaseClass cp, string ccGuid)
        {
            delete<MenuPageRuleModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static List<MenuPageRuleModel> createList(CPBaseClass cp, string sqlCriteria, string sqlOrderBy = "id")
        {
            return createList<MenuPageRuleModel>(cp, sqlCriteria, sqlOrderBy);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, int recordId)
        {
            return BaseModel.getRecordName<MenuPageRuleModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, string ccGuid)
        {
            return BaseModel.getRecordName<MenuPageRuleModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static int getRecordId(CPBaseClass cp, string ccGuid)
        {
            return BaseModel.getRecordId<MenuPageRuleModel>(cp, ccGuid);
        }
    }
}
