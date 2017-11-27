
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using Contensive.BaseClasses;

namespace Contensive.Addons.BootstrapNav.Models.Entity
{
    public class pageContentModel : baseModel
    {
        //
        //====================================================================================================
        //-- const
        public const string contentName = "page content";
        public const string contentTableName = "ccpagecontent";
        //
        //====================================================================================================
        // -- instance properties
        //public bool AllowBrief { get; set; }
        //public bool AllowChildListDisplay { get; set; }
        //public bool AllowEmailPage { get; set; }
        //public bool AllowFeedback { get; set; }
        //public bool AllowHitNotification { get; set; }
        //public bool AllowInChildLists { get; set; }
        //public bool AllowInMenus { get; set; }
        //public bool AllowLastModifiedFooter { get; set; }
        //public bool AllowMessageFooter { get; set; }
        //public bool AllowMetaContentNoFollow { get; set; }
        //public bool AllowMoreInfo { get; set; }
        //public bool AllowPrinterVersion { get; set; }
        //public bool AllowReturnLinkDisplay { get; set; }
        //public bool AllowReviewedFooter { get; set; }
        //public bool AllowSeeAlso { get; set; }
        //public int ArchiveParentID { get; set; }
        public bool BlockContent { get; set; }
        //public bool BlockPage { get; set; }
        //public int BlockSourceID { get; set; }
        //public string BriefFilename { get; set; }
        //public string ChildListInstanceOptions { get; set; }
        //public int ChildListSortMethodID { get; set; }
        //public bool ChildPagesFound { get; set; }
        //public int Clicks { get; set; }
        //public int ContactMemberID { get; set; }
        //public int ContentCategoryID { get; set; }
        //public int ContentPadding { get; set; }
        //public string Copyfilename { get; set; }
        //public string CustomBlockMessage { get; set; }
        //public DateTime DateArchive { get; set; }
        //public DateTime DateExpires { get; set; }
        //public DateTime DateReviewed { get; set; }
       
        //public string DocLabel { get; set; }
        //public bool EditArchive { get; set; }
        //public bool EditBlank { get; set; }
        //public int EditSourceID { get; set; }
        //public string Headline { get; set; }
        //public string ImageFilename { get; set; }
        //public bool IsSecure { get; set; }
        //public string JSEndBody { get; set; }
        //public string JSFilename { get; set; }
        //public string JSHead { get; set; }
        //public string JSOnLoad { get; set; }
        //public string Link { get; set; }
        //public string LinkAlias { get; set; }
        //public string LinkLabel { get; set; }
        //public string Marquee { get; set; }
        public string MenuHeadline { get; set; }
        //public string MenuImageFileName { get; set; }
        //public int OrganizationID { get; set; }
        //public string PageLink { get; set; }
        //public int ParentID { get; set; }
        //public string ParentListName { get; set; }
        //public string PodcastMediaLink { get; set; }
        //public int PodcastSize { get; set; }
        //public DateTime PubDate { get; set; }
        //public int RegistrationGroupID { get; set; }
        //public bool RegistrationRequired { get; set; }
        //public int ReviewedBy { get; set; }
        //public DateTime RSSDateExpire { get; set; }
        //public DateTime RSSDatePublish { get; set; }
        //public string RSSDescription { get; set; }
        //public string RSSLink { get; set; }
        //public string RSSTitle { get; set; }
        //public int TemplateID { get; set; }
        //public int TriggerAddGroupID { get; set; }
        //public int TriggerConditionGroupID { get; set; }
        //public int TriggerConditionID { get; set; }
        //public int TriggerRemoveGroupID { get; set; }
        //public int TriggerSendSystemEmailID { get; set; }
        //public int Viewings { get; set; }
        public string menuClass { get; set; }
        //
        //====================================================================================================
        public static pageContentModel @add(CPBaseClass cp)
        {
            return @add<pageContentModel>(cp);
        }
        //
        //====================================================================================================
        public static pageContentModel create(CPBaseClass cp, int recordId)
        {
            return create<pageContentModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static pageContentModel create(CPBaseClass cp, string recordGuid)
        {
            return create<pageContentModel>(cp, recordGuid);
        }
        //
        //====================================================================================================
        public static pageContentModel createByName(CPBaseClass cp, string recordName)
        {
            return createByName<pageContentModel>(cp, recordName);
        }
        //
        //====================================================================================================
        public new void save(CPBaseClass cp)
        {
            base.save(cp);
        }
        //
        //====================================================================================================
        public static void delete(CPBaseClass cp, int recordId)
        {
            delete<pageContentModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static void delete(CPBaseClass cp, string ccGuid)
        {
            delete<pageContentModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static List<pageContentModel> createList(CPBaseClass cp, string sqlCriteria, string sqlOrderBy = "id")
        {
            return createList<pageContentModel>(cp, sqlCriteria, sqlOrderBy);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, int recordId)
        {
            return baseModel.getRecordName<pageContentModel>(cp, recordId);
        }
        //
        //====================================================================================================
        public static string getRecordName(CPBaseClass cp, string ccGuid)
        {
            return baseModel.getRecordName<pageContentModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        public static int getRecordId(CPBaseClass cp, string ccGuid)
        {
            return baseModel.getRecordId<pageContentModel>(cp, ccGuid);
        }
        //
        //====================================================================================================
        // -- a list of sections you have access to
        // -- all sections without blocking, plus section-groups that you are in the group
        public static List<int> getAllowedPageIdList(CPBaseClass cp)
        {
            List<int> result = new List<int>();
            try
            {
                string sql = "select pr.recordId as id"
                    + " from ccPageContentBlockRules pr"
                    + " left join ccMemberRules mr on mr.groupId=pr.groupid"
                    + " where mr.memberId=" + cp.User.Id.ToString();
                CPCSBaseClass cs = cp.CSNew();
                if (cs.OpenSQL(sql))
                {
                    do
                    {
                        result.Add(cs.GetInteger("id"));
                        cs.GoNext();
                    } while (cs.OK());
                }
                cs.Close();
            }
            catch (Exception ex)
            {
                cp.Site.ErrorReport(ex);
            }
            return result;
        }
    }
}
