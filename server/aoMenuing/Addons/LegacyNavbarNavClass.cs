
using Contensive.Addons.Menuing.Controllers;
using Contensive.Addons.Menuing.Models.DbModels;
using Contensive.Addons.Menuing.Models.ViewModels;
using Contensive.BaseClasses;
using System;

namespace Contensive.Addons.Menuing.Views {
    /// <summary>
    /// menuing based on pages directly attached to the menu (menuPageRules)
    /// </summary>
    public class LegacyNavbarNavClass : AddonBaseClass {
        //
        // ====================================================================================================
        //
        public override object Execute(CPBaseClass cp) {
            try {
                //
                // cp.Site.SetSiteWarning($"Legacy menu add-on", $"Legacy menu LegacyNavbarNavClass in use on page {cp.Request.PathPage}");
                //
                // -- read instanceId, guid created uniquely for this instance of the addon on a page
                var result = string.Empty;
                string instanceGuid = GenericController.getInstanceGuid(cp, "NavbarNav", ref result);
                if (string.IsNullOrEmpty(instanceGuid)) { return result; }
                // 
                // -- locate or create a data record for this guid
                var settings = MenuModel.createOrAddDefault(cp, instanceGuid) ?? throw new ApplicationException("Could not create design block data record.");
                // 
                // -- translate the Db model to a view model and mustache it into the layout
                var viewModel = NavbarNavModel.create(cp, settings) ?? throw new ApplicationException("Could not create design block view model.");
                result = cp.Mustache.Render(Properties.Resources.BootstrapNav40Layout, viewModel);
                // 
                // -- if editing enabled, add the link and wrapperwrapper
                return DesignBlockBase.Controllers.DesignBlockController.addDesignBlockEditWrapper(cp, result, settings, "Menus", viewModel, "", false);
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
    }
}
