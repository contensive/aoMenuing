

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Contensive.BaseClasses;

namespace Menuing.Views
{
    public class menuClass : Contensive.BaseClasses.AddonBaseClass
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
                int menuId = cp.Doc.GetInteger(constants.rnSelectMenuId);
                if (menuId == 0)
                {
                    result = "<!-- No Menu Selected -->";
                }
                else
                {
                    Models.dynamicMenuModel menu = Models.dynamicMenuModel.create(cp, menuId);
                    if (menu == null)
                    {
                        result = "<!-- Selected Menu not found -->";
                    }
                    else
                    {
                        //
                        // -- create toplists
                        StringBuilder topItemList = new StringBuilder();
                        string sql = "(not(HideMenu=1))and(id in (select sectionId from ccDynamicMenuSectionRules where DynamicMenuID=" + menuId + "))";
                        List<Models.siteSectionsModel> sectionList = Models.siteSectionsModel.createList(cp, sql, "sortOrder,id");
                        foreach (Models.siteSectionsModel section in sectionList)
                        {
                            //
                            // -- check if section is blocked to this person, if so skip it
                            bool sectionBlocked = section.BlockSection;
                            if(sectionBlocked & cp.User.IsAuthenticated)
                            {
                                List<int> allowedSectionIdList = Models.siteSectionsModel.getAllowedSectionIdList(cp);
                                sectionBlocked = ! allowedSectionIdList.Contains(section.id);
                            }
                            if (!sectionBlocked)
                            {
                                string classTopItem = menu.classTopItem;
                                if (section == sectionList.First()) { classTopItem += " " + menu.classItemFirst; }
                                if (section == sectionList.Last()) { classTopItem += " " + menu.classItemLast; }
                                //
                                // -- create tieritems
                                Models.pageContentModel rootPage = Models.pageContentModel.create(cp, section.RootPageID);
                                StringBuilder tierItemList = new StringBuilder();
                                if (rootPage != null)
                                {
                                    bool blockRootPage = rootPage.BlockContent;
                                    if (blockRootPage & cp.User.IsAuthenticated)
                                    {
                                        blockRootPage = !allowedPageIdList.Contains(rootPage.id);
                                    }
                                    if (!blockRootPage)
                                    {
                                        //
                                        // -- build child page list (tier list)
                                        string itemHtmlId;
                                        string tierList;
                                        sql = "(ParentID=" + section.RootPageID + ")";
                                        List<Models.pageContentModel> childPageList = Models.pageContentModel.createList(cp, sql);
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
                                                if (childPage == childPageList.First()) { classTierItem += " " + menu.classItemFirst; }
                                                if (childPage == childPageList.Last()) { classTierItem += " " + menu.classItemLast; }
                                                itemHtmlId = string.Format("menu{0}Page{1}", menu.id.ToString(), childPage.id.ToString());
                                                tierItemList.Append(cp.Html.li(getPageLink(cp, childPage), itemHtmlId, classTopItem));
                                            }
                                        }
                                        itemHtmlId = string.Format("menu{0}Page{1}", menu.id.ToString(), rootPage.id.ToString());
                                        tierList = cp.Html.ul(tierItemList.ToString(), "", menu.classTierList, itemHtmlId + "List");
                                        topItemList.Append(cp.Html.li(getPageLink(cp, rootPage) + tierList, itemHtmlId, classTopItem));
                                    }
                                }
                            }
                        }
                        result = cp.Html.ul(topItemList.ToString() , "menu" + menu.id.ToString()+"List", menu.classTopList);
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
