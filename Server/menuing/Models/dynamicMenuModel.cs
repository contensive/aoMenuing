
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
    public class dynamicMenuModel : baseModel
    {
        //
        //====================================================================================================
        //-- const
        public const string contentName = "dynamic menus";
        public const string contentTableName = "ccdynamicmenus";
        //
        //====================================================================================================
        // -- instance properties
        public string classFlyoutParent { get; set; }
        public string classItemActive { get; set; }
        public string classItemFirst { get; set; }
        public string classItemHover { get; set; }
        public string classItemLast { get; set; }
        public string classTierItem { get; set; }
        public string classTierList { get; set; }
        public string classTopItem { get; set; }
        public string classTopList { get; set; }
        public string classTopWrapper { get; set; }
        public int ContentCategoryID { get; set; }
        public string Delimiter { get; set; }
        public int Depth { get; set; }
        public bool EditArchive { get; set; }
        public bool EditBlank { get; set; }
        public int EditSourceID { get; set; }
        public int FlyoutDirection { get; set; }
        public bool FlyoutOnHover { get; set; }
        public string JavaScriptOnLoad { get; set; }
        public string JSFilename { get; set; }
        public int Layout { get; set; }
        public string listStylesFilename { get; set; }
        public string StylePrefix { get; set; }
        public string StylesFilename { get; set; }
        public bool useJsFlyoutCode { get; set; }
        //
        //====================================================================================================
        public static dynamicMenuModel @add(CPBaseClass cp)
        {
            return @add<dynamicMenuModel>(cp);
        }
        //
        //====================================================================================================
        public static dynamicMenuModel create(CPBaseClass cp, int recordId)
        {
            return create<dynamicMenuModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static dynamicMenuModel create(CPBaseClass cp, string recordGuid)
        {
            return create<dynamicMenuModel>(cp, recordGuid);
        }
        //
        //====================================================================================================
        public static dynamicMenuModel createByName(CPBaseClass cp, string recordName)
        {
            return createByName<dynamicMenuModel>(cp, recordName);
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
            delete<dynamicMenuModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static void delete(CPBaseClass cp, string ccGuid)
        {
            delete<dynamicMenuModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static List<dynamicMenuModel> createList(CPBaseClass cp, string sqlCriteria, string sqlOrderBy = "id")
        {
            return createList<dynamicMenuModel>(cp, sqlCriteria, sqlOrderBy);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, int recordId)
        {
            return baseModel.getRecordName<dynamicMenuModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, string ccGuid)
        {
            return baseModel.getRecordName<dynamicMenuModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static int getRecordId(CPBaseClass cp, string ccGuid)
        {
            return baseModel.getRecordId<dynamicMenuModel>(cp, ccGuid);
        }
    }
}
