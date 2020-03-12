using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;

namespace QuizAttendance
{
    /// <summary>
    /// Interaction logic for attendanceCheck.xaml
    /// </summary>
    public partial class attendanceCheck : Page
    {
        public attendanceCheck()
        {
            InitializeComponent();
        }

        private void Search_button_Click(object sender, RoutedEventArgs e)
        {
            checkAttendance_dataGrid.Items.Clear();
            checkAttendance_dataGrid.Items.Refresh();

            var a = new ListEnrolledStudents.ListEnrolledStudentsSoapClient();
            string date = month_comboBox.Text + '/' + day_comboBox.Text + '/' + year_textBox.Text;


            using (a)
            {

                var dt = new DataTable { TableName = "Attendance Sheet" };
                var li = a.ShowEnrolledStudents(
                    term_comboBox.Text,
                    schoolYear_textBox.Text,
                    startTime_comboBox.Text,
                    endTime_comboBox.Text,
                    room_textBox.Text
                    );
                foreach (ListEnrolledStudents.Student st in li)
                {
                    checkAttendance_dataGrid.Items.Add(st);
                }
            }
            a = null;
        }
        
        private void Submit_button_Click(object sender, RoutedEventArgs e)
        {
            var a = new ListEnrolledStudents.ListEnrolledStudentsSoapClient();
            string date = month_comboBox.Text + '/' + day_comboBox.Text + '/' + year_textBox.Text;
            var IsExisting = a.CheckExistingRecord(
                room_textBox.Text,
                date,
                startTime_comboBox.Text,
                endTime_comboBox.Text);
            using (a)
            {
                foreach (ListEnrolledStudents.Student l in checkAttendance_dataGrid.Items)
                {
                    if(IsExisting == true)
                    {
                        a.UpdateAttendanceRecord(
                       date,
                       l.studNum,
                       l.studAtt
                       );
                    }
                    else
                    {
                        

                        a.InsertToAttendanceRecord(
                       term_comboBox.Text,
                       year_textBox.Text,
                       subSec_textBox.Text,
                       date,
                       l.studNum,
                       l.studAtt
                       );
                    }
                   
                   
                }
            }
            a = null;

            checkAttendance_dataGrid.Items.Clear();
            checkAttendance_dataGrid.Items.Refresh();
        }
    }
}
