
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
    public class groupModel : baseModel
    {
        //
        //====================================================================================================
        //-- const
        public const string contentName = "groups";
        public const string contentTableName = "ccgroups";
        //
        //====================================================================================================
        // -- instance properties
        public bool AllowBulkEmail { get; set; }
        public string Caption { get; set; }
        public string CopyFilename { get; set; }
        public bool PublicJoin { get; set; }
        //
        //====================================================================================================
        public static groupModel @add(CPBaseClass cp)
        {
            return @add<groupModel>(cp);
        }
        //
        //====================================================================================================
        public static groupModel create(CPBaseClass cp, int recordId)
        {
            return create<groupModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static groupModel create(CPBaseClass cp, string recordGuid)
        {
            return create<groupModel>(cp, recordGuid);
        }
        //
        //====================================================================================================
        public static groupModel createByName(CPBaseClass cp, string recordName)
        {
            return createByName<groupModel>(cp, recordName);
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
            delete<groupModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static void delete(CPBaseClass cp, string ccGuid)
        {
            delete<groupModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static List<groupModel> createList(CPBaseClass cp, string sqlCriteria, string sqlOrderBy = "id")
        {
            return createList<groupModel>(cp, sqlCriteria, sqlOrderBy);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, int recordId)
        {
            return baseModel.getRecordName<groupModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, string ccGuid)
        {
            return baseModel.getRecordName<groupModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static int getRecordId(CPBaseClass cp, string ccGuid)
        {
            return baseModel.getRecordId<groupModel>(cp, ccGuid);
        }
    }
}
