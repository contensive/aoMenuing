﻿

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Reflection;
using Contensive.BaseClasses;

namespace Contensive.Addons.MenuPages.Models.ViewModels {
    public class MenuPagesViewModel {
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
        //public int Depth { get; set; }
        //
        //====================================================================================================
        public static MenuPagesViewModel create(CPBaseClass cp, Models.DbModels.MenuModel menu) {
            MenuPagesViewModel result = null;
            try {

            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
            return result;
        }
    }
}

