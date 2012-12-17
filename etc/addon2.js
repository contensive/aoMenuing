
//
// list menu globals ---------------------
//
var lmLastItemHovered = null;
var lmItemIsHovered = false;           // 1=turn on, 0=turn off after delay
var lmHoverModeUpdated = 0;
var lmClickButton = 0;
var lmMenuClicks = 0;
//
// ----- Set it Open, or set a 1 second autoclose
//	 newMode=1 to open
//	 newMode=0 to close in 1 second
//
function lmHover(event,hoverClass) {
	var listItem;
	var parentItem;
	lmItemIsHovered = true;
	listItem = lmGetListItem(event);
	if (listItem == null) return false;
	if (lmLastItemHovered != null) {
		// unhover last item if not item parent
		parentItem = listItem;
		while (parentItem != null && parentItem != lmLastItemHovered) {
			parentItem = getContainer( parentItem.parentNode, "LI" );
		}
		if (parentItem==null) {
			lmClearItemAndChildList(lmLastItemHovered);
		}
	}

	if (listItem != lmLastItemHovered || lmHoverModeUpdated == 1) {
		// Update the listItem's style so it looks down
		if (lmClickButton != 2) {
			listItem.className += " "+hoverClass;
			listItem.hoverClass = hoverClass;
		}
	}
	lmHoverModeUpdated = 1;
	lmLastItemHovered = listItem;
}
//
//
//
function lmUnHover(event,hoverClass) {
	lmItemIsHovered = false;
	setTimeout(function(){lmClearLastItemHovered(hoverClass)}, 1000);
}
//
//
//
function lmClearLastItemHovered(hoverClass) {
	if (lmItemIsHovered == false) {
		lmClearItemAndChildList(lmLastItemHovered);
		lmHideSelect(0);
		lmHoverModeUpdated = 1;
	}
}
//
//
//
function lmStopEvent(event) {
	if (browser.isIE) {
		window.event.cancelBubble = true;
	} else {
		event.stopPropagation();
	}
}
//
// -----
//
function lmOpenChildList(event, childListId, position, StyleSheetPrefix, OpenOnHover, hoverClass) {
	var listItem, x, y, offY, width, height;
	//
	if ((!(lmClickButton == 2 && outOfContext)) && browser.isIE) {
		lmHideSelect(true);
	}
	listItem = lmGetListItem(event);
	if (listItem == null) return false;
	if (browser.isIE) {
		offY = event.offsetY;
	}
	if (childListId == '') return false;
	width=document.body.scrollWidth;
	height=document.body.scrollHeight;
	listItem.blur();
	if (listItem.childList == null) {
		listItem.childList = document.getElementById(childListId);
		if (listItem.childList.title == "")
			lmInit(listItem.childList);
	}
	if (lmLastItemHovered != null)
		lmClearItemAndChildList(lmLastItemHovered);
	if (listItem != lmLastItemHovered || lmHoverModeUpdated == 1) {
		lmHoverModeUpdated = 0;
		// Update the listItem's style so it looks down
		if (lmClickButton != 2) {
			listItem.className += " "+hoverClass;
			listItem.hoverClass = hoverClass;
		}
		if (position==3) {
			// Flyout to the right
			x = listItem.offsetWidth;
			y = 0;
			//x = jQuery(listItem).position().left + listItem.offsetWidth;
			//y = jQuery(listItem).position().top;
		} else if (position==1) {
			// Flyout up
			x = 0;
			y = -listItem.childList.offsetHeight;
			//x = jQuery(listItem).position().left;
			//y = jQuery(listItem).position().top - listItem.childList.offsetHeight;
		} else if (position==4) {
			// Flyout to the left
			x = -listItem.childList.offsetWidth;
			y = 0;
			//x = jQuery(listItem).position().left - listItem.childList.offsetWidth;
			//y = jQuery(listItem).position().top;
		} else {
			// Flyout down
			x = 0;
			y = listItem.offsetHeight;
			//x = jQuery(listItem).position().left;
			//y = jQuery(listItem).position().top + listItem.offsetHeight;
		}

		if (lmClickButton == 2) {
			x = event.clientX;
			y = event.clientY;
			if (document.body) {
				x += document.body.scrollLeft;
				y += document.body.scrollTop;
			}
		}
		if ((x + listItem.childList.scrollWidth) > width) {
			x = width - listItem.childList.scrollWidth;
		}
		listItem.childList.style.left = x + "px";
		listItem.childList.style.top  = y + "px";
		// turn it on
		listItem.childList.style.visibility = "visible";
		listItem.childList.style.display = "block";
		lmLastItemHovered = listItem;
	}
	else {
		if (!OpenOnHover) {
			lmLastItemHovered = null;
			}
	}
	lmClickButton=1;
	return false;
}
//
// -----
//
function lmButtonHover(event,childListId,position,hoverClass) {
	var listItem;
	if (browser.isIE) {
		listItem = window.event.srcElement;
	} else {
		listItem = event.currentTarget;
	}
	if (lmLastItemHovered != null && lmLastItemHovered != listItem) {
		lmOpenChildList(event, childListId, position,'',false,hoverClass );
	}
}
//
// -----
//
function lmClearItemAndChildList(listItem) {
	if (listItem) {
		removeClassNameAfter(listItem, listItem.hoverClass);
		if (listItem.childList != null) {
			lmCloseChildList(listItem.childList);
			listItem.childList.style.visibility = "hidden";
		}
	}
}
//
// -----
//
function lmListHover(event,StyleSheetPrefix,hoverClass) {
	var parentList;
	//
	if (browser.isIE) {
		parentList = getContainer(window.event.srcElement, "UL");
	} else {
		parentList = event.currentTarget;
	}
	if (parentList.title != "") {
		if (parentList.activeItem == null) {
			// do nothing
		} else {
			lmCloseChildList(parentList);
		}
	}
}
//
// -----
//
function lmListItemHover(event, childListId, StyleSheetPrefix, hoverClass, tierItemClass, tierListClass) {
	var listItem, parentList, x, y, maxX, maxY;
	//
	if (browser.isIE) {
		listItem = getContainerWith(window.event.srcElement, "LI", tierItemClass);
	} else {
		listItem = event.currentTarget;
	}
	parentList = getContainerWith(listItem, "UL", tierListClass);
	if (parentList) {
		//
		// LI-parentList
		//
		if (parentList.activeItem == null) {
			// nothing
		} else {
			lmCloseChildList(parentList);
		}
		parentList.activeItem = listItem;
		listItem.className += " "+hoverClass;
		listItem.hoverClass = hoverClass;
		if (listItem.childList == null) {
			listItem.childList = document.getElementById(childListId);
			if (listItem.childList.title == "")
			//if (listItem.childList.isInitialized == null)
				lmInit(listItem.childList);
		}
		//alert('listItem.childList.id='+listItem.childList.id)
		x = listItem.offsetWidth;
		y = listItem.offsetTop;
		if (browser.isNS) {
			maxX = window.scrollX + window.innerWidth;
			maxY = window.scrollY + window.innerHeight;
		//alert('110 maxX='+maxX+', maxY='+maxY);
		}
		if (browser.isIE) {
			maxX = (document.documentElement.scrollLeft != 0 ? document.documentElement.scrollLeft   : document.body.scrollLeft)
				+ (document.documentElement.clientWidth  != 0 ? document.documentElement.clientWidth  : document.body.clientWidth);
			maxY = (document.documentElement.scrollTop    != 0 ? document.documentElement.scrollTop    : document.body.scrollTop)
				+ (document.documentElement.clientHeight != 0 ? document.documentElement.clientHeight : document.body.clientHeight);
		//alert('200 maxX='+maxX+', maxY='+maxY);
		}
		maxX -= listItem.childList.offsetWidth;
		maxY -= listItem.childList.offsetHeight;
		//alert('300 maxX='+maxX+', maxY='+maxY);
		if (x > maxX) {
			x = Math.max(0, x - listItem.offsetWidth - listItem.childList.offsetWidth + (parentList.offsetWidth - listItem.offsetWidth));
		}
		//alert('400 maxX='+maxX+', maxY='+maxY);
		y = Math.max(0, Math.min(y, maxY));
		listItem.childList.style.left = x + "px";
		if (browser.isMac) {
			if (browser.version>5.1) {
				listItem.childList.style.top  = y + "px";
			}
		} else {
			listItem.childList.style.top  = y + "px";
		}
		//alert('500 x='+x+', y='+y);
		listItem.childList.style.visibility = "visible";
		listItem.childList.style.display = "block";
		lmStopEvent(event);
		//if (browser.isIE) {
		//	window.event.cancelBubble = true;
		//} else {
		//	event.stopPropagation();
		//}
	}
}
//
// -----
//
function lmCloseChildList(list) {
	if (!list)
		return;
	if (list.activeItem == null)
		return;
	if (list.activeItem.childList) {
		lmCloseChildList(list.activeItem.childList);
		list.activeItem.childList.style.visibility = "hidden";
		//list.activeItem.childList.style.display = "none";
		list.activeItem.childList = null;
		}
	removeClassNameAfter(list.activeItem, list.activeItem.hoverClass);
	list.activeItem = null;
	}
//
// -----
//
function lmInit(list) {
	var itemList, spanList, textEl, arrowEl, itemWidth, w, dw, i, j;
	//
	itemList = list.getElementsByTagName("LI");
	if (itemList.length == 0) {
		list.title = 'Select One';
		list.activeItem = null
		return false;
	}
	if (browser.isIE) {
		w = itemList[0].offsetWidth;
		itemList[0].style.width = w + "px";
		dw = itemList[0].offsetWidth - w;
		w -= dw;
		itemList[0].style.width = w + "px";
	}
	list.title = 'Select One';
	list.activeItem = null
	}
function lmHideSelect(hiddenIn) {						
	var i, j;
	var index;
	var selects;
	//
	if (hiddenIn) {
		lmMenuClicks++;
	}
	//
	if (!hiddenIn && lmMenuClicks == 0 && !selectsHidden) {
		return;
	}
	// Hide MS Selects
	selects = document.getElementsByTagName("SELECT");
	if (selectsHidden && lmMenuClicks == 0) {
		for (i = 0; i < selects.length; i++) {
			selects[i].style.visibility = "";
		}
	}
	else {
		for (i = 0; i < selects.length; i++) {
			selects[i].style.visibility = "hidden";
		}
	}
	// Hide MS Embeds
	selects = document.getElementsByTagName("EMBED");
	if (selectsHidden && lmMenuClicks == 0) {
		for (i = 0; i < selects.length; i++) {
			selects[i].style.visibility = "";
		}
	}
	else {
		for (i = 0; i < selects.length; i++) {
			selects[i].style.visibility = "hidden";
		}
	}
	//
	if (selectsHidden && lmMenuClicks == 0) {
		selectsHidden = false;
	}
	//
	else if (selectsHidden && lmMenuClicks != 0 && lmClickButton != 2) {
		lmMenuClicks--;
	}
	//
	else {
		selectsHidden = true;
	}
}
//
// ----- Code for handling the menu bar and active listItem.
//
function lmPageClick(event) {
	//alert("lmPageClick");
	var el;
	var listItem;

	if (lmLastItemHovered == null) return;
	if (browser.isIE)
		el = window.event.srcElement;
	else
		el = (event.target.tagName ? event.target : event.target.parentNode);

	if (el == lmLastItemHovered)
		return true;
	listItem = getContainer(el,"LI");
	if (listItem) {
		if (listItem.hoverClass==null) {
			lmClearItemAndChildList(lmLastItemHovered);
			lmLastItemHovered = null;
		}
	}
}
//
//
//
function lmGetListItem(event) {
	var listItem;
	if (browser.isIE) {
		listItem = event.srcElement;
	} else {
		listItem = event.currentTarget;
	}
	if(listItem.tagName!='LI') {
		listItem = getContainer( listItem, 'LI' )
	}
	return listItem;
}
