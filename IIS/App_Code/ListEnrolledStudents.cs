using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
/// <summary>
/// Summary description for ListEnrolledStudents
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class ListEnrolledStudents : System.Web.Services.WebService
{

    public ListEnrolledStudents()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public List<Student> ShowEnrolledStudents(string term, string year, string stTime, string edTime, string rmName)
    {
        var arr = new List<Student>();

        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
        var cmd = new SqlCommand("ListClassRecord", conn);


        cmd.Parameters.Add("@SchoolTermName", SqlDbType.Char, 2).Value = term;
        cmd.Parameters.Add("@SchoolYearName", SqlDbType.Char, 9).Value = year;
        cmd.Parameters.Add("@ClassStartTime", SqlDbType.Time).Value = stTime;
        cmd.Parameters.Add("@ClassEndTime", SqlDbType.Time).Value = edTime;
        cmd.Parameters.Add("@RoomName", SqlDbType.NVarChar, 10).Value = rmName;

        conn.Open();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.ExecuteNonQuery();
        var dataAdapter = new SqlDataAdapter(cmd);
        var dt = new DataTable();
        dataAdapter.Fill(dt);
        foreach (DataRow dr in dt.Rows)
        {
            var st = new Student();
            st.studNum = dr["Student Number"].ToString();
            st.studName = dr["Full Name"].ToString();
            st.studAtt = false;
            arr.Add(st);
            st = null;
        }

        conn.Close();
        return arr;
    }
    [WebMethod]
    public void UpdateAttendanceRecord(string date, string studNum, bool att)
    {
        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
        conn.Open();
        var cmd = new SqlCommand("UpdateExistingAttendanceRecord", conn);
        cmd.Parameters.Add("@studNum", SqlDbType.Char, 10).Value = studNum;
        cmd.Parameters.Add("@date", SqlDbType.Date).Value = date;
        if (att)
            cmd.Parameters.Add("@att", SqlDbType.TinyInt).Value = 1;
        else
            cmd.Parameters.Add("@att", SqlDbType.TinyInt).Value = 2;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.ExecuteNonQuery();
        conn.Close();
    }
    [WebMethod]
    public void InsertToAttendanceRecord(string term, string year, string subSec, string date, string studNum, bool att)
    {
        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
        conn.Open();

        var cmd = new SqlCommand("InsertToAttendanceRecord", conn);
        cmd.Parameters.Add("@term", SqlDbType.NVarChar, 2).Value = term;
        cmd.Parameters.Add("@year", SqlDbType.NVarChar, 9).Value = year;
        cmd.Parameters.Add("@subSec", SqlDbType.NVarChar, 20).Value = subSec;
        cmd.Parameters.Add("@date", SqlDbType.Date).Value = date;
        cmd.Parameters.Add("@studNum", SqlDbType.Char, 10).Value = studNum;
        if (att)
            cmd.Parameters.Add("@att", SqlDbType.TinyInt).Value = 1;
        else
            cmd.Parameters.Add("@att", SqlDbType.TinyInt).Value = 2;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    [WebMethod]
    public bool CheckExistingRecord(string Class, string date, string endTime, string startTime)
    {
        bool IsExisting = false;
        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
        conn.Open();

        var cmd = new SqlCommand("CheckExistingAttendanceRecord", conn);
        cmd.Parameters.Add("@room", SqlDbType.NVarChar, 10).Value = Class;
        cmd.Parameters.Add("@date", SqlDbType.Date).Value = date;
        cmd.Parameters.Add("@endTime", SqlDbType.Time, 7).Value = endTime;
        cmd.Parameters.Add("@startTime", SqlDbType.Time, 7).Value = startTime;

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.ExecuteNonQuery();

        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            if (dr.GetString(1).ToString() == null)
            {
                conn.Close();
                IsExisting = false;

            }

        }
        else
        {
            conn.Close();
            IsExisting = true;

        }

        return IsExisting;
    }

    public class Student
    {
        public string studNum { get; set; }
        public string studName { get; set; }
        public bool studAtt { get; set; }
    }
}
