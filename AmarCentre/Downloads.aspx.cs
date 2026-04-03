using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace AmarCentre
{
    public partial class Downloads : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FileInfo FileInfo = default(FileInfo);
            string sFileExt = null;
            string sMIMEType = null;
            string[] sDocInfo = null;
            string file_name = Request.QueryString["file_name"].ToString();
            string path = Server.MapPath("UploadedFiles");
            string full_path = path + "\\" + file_name.ToString();
            if (File.Exists(full_path))
            {
                FileInfo = new FileInfo(full_path);
                sDocInfo = FileInfo.Name.Split('.');
                sFileExt = sDocInfo[1];
                sMIMEType = GetMIMEType(sFileExt);
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + FileInfo.Name + "\"");
                HttpContext.Current.Response.AddHeader("Content-Length", FileInfo.Length.ToString());
                HttpContext.Current.Response.ContentType = sMIMEType;
                HttpContext.Current.Response.WriteFile(FileInfo.FullName);
                Response.Flush();
                FileInfo = null;

            }
            else
            {
                Response.Write("Sorry... File Not exists..");
            }
        }
        private string GetMIMEType(string sExtension)
        {
            try
            {
                //sExtension = sExtension.Substring(sExtension.Length - 1);
                switch (sExtension.ToUpper())
                {
                    case "PDF":
                        return "Application/pdf";
                        break; // TODO: might not be correct. Was : Exit Select

                        break;
                    case "DOC":
                        return "Application/msword";
                        break; // TODO: might not be correct. Was : Exit Select

                        break;
                    case "DOCX":
                        return "Application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break; // TODO: might not be correct. Was : Exit Select

                        break;
                    case "DOT":
                        return "Application/msword";
                        break; // TODO: might not be correct. Was : Exit Select

                        break;
                    case "XLS":
                        return "Application/vnd.ms-excel";
                        break; // TODO: might not be correct. Was : Exit Select

                        break;
                    case "XLSX":
                        return "Application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        break; // TODO: might not be correct. Was : Exit Select

                        break;
                }
                return "";
            }
            catch (Exception ex)
            {
                return "";
                Response.Write(ex.Message);
            }
        }
    }
}