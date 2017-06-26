
Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses
Imports Newtonsoft.Json

Namespace Menuing.Models
    Public Class dynamicMenuModel
        Inherits baseModel
        '
        '====================================================================================================
        '-- const
        Public Const contentName As String = "dynamic menus"
        Public Const contentTableName As String = "ccDynamicMenus"
        Private Shadows Const contentDataSource As String = "default"
        '
        '====================================================================================================
        ' -- instance properties
        Public Property classFlyoutParent As String
        Public Property classItemActive As String
        Public Property classItemFirst As String
        Public Property classItemHover As String
        Public Property classItemLast As String
        Public Property classTierItem As String
        Public Property classTierList As String
        Public Property classTopItem As String
        Public Property classTopList As String
        Public Property classTopWrapper As String
        Public Property ContentCategoryID As Integer
        Public Property Delimiter As String
        Public Property Depth As Integer
        Public Property EditArchive As Boolean
        Public Property EditBlank As Boolean
        Public Property EditSourceID As Integer
        Public Property FlyoutDirection As Integer
        Public Property FlyoutOnHover As Boolean
        Public Property JavaScriptOnLoad As String
        Public Property JSFilename As String
        Public Property Layout As Integer
        Public Property listStylesFilename As String
        Public Property StylePrefix As String
        Public Property StylesFilename As String
        Public Property useJsFlyoutCode As Boolean
        '
        '====================================================================================================
        Public Overloads Shared Function add(cp As CPBaseClass) As dynamicMenuModel
            Return add(Of dynamicMenuModel)(cp)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function create(cp As CPBaseClass, recordId As Integer) As dynamicMenuModel
            Return create(Of dynamicMenuModel)(cp, recordId)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function create(cp As CPBaseClass, recordGuid As String) As dynamicMenuModel
            Return create(Of dynamicMenuModel)(cp, recordGuid)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function createByName(cp As CPBaseClass, recordName As String) As dynamicMenuModel
            Return createByName(Of dynamicMenuModel)(cp, recordName)
        End Function
        '
        '====================================================================================================
        Public Overloads Sub save(cp As CPBaseClass)
            MyBase.save(cp)
        End Sub
        '
        '====================================================================================================
        Public Overloads Shared Sub delete(cp As CPBaseClass, recordId As Integer)
            delete(Of dynamicMenuModel)(cp, recordId)
        End Sub
        '
        '====================================================================================================
        Public Overloads Shared Sub delete(cp As CPBaseClass, ccGuid As String)
            delete(Of dynamicMenuModel)(cp, ccGuid)
        End Sub
        '
        '====================================================================================================
        Public Overloads Shared Function createList(cp As CPBaseClass, sqlCriteria As String, Optional sqlOrderBy As String = "id") As List(Of dynamicMenuModel)
            Return createList(Of dynamicMenuModel)(cp, sqlCriteria, sqlOrderBy)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getRecordName(cp As CPBaseClass, recordId As Integer) As String
            Return baseModel.getRecordName(Of dynamicMenuModel)(cp, recordId)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getRecordName(cp As CPBaseClass, ccGuid As String) As String
            Return baseModel.getRecordName(Of dynamicMenuModel)(cp, ccGuid)
        End Function
        '
        '====================================================================================================
        Public Overloads Shared Function getRecordId(cp As CPBaseClass, ccGuid As String) As Integer
            Return baseModel.getRecordId(Of dynamicMenuModel)(cp, ccGuid)
        End Function
    End Class
End Namespace
