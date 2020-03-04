using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for StudentSearchWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class StudentSearchWebService : System.Web.Services.WebService
{

    public StudentSearchWebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }
    [WebMethod]
    public List<Student> StudentSearch(string fullName)
    {
        var arr = new List<Student>();

        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
        var cmd = new SqlCommand("ShowStudentListBasedOnName", conn);
        cmd.Parameters.Add("@FullName", SqlDbType.NVarChar).Value = fullName;

        conn.Open();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.ExecuteNonQuery();
        var dataAdapter = new SqlDataAdapter(cmd);
        var dt = new DataTable();
        dataAdapter.Fill(dt);
        foreach (DataRow dr in dt.Rows)
        {
            var st = new Student();
            st.fullName = dr["Full Name"].ToString();
            st.studNum = dr["Student Number"].ToString();
            st.Room = dr["Room"].ToString();
            st.subSec = dr["Subject/Section"].ToString();
            st.termYear = dr["Term/Year"].ToString();
            st.date = dr["Date"].ToString();
            st.attStat = dr["AttendanceStatus"].ToString();

            arr.Add(st);
            st = null;
        }

        conn.Close();
        return arr;
    }
    [WebMethod]
    public List<Student> StudentSearchNo(string studNo)
    {
        var arr = new List<Student>();

        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
        var cmd = new SqlCommand("ShowStudentListBasedOnStudentNumber", conn);
        cmd.Parameters.Add("@StudNo", SqlDbType.NVarChar,10).Value = studNo;

        conn.Open();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.ExecuteNonQuery();
        var dataAdapter = new SqlDataAdapter(cmd);
        var dt = new DataTable();
        dataAdapter.Fill(dt);
        foreach (DataRow dr in dt.Rows)
        {
            var st = new Student();
            st.fullName = dr["Full Name"].ToString();
            st.studNum = dr["Student Number"].ToString();
            st.Room = dr["Room"].ToString();
            st.subSec = dr["Subject/Section"].ToString();
            st.termYear = dr["Term/Year"].ToString();
            st.date = dr["Date"].ToString();
            st.attStat = dr["AttendanceStatus"].ToString();

            arr.Add(st);
            st = null;
        }

        conn.Close();
        return arr;
    }

    public class Student
    {
        public string fullName { get; set; }
        public string studNum { get; set; }
        public string Room { get; set; }
        public string subSec { get; set; }
        public string termYear { get; set; }
        public string date { get; set; }
        public string attStat { get; set; }
    }

}
