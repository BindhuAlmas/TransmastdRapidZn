using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Telerik.Web.UI;

namespace AmarCentre.Transactions.UserControl
{
    /// <summary>
    /// Summary description for WebServiceDrpFill
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebServiceDrpFill : System.Web.Services.WebService
    {

        public static DataTable dtCustomername = new DataTable();
        public static DataTable dtCustomernameAgent = new DataTable();

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        private const int ItemsPerRequest = 10;

        [WebMethod]
        public static RadComboBoxData GetCustomerNames(RadComboBoxContext context)
        {
            DataTable data = GetData(context.Text);

            RadComboBoxData comboData = new RadComboBoxData();

            int itemOffset = context.NumberOfItems;
            int endOffset = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count);
            comboData.EndOfItems = endOffset == data.Rows.Count;

            List<RadComboBoxItemData> result = new List<RadComboBoxItemData>(endOffset - itemOffset);

            for (int i = itemOffset; i < endOffset; i++)
            {
                RadComboBoxItemData itemData = new RadComboBoxItemData();
                itemData.Text = data.Rows[i]["Text"].ToString();
                itemData.Value = data.Rows[i]["Value"].ToString();
                result.Add(itemData);
            }

            comboData.Items = result.ToArray();
            return comboData;
        }

        private static DataTable GetData(string text)
        {
            DataTable dh = new DataTable();
            if (dtCustomernameAgent.Rows.Count > 0)
            {
                dh = dtCustomernameAgent.Clone();

                DataRow[] dr = dtCustomernameAgent.Select("Text LIKE '%" + text + "%'");
                int cv = dr.Length;

                if (cv > 0)
                {
                    dh = dr.CopyToDataTable();
                }
            }
            else
            {
                dh = dtCustomername.Clone();

                DataRow[] dr = dtCustomername.Select("Text LIKE '%" + text + "%'");
                int cv = dr.Length;

                if (cv > 0)
                {
                    dh = dr.CopyToDataTable();
                }
            }

            return dh;
        }

    }
}
