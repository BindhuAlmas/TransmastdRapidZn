using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Timers;
using AmarCentre.BAL;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Configuration;

namespace AmarCentre
{
    public class Global : System.Web.HttpApplication
    {
        Master_Bal obj_master = new Master_Bal();

        void Application_Start(object sender, EventArgs e)
        {
            DataTable dt = obj_master.Edit_GeneralSettings();
            Application["PrintHeader"] = dt.Rows[0]["PrintHeader"].ToString();
            Application["PrintFooter"] = dt.Rows[0]["PrintFooter"].ToString();
            Application["Company"] = dt.Rows[0]["CompanyName"].ToString();

            // Code that runs on application startup
            System.Timers.Timer myTimer = new System.Timers.Timer();
            // Set the Interval to 5 seconds (5000 milliseconds).
            myTimer.Interval = 7200000;/*for 2 hours interval*/
            myTimer.AutoReset = true;
            myTimer.Elapsed += new ElapsedEventHandler(myTimer_Elapsed);
            myTimer.Enabled = true;

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11;

        }

        public void myTimer_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            System_Utilities obj = new System_Utilities();
            obj.Back_up();
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (exception != null)
            {
                //log the error
            }

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            Session.Timeout = 120;
        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

    }
}
