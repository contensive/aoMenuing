

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
                                            // -- unblock if you are in the necessary groups
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
        // -- list of GroupRules that the current user belongs
        //private List<int>userGroupIdList {
        //    get
        //    {
        //        if (_userGroupIdList == null)
        //        {
        //            List<Models.MemberRuleModel> ruleList = Models.MemberRuleModel.createList(cp, "(memberid="+cp.User.Id+")");
        //            foreach (Models.MemberRuleModel rule in ruleList)
        //            {
        //                if (!_userGroupIdList.Contains(rule.GroupID))
        //                {
        //                    _userGroupIdList.Add(rule.GroupID);
        //                }
        //            }
        //        }
        //        return _userGroupIdList;
        //    }
        //}
        //private List<int> _userGroupIdList = null;
    }
}
