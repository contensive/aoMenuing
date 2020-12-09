
using System.Collections.Generic;

namespace Contensive.Addons.Menuing.Models.ViewModels {
    //
    // ==========================================================================================
    //
    public class TopListItemModel {
        /// <summary>
        /// The class in the top level LI
        /// </summary>
        public string classTopItem { get; set; } = "";
        /// <summary>
        /// If this top list item is active (currently on this page), this is the word "active"
        /// </summary>
        public string classTopItemActive { get; set; } = "";
        /// <summary>
        /// if this top list item has a drop-down list, this is the word "dropdown"
        /// </summary>
        public string classTopItemDropdown { get; set; } = "";
        /// <summary>
        /// The class in the top list items anchor, if the item has a child list, "dropdown-toggle", else blank
        /// </summary>
        public string classTopItemAnchor { get; set; } = "";
        /// <summary>
        /// the id of the page record for this list item. Used for the html id between the link and the flyout
        /// </summary>
        public int topItemPageId { get; set; } = 0;
        /// <summary>
        /// The name caption for this menu item
        /// </summary>
        public string topItemName { get; set; } = "";
        /// <summary>
        /// the link for this menu item
        /// </summary>
        public string topItemHref { get; set; } = "";
        /// <summary>
        /// if this item clicks to open a child list, populate this list
        /// </summary>
        public List<ChildListItemModel> childList = new List<ChildListItemModel>();
        /// <summary>
        /// true if childList.length>0
        /// </summary>
        public bool hasChildItems { get; set; } = false;
        /// <summary>
        /// Class added to items at all levels when edit is turned on to display draggable region
        /// </summary>
        public string classItemDraggable { get; set; } = "";
        /// <summary>
        /// the link for this menu item
        /// </summary>
        public string topItemHtmlId { get; set; } = "";
    }
}

