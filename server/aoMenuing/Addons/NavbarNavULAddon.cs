
using Contensive.Addons.Menuing.Controllers;
using Contensive.Addons.Menuing.Models.DbModels;
using Contensive.Addons.Menuing.Models.ViewModels;
using Contensive.BaseClasses;
using System;

namespace Contensive.Addons.Menuing.Views {
    /// <summary>
    /// menuing based on pages directly attached to the menu (menuPageRules)
    /// </summary>
    public class NavbarNavULAddon : AddonBaseClass {
        //
        // ====================================================================================================
        //
        public override object Execute(Contensive.BaseClasses.CPBaseClass cp) {
            try {
                // 
                // -- read instanceId, guid created uniquely for this instance of the addon on a page
                var result = string.Empty;
                string instanceGuid = genericController.getInstanceGuid(cp, "Navbar-UL", ref result);
                if (string.IsNullOrEmpty(instanceGuid)) { return result; }
                // 
                // -- locate or create a data record for this guid
                var settings = MenuModel.createOrAddDefault(cp, instanceGuid);
                if (settings == null) { throw new ApplicationException("Could not create design block data record."); }
                // 
                // -- translate the Db model to a view model and mustache it into the layout
                var viewModel = NavbarNavModel.create(cp, settings);
                if (viewModel == null) { throw new ApplicationException("Could not create design block view model."); }
                string layout = cp.Layout.GetLayout(settings.layoutId);
                if (string.IsNullOrEmpty(layout)) {
                    //
                    // -- if layout is not valid, create a copy of the default layout
                    layout = cp.Layout.GetLayout(Constants.guidNavbarNavULDefaultLayout, Constants.nameNavbarNavULDefaultLayout, Constants.pathFilenameNavbarNavULDefaultLayout);
                    var layoutrecord = Contensive.Models.Db.DbBaseModel.addDefault<Contensive.Models.Db.LayoutModel>(cp);
                    layoutrecord.name = "Instance " + settings.id + " of " + Constants.nameNavbarNavULDefaultLayout;
                    layoutrecord.layout.content = layout;
                    layoutrecord.save(cp);
                    settings.layoutId = layoutrecord.id;
                    settings.save(cp);
                }
                result = cp.Mustache.Render(layout, viewModel);
                // 
                // -- if editing enabled, add the link and wrapperwrapper
                return DesignBlockBase.Controllers.DesignBlockController.addDesignBlockEditWrapper(cp, result, settings, "Menus",viewModel);
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
    }
}
