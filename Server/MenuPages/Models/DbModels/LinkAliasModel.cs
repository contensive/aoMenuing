
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using Contensive.BaseClasses;

namespace Contensive.Addons.MenuPages.Models.DbModels
{
    public class LinkAliasModel : BaseModel
    {
        //
        //====================================================================================================
        //-- const
        public const string contentName = "Link Aliases";
        public const string contentTableName = "ccLinkAliases";
        //
        //====================================================================================================
        // -- instance properties
        public int PageID { get; set; }
        public string QueryStringSuffix { get; set; }
        //
        //====================================================================================================
        public static LinkAliasModel @add(CPBaseClass cp)
        {
            return @add<LinkAliasModel>(cp);
        }
        //
        //====================================================================================================
        public static LinkAliasModel create(CPBaseClass cp, int recordId)
        {
            return create<LinkAliasModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static LinkAliasModel create(CPBaseClass cp, string recordGuid)
        {
            return create<LinkAliasModel>(cp, recordGuid);
        }
        //
        //====================================================================================================
        public static LinkAliasModel createByName(CPBaseClass cp, string recordName)
        {
            return createByName<LinkAliasModel>(cp, recordName);
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
            delete<LinkAliasModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static void delete(CPBaseClass cp, string ccGuid)
        {
            delete<LinkAliasModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static List<LinkAliasModel> createList(CPBaseClass cp, string sqlCriteria, string sqlOrderBy = "id")
        {
            return createList<LinkAliasModel>(cp, sqlCriteria, sqlOrderBy);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, int recordId)
        {
            return BaseModel.getRecordName<LinkAliasModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, string ccGuid)
        {
            return BaseModel.getRecordName<LinkAliasModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static int getRecordId(CPBaseClass cp, string ccGuid)
        {
            return BaseModel.getRecordId<LinkAliasModel>(cp, ccGuid);
        }
    }
}
