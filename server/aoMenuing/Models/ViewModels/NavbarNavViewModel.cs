
using Contensive.Addons.Menuing.Models.DbModels;
using Contensive.BaseClasses;
using System;
using System.Collections.Generic;

namespace Contensive.Addons.Menuing.Models.ViewModels {
    public class NavbarNavViewModel {
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
        /// <summary>
        /// id of the top ul list
        /// </summary>
        public string topListHtmlId { get; set; }
        //
        //====================================================================================================
        public static NavbarNavViewModel create(CPBaseClass cp, MenuModel menu) {
            try {
                //
                cp.Utils.AppendLog("BootstrapNav40ViewModel, MenuPageList enter");
                //
                if (menu == null) { return new NavbarNavViewModel(); }
                //
                // -- use cache
                string cacheKey = cp.Cache.CreateKey("menu-" + menu.id + "-user=" + cp.User.Id.ToString());
                NavbarNavViewModel result = cp.Cache.GetObject<NavbarNavViewModel>(cacheKey);
                if (result == null) {
                    result = new NavbarNavViewModel {
                        menuId = menu.id,
                        topListHtmlId = "navbarNav" + menu.id,
                        classTopWrapper = menu.classTopWrapper,
                        topList = new List<TopListItemModel>()
                    };
                    if (!result.classTopWrapper.Contains("collapse navbar-collapse") && !result.classTopWrapper.Contains("navbar-collapse collapse")) {
                        //
                        // -- add collapse if not already incluided
                        result.classTopWrapper += " collapse navbar-collapse";
                    }
                    //
                    // -- checks if the top list contains the default bootstrap navbar-nav
                    result.classTopList = "";
                    if (!menu.classTopList.Contains("navbar-nav")) {
                        result.classTopList += " navbar-nav";
                    }
                    result.classTopList += " " + menu.classTopList;
                    bool editMode = cp.User.IsEditingAnything;
                    //
                    List<PageContentModel> MenuPageList = PageContentModel.getMenuRootList(cp, menu.id);
                    //
                    cp.Utils.AppendLog("BootstrapNav40ViewModel, MenuPageList.count [" + MenuPageList.Count + "]");
                    //
                    // -- create toplists
                    foreach (PageContentModel rootPage in MenuPageList) {
                        bool blockRootPage = rootPage.BlockContent & !cp.User.IsAdmin;
                        if (blockRootPage & cp.User.IsAuthenticated) {
                            blockRootPage = !result.allowedPageIdList(cp).Contains(rootPage.id);
                        }
                        if (!blockRootPage) {
                            //
                            cp.Utils.AppendLog("BootstrapNav40ViewModel, add rootPage to menu [" + rootPage.id + "]");
                            //
                            string topItemName = (!string.IsNullOrWhiteSpace(rootPage.MenuHeadline)) ? rootPage.MenuHeadline : (!string.IsNullOrWhiteSpace(rootPage.name)) ? rootPage.name : "Page" + rootPage.id.ToString();
                            var topListItem = new TopListItemModel {
                                classTopItem = menu.classTopItem.Replace("nav-item", ""),
                                classTopItemActive = (rootPage.id.Equals(cp.Doc.PageId)) ? "active" : string.Empty,
                                classTopItemAnchor = menu.classTopAnchor,
                                topItemPageId = rootPage.id,
                                topItemHref = cp.Content.GetPageLink(rootPage.id),
                                topItemName = string.IsNullOrWhiteSpace(topItemName) ? rootPage.name : topItemName,
                                classItemDraggable = (editMode ? "ccEditWrapper" : ""),
                                topItemHtmlId = "m" + menu.id + "p" + rootPage.id,
                                childList = new List<ChildListItemModel>()
                            };
                            if (menu.depth == 2) {
                                List<Models.DbModels.PageContentModel> pageChildList = Models.DbModels.PageContentModel.createList(cp, "(ParentID=" + rootPage.id + ")and(AllowInMenus>0)", "sortOrder,id");
                                if (pageChildList.Count > 0) {
                                    //
                                    // -- add root page as a child page
                                    if (menu.addRootToTier) {
                                        topListItem.childList.Add(new ChildListItemModel {
                                            childItemHref = topListItem.topItemHref,
                                            childItemName = topListItem.topItemName
                                        });
                                    }
                                    //
                                    // -- add child pages 
                                    foreach (var childPage in pageChildList) {
                                        bool blockPage = childPage.BlockContent && !menu.includeBlockedFlyoutPages;
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
                            }
                            result.topList.Add(topListItem);
                        }
                    }
                    if (editMode) {
                        result.topList.Add(new TopListItemModel {
                            topItemName = "Add-Page",
                            topItemHref = "/AddMenuPage?menuId=" + menu.id.ToString(),
                            classTopItemAnchor = menu.classTopAnchor,
                            classItemDraggable = ""
                        });
                    }
                    //
                    // -- build new cache
                    var dependentKeyList = new List<string>() { "page content", "menus", "menu page rules" };
                    cp.Cache.Store(cacheKey, result,DateTime.Now.AddHours(1),dependentKeyList);
                }
                //
                cp.Utils.AppendLog("BootstrapNav40ViewModel, MenuPageList exit");
                //
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
        private List<int> allowedPageIdList(CPBaseClass cp) {
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

