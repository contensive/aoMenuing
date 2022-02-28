
using Contensive.Addons.Menuing.Models.DbModels;
using Contensive.BaseClasses;
using System;
using System.Collections.Generic;

namespace Contensive.Addons.Menuing.Models.ViewModels {
    public class NavbarNavModel : DesignBlockBase.Models.View.DesignBlockViewBaseModel {
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
        public List<NavbarNavTopListItemModel> topList = new List<NavbarNavTopListItemModel>();
        /// <summary>
        /// id of the top ul list
        /// </summary>
        public string topListHtmlId { get; set; }
        /// <summary>
        /// when true, the view is editing
        /// </summary>
        public bool isEditing { get; set; }
        //
        //====================================================================================================
        public static NavbarNavModel create(CPBaseClass cp, MenuModel menu) {
            try {
                if (menu == null) { return new NavbarNavModel(); }
                //
                // -- use cache
                NavbarNavModel result = null;
                string cacheKey = cp.Cache.CreateKey("menu-" + menu.id + "-user=" + cp.User.Id.ToString());
                if (!cp.User.IsEditingAnything) {
                    result = cp.Cache.GetObject<NavbarNavModel>(cacheKey);
                }
                if (result == null) {
                    result = new NavbarNavModel {
                        menuId = menu.id,
                        topListHtmlId = "navbarNav" + menu.id,
                        classTopWrapper = menu.classTopWrapper,
                        topList = new List<NavbarNavTopListItemModel>()
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
                    result.isEditing = cp.User.IsEditingAnything;
                    //
                    List<PageContentModel> MenuPageList = PageContentModel.getMenuRootList(cp, menu.id);
                    //
                    cp.Utils.AppendLog("BootstrapNav40ViewModel, MenuPageList.count [" + MenuPageList.Count + "]");
                    //
                    // -- create toplists
                    foreach (PageContentModel rootPage in MenuPageList) {
                        bool blockRootPage = rootPage.blockContent & !cp.User.IsAdmin && !menu.includeBlockedFlyoutPages;
                        if (blockRootPage & cp.User.IsAuthenticated) {
                            blockRootPage = !result.allowedPageIdList(cp).Contains(rootPage.id);
                        }
                        if (!blockRootPage) {
                            //
                            cp.Utils.AppendLog("BootstrapNav40ViewModel, add rootPage to menu [" + rootPage.id + "]");
                            //
                            string topItemName = (!string.IsNullOrWhiteSpace(rootPage.menuHeadline)) ? rootPage.menuHeadline : (!string.IsNullOrWhiteSpace(rootPage.name)) ? rootPage.name : "Page" + rootPage.id.ToString();
                            var topListItem = new NavbarNavTopListItemModel {
                                classTopItem = menu.classTopItem.Replace("nav-item", "") + (string.IsNullOrEmpty(rootPage.menuClass) ? "" : " " + rootPage.menuClass),
                                classTopItemActive = (rootPage.id.Equals(cp.Doc.PageId)) ? "active" : string.Empty,
                                classTopItemAnchor = menu.classTopAnchor,
                                topItemPageId = rootPage.id,
                                topItemHref = !string.IsNullOrEmpty(rootPage.link) ? rootPage.link : cp.Content.GetPageLink(rootPage.id),
                                topItemName = string.IsNullOrWhiteSpace(topItemName) ? rootPage.name : topItemName,
                                classItemDraggable = (result.isEditing ? "ccEditWrapper" : ""),
                                topItemHtmlId = "m" + menu.id + "p" + rootPage.id,
                                childList = new List<ChildListItemModel>()
                            };
                            if (menu.depth == 2) {
                                List<Models.DbModels.PageContentModel> pageChildList = Contensive.Models.Db.DbBaseModel.createList<PageContentModel>(cp, "(ParentID=" + rootPage.id + ")and(AllowInMenus>0)", "sortOrder,id");
                                if (pageChildList.Count > 0) {
                                    //
                                    // -- add root page as a child page
                                    if (menu.addRootToTier) {
                                        topListItem.childList.Add(new ChildListItemModel {
                                            childItemHref = topListItem.topItemHref,
                                            childItemName = topListItem.topItemName,
                                            childItemClass = rootPage.menuClass
                                        });
                                    }
                                    //
                                    // -- add child pages 
                                    foreach (var childPage in pageChildList) {
                                        bool blockPage = childPage.blockContent && !menu.includeBlockedFlyoutPages;
                                        if (blockPage & cp.User.IsAuthenticated) {
                                            blockPage = !result.allowedPageIdList(cp).Contains(childPage.id);
                                        }
                                        if (!blockPage) {
                                            topListItem.childList.Add(new ChildListItemModel {
                                                childItemHref = !string.IsNullOrEmpty(childPage.link) ? childPage.link : cp.Content.GetPageLink(childPage.id),
                                                childItemName = !string.IsNullOrWhiteSpace(childPage.menuHeadline) ? childPage.menuHeadline : !string.IsNullOrWhiteSpace(childPage.name) ? childPage.name : "Page" + childPage.id.ToString(),
                                                childItemClass = childPage.menuClass
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
                    if (result.isEditing) {
                        result.topList.Add(new NavbarNavTopListItemModel {
                            topItemName = "Add-Page",
                            topItemHref = "/AddMenuPage?menuId=" + menu.id.ToString(),
                            classTopItemAnchor = menu.classTopAnchor,
                            classItemDraggable = ""
                        });
                    }
                    //
                    // -- build new cache
                    if (!result.isEditing) {
                        //
                        // -- if not editing, save cache
                        var dependentKeyList = new List<string>() {
                            cp.Cache.CreateTableDependencyKey(PageContentModel.tableMetadata.tableNameLower), cp.
                            Cache.CreateTableDependencyKey(MenuModel.tableMetadata.tableNameLower),
                            cp.Cache.CreateTableDependencyKey(MenuPageRuleModel.tableMetadata.tableNameLower)};
                        cp.Cache.Store(cacheKey, result, DateTime.Now.AddHours(1), dependentKeyList);
                    }
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
        // -- list of pages explicitly allowed by this users group membership
        private List<int> allowedPageIdList(CPBaseClass cp) {
            if (local_allowedPageIdList == null) {
                local_allowedPageIdList = Models.DbModels.PageContentModel.getAllowedPageIdList(cp);
            }
            return local_allowedPageIdList;
        }
        private List<int> local_allowedPageIdList { get; set; } = null;
    }
}

