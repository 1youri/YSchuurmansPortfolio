using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using AjaxControlToolkit;

public partial class CS : System.Web.UI.Page
{
    protected void OnUploadComplete(object sender, AjaxFileUploadEventArgs e)
    {
        string fileName = Path.GetFileName(e.FileName);
        AjaxFileUpload11.SaveAs(Server.MapPath("~/Uploads/" + fileName));
    }
}