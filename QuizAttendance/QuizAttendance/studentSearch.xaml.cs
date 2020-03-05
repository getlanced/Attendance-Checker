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
    /// Interaction logic for studentSearch.xaml
    /// </summary>
    public partial class studentSearch : Page
    {
        public studentSearch()
        {
            InitializeComponent();
            searchStudent_dataGrid.Items.Clear();
            searchStudent_dataGrid.Items.Refresh();

        }

        private void FullName_search_button_Click(object sender, RoutedEventArgs e)
        {
            ClearTable_button_Click(sender, e);
            var a = new StudentSearchWebService.StudentSearchWebServiceSoapClient();
            using (a)
            {
                var dt = new DataTable { TableName = "Student Record" };
                var li = a.StudentSearch(fullName_textBox.Text);
                foreach (StudentSearchWebService.Student st in li)
                    searchStudent_dataGrid.Items.Add(st);
            }
            a = null;
            fullName_textBox.Clear();
        }

        private void StudNumber_search_button_Click(object sender, RoutedEventArgs e)
        {
            ClearTable_button_Click(sender,e);
            StudentSearchWebService.StudentSearchWebServiceSoapClient a = new StudentSearchWebService.StudentSearchWebServiceSoapClient();
            using (a)
            {
                var dt = new DataTable { TableName = "Student Record" };
                var li = a.StudentSearchNo(studentNumber_textBox.Text);
                foreach (StudentSearchWebService.Student st in li)
                    searchStudent_dataGrid.Items.Add(li);
            }
            a = null;
            studentNumber_textBox.Clear();
        }

        private void ClearTable_button_Click(object sender, RoutedEventArgs e)
        {
            searchStudent_dataGrid.Items.Clear();
            searchStudent_dataGrid.Items.Refresh();
        }
    }
}
