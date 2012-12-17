

//
// FlyoutMenu Globals ---------------------
//
var lmActiveButton = null;
var lmHoverMode = 0;
var lmHoverModeUpdated = 0;
var lmClickButton;
//
//var selectsHidden = false;
//var menuClicks = 0;
//var lastClipCheck = false;
//var lastSelCheck = false;
//var currentPanel;
//var ButtonObjectCount = 0;
//var MenuObjectCount = 0;
//var MenuHideBlock = false;
//
// ----- Set it Open, or set a 1 second autoclose
//	 val=1 to open
//	 val=0 to close in 1 second
//
function lmSetHoverMode(newMode) {
	lmHoverMode = newMode;
	if (newMode==0) {
		setTimeout(lmHoveModeClear, 1000);
	}
	if (newMode==1) {
		lmHoverModeUpdated = 1;
	}
	return;
}
//
// -----
//
function lmHoveModeClear() {
	if (lmHoverMode == 0) {
		//resetFlag = 1;
		lmResetButton(lmActiveButton);
		//menuClicks = 0;
		lmHideSelect(0);
		lmHoverModeUpdated = 1;
	}

}//
// -----
//
function lmOpenPanel(event, menuId, position, StyleSheetPrefix, OpenOnHover) {
	//alert('lmOpenPanel(event,'+menuId+','+position+','+StyleSheetPrefix+','+OpenOnHover+')')
	var button, x, y, offY, width, height;
	//
	if ((!(lmClickButton == 2 && outOfContext)) && browser.isIE) {
		lmHideSelect(true);
	}
	if (browser.isIE) {
		offY = event.offsetY;
		button = event.srcElement;
	} else {
		button = event.currentTarget;
	}
	if(button.tagName!='A') {
		button = getContainer( button, 'A' )
		if (button == null) return false;
	}
	if (menuId == '') return false;
	width=document.body.scrollWidth;
	height=document.body.scrollHeight;
	button.blur();
	if (button.menu == null) {
		button.menu = document.getElementById(menuId);
		if (button.menu.title == "")
			lmInit(button.menu);
	}
	if (lmActiveButton != null)
		lmResetButton(lmActiveButton);
	if (button != lmActiveButton || lmHoverModeUpdated == 1) {
		lmHoverModeUpdated = 0;
		// Update the button's style so it looks down
		if (lmClickButton != 2) {
			button.className += " down";
		}
		if (position==1) {
			// Flyout to the right
			x = getPageOffsetLeft(button) + button.offsetWidth;
			y = getPageOffsetTop(button);
		} else if (position==2) {
			// Flyout up
			x = getPageOffsetLeft(button);
			y = getPageOffsetTop(button) - button.menu.offsetHeight;
		} else if (position==3) {
			// Flyout to the left
			x = getPageOffsetLeft(button) - button.menu.offsetWidth;
			y = getPageOffsetTop(button);
		} else {
			// Flyout down
			x = getPageOffsetLeft(button);
			y = getPageOffsetTop(button) + button.offsetHeight;

		}

		if (lmClickButton == 2) {
			x = event.clientX;
			y = event.clientY;
			if (document.body) {
				x += document.body.scrollLeft;
				y += document.body.scrollTop;
			}
		}

		if ((x + button.menu.scrollWidth) > width) {
			x = width - button.menu.scrollWidth;
		}

		button.menu.style.left = x + "px";
		button.menu.style.top  = y + "px";
		// turn it on
		button.menu.style.visibility = "visible";
		button.menu.style.display = "block";
		lmActiveButton = button;
	}
	else {
		if (!OpenOnHover) {
			lmActiveButton = null;
			}
	}
	lmClickButton=1;
	return false;
}
//
// -----
//
function lmButtonHover(event, menuId, position) {
	//alert('lmButtonHover(event,'+menuId+','+position+')');
	var button;
	if (browser.isIE) {
		button = window.event.srcElement;
	} else {
		button = event.currentTarget;
	}
	if (lmActiveButton != null && lmActiveButton != button) {
		lmOpenPanel(event, menuId, position);
	}
}
//
// -----
//
function lmResetButton(button) {
	if (button) {
		removeClassNameAfter(button, "down");
		if (button.menu != null) {
			lmCloseSubMenu(button.menu);
			button.menu.style.visibility = "hidden";
			//test-added for limenu ------------------------------------------
			button.menu.style.display = "none";
		}
	}
}
//
// -----
//
function lmPanelHover(event, StyleSheetPrefix) {
	//alert('lmPanelHover(event,'+StyleSheetPrefix+')');
	var menu;
	//
	if (browser.isIE) {
		menu = getContainerWith(window.event.srcElement, "UL", "panel");
		if (!menu) {
			menu = getContainerWith(window.event.srcElement, "DIV", StyleSheetPrefix+"Panel");
		}
	} else {
		menu = event.currentTarget;
	}
	if (menu.title != "") {
		if (menu.activeItem == null) {
			// do nothing
		} else {
			lmCloseSubMenu(menu);
		}
	}
}
//
// -----
//
function lmPanelButtonHover(event, menuId, StyleSheetPrefix) {
	//alert('lmPanelButtonHover(event,'+menuId+','+StyleSheetPrefix+')');
	var item, menu, x, y, maxX, maxY;
	//
	if (browser.isIE) {
		item = getContainerWith(window.event.srcElement, "LI", "panelButton");
		if (!item ) {
			item = getContainerWith(window.event.srcElement, "A", StyleSheetPrefix+"PanelButton");
		}
	} else {
		item = event.currentTarget;
	}

menu = getContainerWith(item, "UL", "panel");
if (menu) {
	//
	// LI-Menu
	//
	//alert('LI-Menu')
	if (menu.activeItem == null) {
		// nothing
	} else {
		lmCloseSubMenu(menu);
	}
	menu.activeItem = item;
	item.className += " down";
	if (item.subMenu == null) {
		item.subMenu = document.getElementById(menuId);
		if (item.subMenu.title == "")
		//if (item.subMenu.isInitialized == null)
			lmInit(item.subMenu);
	}
	//alert('item.subMenu.id='+item.subMenu.id)
	x = item.offsetWidth;
	y = item.offsetTop;
	//x = getPageOffsetLeft(item) + item.offsetWidth;
	//y = getPageOffsetTop(item);
	//alert('100 x='+x+', y='+y);
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
	maxX -= item.subMenu.offsetWidth;
	maxY -= item.subMenu.offsetHeight;
	//alert('300 maxX='+maxX+', maxY='+maxY);
	if (x > maxX) {
		x = Math.max(0, x - item.offsetWidth - item.subMenu.offsetWidth + (menu.offsetWidth - item.offsetWidth));
	}
	//alert('400 maxX='+maxX+', maxY='+maxY);
	y = Math.max(0, Math.min(y, maxY));
	item.subMenu.style.left = x + "px";
	if (browser.isMac) {
		if (browser.version>5.1) {
			item.subMenu.style.top  = y + "px";
		}
	} else {
		item.subMenu.style.top  = y + "px";
	}
	//alert('500 x='+x+', y='+y);
	item.subMenu.style.visibility = "visible";
	item.subMenu.style.display = "block";
	if (browser.isIE) {
		window.event.cancelBubble = true;
	} else {
		event.stopPropagation();
	}
} else {
	//
	// A-Menu
	//
	// alert('A-Menu')
	menu = getContainerWith(item, "DIV", StyleSheetPrefix+"Panel");
	if (menu.activeItem == null) {
		// nothing
	} else {
		lmCloseSubMenu(menu);
	}
	menu.activeItem = item;
	item.className += " down";
	if (item.subMenu == null) {
		item.subMenu = document.getElementById(menuId);
		if (item.subMenu.title == "")
		//if (item.subMenu.isInitialized == null)
			lmInit(item.subMenu);
	}
	x = getPageOffsetLeft(item) + item.offsetWidth;
	y = getPageOffsetTop(item);
	if (browser.isNS) {
		maxX = window.scrollX + window.innerWidth;
		maxY = window.scrollY + window.innerHeight;
	}
	if (browser.isIE) {
		maxX = (document.documentElement.scrollLeft != 0 ? document.documentElement.scrollLeft   : document.body.scrollLeft)
			+ (document.documentElement.clientWidth  != 0 ? document.documentElement.clientWidth  : document.body.clientWidth);
		maxY = (document.documentElement.scrollTop    != 0 ? document.documentElement.scrollTop    : document.body.scrollTop)
			+ (document.documentElement.clientHeight != 0 ? document.documentElement.clientHeight : document.body.clientHeight);
	}
	maxX -= item.subMenu.offsetWidth;
	maxY -= item.subMenu.offsetHeight;
	if (x > maxX) {
		x = Math.max(0, x - item.offsetWidth - item.subMenu.offsetWidth + (menu.offsetWidth - item.offsetWidth));
	}
	y = Math.max(0, Math.min(y, maxY));
	item.subMenu.style.left = x + "px";
	if (browser.isMac) {
		if (browser.version>5.1) {
			item.subMenu.style.top  = y + "px";
		}
	} else {
		item.subMenu.style.top  = y + "px";
	}
	item.subMenu.style.visibility = "visible";
	if (browser.isIE) {
		window.event.cancelBubble = true;
	} else {
		event.stopPropagation();
	}
}
}
//
// -----
//
function lmCloseSubMenu(menu) {
	if (!menu)
		return;
	if (menu.activeItem == null)
		return;
	if (menu.activeItem.subMenu) {
		lmCloseSubMenu(menu.activeItem.subMenu);
		menu.activeItem.subMenu.style.visibility = "hidden";
		menu.activeItem.subMenu.style.display = "none";
		menu.activeItem.subMenu = null;
		}
	removeClassNameAfter(menu.activeItem, "down");
	menu.activeItem = null;
	}
//
// -----
//
function lmInit(menu) {
	//alert('lmInit()');
	var itemList, spanList, textEl, arrowEl, itemWidth, w, dw, i, j;
	//
	itemList = menu.getElementsByTagName("LI");
	if (itemList.length == 0) {
//alert('lmInit, no LI found');
		itemList = menu.getElementsByTagName("A");
	}
	if (itemList.length == 0) {
//alert('lmInit, no A found');
		menu.title = 'Select One';
		menu.activeItem = null
		return false;
	}
	if (browser.isIE) {
		w = itemList[0].offsetWidth;
		itemList[0].style.width = w + "px";
		dw = itemList[0].offsetWidth - w;
		w -= dw;
		itemList[0].style.width = w + "px";
	}
	menu.title = 'Select One';
	menu.activeItem = null
	}
function lmHideSelect(hiddenIn) {						
	var i, j;
	var index;
	var selects;
	//
	if (hiddenIn) {
		menuClicks++;
	}
	//
	if (!hiddenIn && menuClicks == 0 && !selectsHidden) {
		return;
	}
	// Hide MS Selects
	selects = document.getElementsByTagName("SELECT");
	if (selectsHidden && menuClicks == 0) {
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
	if (selectsHidden && menuClicks == 0) {
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
	if (selectsHidden && menuClicks == 0) {
		selectsHidden = false;
	}
	//
	else if (selectsHidden && menuClicks != 0 && lmClickButton != 2) {
		menuClicks--;
	}
	//
	else {
		selectsHidden = true;
	}
}