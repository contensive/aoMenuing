

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Reflection;
using Contensive.BaseClasses;
using System.Linq;
using Contensive.Addons.MenuPages.Models.DbModels;

namespace Contensive.Addons.MenuPages.Models.ViewModels {
    public class BootstrapNav40ViewModel {
        /// <summary>
        /// class for the div wrapper. default blank
        /// </summary>
        public string classTopWrapper { get; set; }
        /// <summary>
        /// the id of the menu record
        /// </summary>
        public int menuId { get; set; }
        /// <summary>
        /// the class in the top level ul. default blank
        /// </summary>
        public string classTopList { get; set; }
        /// <summary>
        /// pages in the top row of the emenu
        /// </summary>
        public List<TopListItemModel> topList = new List<TopListItemModel>();

        //public string classTierList { get; set; }
        //public string classItemFirst { get; set; }
        ////public string classItemHover { get; set; }
        //public string classItemLast { get; set; }
        //public string classTierItem { get; set; }
        //public string classTierAnchor { get; set; }
        //public string classTopItem { get; set; }
        //public string classTopParentItem { get; set; }
        //public string classTopAnchor { get; set; }
        //public string classTopParentAnchor { get; set; }
        //public string dataToggleTopParentAnchor { get; set; }

        public class TopListItemModel {
            /// <summary>
            /// If this top list item is active (currently on this page), this is the word "active"
            /// </summary>
            public string classTopItemActive = "";
            /// <summary>
            /// if this top list item has a drop-down list, this is the word "dropdown"
            /// </summary>
            public string classTopItemDropdown = "";
            /// <summary>
            /// The class in the top list items anchor, if the item has a child list, "dropdown-toggle", else blank
            /// </summary>
            public string classTopItemAnchor = "";
            /// <summary>
            /// the id of the page record for this list item. Used for the html id between the link and the flyout
            /// </summary>
            public int topItemPageId = 0;
            /// <summary>
            /// a string of attributes to add to the item anchor if it has a child list -- data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"
            /// </summary>
            //public string dropdownAttributes = "";
            /// <summary>
            /// The name caption for this menu item
            /// </summary>
            public string topItemName = "";
            /// <summary>
            /// the link for this menu item
            /// </summary>
            public string topItemHref = "";
            /// <summary>
            /// if this item clicks to open a child list, populate this list
            /// </summary>
            public List<ChildListItemModel> childList = new List<ChildListItemModel>();
            /// <summary>
            /// true if childList.length>0
            /// </summary>
            public bool hasChildItems = false;
        }
        /// <summary>
        /// An item in the child list
        /// </summary>
        public class ChildListItemModel {
            public string childItemName;
            public string childItemHref;
        }

        //public int Depth { get; set; }
        //
        //====================================================================================================
        public static BootstrapNav40ViewModel create(CPBaseClass cp, Models.DbModels.MenuModel menu) {
            try {
                //var allowedPageIdList = new List<int>();
                var result = new BootstrapNav40ViewModel();
                if (menu == null) { return result; }
                {
                    result.menuId = menu.id;
                   
                    //checks if the top wrapper already contains the default bootstrap navbar-collapse collapse
                    String appendedTopWrapper = "";
                    String classTopWrapper = menu.classTopWrapper;
                    String firstToCheck = "collapse navbar-collapse";
                    String secondToCheck = "navbar-collapse collapse";
                    if(!classTopWrapper.Contains(firstToCheck) && !classTopWrapper.Contains(secondToCheck))
                    {
                        appendedTopWrapper += "collapse navbar-collapse";
                    }
                    appendedTopWrapper += classTopWrapper;

                    //checks if the top list contains the default bootstrap navbar-nav
                    String appendedTopList = "";
                    String defaultTopList = "navbar-nav";
                    String classTopList = menu.classTopList;
                    if(!classTopList.Contains(defaultTopList))
                    {
                        appendedTopList += "navbar-nav";
                    }
                    appendedTopList += classTopList;

                    result.classTopWrapper = appendedTopWrapper; // "bootstrapNavCon" + ((string.IsNullOrWhiteSpace(menu.classTopWrapper) ? "" : " " + menu.classTopWrapper));
                    result.classTopList = appendedTopList;
                    result.topList = new List<BootstrapNav40ViewModel.TopListItemModel>();
                    //
                    // -- create toplists
                    foreach (var rootPage in PageContentModel.getMenuRootList(cp, menu.id)) {
                        bool blockRootPage = rootPage.BlockContent & !cp.User.IsAdmin;
                        if (blockRootPage & cp.User.IsAuthenticated) {
                            blockRootPage = !result.allowedPageIdList(cp).Contains(rootPage.id);
                        }
                        if (!blockRootPage) {
                            var topListItem = new BootstrapNav40ViewModel.TopListItemModel();
                            topListItem.classTopItemActive = (rootPage.id.Equals(cp.Doc.PageId)) ? "active" : string.Empty;
                            topListItem.classTopItemAnchor = menu.classTopAnchor;
                            topListItem.topItemPageId = rootPage.id;
                            topListItem.topItemHref = cp.Content.GetPageLink(rootPage.id);
                            topListItem.topItemName = (!string.IsNullOrWhiteSpace(rootPage.MenuHeadline)) ? rootPage.MenuHeadline : (!string.IsNullOrWhiteSpace(rootPage.name)) ? rootPage.name : "Page" + rootPage.id.ToString();
                            if (string.IsNullOrEmpty(topListItem.topItemName)) topListItem.topItemName = rootPage.name;
                            topListItem.childList = new List<ChildListItemModel>();
                            List<Models.DbModels.PageContentModel> pageChildList = Models.DbModels.PageContentModel.createList(cp, "(ParentID=" + rootPage.id + ")and(AllowInMenus>0)", "sortOrder,id");
                            if (pageChildList.Count > 0) {
                                //
                                // -- add root page as a child page
                                if (menu.addRootToTier){
                                    topListItem.childList.Add(new ChildListItemModel
                                    {
                                        childItemHref = topListItem.topItemHref,
                                        childItemName = topListItem.topItemName
                                    });
                                }
                                //
                                // -- add child pages 
                                foreach (var childPage in pageChildList) {
                                    bool blockPage = childPage.BlockContent;
                                    if (blockPage & cp.User.IsAuthenticated) {
                                        blockPage = !result.allowedPageIdList(cp).Contains(childPage.id);
                                    }
                                    if (!blockPage) {
                                        topListItem.childList.Add(new ChildListItemModel {
                                            childItemHref = cp.Content.GetPageLink(childPage.id),
                                            childItemName = (!string.IsNullOrWhiteSpace(childPage.MenuHeadline)) ? childPage.MenuHeadline : (!string.IsNullOrWhiteSpace(childPage.name)) ? childPage.name : "Page" + childPage.id.ToString()
                                        });
                                    }
                                }
                            }
                            if (!topListItem.childList.Count.Equals(0)) {
                                topListItem.classTopItemDropdown = "dropdown";
                                topListItem.hasChildItems = true;
                            }
                            result.topList.Add(topListItem);
                        }
                    }
                    if(cp.User.IsEditing("")) {
                        var fakeItem = new TopListItemModel();
                        fakeItem.topItemName = "Add-Page";
                        fakeItem.topItemHref = "/AddMenuPage?menuId=" + menu.id.ToString();
                        fakeItem.classTopItemAnchor = menu.classTopAnchor;
                        result.topList.Add(fakeItem);
                    }
                }
                return result;
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }

        //
        // -- create a listItem from a page
        private static string getAnchor(CPBaseClass cp, Models.DbModels.PageContentModel page, string htmlClass, string dataToggleValue) {
            try {
                string topItemCaption = page.MenuHeadline;
                if (string.IsNullOrEmpty(topItemCaption)) topItemCaption = page.name;
                string pageLink = cp.Content.GetPageLink(page.id);
                string dataToggleAttr;
                if (string.IsNullOrEmpty(dataToggleValue)) { dataToggleAttr = string.Empty; } else { dataToggleAttr = " data-toggle=\"" + dataToggleValue + "\""; }
                //string pageList = cp.Content.GetLinkAliasByPageID(page.id, "", "");
                return string.Format("<a class=\"{2}\" title=\"{1}\" href=\"{0}\"{3}>{1}</a>", pageLink, topItemCaption, htmlClass, dataToggleAttr);
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
            }
            return "";
        }
        //
        // -- list of pages explicitly allowed by this users group membership
        private List<int> allowedPageIdList( CPBaseClass cp ) {
                if (_allowedPageIdList == null) {
                    _allowedPageIdList = Models.DbModels.PageContentModel.getAllowedPageIdList(cp);
                }
                return _allowedPageIdList;
        }
        private List<int> _allowedPageIdList = null;
        //
        // -- list of sections explicitly allowed by this users group membership
        //private List<int> allowedSectionIdList {
        //    get {
        //        if (_allowedSectionIdList == null) {
        //            _allowedSectionIdList = Models.DbModels.SiteSectionsModel.getAllowedSectionIdList(cp);
        //        }
        //        return _allowedSectionIdList;
        //    }
        //}
        //private List<int> _allowedSectionIdList = null;
    }
}

