
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
    public class linkAliasModel : baseModel
    {
        //
        //====================================================================================================
        //-- const
        public const string contentName = "Link Aliases";
        public const string contentTableName = "ccLinkAliases";
        //
        //====================================================================================================
        // -- instance properties
        public int ContentCategoryID { get; set; }
        public bool EditArchive { get; set; }
        public bool EditBlank { get; set; }
        public int EditSourceID { get; set; }
        public string Link { get; set; }
        public int PageID { get; set; }
        public string QueryStringSuffix { get; set; }
        //
        //====================================================================================================
        public static linkAliasModel @add(CPBaseClass cp)
        {
            return @add<linkAliasModel>(cp);
        }
        //
        //====================================================================================================
        public static linkAliasModel create(CPBaseClass cp, int recordId)
        {
            return create<linkAliasModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static linkAliasModel create(CPBaseClass cp, string recordGuid)
        {
            return create<linkAliasModel>(cp, recordGuid);
        }
        //
        //====================================================================================================
        public static linkAliasModel createByName(CPBaseClass cp, string recordName)
        {
            return createByName<linkAliasModel>(cp, recordName);
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
            delete<linkAliasModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static void delete(CPBaseClass cp, string ccGuid)
        {
            delete<linkAliasModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static List<linkAliasModel> createList(CPBaseClass cp, string sqlCriteria, string sqlOrderBy = "id")
        {
            return createList<linkAliasModel>(cp, sqlCriteria, sqlOrderBy);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, int recordId)
        {
            return baseModel.getRecordName<linkAliasModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, string ccGuid)
        {
            return baseModel.getRecordName<linkAliasModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static int getRecordId(CPBaseClass cp, string ccGuid)
        {
            return baseModel.getRecordId<linkAliasModel>(cp, ccGuid);
        }
    }
}
