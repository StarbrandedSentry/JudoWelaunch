 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

/// <summary>
/// Summary description for DBConnector
/// </summary>
public class DBConnector
{
    public static string defaultCon = ConfigurationManager.ConnectionStrings["defaultConWin"].ToString();
    public static SqlConnection con = new SqlConnection(defaultCon);
    public static SqlCommand cmd;
    public static SqlDataReader rdr;
    public static SqlDataAdapter da;
    public static DataSet ds;

	public DBConnector()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    //GET DATATABLE WITHOUT PARAMETER
    public static DataTable getTable(string query, bool stored_proc)
    {
        DataTable dt = new DataTable();

        if (stored_proc == false)
        {
            try
            {
                da = new SqlDataAdapter(query, con);
                ds = new DataSet();
                da.Fill(ds);
                dt = ds.Tables[0];
            }
            catch { }
        }
        else
        {
            try
            {
                con.Open();
                cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.StoredProcedure;
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
            }
            catch { }
            finally { con.Close(); SqlConnection.ClearPool(con); }
        }

        return dt;
    }

    //GET DATATABLE WITH PARAMETER
    public static DataTable getTableParam(string query, string[] param, string[] val)
    {
        DataTable dt = new DataTable();
        try
        {
            con.Open();
            cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            for (int x = 0; x < param.Length; x++)
            {
                cmd.Parameters.Add(new SqlParameter(param[x], val[x]));
            }

            ds = new DataSet();
            rdr = cmd.ExecuteReader();
            dt.Load(rdr);
        }
        catch { }
        finally { con.Close(); SqlConnection.ClearPool(con); }
        return dt;
    }

    //EXECUTE QUERY(INSERT/UPDATE/DELETE)
    public static void execute_query(string querySTR, string[] param, string[] val)
    {
        //Boolean ret = false;
        try
        {
            con.Open();
            cmd = new SqlCommand(querySTR, con);
            cmd.CommandType = CommandType.StoredProcedure;
            for (int x = 0; x < param.Length; x++)
            {
                cmd.Parameters.Add(new SqlParameter(param[x], val[x]));
            }

            cmd.ExecuteNonQuery();
        }
        catch { }
        finally { con.Close(); SqlConnection.ClearPool(con); }
    }
}