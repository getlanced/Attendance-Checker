using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for ListRoomWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class ListRoomWebService : System.Web.Services.WebService
{

    public ListRoomWebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public List<Student> RoomList(string term, string year, string stTime, string edTime, string rmName, string cDate )
    {
        var arr = new List<Student>();

        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
        var cmd = new SqlCommand("ShowStudentListBasedOnRoom", conn);


        cmd.Parameters.Add("@SchoolTermName", SqlDbType.Char, 2).Value = term;
        cmd.Parameters.Add("@SchoolYearName", SqlDbType.Char, 9).Value = year;
        cmd.Parameters.Add("@ClassStartTime", SqlDbType.Time).Value = stTime;
        cmd.Parameters.Add("@ClassEndTime",SqlDbType.Time).Value = edTime;
        cmd.Parameters.Add("@RName", SqlDbType.NVarChar,10 ).Value = rmName;
        cmd.Parameters.Add("@ClassDate", SqlDbType.Date).Value = cDate;

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
            st.fullName = dr["Full Name"].ToString();
            st.attStat = dr["AttendanceStatus"].ToString();
            st.subSec = dr["Subject/Section"].ToString();
            arr.Add(st);
            st = null;
        }

        conn.Close();
        return arr;
    }

    public class Student
    {
        public string studNum { get; set; }
        public string fullName { get; set; }
        public string attStat { get; set; }
        public string subSec { get; set; }
    }
}
