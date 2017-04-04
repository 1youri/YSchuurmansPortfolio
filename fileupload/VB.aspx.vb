Imports System.IO
Imports AjaxControlToolkit

Partial Class VB
    Inherits System.Web.UI.Page
    Protected Sub OnUploadComplete(sender As Object, e As AjaxFileUploadEventArgs)
        Dim fileName As String = Path.GetFileName(e.FileName)
        AjaxFileUpload11.SaveAs(Server.MapPath(Convert.ToString("~/Uploads/") & fileName))
    End Sub
End Class
