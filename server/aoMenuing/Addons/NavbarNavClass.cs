
using Contensive.Addons.Menuing.Controllers;
using Contensive.Addons.Menuing.Models.DbModels;
using Contensive.Addons.Menuing.Models.ViewModels;
using Contensive.BaseClasses;
using System;

namespace Contensive.Addons.Menuing.Views {
    /// <summary>
    /// menuing based on pages directly attached to the menu (menuPageRules)
    /// </summary>
    public class NavbarNavClass : AddonBaseClass {
        //
        // ====================================================================================================
        //
        public override object Execute(CPBaseClass cp) {
            try {
                // 
                // -- read instanceId, guid created uniquely for this instance of the addon on a page
                var result = string.Empty;
                string instanceGuid = genericController.getInstanceGuid(cp, "NavbarNav", ref result);
                if (string.IsNullOrEmpty(instanceGuid)) { return result; }
                // 
                // -- locate or create a data record for this guid
                var instance = MenuModel.createOrAddDefault(cp, instanceGuid);
                if (instance == null) { throw new ApplicationException("Could not create design block data record."); }
                // 
                // -- translate the Db model to a view model and mustache it into the layout
                var viewModel = NavbarNavViewModel.create(cp, instance);
                if (viewModel == null) { throw new ApplicationException("Could not create design block view model."); }
                result = Nustache.Core.Render.StringToString(Properties.Resources.BootstrapNav40Layout, viewModel);
                // 
                // -- if editing enabled, add the link and wrapperwrapper
                return genericController.addEditWrapper(cp, result, instance.id, MenuModel.contentName, instance.name);
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
                throw;
            }
        }
    }
}
