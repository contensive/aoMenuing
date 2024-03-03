
using Contensive.Addons.Menuing.Controllers;
using Contensive.Addons.Menuing.Models.DbModels;
using Contensive.Addons.Menuing.Models.ViewModels;
using Contensive.BaseClasses;
using System;

namespace Contensive.Addons.Menuing.Views {
    /// <summary>
    /// menuing based on pages directly attached to the menu (menuPageRules)
    /// </summary>
    public class LegacyNavbarLiListClass : AddonBaseClass {
        //
        // ====================================================================================================
        //
        public override object Execute(Contensive.BaseClasses.CPBaseClass cp) {
            try {
                // 
                // -- read instanceId, guid created uniquely for this instance of the addon on a page
                var result = string.Empty;
                string instanceGuid = genericController.getInstanceGuid(cp, "Navbar-Li-List", ref result);
                if (string.IsNullOrEmpty(instanceGuid)) { return result; }
                // 
                // -- locate or create a data record for this guid
                var settings = MenuModel.createOrAddDefault(cp, instanceGuid);
                if (settings == null) { throw new ApplicationException("Could not create design block data record."); }
                // 
                // -- translate the Db model to a view model and mustache it into the layout
                var viewModel = NavbarNavModel.create(cp, settings);
                if (viewModel == null) { throw new ApplicationException("Could not create design block view model."); }
                result = cp.Mustache.Render(Properties.Resources.NavbarLiListLayout, viewModel);
                //
                // -- if editing enabled, add the link and wrapperwrapper
                return DesignBlockBase.Controllers.DesignBlockController.addDesignBlockEditWrapper(cp, result, settings, "Menus", viewModel, "Edit Menu Block", false);
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
    }
}
