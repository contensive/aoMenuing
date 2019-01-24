
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Contensive.BaseClasses;
using Contensive.Addons.MenuPages.Models;
using Contensive.Addons.MenuPages.Models.DbModels;

namespace Contensive.Addons.MenuPages.Views {
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
                // -- determine controlling record in MenuModel
                MenuModel menu = null;
                string instanceId = cp.Doc.GetText("instanceId");
                if (string.IsNullOrEmpty(instanceId)) {
                    // -- no instanceId, find or create default menu
                    menu = BaseModel.createByName<MenuModel>(cp, "Default");
                    if (menu == null) {
                        // -- no Default Menu, create it
                        menu = BaseModel.add<MenuModel>(cp);
                        menu.name = "Default";
                        menu.save(cp);
                    }
                } else {
                    // -- find or create instance menu
                    menu = BaseModel.create<MenuModel>(cp, instanceId);
                    if (menu == null) {
                        // -- no Default Menu, create it
                        menu = BaseModel.add<MenuModel>(cp);
                        menu.ccguid = instanceId;
                        menu.name = string.Format("Menu {0}", menu.id);
                        menu.save(cp);
                    }
                }
                if (menu == null) {
                    result = "<!-- Instance Menu not found -->";
                } else {
                    //
                    // -- create toplists
                    int activePageId = cp.Doc.PageId;
                    StringBuilder topItemList = new StringBuilder();
                    string sql = "(AllowInMenus=1)and(id in (select pageId from ccMenuPageRules where menuID=" + menu.id + "))";
                    List<PageContentModel> rootPageList = PageContentModel.createList(cp, sql,"sortOrder,id");
                    foreach (PageContentModel rootPage in rootPageList) {
                        bool blockRootPage = rootPage.BlockContent & !cp.User.IsAdmin;
                        if (blockRootPage & cp.User.IsAuthenticated) {
                            blockRootPage = !allowedPageIdList.Contains(rootPage.id);
                        }
                        if (!blockRootPage) {
                            string classTopItem = menu.classTopItem;
                            if (!string.IsNullOrEmpty(rootPage.menuClass)) { classTopItem += " " + rootPage.menuClass; }
                            if (rootPage == rootPageList.First()) { classTopItem += " " + menu.classItemFirst; }
                            if (rootPage == rootPageList.Last()) { classTopItem += " " + menu.classItemLast; }
                            if (rootPage.id == activePageId ) { classTopItem += " " + menu.classItemActive; }
                            //
                            // -- build child page list (tier list)
                            string itemHtmlId;
                            string tierList;
                            StringBuilder tierItemList = new StringBuilder();
                            sql = "(ParentID=" + rootPage.id + ")";
                            List<PageContentModel> childPageList = null;
                            if (menu.depth > 0) {
                                childPageList = PageContentModel.createList(cp, sql, "sortOrder,id");
                            } else {
                                childPageList = new List<PageContentModel>();
                            }
                            //
                            // -- add the root page to the tier flyout as needed
                            string classTierItem = menu.classTierItem;
                            classTierItem += " " + menu.classItemFirst;
                            if (childPageList.Count == 0) { classTierItem += " " + menu.classItemLast; }
                            if (menu.addRootToTier) {
                                cp.Utils.AppendLog("menuAddRootToTier1" + menu.addRootToTier.ToString());
                                itemHtmlId = string.Format("menu{0}Page{1}", menu.id.ToString(), rootPage.id.ToString());
                                tierItemList.Append(cp.Html.li(getAnchor(cp, rootPage, menu.classTierAnchor), "", classTopItem, itemHtmlId));
                            }
                            cp.Utils.AppendLog("menuAddRootToTier2" + menu.addRootToTier.ToString());
                            foreach (PageContentModel childPage in childPageList) {
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
                                    if (childPage.id == activePageId) { classTierItem += " " + menu.classItemActive; }
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
        private string getAnchor(CPBaseClass cp, PageContentModel page, string htmlClass) {
            try {
                string topItemCaption = page.MenuHeadline;
                if (string.IsNullOrEmpty(topItemCaption)) topItemCaption = page.name;
                if (string.IsNullOrEmpty(topItemCaption)) topItemCaption = "UnnamedPage" + page.id;
                string pageLink = (string.IsNullOrWhiteSpace(page.Link)) ? cp.Content.GetPageLink(page.id) : page.Link;
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
                    _allowedPageIdList = PageContentModel.getAllowedPageIdList(cp);
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
                    _allowedSectionIdList = SiteSectionsModel.getAllowedSectionIdList(cp);
                }
                return _allowedSectionIdList;
            }
        }
        private List<int> _allowedSectionIdList = null;
    }
}
