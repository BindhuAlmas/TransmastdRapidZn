using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace AmarCentre.WAImplementation
{
    public class WABAL
    {
        public DataTable GetWAMessages(DataTable dtMessageId)
        {
            Database_Operations db_obj = new Database_Operations("GetWAMessages", true);
            db_obj.AddParameter("@messagetbl", dtMessageId);
            return db_obj.GetDataTable();
        }
        public DataTable GetWACachedMessagesCount(DateTime fromdate,DateTime todate)
        {
            Database_Operations db_obj = new Database_Operations("GetWACachedMessagesCount", true);
            db_obj.AddParameter("@fromdate", fromdate);
            db_obj.AddParameter("@todate", todate);
            return db_obj.GetDataTable();
        }
        public int SaveWAMessageLog(string messageid,string customername,DateTime messagedate,string mobileno,string documenttype,DateTime? docexpirydate,string documentno)
        {
            Database_Operations db_obj = new Database_Operations("SaveWAMessageLog", true);
            db_obj.AddParameter("@messageid", messageid);
            db_obj.AddParameter("@customername", customername);
            db_obj.AddParameter("@mobilenumber", mobileno);
            db_obj.AddParameter("@documenttype", documenttype);
            db_obj.AddParameter("@documentno", documentno);
            db_obj.AddParameter("@docexpirydate", docexpirydate);

            return db_obj.ExecuteQuery();
        }
        public int SaveWAFailedMessageLog(string RecipientPhoneNumber, string TemplateCode, string BodyParameters, string MediaTypeName, string Priority, string CustomerName,     string documenttype, string documentno, DateTime? docexpirydate)
        {
            Database_Operations db_obj = new Database_Operations("SaveWAMessageFailedLog", true);
            db_obj.AddParameter("@RecipientPhoneNumber", RecipientPhoneNumber);
            db_obj.AddParameter("@TemplateCode", TemplateCode);
            db_obj.AddParameter("@BodyParameters", BodyParameters);
            db_obj.AddParameter("@MediaTypeName", MediaTypeName);
            db_obj.AddParameter("@Priority", Priority);
            db_obj.AddParameter("@CustomerName", CustomerName);
            db_obj.AddParameter("@documenttype", documenttype);
            db_obj.AddParameter("@DocumentDescription", documentno);
            db_obj.AddParameter("@docexpirydate", docexpirydate);

            return db_obj.ExecuteQuery();
        }
        public int SaveWAFailedMessageLog(string MessageId,string RecipientPhoneNumber, string TemplateCode, string BodyParameters, string MediaTypeName, string Priority, string CustomerName, string documenttype, string documentno)
        {
            Database_Operations db_obj = new Database_Operations("SaveWAMessageFailedLog", true);
            db_obj.AddParameter("@RecipientPhoneNumber", RecipientPhoneNumber);
            db_obj.AddParameter("@MessageId", MessageId);
            db_obj.AddParameter("@TemplateCode", TemplateCode);
            db_obj.AddParameter("@BodyParameters", BodyParameters);
            db_obj.AddParameter("@MediaTypeName", MediaTypeName);
            db_obj.AddParameter("@Priority", Priority);
            

            return db_obj.ExecuteQuery();
        }
        public int SaveWAHistoryLog(string MessageId, string RecipientPhoneNumber, string TemplateCode, string BodyParameters, string MediaTypeName, string Priority)
        {
            Database_Operations db_obj = new Database_Operations("SaveWAMessageHistoryLog", true);
            db_obj.AddParameter("@RecipientPhoneNumber", RecipientPhoneNumber);
            db_obj.AddParameter("@MessageId", MessageId);
            db_obj.AddParameter("@TemplateCode", TemplateCode);
            db_obj.AddParameter("@BodyParameters", BodyParameters);
            db_obj.AddParameter("@MediaTypeName", MediaTypeName);
            db_obj.AddParameter("@Priority", Priority);


            return db_obj.ExecuteQuery();
        }
    }
}