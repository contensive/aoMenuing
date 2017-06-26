

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Menuing
    '
    ' Sample Vb addon
    '
    Public Class menu2Class
        Inherits AddonBaseClass
        Public Overrides Function Execute(CP As CPBaseClass) As Object
            Dim result As String = ""
            Try
                Dim menuId As Integer = CP.Doc.GetInteger("menuId")
            Catch ex As Exception
                CP.Site.ErrorReport(ex)
            End Try
            Return result
        End Function
    End Class
End Namespace
