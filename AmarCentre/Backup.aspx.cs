using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AmarCentre
{
    public partial class Backup : System.Web.UI.Page
    {
        BAL.System_Utilities obj_business = new BAL.System_Utilities();

        protected void Page_Load(object sender, EventArgs e)
        {
            obj_business.Back_up();
        }
    }
}