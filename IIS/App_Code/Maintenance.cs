using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Maintenance
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class Maintenance : System.Web.Services.WebService
{

    public Maintenance()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public List<string> GetSchoolYears()
    {
        var arr = new List<string>();

        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
        var cmd = new SqlCommand("GetSchoolYears", conn);
        
        conn.Open();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.ExecuteNonQuery();
        var dataAdapter = new SqlDataAdapter(cmd);
        var dt = new DataTable();
        dataAdapter.Fill(dt);
        foreach (DataRow dr in dt.Rows)
        {
            arr.Add(dr["YearName"].ToString());
        }

        conn.Close();
        return arr;
    }

    [WebMethod]
    public List<string> GetSections()
    {
        var arr = new List<string>();

        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
        var cmd = new SqlCommand("GetSections", conn);

        conn.Open();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.ExecuteNonQuery();
        var dataAdapter = new SqlDataAdapter(cmd);
        var dt = new DataTable();
        dataAdapter.Fill(dt);
        foreach (DataRow dr in dt.Rows)
        {
            arr.Add(dr["SectionName"].ToString());
        }

        conn.Close();
        return arr;
    }

    [WebMethod]
    public List<string> GetRooms()
    {
        var arr = new List<string>();

        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
        var cmd = new SqlCommand("GetRooms", conn);

        conn.Open();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.ExecuteNonQuery();
        var dataAdapter = new SqlDataAdapter(cmd);
        var dt = new DataTable();
        dataAdapter.Fill(dt);
        foreach (DataRow dr in dt.Rows)
        {
            arr.Add(dr["RoomName"].ToString());
        }

        conn.Close();
        return arr;
    }

    [WebMethod]
    public List<string> GetSubjects()
    {
        var arr = new List<string>();

        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
        var cmd = new SqlCommand("GetSubjects", conn);

        conn.Open();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.ExecuteNonQuery();
        var dataAdapter = new SqlDataAdapter(cmd);
        var dt = new DataTable();
        dataAdapter.Fill(dt);
        foreach (DataRow dr in dt.Rows)
        {
            arr.Add(dr["SubjectName"].ToString());
        }

        conn.Close();
        return arr;
    }

    [WebMethod]
    public void InsertSchoolYear(string SY)
    {
        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
        var cmd = new SqlCommand("InsertSchoolYear", conn);

        conn.Open();
        cmd.Parameters.Add("@Year", SqlDbType.NChar, 9).Value = SY;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    [WebMethod]
    public void InsertRoom(string R)
    {
        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
        var cmd = new SqlCommand("InsertRoom", conn);

        conn.Open();
        cmd.Parameters.Add("@Room", SqlDbType.NVarChar, 10).Value = R;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    [WebMethod]
    public void InsertSection(string Sec)
    {
        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
        var cmd = new SqlCommand("InsertSection", conn);

        conn.Open();
        cmd.Parameters.Add("@Section", SqlDbType.NVarChar, 5).Value = Sec;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    [WebMethod]
    public void InsertSubject(string Sub)
    {
        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
        var cmd = new SqlCommand("InsertSubject", conn);

        conn.Open();
        cmd.Parameters.Add("@Subject", SqlDbType.NVarChar, 15).Value = Sub;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.ExecuteNonQuery();
        conn.Close();
    }
	
	[WebMethod]
	public void InsertClass(string rm, string sub, string sect, string year, string term, string st, string et)
	{
		var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
        var cmd = new SqlCommand("InsertClassRecord", conn);

		conn.Open();
		cmd.Parameters.Add("@roomName", SqlDbType.NVarChar,10).Value = rm;
		cmd.Parameters.Add("@subjName",SqlDbType.NVarChar, 15).Value = sub;
		cmd.Parameters.Add("@sectName",SqlDbType.NVarChar,5).Value = sect;
		cmd.Parameters.Add("@yearName", SqlDbType.NChar,9).Value = year;
		cmd.Parameters.Add("@termName", SqlDbType.NVarChar,2).Value = term;
		cmd.Parameters.AddWithValue("@startTime",TimeSpan.Parse(st));
		cmd.Parameters.AddWithValue("@endTime",TimeSpan.Parse(et));
		
		cmd.CommandType = CommandType.StoredProcedure;
		cmd.ExecuteNonQuery();
        conn.Close();
	}
	
	[WebMethod]
	public List<Class> GetClassRecord()
	{
		var classRec = new List<Class>();
		
        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
        var cmd = new SqlCommand("GetClassRecord", conn);
		
        conn.Open();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.ExecuteNonQuery();
        var dataAdapter = new SqlDataAdapter(cmd);
        var dt = new DataTable();
        dataAdapter.Fill(dt);
        foreach (DataRow dr in dt.Rows)
        {
			var c = new Class();
            c.roomName = dr["RoomName"].ToString();
			c.subName = dr["SubjectName"].ToString();
			c.sectName = dr["SectionName"].ToString();
			c.yearName = dr["YearName"].ToString();
			c.termName = dr["TermName"].ToString();
			c.startTime = dr["StartTime"].ToString();
			c.endTime = dr["EndTime"].ToString();
			
			classRec.Add(c);
			c = null;
        }

        conn.Close();
        return classRec;
	}
	
	public class Class
	{
		public string roomName {get; set;}
		public string subName {get; set;}
		public string sectName {get; set;}
		public string yearName {get; set;}
		public string termName {get; set;}
		public string startTime {get; set;}
		public string endTime {get; set;}
	}
}
