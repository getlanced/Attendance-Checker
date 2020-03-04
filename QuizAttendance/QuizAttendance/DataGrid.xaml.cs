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
    /// Interaction logic for DataGrid.xaml
    /// </summary>
    public partial class DataGrid : Page
    {
        public DataGrid()
        {
            InitializeComponent();

            roomSearch_dataGrid.Items.Clear();
            roomSearch_dataGrid.Items.Refresh();

            ClearItems();
        }

        private void Search_button_Click(object sender, RoutedEventArgs e)
        {
            roomSearch_dataGrid.Items.Clear();
            roomSearch_dataGrid.Items.Refresh();

            var a = new ListRoomWebService.ListRoomWebServiceSoapClient();
            string date = day_comboBox.Text + '/' + month_comboBox.Text + '/'+year_textBox.Text;
            
            using (a)
            {
                var dt = new DataTable { TableName = "Class Record" };
                var li = a.RoomList(
                    term_comboBox.Text,
                    schoolYear_textBox.Text,
                    startTime_comboBox.Text, 
                    endTime_comboBox.Text, 
                    room_textBox.Text,date
                    );
                foreach (ListRoomWebService.Student st in li)
                {
                    roomSearch_dataGrid.Items.Add(li);
                    subject_textBox.Text = st.subSec;
                }                
            }
            a = null;
        }

        private void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            roomSearch_dataGrid.Items.Clear();
            roomSearch_dataGrid.Items.Refresh();

            ClearItems();
        }

        private void ClearItems()
        {
            term_comboBox.SelectedIndex = 0;

            day_comboBox.SelectedIndex = 0;
            month_comboBox.SelectedIndex = 0;

            startTime_comboBox.SelectedIndex = 0;
            endTime_comboBox.SelectedIndex = 0;

            schoolYear_textBox.Clear();
            room_textBox.Clear();
            subject_textBox.Clear();
        }
    }
}
