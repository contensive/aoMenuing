
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using Contensive.BaseClasses;

namespace Contensive.Addons.BootstrapNav.Models.Entity
{
    public class menuModel : baseModel
    {
        //
        //====================================================================================================
        //-- const
        public const string contentName = "menus";
        public const string contentTableName = "ccMenus";
        //
        //====================================================================================================
        //// -- instance properties
        //public string classFlyoutParent { get; set; }
        //public string classItemActive { get; set; }
        public string classItemFirst { get; set; }
        //public string classItemHover { get; set; }
        public string classItemLast { get; set; }
        public string classTierItem { get; set; }
        public string classTierAnchor { get; set; }
        public string classTierList { get; set; }
        public string classTopItem { get; set; }
        public string classTopParentItem { get; set; }
        public string classTopAnchor { get; set; }
        public string classTopParentAnchor { get; set; }
        public string dataToggleTopParentAnchor { get; set; }
        public string classTopList { get; set; }
        public string classTopWrapper { get; set; }
        public string classItemActive { get; set; }
        public string classItemHover { get; set; }
        //public int Depth { get; set; }
        //public bool addRootToTier { get; set; }
        //
        //====================================================================================================
        public static menuModel @add(CPBaseClass cp)
        {
            return @add<menuModel>(cp);
        }
        //
        //====================================================================================================
        public static menuModel create(CPBaseClass cp, int recordId)
        {
            return create<menuModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static menuModel create(CPBaseClass cp, string recordGuid)
        {
            return create<menuModel>(cp, recordGuid);
        }
        //
        //====================================================================================================
        public static menuModel createByName(CPBaseClass cp, string recordName)
        {
            return createByName<menuModel>(cp, recordName);
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
            delete<menuModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static void delete(CPBaseClass cp, string ccGuid)
        {
            delete<menuModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static List<menuModel> createList(CPBaseClass cp, string sqlCriteria, string sqlOrderBy = "id")
        {
            return createList<menuModel>(cp, sqlCriteria, sqlOrderBy);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, int recordId)
        {
            return baseModel.getRecordName<menuModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, string ccGuid)
        {
            return baseModel.getRecordName<menuModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static int getRecordId(CPBaseClass cp, string ccGuid)
        {
            return baseModel.getRecordId<menuModel>(cp, ccGuid);
        }
    }
}
