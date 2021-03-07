
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Contensive.BaseClasses;
using Contensive.Addons.Menuing.Models;
using Contensive.Addons.Menuing.Models.DbModels;
using Contensive.Addons.Menuing.Controllers;

namespace Contensive.Addons.Menuing.Views {
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
            string hint = "enter";
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
                hint = "20";
                if (menu == null) {
                    result = "<!-- Instance Menu not found -->";
                } else {
                    //
                    // -- create toplists
                    int activePageId = cp.Doc.PageId;
                    StringBuilder topItemList = new StringBuilder();
                    //string sql = "(AllowInMenus=1)and(id in (select pageId from ccMenuPageRules where menuID=" + menu.id + "))";
                    //List<PageContentModel> rootPageList = PageContentModel.createList(cp, sql,"sortOrder,id");
                    var rootPageList = PageContentModel.getMenuRootList(cp, menu.id);
                    foreach (PageContentModel rootPage in rootPageList) {
                        hint = "30";
                        bool blockRootPage = rootPage.BlockContent & !cp.User.IsAdmin;
                        hint = "31";
                        if (blockRootPage & cp.User.IsAuthenticated) {
                            hint = "32";
                            blockRootPage = !allowedPageIdList.Contains(rootPage.id);
                        }
                        hint = "33";
                        if (!blockRootPage) {
                            hint = "40";
                            string classTopItem = menu.classTopItem;
                            if (!string.IsNullOrEmpty(rootPage.menuClass)) { classTopItem += " " + rootPage.menuClass; }
                            if (rootPage == rootPageList.First()) { classTopItem += " " + menu.classItemFirst; }
                            if (rootPage == rootPageList.Last()) { classTopItem += " " + menu.classItemLast; }
                            if (rootPage.id == activePageId) { classTopItem += " " + menu.classItemActive; }
                            //
                            // -- build child page list (tier list)
                            string itemHtmlId;
                            string tierList;
                            StringBuilder tierItemList = new StringBuilder();
                            string sql = "(ParentID=" + rootPage.id + ") and (allowinmenus>0)";
                            List<PageContentModel> childPageList = null;
                            //
                            // -- depth is a lookup list. Easier for user to understand, and later we can add high limits if required. For now, just 0 tiers,1 tier
                            // -- depth field made visible c5.1.191010
                            if (menu.depth >= 2) {
                                //
                                // -- child flyouts
                                childPageList = PageContentModel.createList(cp, sql, "sortOrder,id");
                            } else {
                                //
                                // -- 0,1 = no flyout
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
                                hint = "50";
                                bool blockPage = childPage.BlockContent;
                                if (blockPage & cp.User.IsAuthenticated) {
                                    blockPage = !allowedPageIdList.Contains(childPage.id);
                                }
                                if (!blockPage) {
                                    hint = "55";
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
                            hint = "60";
                            itemHtmlId = string.Format("menu{0}Page{1}", menu.id.ToString(), rootPage.id.ToString());
                            tierList = cp.Html.ul(tierItemList.ToString(), "", menu.classTierList, itemHtmlId + "List");
                            topItemList.Append(cp.Html.li(getAnchor(cp, rootPage, menu.classTopAnchor) + tierList, "", classTopItem, itemHtmlId));
                        }
                    }
                    if (cp.User.IsEditing("")) {
                        topItemList.Append(cp.Html.li(string.Format("<a class=\"{2}\" title=\"{1}\" href=\"{0}\">{1}</a>", "/AddMenuPage?menuId=" + menu.id, "Add-Page", menu.classTopAnchor), "", menu.classTopItem));
                    }
                    hint = "70";
                    result = cp.Html.ul(topItemList.ToString(), "menu" + menu.id.ToString() + "List", menu.classTopList);
                    // 
                    // -- if editing enabled, add the link and wrapperwrapper
                    result = genericController.addEditWrapper(cp, result, menu.id, MenuModel.contentName, menu.name);
                    //
                    // -- container
                    result = cp.Html.div(result, "", "menuPagesCon" + ((string.IsNullOrWhiteSpace(menu.classTopWrapper) ? "" : " " + menu.classTopWrapper)));
                }
                return result;
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex, "hint [" + hint + "]");
                throw;
            }
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
