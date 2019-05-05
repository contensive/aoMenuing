
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Contensive.BaseClasses;
using Contensive.Addons.MenuPages.Controllers;
using Contensive.Addons.MenuPages.Models.DbModels;
using Contensive.Addons.MenuPages.Models.ViewModels;

namespace Contensive.Addons.MenuPages.Views {
    /// <summary>
    /// menuing based on pages directly attached to the menu (menuPageRules)
    /// </summary>
    public class BootstrapNav40Class : Contensive.BaseClasses.AddonBaseClass {
        //
        // -- instance properties
        CPBaseClass cp;
        //
        // ====================================================================================================
        //
        public override object Execute(Contensive.BaseClasses.CPBaseClass cp) {
            const string designBlockName = "Menu";
            try {
                // 
                // -- read instanceId, guid created uniquely for this instance of the addon on a page
                var result = string.Empty;
                string instanceGuid = genericController.getInstanceGuid(cp, designBlockName, ref result);
                if ((string.IsNullOrEmpty(instanceGuid)))
                    return result;
                // 
                // -- locate or create a data record for this guid
                var instance = MenuModel.createOrAddDefault(cp, instanceGuid);
                if ((instance == null))
                    throw new ApplicationException("Could not create design block data record.");
                // 
                // -- translate the Db model to a view model and mustache it into the layout
                var viewModel = BootstrapNav40ViewModel.create(cp, instance);
                if ((viewModel == null)) { throw new ApplicationException("Could not create design block view model."); }
                result = Nustache.Core.Render.StringToString(Properties.Resources.BootstrapNav40Layout, viewModel);
                // 
                // -- if editing enabled, add the link and wrapperwrapper
                result = genericController.addEditWrapper(cp, result, instance.id, MenuModel.contentName, instance.name);
                //
                return cp.Html.div(result, "", "bootstrapNavCon");
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                return "<!-- " + designBlockName + ", Unexpected Exception -->";
            }
        }
        //
        // -- create a listItem from a page
        private string getAnchor(CPBaseClass cp, Models.DbModels.PageContentModel page, string htmlClass, string dataToggleValue) {
            try {
                string topItemCaption = page.MenuHeadline;
                if (string.IsNullOrEmpty(topItemCaption)) topItemCaption = page.name;
                string pageLink = cp.Content.GetPageLink(page.id);
                string dataToggleAttr;
                if (string.IsNullOrEmpty(dataToggleValue)) { dataToggleAttr = string.Empty; } else { dataToggleAttr = " data-toggle=\"" + dataToggleValue + "\""; }
                //string pageList = cp.Content.GetLinkAliasByPageID(page.id, "", "");
                return string.Format("<a class=\"{2}\" title=\"{1}\" href=\"{0}\"{3}>{1}</a>", pageLink, topItemCaption, htmlClass, dataToggleAttr);
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
                    _allowedPageIdList = Models.DbModels.PageContentModel.getAllowedPageIdList(cp);
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
                    _allowedSectionIdList = Models.DbModels.SiteSectionsModel.getAllowedSectionIdList(cp);
                }
                return _allowedSectionIdList;
            }
        }
        private List<int> _allowedSectionIdList = null;
    }
}
