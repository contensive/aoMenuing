
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using Contensive.BaseClasses;

namespace Contensive.Addons.MenuPages.Models.DbModels {
    public class MenuModel : BaseModel {
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
        public int depth { get; set; }
        public bool addRootToTier { get; set; }
        ////
        ////====================================================================================================
        //public static MenuModel @add(CPBaseClass cp) {
        //    return @add<MenuModel>(cp);
        //}
        ////
        ////====================================================================================================
        //public static MenuModel create(CPBaseClass cp, int recordId) {
        //    return create<MenuModel>(cp, recordId);
        //}
        ////
        ////====================================================================================================
        //public static MenuModel create(CPBaseClass cp, string recordGuid) {
        //    return create<MenuModel>(cp, recordGuid);
        //}
        ////
        ////====================================================================================================
        //public static MenuModel createByName(CPBaseClass cp, string recordName) {
        //    return createByName<MenuModel>(cp, recordName);
        //}
        ////
        ////====================================================================================================
        //public new void save(CPBaseClass cp) {
        //    base.save(cp);
        //}
        ////
        ////====================================================================================================
        //public static void delete(CPBaseClass cp, int recordId) {
        //    delete<MenuModel>(cp, recordId);
        //}
        ////
        ////====================================================================================================
        //public static void delete(CPBaseClass cp, string ccGuid) {
        //    delete<MenuModel>(cp, ccGuid);
        //}
        ////
        ////====================================================================================================
        //public static List<MenuModel> createList(CPBaseClass cp, string sqlCriteria, string sqlOrderBy = "id") {
        //    return createList<MenuModel>(cp, sqlCriteria, sqlOrderBy);
        //}
        ////
        ////====================================================================================================
        //public static string getRecordName(CPBaseClass cp, int recordId) {
        //    return BaseModel.getRecordName<MenuModel>(cp, recordId);
        //}
        ////
        ////====================================================================================================
        //public static string getRecordName(CPBaseClass cp, string ccGuid) {
        //    return BaseModel.getRecordName<MenuModel>(cp, ccGuid);
        //}
        ////
        ////====================================================================================================
        //public static int getRecordId(CPBaseClass cp, string ccGuid) {
        //    return BaseModel.getRecordId<MenuModel>(cp, ccGuid);
        //}
        //
        //====================================================================================================
        public static MenuModel createOrAddDefault(CPBaseClass cp, string instanceGuid) {
            MenuModel result = create<MenuModel>(cp, instanceGuid);
            if ( result != null ) { return result;  }
            result = BaseModel.add<MenuModel>(cp);
            result.ccguid = instanceGuid;
            result.addRootToTier = true;
            result.classItemActive = "";
            result.classItemFirst = "";
            result.classItemHover = "";
            result.classItemLast = "";
            result.classTierAnchor = "";
            result.classTierItem = "";
            result.classTierList = "";
            result.classTopAnchor = "";
            result.classTopItem = "";
            result.classTopList = "";
            result.classTopParentAnchor = "";
            result.classTopParentItem = "";
            result.classTopWrapper = "";
            result.dataToggleTopParentAnchor = "";
            result.save(cp);
            return result;
        }
    }
}
