using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace AmarCentre.BAL
{
    public class dtClass : System.Web.UI.Page
    {
        public static DataTable dtmultiple = new DataTable();

        public void setdtmultiple(DataTable dt)
        {
            dtmultiple = dt;
        }
        public DataTable returndtmultiple()
        {
            return dtmultiple;
        }
    }
}