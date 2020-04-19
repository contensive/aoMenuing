
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
    public class GroupModel : BaseModel
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
        public static GroupModel @add(CPBaseClass cp)
        {
            return @add<GroupModel>(cp);
        }
        //
        //====================================================================================================
        public static GroupModel create(CPBaseClass cp, int recordId)
        {
            return create<GroupModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static GroupModel create(CPBaseClass cp, string recordGuid)
        {
            return create<GroupModel>(cp, recordGuid);
        }
        //
        //====================================================================================================
        public static GroupModel createByName(CPBaseClass cp, string recordName)
        {
            return createByName<GroupModel>(cp, recordName);
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
            delete<GroupModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static void delete(CPBaseClass cp, string ccGuid)
        {
            delete<GroupModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static List<GroupModel> createList(CPBaseClass cp, string sqlCriteria, string sqlOrderBy = "id")
        {
            return createList<GroupModel>(cp, sqlCriteria, sqlOrderBy);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, int recordId)
        {
            return BaseModel.getRecordName<GroupModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, string ccGuid)
        {
            return BaseModel.getRecordName<GroupModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static int getRecordId(CPBaseClass cp, string ccGuid)
        {
            return BaseModel.getRecordId<GroupModel>(cp, ccGuid);
        }
    }
}
