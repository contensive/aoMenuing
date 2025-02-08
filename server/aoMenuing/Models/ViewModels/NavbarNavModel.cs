
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
                if (!cp.User.IsEditing("")) {
                    string resultJson = cp.Cache.GetText(cacheKey);
                    if (!string.IsNullOrEmpty(resultJson)) {
                        result = cp.JSON.Deserialize<NavbarNavModel>(resultJson);
                    }
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
                    result.isEditing = cp.User.IsEditing("");
                    //
                    List<PageContentModel> MenuPageList = PageContentModel.getMenuRootList(cp, menu.id);
                    //
                    cp.Utils.AppendLog("BootstrapNav40ViewModel, MenuPageList.count [" + MenuPageList.Count + "]");
                    //
                    // -- remove the extra 'edit' link added to the front of the menu and go back to edit tag and border
                    //if (result.isEditing && !cp.Site.GetBoolean("allow edit modal beta", false)) {
                    //    //
                    //    // -- add edit link if is editing, and not in edit-modal mode
                    //    result.topList.Add(new NavbarNavTopListItemModel {
                    //        topItemName = "Edit-Menu",
                    //        topItemHref = $"{cp.GetAppConfig().adminRoute}?aa=0&cid={cp.Content.GetID("menus")}&tx=&asf=1&af=4&id={menu.id}",
                    //        classTopItemAnchor = menu.classTopAnchor,
                    //        classItemDraggable = "",
                    //        includeDragableIcon = false
                    //    });
                    //}                    //
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
                                classTopItemAnchor = menu.classTopAnchor,
                                topItemPageId = rootPage.id,
                                topItemHref = !string.IsNullOrEmpty(rootPage.link) ? rootPage.link : cp.Content.GetPageLink(rootPage.id),
                                topItemName = string.IsNullOrWhiteSpace(topItemName) ? rootPage.name : topItemName,
                                classItemDraggable = (result.isEditing ? "ccEditWrapper" : ""),
                                topItemHtmlId = "m" + menu.id + "p" + rootPage.id,
                                childList = new List<ChildListItemModel>(),
                                includeDragableIcon = true
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
                                            childPageId = rootPage.id,
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
                                                childPageId = childPage.id,
                                                childItemClass = "" 
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
                            classItemDraggable = "",
                            includeDragableIcon = false
                        });
                    }
                    //
                    // -- build new cache
                    if (!result.isEditing) {
                        //
                        // -- if not editing, save cache
                        var dependentKeyList = new List<string>() {
                            cp.Cache.CreateTableDependencyKey(PageContentModel.tableMetadata.tableNameLower), 
                            cp.Cache.CreateTableDependencyKey(MenuModel.tableMetadata.tableNameLower),
                            cp.Cache.CreateTableDependencyKey(MenuPageRuleModel.tableMetadata.tableNameLower)
                        };
                        string resultJson = cp.JSON.Serialize(result);
                        cp.Cache.Store(cacheKey, resultJson, DateTime.Now.AddHours(1), dependentKeyList);
                    }
                }
                //
                // -- set active item (not included in cache)
                int pageId = cp.Doc.PageId;
                foreach (NavbarNavTopListItemModel topListItem in result.topList) {
                    if (topListItem.topItemPageId == pageId) {
                        topListItem.classTopItemActive = "active";
                    }
                    foreach (ChildListItemModel childItem in topListItem.childList) {
                        if (childItem.childPageId == pageId) {
                            topListItem.classTopItemActive = "active";
                            childItem.childItemClass += " active";
                            break;
                        }
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

