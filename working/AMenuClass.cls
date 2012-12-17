VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "aMenuClass"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True

Option Explicit
'
'========================================================================
'
'========================================================================
'
Private Main As Object
Private Csv As Object
'
'=================================================================================
'   Execute Method
'=================================================================================
'
Public Function Execute(CsvObject As Object, MainObject As Object, OptionString As String, FilterInput As String) As String
    On Error GoTo ErrorTrap
    '
    Set Main = MainObject
    Set Csv = CsvObject
    '
    Dim classAttr As String
    Dim Ptr As Long
    Dim nodeCnt As Long
    Dim cssLegacyFile As String
    Dim jsFile As String
    Dim jsOnLoad As String
    Dim allowEdit As Boolean
    Dim delimiter As String
    Dim nextButton As String
    Dim ClickIndicator As String
    Dim FlyoutOnHover As Boolean
    Dim JSOnOut As String
    Dim JSOnHover As String
    Dim JSOnClick As String
    Dim SubMenu As String
    Dim HotSpot As String
    Dim MenuStyle As String
    Dim HorizontalLayout As Boolean
    Dim ATag As String
    Dim MenuNew As String
    Dim Menu As String
    Dim SiteTree As SiteTreeClass
    Dim SiteTreeData As String
    Dim doc As DOMDocument30
    Dim LoopPtr As Long
    Dim Content As fastStringClass
    Dim TopNode As IXMLDOMNode
    Dim TabNode As IXMLDOMNode
    Dim node As IXMLDOMNode
    Dim IsFound As Boolean
    Dim Caption As String
    Dim ImageSrc As String
    Dim ImageOverSrc As String
    Dim ImageDownScr As String
    Dim ImageDownOverSrc As String
    Dim MenuSectionID As Long
    Dim menuPageId As Long
    Dim renderedSectionId As Long
    Dim renderedPageId As Long
    Dim StylePrefix As String
    Dim FlyoutDirection As Long
    Dim menuId As Long
    '
    Randomize
    '
    allowEdit = Main.isEditing("site sections")
    Set SiteTree = New SiteTreeClass
    SiteTreeData = ""
    SiteTreeData = SiteTree.Execute(CsvObject, MainObject, OptionString, FilterInput)
    If SiteTreeData <> "" Then
        Set doc = New DOMDocument30
        doc.loadXML (SiteTreeData)
        Do While doc.readyState <> 4 And LoopPtr < 50
            Sleep (100)
            DoEvents
            LoopPtr = LoopPtr + 1
        Loop
        If doc.readyState <> 4 Then
            '
            ' error
            '
            Call Main.ReportError("There was a problem with the Dynamic Menu SiteTree. The SiteTree object timed out.")
        ElseIf doc.parseError.errorCode <> 0 Then
            '
            ' error
            '
            Call Main.ReportError("There was a problem with the Dynamic Menu SiteTree. " & doc.parseError.reason)
        Else
            '
            ' data is OK
            '
            On Error Resume Next
            renderedPageId = Main.renderedPageId
            renderedSectionId = Main.renderedSectionId
            If LCase(doc.documentElement.baseName) <> "menu" Then
                '
                ' error - Need a way to reach the user that submitted the file
                '
                Content.Add vbCrLf & vbTab & "<div class=ccError style=""margin:10px;padding:10px;background-color:white;"">There was a problem with the Setting Page you requested.</div>"
            Else
                '
                ' ----- Process Requests
                '
                menuId = kmaEncodeInteger(GetXMLAttribute(IsFound, doc.documentElement, "recordid", "0"))
                StylePrefix = GetXMLAttribute(IsFound, doc.documentElement, "styleprefix", "ccFlyout")
                HorizontalLayout = kmaEncodeBoolean(GetXMLAttribute(IsFound, doc.documentElement, "layout", "1") = 1)
                FlyoutOnHover = kmaEncodeBoolean(GetXMLAttribute(IsFound, doc.documentElement, "flyoutonhover", "1") = 1)
                FlyoutDirection = kmaEncodeInteger(GetXMLAttribute(IsFound, doc.documentElement, "flyoutdirection", "1"))
                delimiter = GetXMLAttribute(IsFound, doc.documentElement, "delimiter", "")
                ClickIndicator = "&nbsp;&#187;"
                With doc.documentElement
                    For Each TopNode In .childNodes
                        If LCase(TopNode.baseName) = "node" Then
                            nextButton = GetMenuFlyout(TopNode, StylePrefix, HorizontalLayout, FlyoutOnHover, FlyoutDirection, ClickIndicator, allowEdit)
                            If (delimiter <> "") And (Execute <> "") And (nextButton <> "") Then
                                '
                                ' no whitespace around delimiter -- it causes extra space if not styled.
                                '
                                Execute = Execute & "<span class=""" & StylePrefix & "Delimiter"">" & delimiter & "</span>"
                            End If
                            If allowEdit Then
                                nextButton = Main.GetRecordEditLink2("Site Sections", menuId, False, "", True) & nextButton
                            End If
                            Execute = Execute & nextButton
                        End If
                    Next
                End With
                If allowEdit Then
                    '
                    ' add record link
                    '
                    If (delimiter <> "") And (Execute <> "") And (nextButton <> "") Then
                        '
                        ' no whitespace around delimiter -- it causes extra space if not styled.
                        '
                        Execute = Execute & "<span class=""" & StylePrefix & "Delimiter"">" & delimiter & "</span>"
                    End If
                    Execute = Execute & Main.GetRecordAddLink("site sections", "menuid=" & menuId)
                End If
                If Main.isAdvancedEditing("Dynamic menus") Then
                    '
                    ' edit the menu record link
                    '
                    Execute = vbCrLf & vbTab & "<div style=""border-bottom:1px dashed #404040; padding:5px;margin-bottom:5px;"">Dynamic Menu [" & Main.GetRecordName("dynamic menus", menuId) & "]" & Main.GetRecordEditLink2("Dynamic Menus", menuId, False, "", True) & "</div><div>" & Execute & "</div>"
                End If
                '
                ' Add top list
                '
                If Execute <> "" Then
                    With doc.documentElement
                        nodeCnt = doc.documentElement.childNodes.length
                        If nodeCnt > 0 Then
                            For Ptr = 0 To nodeCnt - 1
                                Set node = doc.documentElement.childNodes(Ptr)
                                If LCase(node.baseName) = "jsfile" Then
                                    jsFile = GetXMLAttribute(IsFound, node, "name", "")
                                    If jsFile <> "" Then
                                        Call Main.AddHeadScriptLink(Main.ServerFilePath & jsFile, "menuing")
                                    End If
                                End If
                                If LCase(node.baseName) = "jsonload" Then
                                    jsOnLoad = node.Text
                                    If jsOnLoad <> "" Then
                                        Call Main.AddOnLoadJavascript2(jsOnLoad, "menuing")
                                    End If
                                End If
                                If LCase(node.baseName) = "csslegacyfile" Then
                                    cssLegacyFile = GetXMLAttribute(IsFound, node, "name", "")
                                    If cssLegacyFile <> "" Then
                                        Call Main.AddStylesheetLink2(Main.ServerFilePath & cssLegacyFile, "menuing")
                                    End If
                                End If
                            Next
                        End If
                    End With
                End If
            End If
        End If
    End If
    '
    Exit Function
ErrorTrap:
    'HandleError
End Function
'
'
'
Private Function GetSubMenu(node As IXMLDOMNode, Depth As Long, DepthMax As Long)
    On Error GoTo ErrorTrap
    '
    GetSubMenu = ""
    '
    Exit Function
ErrorTrap:
    'HandleError
End Function
'
'========================================================================
' ----- Get an XML nodes attribute based on its name
'========================================================================
'
Private Function GetXMLAttribute(found As Boolean, node As IXMLDOMNode, Name As String, DefaultIfNotFound As String) As String
    On Error GoTo ErrorTrap
    '
    Dim NodeAttribute As IXMLDOMAttribute
    Dim REsultNode As IXMLDOMNode
    Dim UcaseName As String
    '
    found = False
    Set REsultNode = node.Attributes.getNamedItem(Name)
    If (REsultNode Is Nothing) Then
        UcaseName = UCase(Name)
        For Each NodeAttribute In node.Attributes
            If UCase(NodeAttribute.NodeName) = UcaseName Then
                GetXMLAttribute = NodeAttribute.nodeValue
                found = True
                Exit For
                End If
            Next
    Else
        GetXMLAttribute = REsultNode.nodeValue
        found = True
    End If
    If Not found Then
        GetXMLAttribute = DefaultIfNotFound
    End If
    Exit Function
    '
    ' ----- Error Trap
    '
ErrorTrap:
    'HandleError
End Function
'
'===============================================================================
'   Returns the menu specified, if it is in local storage
'       It also creates the menu data in a close string that is returned in GetMenuClose.
'===============================================================================
'
Private Function GetMenuFlyout(node As IXMLDOMNode, StylePrefix As String, HorizontalLayout As Boolean, FlyoutOnHover As Boolean, FlyoutDirection As Long, ClickIndicator As String, allowEdit As Boolean) As String
    On Error GoTo ErrorTrap
    '
    Dim Link As String
    Dim EntryPointer As Long
    Dim MenuUniqueName As String
    Dim MenuEntries As String
    Dim Target As String
    Dim buttonClass As String
    Dim buttonStyle As String
    Dim HotSpotHTML As String
    Dim HotSpotHTMLHover As String
    Dim FlyoutPanel As String
    Dim MouseClickCode As String
    Dim MouseOverCode As String
    Dim MouseOutCode As String
    Dim ImageID As String
    Dim JavaCode As String
    Dim PanelButtonCount As Long
    Dim IsTextHotSpot As Boolean
    Dim IsFound As Boolean
    Dim MenuSectionID As Long
    Dim Caption As String
    Dim CaptionImageSrc As String
    Dim ImageSrc As String
    Dim ImageOverSrc As String
    '
    MouseClickCode = ""
    MouseOverCode = ""
    MouseOutCode = ""
    ImageID = "img" & CStr(GetRandomInteger) & "s"
    buttonClass = StylePrefix & "Button"
    MenuUniqueName = CStr(GetRandomInteger)
    Link = kmaEncodeHTML(GetXMLAttribute(IsFound, node, "link", ""))
    MenuSectionID = kmaEncodeInteger(GetXMLAttribute(IsFound, node, "SectionID", "menu"))
    Caption = GetXMLAttribute(IsFound, node, "Caption", "menu")
    CaptionImageSrc = GetXMLAttribute(IsFound, node, "CaptionImage", "menu")
    If HorizontalLayout Then
        buttonStyle = "display:inline;"
    Else
        buttonStyle = "display:block;"
    End If
    If MenuSectionID = Main.renderedSectionId Then
        ImageSrc = kmaEncodeHTML(GetXMLAttribute(IsFound, node, "ImageDownScr", ""))
        ImageOverSrc = kmaEncodeHTML(GetXMLAttribute(IsFound, node, "ImageDownOverSrc", ""))
    Else
        ImageSrc = kmaEncodeHTML(GetXMLAttribute(IsFound, node, "ImageSrc", ""))
        ImageOverSrc = kmaEncodeHTML(GetXMLAttribute(IsFound, node, "ImageOverSrc", ""))
    End If
    If ImageSrc <> "" Then
        '
        ' Create hotspot from image
        '
        If ImageOverSrc = "" Then
            ImageOverSrc = ImageSrc
        End If
        HotSpotHTML = "<img src=""" & ImageSrc & """ border=""0"" alt=""" & Caption & """ ID=" & ImageID & " Name=" & ImageID & ">"
        If ImageOverSrc <> "" Then
            JavaCode = JavaCode _
            & "var " & ImageID & "n=new Image; " _
            & ImageID & "n.src='" & ImageSrc & "'; " _
            & "var " & ImageID & "o=new Image; " _
            & ImageID & "o.src='" & ImageOverSrc & "'; "
            MouseOverCode = MouseOverCode & " document." & ImageID & ".src=" & ImageID & "o.src;"
            MouseOutCode = MouseOutCode & " document." & ImageID & ".src=" & ImageID & "n.src;"
        End If
    ElseIf Caption <> "" Then
        '
        ' Create hotspot text
        '
        If CaptionImageSrc <> "" Then
            HotSpotHTML = "<img alt=""" & Caption & """ src=""" & CaptionImageSrc & """ border=""0"">"
        End If
        HotSpotHTML = HotSpotHTML & Caption
        IsTextHotSpot = True
    Else
        '
        ' Create hotspot from name
        '
        HotSpotHTML = "Section " & MenuSectionID
        IsTextHotSpot = True
    End If
    '
    FlyoutPanel = GetMenuFlyoutPanel(node, MenuUniqueName, "", StylePrefix, FlyoutOnHover, PanelButtonCount, ClickIndicator)
    '
    ' do not fix the navigation menus by making an exception with the menu object. It is also used for Record Add tags, which need a flyout of 1.
    '   make the exception in the menuing code above this.
    '
    If PanelButtonCount > 0 Then
        If FlyoutOnHover Then
            MouseClickCode = MouseClickCode
            MouseOverCode = MouseOverCode & " amSetHoverMode(1); return amOpenPanel(event, '" & MenuUniqueName & "','" & FlyoutDirection & "','" & StylePrefix & "','true');"
            MouseOutCode = MouseOutCode & " amSetHoverMode(0);"
        Else
            If IsTextHotSpot Then
                HotSpotHTML = HotSpotHTML & ClickIndicator
            End If
            MouseClickCode = MouseClickCode & " amSetHoverMode(1); return amOpenPanel(event, '" & MenuUniqueName & "','" & FlyoutDirection & "','" & StylePrefix & "');"
            MouseOverCode = MouseOverCode & " amSetHoverMode(1); amButtonHover(event,'" & MenuUniqueName & "','" & FlyoutDirection & "','false');"
            MouseOutCode = MouseOutCode & " amSetHoverMode(0);"
'            MouseClickCode = MouseClickCode & " return amOpenPanel(event, '" & MenuUniqueName & "','" & FlyoutDirection & "','" & StylePrefix & "');"
'            MouseOverCode = MouseOverCode & " amButtonHover(event,'" & MenuUniqueName & "','" & FlyoutDirection & "','false');"
'            MouseOutCode = MouseOutCode
        End If
    End If
    '
    ' Convert js code to action
    '
    If MouseClickCode <> "" Then
        MouseClickCode = " onClick=""" & MouseClickCode & """ "
    End If
    If MouseOverCode <> "" Then
        MouseOverCode = " onMouseOver=""" & MouseOverCode & """ "
    End If
    If MouseOutCode <> "" Then
        MouseOutCode = " onMouseOut=""" & MouseOutCode & """ "
    End If
    '
    If FlyoutPanel <> "" Then
        '
        ' Create a flyout link - no crlf whitespace
        '
        GetMenuFlyout = "<a style=""" & buttonStyle & """ class=""" & buttonClass & """ " & MouseOutCode & " " & MouseOverCode & " " & MouseClickCode & " HREF=""" & Link & """>" & HotSpotHTML & "</a>"
        GetMenuFlyout = GetMenuFlyout & FlyoutPanel
        'iMenuCloseString = iMenuCloseString & FlyoutPanel
    ElseIf Link <> "" Then
        '
        ' Create a linked element - no crlf whitespace
        '
        GetMenuFlyout = "<a style=""" & buttonStyle & """ class=""" & buttonClass & """ " & MouseOutCode & " " & MouseOverCode & " " & MouseClickCode & " HREF=""" & Link & """>" & HotSpotHTML & "</a>"
    Else
        '
        ' no links and no flyouts, create just the caption
        '
    End If
    '
    ' Add in the inline java code if required
    '
    If JavaCode <> "" Then
        GetMenuFlyout = "" _
        & "<SCRIPT language=javascript type=text/javascript>" _
        & JavaCode _
        & "</script>" _
        & GetMenuFlyout
    End If
    Exit Function
    '
ErrorTrap:
    'Call HandleClassError("GetMenuFlyout", Err.Number, Err.Source, Err.Description)
    End Function
'
'===============================================================================
'   Gets the Menu Branch for the Default Menu
'===============================================================================
'
Private Function GetMenuFlyoutPanel(ParentNode As IXMLDOMNode, PanelUniqueName As String, UsedEntries As String, StyleSheetPrefix As String, FlyoutHover As Boolean, PanelButtonCount As Long, ClickIndicator As String) As String
    On Error GoTo ErrorTrap
    '
    Dim EntryPointer As Long
    Dim iUsedEntries As String
    Dim SubMenuName As String
    Dim SubMenuCount As Long
    Dim Target As String
    Dim SubMenus As String
    Dim PanelChildren As String
    Dim PanelButtons As String
    Dim PanelButtonStyle As String
    Dim HotSpotHTML As String
    Dim node As IXMLDOMNode
    Dim NewWindow As Boolean
    Dim IsFound As Boolean
    Dim Link As String
    Dim MenuSectionID As Long
    Dim Caption As String
    Dim ImageSrc As String
    Dim ImageOverSrc As String
    Dim CaptionImageSrc As String
    Dim ChildPanelUniqueName As String
    '
    For Each node In ParentNode.childNodes
        If LCase(ParentNode.baseName) = "node" Then
            PanelButtonCount = PanelButtonCount + 1
            ChildPanelUniqueName = CStr(Main.GetRandomLong)
            PanelButtonStyle = StyleSheetPrefix & "PanelButton"
            NewWindow = kmaEncodeBoolean(GetXMLAttribute(IsFound, node, "NewWindow", "menu"))
            Link = kmaEncodeHTML(GetXMLAttribute(IsFound, node, "link", ""))
            MenuSectionID = kmaEncodeInteger(GetXMLAttribute(IsFound, node, "SectionID", "menu"))
            Caption = GetXMLAttribute(IsFound, node, "Caption", "menu")
            CaptionImageSrc = GetXMLAttribute(IsFound, node, "CaptionImage", "menu")
            If MenuSectionID = Main.renderedSectionId Then
                ImageSrc = kmaEncodeHTML(GetXMLAttribute(IsFound, node, "ImageDownScr", ""))
                ImageOverSrc = kmaEncodeHTML(GetXMLAttribute(IsFound, node, "ImageDownOverSrc", ""))
            Else
                ImageSrc = kmaEncodeHTML(GetXMLAttribute(IsFound, node, "ImageSrc", ""))
                ImageOverSrc = kmaEncodeHTML(GetXMLAttribute(IsFound, node, "ImageOverSrc", ""))
            End If
            
            Target = ""
            If NewWindow Then
                Target = " target=""_blank"""
            End If
            PanelChildren = GetMenuFlyoutPanel(node, ChildPanelUniqueName, iUsedEntries, StyleSheetPrefix, FlyoutHover, PanelButtonCount, ClickIndicator)
            If ImageSrc <> "" Then
                HotSpotHTML = "<img src=""" & ImageSrc & """ border=""0"" alt=""" & Caption & """>"
            Else
                HotSpotHTML = Caption
            End If
            'HotSpotHTML = Caption
            'If (.StyleSheet <> "") And (.StyleSheet <> "ccFlyoutPanelButton") Then
            '    HotSpotHTML = "<SPAN class=""" & .StyleSheet & """>" & HotSpotHTML & "</SPAN>"
            '    End If
            If PanelChildren = "" Then
                If Link = "" Then
                    '
                    ' ----- no link and no child panel
                    '
                    'PanelButtons = PanelButtons & "<SPAN class=""" & PanelButtonStyle & """>" & HotSpotHTML & "</SPAN>"
                Else
                    '
                    ' ----- Link but no child panel - no crlf whitespace
                    '
                    PanelButtons = PanelButtons & "<a class=""" & PanelButtonStyle & """ href=""" & kmaEncodeHTML(Link) & """" & Target & ">" & HotSpotHTML & "</a>"
                End If
            Else
                If Link = "" Then
                    '
                    ' ----- Child Panel and no link, block the href so menu "parent" buttons will not be clickable
                    '  - no crlf whitespace
                    '
                    If FlyoutHover Then
                        PanelButtons = PanelButtons & "<a class=""" & PanelButtonStyle & """ onmouseover=""amSetHoverMode(1); amPanelButtonHover(event,'" & ChildPanelUniqueName & "','" & StyleSheetPrefix & "');"" onmouseout=""amSetHoverMode(0);"" onclick=""return false;"" href=""#""" & Target & ">" & HotSpotHTML & ClickIndicator & "</a>"
                        'PanelButtons = PanelButtons & "<a class=""" & PanelButtonStyle & """ onmouseover=""amSetHoverMode(1); amPanelButtonHover(event,'" & ChildPanelUniqueName & "','" & StyleSheetPrefix & "');"" onmouseout=""amSetHoverMode(0);"" onclick=""return false;"" href=""#""" & Target & ">" & HotSpotHTML & "&nbsp;<font face=""webdings"">4</font></a>"
                    Else
                        PanelButtons = PanelButtons & "<a class=""" & PanelButtonStyle & """ onmouseover=""amSetHoverMode(1); amPanelButtonHover(event,'" & ChildPanelUniqueName & "','" & StyleSheetPrefix & "');"" onclick=""return false;"" href=""#""" & Target & ">" & HotSpotHTML & ClickIndicator & "</a>"
                        'PanelButtons = PanelButtons & "<a class=""" & PanelButtonStyle & """ onmouseover=""amPanelButtonHover(event,'" & ChildPanelUniqueName & "','" & StyleSheetPrefix & "');"" onclick=""return false;"" href=""#""" & Target & ">" & HotSpotHTML & ClickIndicator & "</a>"
                        'PanelButtons = PanelButtons & "<a class=""" & PanelButtonStyle & """ onmouseover=""amPanelButtonHover(event,'" & ChildPanelUniqueName & "','" & StyleSheetPrefix & "');"" onclick=""return false;"" href=""#""" & Target & ">" & HotSpotHTML & "&nbsp;<font face=""webdings"">4</font></a>"
                    End If
                    'PanelButtons = PanelButtons & "<a class=""" & PanelButtonStyle & """ onmouseover=""amPanelButtonHover(event,'" & ChildPanelUniqueName & "');"" onclick=""return false;"" href=""#""" & Target & ">" & HotSpotHTML & "</a>"
                    'PanelButtons = PanelButtons & "<a class=""" & PanelButtonStyle & """ onmouseover=""amPanelButtonHover(event,'" & ChildPanelUniqueName & "');"" onclick=""return false;"" href=""#""" & Target & "><span style=""font-family: dingbats"">4</SPAN>" & HotSpotHTML & "</a>"
                Else
                    '
                    ' ----- Child Panel and a link  - no crlf whitespace
                    '
                    If FlyoutHover Then
                        PanelButtons = PanelButtons & "<a class=""" & PanelButtonStyle & """ onmouseover=""amSetHoverMode(1); amPanelButtonHover(event,'" & ChildPanelUniqueName & "','" & StyleSheetPrefix & "');"" onmouseout=""amSetHoverMode(0);"" href=""" & kmaEncodeHTML(Link) & """" & Target & ">" & HotSpotHTML & ClickIndicator & "</a>"
                        'PanelButtons = PanelButtons & "<a class=""" & PanelButtonStyle & """ onmouseover=""amSetHoverMode(1); amPanelButtonHover(event,'" & ChildPanelUniqueName & "','" & StyleSheetPrefix & "');"" onmouseout=""amSetHoverMode(0);"" href=""" & Link & """" & Target & ">" & HotSpotHTML & "&nbsp;<font face=""webdings"">4</font></a>"
                    Else
                        PanelButtons = PanelButtons & "<a class=""" & PanelButtonStyle & """ onmouseover=""amSetHoverMode(1);amPanelButtonHover(event,'" & ChildPanelUniqueName & "','" & StyleSheetPrefix & "');"" href=""" & kmaEncodeHTML(Link) & """" & Target & ">" & HotSpotHTML & ClickIndicator & "</a>"
                        'PanelButtons = PanelButtons & "<a class=""" & PanelButtonStyle & """ onmouseover=""amPanelButtonHover(event,'" & ChildPanelUniqueName & "','" & StyleSheetPrefix & "');"" href=""" & kmaEncodeHTML(Link) & """" & Target & ">" & HotSpotHTML & ClickIndicator & "</a>"
                        'PanelButtons = PanelButtons & "<a class=""" & PanelButtonStyle & """ onmouseover=""amPanelButtonHover(event,'" & ChildPanelUniqueName & "','" & StyleSheetPrefix & "');"" href=""" & Link & """" & Target & ">" & HotSpotHTML & "&nbsp;<font face=""webdings"">4</font></a>"
                    End If
                End If
            End If
            SubMenus = SubMenus & PanelChildren
        End If
    Next
    '
    '
    '
    If PanelButtons <> "" Then
        '
        ' ----- If panel buttons are returned, wrap them in a DIV  - no crlf whitespace
        '
        If FlyoutHover Then
            GetMenuFlyoutPanel = "" _
                & "<div style=""position: absolute; left: 0px;visibility:hidden;"" class=""kmaMenu " & StyleSheetPrefix & "Panel"" id=""" & PanelUniqueName & """ onmouseover=""amSetHoverMode(1);amPanelHover(event,'" & StyleSheetPrefix & "');amStopEvent(event);"" onmouseout=""amSetHoverMode(0);"">" _
                & PanelButtons _
                & SubMenus _
                & "</div>" _
                & ""
'            GetMenuFlyoutPanel = "" _
'                & "<div style=""position: absolute; left: 0px;visibility:hidden;"" class=""kmaMenu " & StyleSheetPrefix & "Panel"" id=""" & PanelUniqueName & """ onmouseover=""amSetHoverMode(1); amPanelHover(event,'" & StyleSheetPrefix & "');"" onmouseout=""amSetHoverMode(0);"">" _
'                & PanelButtons _
'                & SubMenus _
'                & "</div>" _
'                & ""
        Else
            GetMenuFlyoutPanel = "" _
                & "<div style=""position: absolute; left: 0px;visibility:hidden;"" class=""kmaMenu " & StyleSheetPrefix & "Panel"" id=""" & PanelUniqueName & """ onmouseover=""amSetHoverMode(1);amPanelHover(event,'" & StyleSheetPrefix & "');amStopEvent(event);"" onmouseout=""amSetHoverMode(0);"">" _
                & PanelButtons _
                & SubMenus _
                & "</div>" _
                & ""
'            GetMenuFlyoutPanel = "" _
'                & "<div style=""position: absolute; left: 0px;visibility:hidden;"" class=""kmaMenu " & StyleSheetPrefix & "Panel"" id=""" & PanelUniqueName & """ onmouseover=""amPanelHover(event,'" & StyleSheetPrefix & "');amStopEvent(event);"">" _
'                & PanelButtons _
'                & SubMenus _
'                & "</div>" _
'                & ""
'            GetMenuFlyoutPanel = "" _
'                & "<div style=""position: absolute; left: 0px;visibility:hidden;"" class=""kmaMenu " & StyleSheetPrefix & "Panel"" id=""" & PanelUniqueName & """ onmouseover=""amPanelHover(event,'" & StyleSheetPrefix & "');"">" _
'                & PanelButtons _
'                & SubMenus _
'                & "</div>" _
'                & ""
        End If
    End If
    '
    Exit Function
    '
ErrorTrap:
    'Call HandleClassError("GetMenuFlyoutPanel", Err.Number, Err.Source, Err.Description)
    End Function

