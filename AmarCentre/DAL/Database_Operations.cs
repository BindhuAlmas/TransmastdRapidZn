
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Configuration;

public partial class Database_Operations : System.Web.UI.Page
{
    #region Private fields

    private SqlConnection SqlCon = new SqlConnection();
    public SqlCommand SqlCmd = new SqlCommand();

    public static string connectionstring = ConfigurationManager.ConnectionStrings["SQL_CONNECTION"].ToString();

    //public static string connectionstring = @"Data Source=LAPTOP-8QCOSBF4\MSSQLSERVER2014; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = "Data Source=localhost,1441; Initial Catalog =ttsdemoi_transmasstd;uid=ttsdemoi_transmasstd;password=ttsdemo*1";

    //public static string connectionstring = @"Data Source=DESKTOP-M2FHFV5; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=USER1; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=DESKTOP-GAETJ4U; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=USER-PC; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=HPSYS; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=ALSAFA; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=PC2; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=PC-03; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=HP-PC; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=DESKTOP-OMQ90VA; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=PC1; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=PC01; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=SYSTEM002; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=COUNTER3; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=DESKTOP-8MONC24; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=DESKTOP-CF47KMP; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=DESKTOP-1CE98BD; Initial Catalog =AmerTransmas8200;Integrated Security=True";
    //public static string connectionstring = @"Data Source= DESKTOP-EBPRGKQ; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=DESKTOP-8LGGC4Q; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=DESKTOP-E7ALMJ9; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=DESKTOP-E2JA2JF; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=DESKTOP-649R5N7\SQLEXPRESS; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=SOFTWARE-LEHBAB; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=ID009; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=Patels; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=DESKTOP-OHLAJJ0; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=RECEPTION-PC; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=RECEPTION-PC; Initial Catalog =AmerTransmas8500;Integrated Security=True";
    //public static string connectionstring = @"Data Source=HMS-PC; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=HMS-PC; Initial Catalog =AmerTransmas8200;Integrated Security=True";
    //public static string connectionstring = @"Data Source=DESKTOP-I20UNB5; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=COUNTER-1; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=MEKTOUBPC-2; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=ENOXX3\SQLEXPRESS; Initial Catalog =AmarCentre;Integrated Security=True";
    //public static string connectionstring = @"Data Source=DESKTOP-NKFJRJ4\SQLEXPRESS; Initial Catalog =AmarCentre;Integrated Security=True";
    //public static string connectionstring = @"Data Source=NTS-SOFTWARE\SQLEXPRESS; Initial Catalog =AmarCentre;Integrated Security=True";
    //public static string connectionstring = @"Data Source=NAIFTYPING-PC\SQLEXPRESS; Initial Catalog =AmarCentre;Integrated Security=True";
    //public static string connectionstring = @"Data Source=USER1-PC; Initial Catalog =AmarCentre;Integrated Security=True";
    //public static string connectionstring = @"Data Source=USER1-PC; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=ABCSVR-2; Initial Catalog =AmarCentre;uid=sa;password=Abc123!@#";
    //public static string connectionstring = @"Data Source=HP\SQLEXPRESS; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=NAIF1-PC\SQLEXPRESS; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=AWIR2016-PC\SQLEXPRESS; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=SERVER\SQLEXPRESS; Initial Catalog =AmerCentre;Integrated Security=True";
    //public static string connectionstring = @"Data Source=NAIF-6-PC; Initial Catalog =AmarCentre;Integrated Security=True";
    //public static string connectionstring = @"Data Source=PC4\TTS; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=COUNTER-5; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=DESKTOP-SHARP; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=SERVER; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=DESKTOP-KCH1V1O; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=RECEP-PC; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=DESKTOP-1CE98BD; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=DESKTOP-3CC843K; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=AL-SADA\SQLEXPRESS; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=SYSTEM3\SQLEXPRESS; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=ACCOUNTS-PC\SQLEXPRESS; Initial Catalog =AmerTransmas;Integrated Security=True";
    //public static string connectionstring = @"Data Source=SERVER_ICM; Initial Catalog =AmerTransmas;Integrated Security=True";

    #endregion

    #region Public methods
    public int ExecuteQuery_backup()
    {
        int rowsAffected = -1;
        SqlCon = new SqlConnection();
        using (SqlCon = new SqlConnection(connectionstring))
        {
            try
            {
                SqlCon.Open();
                SqlCmd.Connection = SqlCon;

                rowsAffected = SqlCmd.ExecuteNonQuery();

                rowsAffected = 1;

            }
            catch (Exception exp)
            {
                rowsAffected = -1;
                //Response.Write(exp.Message);
            }
            finally
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }
        }
        return rowsAffected;
    }
    public Database_Operations(ref StringBuilder Query, bool Isprocedure)
    {
        SqlCmd = new SqlCommand(Query.ToString());
        SqlCmd.CommandTimeout = 500;
        if (Isprocedure)
            SqlCmd.CommandType = CommandType.StoredProcedure;
    }
    public Database_Operations(string Query, bool Isprocedure)
    {
        try
        {
            SqlCmd = new SqlCommand(Query);
            SqlCmd.CommandTimeout = 500;
            if (Isprocedure)
                SqlCmd.CommandType = CommandType.StoredProcedure;
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.Message);
        }
    }
    public DataSet FillDataSet()
    {
        //SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL_CONNECTION"].ToString());
        SqlCon = new SqlConnection(connectionstring);
        using (SqlCon)
        {
            DataSet Ds = new DataSet();
            try
            {
                SqlCon.Open();
                SqlCmd.Connection = SqlCon;
                SqlDataAdapter SqlAd = new SqlDataAdapter(SqlCmd);
                SqlAd.Fill(Ds);
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                HttpContext.Current.Response.Write(ex.Message);

            }
            finally
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }
            return (Ds);
        }
    }
    public DataTable GetDataTable()
    {
        DataTable Dt = new DataTable();
        try
        {

            DataSet Ds = FillDataSet();
            if (Ds.Tables.Count != 0)
            {
                Dt = Ds.Tables[0];
                return Dt;
            }
            //else
            //    Dt = null;
        }
        catch
        {
        }

        return Dt;
    }

    public DataSet GetDataSet()
    {
        DataSet Ds = new DataSet();
        try
        {

            Ds = FillDataSet();
            if (Ds.Tables.Count != 0)
            {
                //Dt = Ds.Tables[0];
                return Ds;
            }
            //else
            //    Dt = null;
        }
        catch (Exception exp)
        {
        }

        return Ds;
    }

    public DataRow GetDataRow()
    {
        DataRow Dr;
        DataSet Ds = FillDataSet();
        if (Ds != null && Ds.Tables.Count != 0 && Ds.Tables[0].Rows.Count == 1)
            Dr = Ds.Tables[0].Rows[0];
        else
            Dr = null;
        return Dr;
    }
    public int ExecuteQuery()
    {
        int rowsAffected = -1;
        SqlCon = new SqlConnection();
        using (SqlCon = new SqlConnection(connectionstring))
        {
            try
            {
                SqlCon.Open();
                SqlCmd.Connection = SqlCon;
                SqlTransaction transaction;
                transaction = SqlCon.BeginTransaction();
                try
                {
                    SqlCmd.Transaction = transaction;
                    rowsAffected = SqlCmd.ExecuteNonQuery();
                    transaction.Commit();
                    rowsAffected = 1;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    rowsAffected = -1;
                    Response.Write(ex.Message);
                }
            }
            catch (Exception exp)
            {
                rowsAffected = -1;
                //Response.Write(exp.Message);
            }
            finally
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }
        }
        return rowsAffected;
    }
    public bool RecordExists()
    {
        SqlCon = new SqlConnection();
        using (SqlCon = new SqlConnection(connectionstring))
        {
            try
            {
                SqlCon.Open();
                SqlCmd.Connection = SqlCon;
                SqlDataReader SqlDr = SqlCmd.ExecuteReader();
                if (SqlDr.Read())
                {
                    SqlDr.Close();
                    return (true);
                }
                else
                    return (false);
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
            }
            finally
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }
            return false;
        }
    }
    public void AddParameter(string parameterName, int parameterValue)
    {
        SqlParameter Parameter = new SqlParameter(parameterName, SqlDbType.Int);
        Parameter.Value = parameterValue;
        SqlCmd.Parameters.Add(Parameter);
    }
    public void AddParameter(string parameterName, long parameterValue)
    {
        SqlParameter Parameter = new SqlParameter(parameterName, SqlDbType.BigInt);
        Parameter.Value = parameterValue;
        SqlCmd.Parameters.Add(Parameter);
    }

    public void AddParameter(string parameterName, DataTable parameterValue)
    {
        SqlCmd.Parameters.AddWithValue(parameterName, parameterValue);

    }
    public void AddParameter(string parameterName, decimal parameterValue)
    {
        SqlParameter Parameter = new SqlParameter(parameterName, SqlDbType.Decimal);
        Parameter.Value = parameterValue;
        SqlCmd.Parameters.Add(Parameter);
    }
    public void AddParameter(string parameterName, decimal? parameterValue)
    {
        SqlParameter Parameter = new SqlParameter(parameterName, SqlDbType.Decimal);
        Parameter.Value = parameterValue;
        SqlCmd.Parameters.Add(Parameter);
    }
    public void AddParameter(string parameterName, int? parameterValue)
    {
        SqlParameter Parameter = new SqlParameter(parameterName, SqlDbType.Int);
        if (parameterValue == null)
            Parameter.Value = DBNull.Value;
        else
            Parameter.Value = parameterValue;
        SqlCmd.Parameters.Add(Parameter);
    }
    public void AddParameter(string parameterName, string parameterValue)
    {
        SqlParameter Parameter = new SqlParameter(parameterName, SqlDbType.VarChar);
        if (parameterValue == "")
            Parameter.Value = DBNull.Value;
        else
            Parameter.Value = parameterValue;
        SqlCmd.Parameters.Add(Parameter);
    }
    public void AddParameter(string parameterName, string parameterValue,int nvarchar)
    {
        SqlParameter Parameter = new SqlParameter(parameterName, SqlDbType.NVarChar);
        if (parameterValue == "")
            Parameter.Value = DBNull.Value;
        else
            Parameter.Value = parameterValue;
        SqlCmd.Parameters.Add(Parameter);
    }
    public void AddParameter(string parameterName, float parameterValue)
    {
        SqlParameter Parameter = new SqlParameter(parameterName, SqlDbType.Float);
        Parameter.Value = parameterValue;
        SqlCmd.Parameters.Add(Parameter);
    }
    public void AddParameter(string parameterName, float? parameterValue)
    {
        SqlParameter Parameter = new SqlParameter(parameterName, SqlDbType.Float);
        if (parameterValue == null)
            Parameter.Value = DBNull.Value;
        else
            Parameter.Value = parameterValue;
        SqlCmd.Parameters.Add(Parameter);
    }
    public void AddParameter(string parameterName, double parameterValue)
    {
        SqlParameter Parameter = new SqlParameter(parameterName, SqlDbType.Float);
        Parameter.Value = parameterValue;
        SqlCmd.Parameters.Add(Parameter);
    }
    public void AddParameter(string parameterName, double? parameterValue)
    {
        SqlParameter Parameter = new SqlParameter(parameterName, SqlDbType.Float);
        if (parameterValue == null)
            Parameter.Value = DBNull.Value;
        else
            Parameter.Value = parameterValue;
        SqlCmd.Parameters.Add(Parameter);
    }
    public void AddParameter(string parameterName, bool parameterValue)
    {
        SqlParameter Parameter = new SqlParameter(parameterName, SqlDbType.Bit);
        Parameter.Value = parameterValue;
        SqlCmd.Parameters.Add(Parameter);
    }
    public void AddParameter(string parameterName, bool? parameterValue)
    {
        SqlParameter Parameter = new SqlParameter(parameterName, SqlDbType.Bit);
        if (parameterValue == null)
            Parameter.Value = DBNull.Value;
        else
            Parameter.Value = parameterValue;
        SqlCmd.Parameters.Add(Parameter);

    }
    public void AddParameter(string parameterName, char parameterValue)
    {
        SqlParameter Parameter = new SqlParameter(parameterName, SqlDbType.Char);
        Parameter.Value = parameterValue;
        SqlCmd.Parameters.Add(Parameter);
    }
    public void AddParameter(string parameterName, char? parameterValue)
    {
        SqlParameter Parameter = new SqlParameter(parameterName, SqlDbType.Char);
        if (parameterValue == null)
            Parameter.Value = DBNull.Value;
        else
            Parameter.Value = parameterValue;
        SqlCmd.Parameters.Add(Parameter);
    }
    public void AddParameter(string parameterName, DateTime parameterValue)
    {
        SqlParameter Parameter = new SqlParameter(parameterName, SqlDbType.DateTime);
        Parameter.Value = parameterValue;
        SqlCmd.Parameters.Add(Parameter);
    }
    public void AddParameter(string parameterName, DateTime? parameterValue)
    {
        SqlParameter Parameter = new SqlParameter(parameterName, SqlDbType.DateTime);
        if (parameterValue == null)
            Parameter.Value = DBNull.Value;
        else
            Parameter.Value = parameterValue;
        SqlCmd.Parameters.Add(Parameter);
    }
    public void AddParameter(string parameterName, byte[] parameterValue)
    {
        SqlParameter Parameter = new SqlParameter(parameterName, SqlDbType.Image);
        if (parameterValue == null)
            Parameter.Value = DBNull.Value;
        else
            Parameter.Value = parameterValue;
        SqlCmd.Parameters.Add(Parameter);
    }
    public void AddOutputParameter(string parameterName)
    {
        SqlParameter Parameter = new SqlParameter(parameterName, SqlDbType.Int);
        Parameter.Direction = ParameterDirection.Output;
        SqlCmd.Parameters.Add(Parameter);
    }
    public void AddOutputParameter(string parameterName, decimal i)
    {
        SqlParameter Parameter = new SqlParameter(parameterName, SqlDbType.Decimal);
        Parameter.Direction = ParameterDirection.Output;
        SqlCmd.Parameters.Add(Parameter);
    }
    public string executeScalarQuery(string Query, SqlParameter[] paramValue)
    {
        SqlCon = new SqlConnection();
        using (SqlCon = new SqlConnection(connectionstring))
        {
            object val = null;
            SqlCon.Open();
            SqlTransaction Trans;
            using (SqlCommand sqlcmd = new SqlCommand(Query, SqlCon))
            {
                Trans = SqlCon.BeginTransaction();
                try
                {
                    sqlcmd.Connection = SqlCon;
                    sqlcmd.Transaction = Trans;
                    sqlcmd.CommandTimeout = 120;
                    sqlcmd.Parameters.Clear();
                    for (int i = 0; i <= paramValue.Length - 1; i++)
                    {
                        sqlcmd.Parameters.Add(paramValue[i]);
                    }
                    sqlcmd.CommandText = Query;
                    val = sqlcmd.ExecuteScalar();
                    sqlcmd.Parameters.Clear();
                    Trans.Commit();
                    if (val == null)
                    {
                        return ("");
                    }
                    else
                    {
                        return (val.ToString());
                    }
                }
                catch
                {
                    Trans.Rollback();
                    return ("");
                }
            }
        }
    }
    public void AddOutputStringParameter(string parameterName)
    {
        SqlParameter Parameter = new SqlParameter(parameterName, SqlDbType.VarChar);
        Parameter.Size = 100;
        Parameter.Direction = ParameterDirection.Output;
        SqlCmd.Parameters.Add(Parameter);
    }
    public string ExecuteScalar()
    {

        SqlCon = new SqlConnection();
        using (SqlCon = new SqlConnection(connectionstring))
        {
            try
            {
                object val = null;
                SqlCon.Open();
                SqlCmd.Connection = SqlCon;
                SqlTransaction transaction;
                transaction = SqlCon.BeginTransaction();
                try
                {
                    SqlCmd.Transaction = transaction;
                    val = SqlCmd.ExecuteScalar();
                    transaction.Commit();
                    if (val == null)
                    {
                        return ("");
                    }
                    else
                    {
                        return (val.ToString());
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ("");
                }
            }
            catch (Exception exp)
            {
                return ("");
            }
            finally
            {
                SqlCon.Close();
                SqlCon.Dispose();
            }
        }
    }

    #endregion

    public Database_Operations()
    {

    }
}
