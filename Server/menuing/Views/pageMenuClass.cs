
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Contensive.BaseClasses;

namespace Menuing.Views
{
    /// <summary>
    /// menuing based on pages directly attached to the menu (menuPageRules)
    /// </summary>
    public class pageMenuClass : Contensive.BaseClasses.AddonBaseClass
    {
        //
        // -- instance properties
        CPBaseClass cp;
        //
        // -- Contensive calls the execute method of your addon class
        public override object Execute(Contensive.BaseClasses.CPBaseClass cp)
        {
            this.cp = cp;
            string result = "";
            try
            {
                //
                // -- detect MenuLegacy and pass-through
                string legacyMenuName = cp.Doc.GetText("Create New Menu");
                int legacyMenuId = cp.Doc.GetInteger("Use Existing Menu");
                if ((!string.IsNullOrEmpty(legacyMenuName))|(legacyMenuId!=0))
                {
                    //
                    // -- legacy addon
                    result = cp.Utils.ExecuteAddon("{58E1A7C6-4136-4A3B-8916-C36883E76043}");
                }
                else
                {
                    //
                    // -- continue with this addon
                    int menuId = cp.Doc.GetInteger(constants.rnSelectMenuId);
                    Models.menuModel menu = null;
                    if (menuId != 0)
                    {
                        menu = Models.menuModel.create(cp, menuId);
                    }
                    if (menu == null)
                    { 
                        // -- No menu selected, try instance menu
                        string instanceId = cp.Doc.GetText("instanceId");
                        if (string.IsNullOrEmpty(instanceId))
                        {
                            // -- no instanceId, find or create default menu
                            menu = Models.menuModel.createByName(cp, "Default");
                            if (menu == null)
                            {
                                // -- no Default Menu, create it
                                menu = Models.menuModel.add(cp);
                                menu.name = "Default";
                                menu.save(cp);
                            }
                        }
                        else
                        {
                            // -- find or create instance menu
                            menu = Models.menuModel.create(cp, instanceId);
                            if (menu == null)
                            {
                                // -- no Default Menu, create it
                                menu = Models.menuModel.add(cp);
                                menu.ccguid = instanceId;
                                menu.save(cp);
                                menu.name = string.Format( "Menu {0}", menu.id);
                                menu.save(cp);
                            }
                        }
                    }
                    if (menu == null)
                    {
                        result = "<!-- Selected Menu not found -->";
                    }
                    else
                    {
                        //
                        // -- create toplists
                        StringBuilder topItemList = new StringBuilder();
                        string sql = "(AllowInMenus=1)and(id in (select pageId from ccMenuPageRules where menuID=" + menu.id + "))";
                        List<Models.pageContentModel> rootPageList = Models.pageContentModel.createList(cp, sql);
                        foreach (Models.pageContentModel rootPage in rootPageList)
                        {
                            bool blockRootPage = rootPage.BlockContent;
                            if (blockRootPage & cp.User.IsAuthenticated)
                            {
                                blockRootPage = !allowedPageIdList.Contains(rootPage.id);
                            }
                            if (!blockRootPage)
                            {
                                string classTopItem = menu.classTopItem;
                                if (rootPage == rootPageList.First()) { classTopItem += " " + menu.classItemFirst; }
                                if (rootPage == rootPageList.Last()) { classTopItem += " " + menu.classItemLast; }
                                //
                                // -- build child page list (tier list)
                                string itemHtmlId;
                                string tierList;
                                StringBuilder tierItemList = new StringBuilder();
                                sql = "(ParentID=" + rootPage.id + ")";
                                List<Models.pageContentModel> childPageList = Models.pageContentModel.createList(cp, sql);
                                if (menu.addRootToTier)
                                {
                                    //
                                    // -- add the root page to the tier flyout as needed
                                    string classTierItem = menu.classTierItem;
                                    classTierItem += " " + menu.classItemFirst;
                                    if (childPageList.Count == 0) { classTierItem += " " + menu.classItemLast; }
                                    itemHtmlId = string.Format("menu{0}Page{1}", menu.id.ToString(), rootPage.id.ToString());
                                    tierItemList.Append(cp.Html.li(getPageLink(cp, rootPage), "", classTopItem, itemHtmlId));
                                }
                                foreach (Models.pageContentModel childPage in childPageList)
                                {
                                    bool blockPage = childPage.BlockContent;
                                    if (blockPage & cp.User.IsAuthenticated)
                                    {
                                        blockPage = !allowedPageIdList.Contains(childPage.id);
                                    }
                                    if (!blockPage)
                                    {
                                        string classTierItem = menu.classTierItem;
                                        if (!menu.addRootToTier)
                                        {
                                            if (childPage == childPageList.First()) { classTierItem += " " + menu.classItemFirst; }
                                        }
                                        if (childPage == childPageList.Last()) { classTierItem += " " + menu.classItemLast; }
                                        itemHtmlId = string.Format("menu{0}Page{1}", menu.id.ToString(), childPage.id.ToString());
                                        tierItemList.Append(cp.Html.li(getPageLink(cp, childPage), "", classTopItem, itemHtmlId));
                                    }
                                }
                                itemHtmlId = string.Format("menu{0}Page{1}", menu.id.ToString(), rootPage.id.ToString());
                                tierList = cp.Html.ul(tierItemList.ToString(), "", menu.classTierList, itemHtmlId + "List");
                                topItemList.Append(cp.Html.li(getPageLink(cp, rootPage) + tierList,"", classTopItem, itemHtmlId));
                            }
                        }
                        result = cp.Html.ul(topItemList.ToString(), "menu" + menu.id.ToString() + "List", menu.classTopList);
                        result = cp.Html.div(result, "", menu.classTopWrapper);
                    }
                }
            }
            catch (Exception ex)
            {
                cp.Site.ErrorReport(ex);
                result = "error response";
            }
            return result;
        }
        //
        // -- create a listItem from a page
        private string getPageLink(CPBaseClass cp, Models.pageContentModel page)
        {
            try
            {
                string topItemCaption = page.MenuHeadline;
                if (string.IsNullOrEmpty(topItemCaption)) topItemCaption = page.name;
                return string.Format("<a title=\"{1}\" href=\"{0}\">{1}</a>", cp.Content.GetLinkAliasByPageID(page.id, "", ""), topItemCaption);
            }
            catch (Exception ex)
            {
                cp.Site.ErrorReport(ex);
            }
            return "";
        }
        //
        // -- list of pages explicitly allowed by this users group membership
        private List<int> allowedPageIdList
        {
            get
            {
                if (_allowedPageIdList == null)
                {
                    _allowedPageIdList = Models.pageContentModel.getAllowedPageIdList(cp);
                }
                return _allowedPageIdList;
            }
        }
        private List<int> _allowedPageIdList = null;
        //
        // -- list of sections explicitly allowed by this users group membership
        private List<int> allowedSectionIdList
        {
            get
            {
                if (_allowedSectionIdList == null)
                {
                    _allowedSectionIdList = Models.siteSectionsModel.getAllowedSectionIdList(cp);
                }
                return _allowedSectionIdList;
            }
        }
        private List<int> _allowedSectionIdList = null;
    }
}
