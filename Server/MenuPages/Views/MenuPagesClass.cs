
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Contensive.BaseClasses;

namespace Contensive.Addons.BootstrapNav.Views {
    /// <summary>
    /// menuing based on pages directly attached to the menu (menuPageRules)
    /// </summary>
    public class MenuPagesClass : Contensive.BaseClasses.AddonBaseClass {
        //
        // -- instance properties
        CPBaseClass cp;
        //
        // ====================================================================================================
        //
        public override object Execute(Contensive.BaseClasses.CPBaseClass cp) {
            this.cp = cp;
            string result = "";
            try {
                //
                // -- get Menu
                int menuId = cp.Doc.GetInteger(constants.rnSelectMenuId);
                Models.Entity.menuModel menu = null;
                if (menuId != 0) {
                    menu = Models.Entity.menuModel.create(cp, menuId);
                }
                if (menu == null) {
                    // -- No menu selected, try instance menu
                    string instanceId = cp.Doc.GetText("instanceId");
                    if (string.IsNullOrEmpty(instanceId)) {
                        // -- no instanceId, find or create default menu
                        menu = Models.Entity.menuModel.createByName(cp, "Default");
                        if (menu == null) {
                            // -- no Default Menu, create it
                            menu = Models.Entity.menuModel.add(cp);
                            menu.name = "Default";
                            menu.save(cp);
                        }
                    } else {
                        // -- find or create instance menu
                        menu = Models.Entity.menuModel.create(cp, instanceId);
                        if (menu == null) {
                            // -- no Default Menu, create it
                            menu = Models.Entity.menuModel.add(cp);
                            menu.ccguid = instanceId;
                            menu.save(cp);
                            menu.name = string.Format("Menu {0}", menu.id);
                            menu.save(cp);
                        }
                    }
                }
                if (menu == null) {
                    result = "<!-- Selected Menu not found -->";
                } else {
                    //
                    // -- create toplists
                    StringBuilder topItemList = new StringBuilder();
                    string sql = "(AllowInMenus=1)and(id in (select pageId from ccMenuPageRules where menuID=" + menu.id + "))";
                    List<Models.Entity.pageContentModel> rootPageList = Models.Entity.pageContentModel.createList(cp, sql);
                    foreach (Models.Entity.pageContentModel rootPage in rootPageList) {
                        bool blockRootPage = rootPage.BlockContent & !cp.User.IsAdmin;
                        if (blockRootPage & cp.User.IsAuthenticated) {
                            blockRootPage = !allowedPageIdList.Contains(rootPage.id);
                        }
                        if (!blockRootPage) {
                            string classTopItem = menu.classTopItem;
                            if (!string.IsNullOrEmpty(rootPage.menuClass)) { classTopItem += " " + rootPage.menuClass; }
                            if (rootPage == rootPageList.First()) { classTopItem += " " + menu.classItemFirst; }
                            if (rootPage == rootPageList.Last()) { classTopItem += " " + menu.classItemLast; }
                            //
                            // -- build child page list (tier list)
                            string itemHtmlId;
                            string tierList;
                            StringBuilder tierItemList = new StringBuilder();
                            sql = "(ParentID=" + rootPage.id + ")";
                            List<Models.Entity.pageContentModel> childPageList = Models.Entity.pageContentModel.createList(cp, sql);
                            //
                            // -- add the root page to the tier flyout as needed
                            string classTierItem = menu.classTierItem;
                            classTierItem += " " + menu.classItemFirst;
                            if (childPageList.Count == 0) { classTierItem += " " + menu.classItemLast; }
                            itemHtmlId = string.Format("menu{0}Page{1}", menu.id.ToString(), rootPage.id.ToString());
                            tierItemList.Append(cp.Html.li(getAnchor(cp, rootPage, menu.classTierAnchor), "", classTopItem, itemHtmlId));
                            foreach (Models.Entity.pageContentModel childPage in childPageList) {
                                bool blockPage = childPage.BlockContent;
                                if (blockPage & cp.User.IsAuthenticated) {
                                    blockPage = !allowedPageIdList.Contains(childPage.id);
                                }
                                if (!blockPage) {
                                    //if (!menu.addRootToTier)
                                    //{
                                    //    if (childPage == childPageList.First()) { classTierItem += " " + menu.classItemFirst; }
                                    //}
                                    if (childPage == childPageList.Last()) { classTierItem += " " + menu.classItemLast; }
                                    if (!string.IsNullOrEmpty(childPage.menuClass)) { classTierItem += " " + childPage.menuClass; }
                                    itemHtmlId = string.Format("menu{0}Page{1}", menu.id.ToString(), childPage.id.ToString());
                                    tierItemList.Append(cp.Html.li(getAnchor(cp, childPage, menu.classTierAnchor), "", classTierItem, itemHtmlId));
                                }
                            }
                            itemHtmlId = string.Format("menu{0}Page{1}", menu.id.ToString(), rootPage.id.ToString());
                            tierList = cp.Html.ul(tierItemList.ToString(), "", menu.classTierList, itemHtmlId + "List");
                            topItemList.Append(cp.Html.li(getAnchor(cp, rootPage, menu.classTopAnchor) + tierList, "", classTopItem, itemHtmlId));
                        }
                    }
                    result = cp.Html.ul(topItemList.ToString(), "menu" + menu.id.ToString() + "List", menu.classTopList);
                    if (!string.IsNullOrEmpty(menu.classTopWrapper)) {
                        result = cp.Html.div(result, "", menu.classTopWrapper);
                    }

                }
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                result = "error response";
            }
            return result;
        }
        //
        // -- create a listItem from a page
        private string getAnchor(CPBaseClass cp, Models.Entity.pageContentModel page, string htmlClass) {
            try {
                string topItemCaption = page.MenuHeadline;
                if (string.IsNullOrEmpty(topItemCaption)) topItemCaption = page.name;
                string pageLink = cp.Content.GetPageLink(page.id);
                //string pageList = cp.Content.GetLinkAliasByPageID(page.id, "", "");
                return string.Format("<a class=\"{2}\" title=\"{1}\" href=\"{0}\">{1}</a>", pageLink, topItemCaption, htmlClass);
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
            }
            return "";
        }
        //
        // -- list of pages explicitly allowed by this users group membership
        private List<int> allowedPageIdList {
            get {
                if (_allowedPageIdList == null) {
                    _allowedPageIdList = Models.Entity.pageContentModel.getAllowedPageIdList(cp);
                }
                return _allowedPageIdList;
            }
        }
        private List<int> _allowedPageIdList = null;
        //
        // -- list of sections explicitly allowed by this users group membership
        private List<int> allowedSectionIdList {
            get {
                if (_allowedSectionIdList == null) {
                    _allowedSectionIdList = Models.Entity.siteSectionsModel.getAllowedSectionIdList(cp);
                }
                return _allowedSectionIdList;
            }
        }
        private List<int> _allowedSectionIdList = null;
    }
}
