
using System;
using System.Collections.Generic;
using System.Text;
using Contensive.BaseClasses;
using Contensive.Addons.MenuPages.Models;
using Contensive.Addons.MenuPages.Views;

namespace Contensive.Addons.MenuPages.Controllers {
    public static class genericController {
        //
        //====================================================================================================
        /// <summary>
        /// if date is invalid, set to minValue
        /// </summary>
        /// <param name="srcDate"></param>
        /// <returns></returns>
        public static DateTime encodeMinDate(DateTime srcDate) {
            DateTime returnDate = srcDate;
            if (srcDate < new DateTime(1900, 1, 1)) {
                returnDate = DateTime.MinValue;
            }
            return returnDate;
        }
        //
        //====================================================================================================
        /// <summary>
        /// if valid date, return the short date, else return blank string 
        /// </summary>
        /// <param name="srcDate"></param>
        /// <returns></returns>
        public static string getShortDateString(DateTime srcDate) {
            string returnString = "";
            DateTime workingDate = encodeMinDate(srcDate);
            if (!isDateEmpty(srcDate)) {
                returnString = workingDate.ToShortDateString();
            }
            return returnString;
        }
        //
        //====================================================================================================
        public static bool isDateEmpty(DateTime srcDate) {
            return (srcDate < new DateTime(1900, 1, 1));
        }
        //
        //====================================================================================================
        public static string getSortOrderFromInteger(int id) {
            return id.ToString().PadLeft(7, '0');
        }
        //
        //====================================================================================================
        public static string getDateForHtmlInput(DateTime source) {
            if (isDateEmpty(source)) {
                return "";
            } else {
                return source.Year + "-" + source.Month.ToString().PadLeft(2, '0') + "-" + source.Day.ToString().PadLeft(2, '0');
            }
        }
        //
        //====================================================================================================
        public static string serializeObject(CPBaseClass cp, object dataObject) => (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(dataObject);//return Newtonsoft.Json.JsonConvert.SerializeObject(dataObject);
        // 
        // ====================================================================================================
        /// <summary>
        /// return the instanceId for a design block. It should be an document argument set when the addon is dropped on the page.
        /// If the addon is created with a json string it should be included as an argument
        /// If it is not included, the page id is used to make a string
        /// If no instanceId can be created a blank is returned and should NOT be used.
        /// If returnHtmlMessage is non-blank, add it to the html
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="designBlockName"></param>
        /// <param name="returnHtmlMessage"></param>
        /// <returns></returns>
        public static string getInstanceGuid(CPBaseClass cp, string designBlockName, ref string returnHtmlMessage) {
            // 
            // -- check arguments
            if ((string.IsNullOrWhiteSpace(designBlockName)))
                throw new ApplicationException("getInstanceId called without valid designBlockName.");
            string result = cp.Doc.GetText("instanceId");
            if ((!string.IsNullOrWhiteSpace(result)))
                return result;
            if ((cp.Doc.PageId > 0)) {
                // 
                // -- no instance Id, create a unquie string for this page, but display error is already used on this page
                result = "DesignBlockUsedWithoutInstanceId-[" + designBlockName + "]-PageId-" + cp.Doc.PageId.ToString();
                if ((!string.IsNullOrEmpty(cp.Doc.GetText(result)))) {
                    // 
                    // -- no instance Id, second occurance, display error
                    returnHtmlMessage += "<p>Error, this design block is used twice on this page. This is only allowed if it was added with the drag-drop tool, or includes a unique instance id.</p>";
                    cp.Site.ErrorReport("Design Block [" + designBlockName + "] on page [#" + cp.Doc.PageId + "," + cp.Doc.PageName + "] does not include an instanceId and was used on the page twice. This is not allowed. To use it twice, used the drag-drop design block tool or manually add the argument \"instanceid\" : \"{unique-guid}\".");
                    return string.Empty;
                }
                cp.Doc.SetProperty(result, "used");
                return result;
            }
            if ((cp.Request.PathPage == cp.Site.GetText("adminurl"))) {
                // 
                // -- addon run on admin site
                result = "DesignBlockUsedOnAdminSite-[" + designBlockName + "]";
                if ((!string.IsNullOrEmpty(cp.Doc.GetText(result)))) {
                    // 
                    // -- admin site, second occurance, display error
                    returnHtmlMessage += "<p>Error, this design block is used twice on the admin site. This is only allowed if it was added with the drag-drop tool, or includes a unique instance id.</p>";
                    cp.Site.ErrorReport("Design Block [" + designBlockName + "] on page [#" + cp.Doc.PageId + "," + cp.Doc.PageName + "] does not include an instanceId and was used on the page twice. This is not allowed. To use it twice, used the drag-drop design block tool or manually add the argument \"instanceid\" : \"{unique-guid}\".");
                    return string.Empty;
                }
                return result;
            }
            throw new ApplicationException("Design Block [" + designBlockName + "] called without instanceId must be on a page or the admin site.");
        }
        ////
        //public static string addEditWrapper(CPBaseClass cp, string innerHtml, int instanceId, string instanceName, string contentName, string designBlockCaption) {
        //    if ((!cp.User.IsEditingAnything)) { return innerHtml; }
        //    string editLink = getEditLink( cp, contentName, instanceId ) ;
        //    //string editLink = cp.Content.GetEditLink(contentName, instanceId.ToString(), false, instanceName, true);
        //    string editHeader = cp.Html.div(editLink + "&nbsp;" + designBlockCaption, "", "dbEditHeader");
        //    return cp.Html.div(editHeader + innerHtml, "", "ccEditWrapper");
        //}
        ////
        //public static string getEditLink( CPBaseClass cp, string contentName, int recordId) {
        //    int contentId = cp.Content.GetID(contentName);
        //    if ( contentId==0 ) { return string.Empty;  }
        //    return "<a href=\"/admin?af=4&aa=2&ad=1&cid=" + contentId + "&id=" + recordId + "\" class=\"ccRecordEditLink\"><span style=\"color:#0c0\"><i title=\"edit\" class=\"fas fa-cog\"></i></span></a>";
        //}
        // 
        public static string addEditWrapper(CPBaseClass cp, string innerHtml, int recordId, string contentName, string caption) {
            if ((!cp.User.IsEditingAnything)) { return innerHtml; }
            string header = cp.Content.GetEditLink(contentName, recordId.ToString(), false, caption, true);
            string content = cp.Html.div(innerHtml, "", "dbSettingWrapper");
            return cp.Html.div(header + content, "ccEditWrapper");
        }
        //// 
        //// 
        //// 
        //public static string getEditLink(CPBaseClass cp, string contentName, int recordId, string Caption) {
        //    int contentId = cp.Content.GetID(contentName);
        //    if (contentId == 0)
        //        return string.Empty;
        //    return "<a href=\"/admin?af=4&aa=2&ad=1&cid=" + contentId + "&id=" + recordId + "\" class=\"ccRecordEditLink\"><span style=\"color:#0c0\"><i title=\"edit\" class=\"fas fa-cog\"></i></span>&nbsp;" + Caption + "</a>";
        //}

        //Public Shared Function addEditWrapper(ByVal cp As CPBaseClass, ByVal innerHtml As String, ByVal recordId As Integer, ByVal instanceName As String, ByVal contentName As String, ByVal caption As String) As String
        //    If(Not cp.User.IsEditingAnything) Then Return innerHtml
        //   Dim header As String = cp.Content.GetEditLink(contentName, recordId.ToString(), False, caption, True)
        //    Dim content As String = cp.Html.div(innerHtml, "", "")
        //    Return cp.Html.div(header + content, "", "ccEditWrapper")
        //End Function
    }
}
