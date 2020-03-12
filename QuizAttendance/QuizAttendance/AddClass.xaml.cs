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
using System.Collections.ObjectModel;
using System.Data;

namespace QuizAttendance
{
    /// <summary>
    /// Interaction logic for AddClass.xaml
    /// </summary>
    public partial class AddClass : Page
    {
        public AddClass()
        {
            InitializeComponent();
            FillClassTable();
        }

        public void PopulateStartTimeComboBox()
        {
            startTime_comboBox.ItemsSource = null;
            startTime_comboBox.Items.Clear();
            string[] time = new string[9] { "07:30:00", "09:00:00", "10:30:00", "12:00:00",
            "13:30:00", "15:00:00", "16:30:00", "18:00:00", "19:30:00" };

            var time_item = new ObservableCollection<string>();
            int k = int.Parse(period_comboBox.Text)-1;
            for (int i = 0; i < 9 - k; i++)
            {
                time_item.Add(time[i]);
            }
            startTime_comboBox.ItemsSource = time_item;
            startTime_comboBox.SelectedIndex = 0;            
        }
        public void PopulateYearComboBox()
        {
            year_comboBox.ItemsSource = null;
            year_comboBox.Items.Clear();
            var a = new Maintenance.MaintenanceSoapClient();
            var year_item = new ObservableCollection<string>();
            using (a)
            {
                foreach (string b in a.GetSchoolYears())
                {
                    year_item.Add(b);
                }
            }
            a = null;
            year_comboBox.ItemsSource = year_item;
            year_comboBox.SelectedIndex = 0;
        }
        public void PopulateSectionComboBox(List<string> list)
        {
            section_comboBox.ItemsSource = null;
            section_comboBox.Items.Clear();
            
            var a = new Maintenance.MaintenanceSoapClient();
            var sect_item = new ObservableCollection<string>();
            using (a)
            {
                foreach (string b in list)
                {
                    sect_item.Add(b);
                }
            }
            a = null;
            section_comboBox.ItemsSource = sect_item;
        }
        public void PopulateRoomComboBox(List<string> list)
        {
            availableRooms_comboBox.ItemsSource = null;
            availableRooms_comboBox.Items.Clear();

            var a = new Maintenance.MaintenanceSoapClient();
            var items = new ObservableCollection<string>();
            using (a)
            {
                foreach (string b in list)
                {
                    items.Add(b);
                }
            }
            a = null;
            availableRooms_comboBox.ItemsSource = items;
        }
        private void List_subjects_button_Click(object sender, RoutedEventArgs e)
        {
            subjects_listView.Items.Clear();

            var a = new Maintenance.MaintenanceSoapClient();

            using (a)
            {
                foreach (string b in a.GetSubjects())
                {
                    subjects_listView.Items.Add(b);
                }
            }
        }
        private void ListView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var a = new Maintenance.MaintenanceSoapClient();
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                string text = subjects_listView.SelectedItems[0].ToString();
                PopulateSectionComboBox(GetAvailableSections(text));

            }
        }
        private List<string> GetAvailableSections(string t)
        {
            var a = new Maintenance.MaintenanceSoapClient();
            a.Open();
            var cr = a.GetClassRecord(); 
            var sect_list = a.GetSections();
            bool[] sect_list_exist_index = new bool[sect_list.Count];
            //populate sect_exist_index
            for (int i = 0; i < sect_list_exist_index.Length; i++)
                sect_list_exist_index[i] = false;
            //Go through each class record(O*n)
            foreach (Maintenance.Class c in cr)
            {
                //If input subject found exists on selected year and quarter
                if (c.subName == t
                    && c.termName == term_comboBox.Text
                    && c.yearName == year_comboBox.Text)
                {
                    //Check off the list from sect_list if the section is taken
                    for (int i = 0; i < sect_list.Count; i++)
                    {
                        if (sect_list[i] == c.sectName)
                            sect_list_exist_index[i] = true;
                    }
                }
            }
            //Prepare allowable sections based on sec_list_exist_index
            var allowedSections = new List<string>();
            for (int i = 0; i < sect_list.Count; i++)
            {
                if (sect_list_exist_index[i] == false)
                    allowedSections.Add(sect_list[i]);
            }
            a.Close();
            return allowedSections;
        }

        public List<string> GetAvailableRooms(string startTime)
        {
            var allowedRooms = new List<string>();
            string[] time_list = new string[startTime_comboBox.Items.Count];
            //Populate time_list
            for (int i = 0; i < time_list.Length; i++)
                time_list[i] = startTime_comboBox.Items[i].ToString();

            string subject = subjects_listView.SelectedItems[0].ToString();
            string section = section_comboBox.Text;

            var a = new Maintenance.MaintenanceSoapClient();
            a.Open();
            var cr = a.GetClassRecord();
            var rooms = a.GetRooms();
            
            int period = int.Parse(period_comboBox.Text);
            bool[] free_rooms_index = new bool[rooms.Count];

            //Populate the free rooms index
            for (int i = 0; i < free_rooms_index.Length; i++)
                free_rooms_index[i] = true;

            foreach (Maintenance.Class c in cr)
            {
                //If subject and section name already exists in the room's schedule
                if (c.subName == subject
                    && c.sectName == section
                    && c.termName == term_comboBox.Text
                    && c.yearName == year_comboBox.Text
                    )
                {
                    //Cross out room
                    for (int i = 0; i < rooms.Count; i++)
                    {
                        if(rooms[i] == c.roomName)
                            free_rooms_index[i] = false;
                    }
                }
                int  p = int.Parse(period_comboBox.Text);
                //Check if period number overlaps with other start times
                for (int i = startTime_comboBox.SelectedIndex; i < 9-p; i++)
                {
                    //Compare startTime with the combobox's index
                    if (c.startTime == time_list[i])
                    {
                        //If so, cross out room
                        for (int j = 0; j < rooms.Count; j++)
                        {
                            if (rooms[j] == c.roomName)
                                free_rooms_index[j] = false;
                        }
                    }
                }
            }
            //Prepare allowable rooms based on free_rooms_index
            for (int i = 0; i < rooms.Count; i++)
            {
                if (free_rooms_index[i] == true)
                    allowedRooms.Add(rooms[i]);
            }
            a.Close();
            return allowedRooms;
        }

        private void Period_comboBox_DropDownClosed(object sender, EventArgs e)
        {
            PopulateStartTimeComboBox();
        }

        private void AvailableRooms_comboBox_DropDownOpened(object sender, EventArgs e)
        {
            PopulateRoomComboBox(GetAvailableRooms(startTime_comboBox.Text));
        }

        private void Year_comboBox_DropDownOpened(object sender, EventArgs e)
        {
            PopulateYearComboBox();
            
        }
        private void Add_class_button_Click(object sender, RoutedEventArgs e)
        {
            //var a = new Maintenance.MaintenanceSoapClient();
            //string sub = subjects_listView.SelectedItems[0].ToString();

            //string[] end_time_list = new string[9] { "09:00:00", "10:30:00", "12:00:00",
            //"13:30:00", "15:00:00", "16:30:00", "18:00:00", "19:30:00", "21:00:00" };

            //int k = int.Parse(period_comboBox.Text);
            //string endTime = end_time_list[startTime_comboBox.SelectedIndex + k-1];
            
            //using (a)
            //{
            //    a.InsertClass(availableRooms_comboBox.Text,
            //                sub,
            //                section_comboBox.Text,
            //                year_comboBox.Text,
            //                term_comboBox.Text,
            //                startTime_comboBox.Text,
            //                endTime
            //        );
            //}
            //end_time_list = null;
            //a = null;
            //UpdateWithNewClass();
            //ResetFields();
        }

        private void FillClassTable()
        {
            var a = new Maintenance.MaintenanceSoapClient();
            using (a)
            {
                var dt = new DataTable { TableName = "Class Record" };
                var li = a.GetClassRecord();
                foreach (Maintenance.Class c in li)
                {
                    classRecord_dataGrid.Items.Add(c);
                }
            }
            a = null;
        }

        private void UpdateWithNewClass()
        {
            classRecord_dataGrid.Items.Clear();
            classRecord_dataGrid.Items.Refresh();
            FillClassTable();
        }

        private void ResetFields()
        {
            year_comboBox.ItemsSource = null;

            subjects_listView.Items.Clear();

            section_comboBox.ItemsSource = null;
            section_comboBox.Items.Clear();
            period_comboBox.SelectedIndex = 1;
            PopulateStartTimeComboBox();
            availableRooms_comboBox.ItemsSource = null;
            availableRooms_comboBox.Items.Clear();
        }
    }
}
