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

namespace QuizAttendance
{
    /// <summary>
    /// Interaction logic for maintenance.xaml
    /// </summary>
    public partial class maintenance : Page
    {
        public int ControlInt = 0; // 1 = SY, 2 = Sec, 3 = R, 4 = Subj

        public maintenance()
        {
            InitializeComponent();
        }

        private void UpdateWithSchoolYears()
        {
            Results.Items.Clear();

            var a = new Maintenance.MaintenanceSoapClient();

            using (a)
            {
                foreach (string b in a.GetSchoolYears())
                {
                    Results.Items.Add(b);
                }
            }
        }

        private void UpdateWithSections()
        {
            Results.Items.Clear();

            var a = new Maintenance.MaintenanceSoapClient();

            using (a)
            {
                foreach (string b in a.GetSections())
                {
                    Results.Items.Add(b);
                }
            }
        }

        private void UpdateWithRooms()
        {
            Results.Items.Clear();

            var a = new Maintenance.MaintenanceSoapClient();

            using (a)
            {
                foreach (string b in a.GetRooms())
                {
                    Results.Items.Add(b);
                }
            }
        }

        private void UpdateWithSubjects()
        {
            Results.Items.Clear();

            var a = new Maintenance.MaintenanceSoapClient();

            using (a)
            {
                foreach (string b in a.GetSubjects())
                {
                    Results.Items.Add(b);
                }
            }
        }

        private void SchoolYear_Click(object sender, RoutedEventArgs e)
        {
            UpdateWithSchoolYears();
            ControlInt = 1;
        }

        private void Section_Click(object sender, RoutedEventArgs e)
        {
            UpdateWithSections();
            ControlInt = 2;
        }

        private void Room_Click(object sender, RoutedEventArgs e)
        {
            UpdateWithRooms();
            ControlInt = 3;
        }

        private void Subject_Click(object sender, RoutedEventArgs e)
        {
            UpdateWithSubjects();
            ControlInt = 4;
        }

        private void AddEntry_Click(object sender, RoutedEventArgs e)
        {
            if (ControlInt == 0)
            {
                MessageBox.Show("Please choose a record to add your data to.", "WARNING", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (Results.Items.Contains(Entry.Text))
                {
                    MessageBox.Show("The item you are trying to add has already been added.", "WARNING", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (Entry.Text == "")
                {
                    MessageBox.Show("Entry box is empty.", "WARNING", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var a = new Maintenance.MaintenanceSoapClient();
                    switch (ControlInt)
                    {
                        case 1:
                            a.InsertSchoolYear(Entry.Text);
                            UpdateWithSchoolYears();
                            break;
                        case 2:
                            a.InsertSection(Entry.Text);
                            UpdateWithSections();
                            break;
                        case 3:
                            a.InsertRoom(Entry.Text);
                            UpdateWithRooms();
                            break;
                        case 4:
                            a.InsertSubject(Entry.Text);
                            UpdateWithSubjects();
                            break;
                    }
                    MessageBox.Show("Record has been added.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Entry.Text = "";
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ControlInt == 0)
            {
                MessageBox.Show("Please choose a record to delete from.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (Results.SelectedItem == null)
                {
                    MessageBox.Show("Please select an item to delete.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (MessageBox.Show("Are you sure about deleting " + Results.SelectedItem.ToString() + "?", "WARNING", MessageBoxButton.YesNo, MessageBoxImage.Information) 
                    == MessageBoxResult.Yes)
                {
                    string b = Results.SelectedItem.ToString();
                    var a = new Maintenance.MaintenanceSoapClient();
                    switch (ControlInt)
                    {
                        case 1:
                            a.DeleteSchoolYear(b);
                            UpdateWithSchoolYears(); 
                            break;
                        case 2:
                            a.DeleteSection(b);
                            UpdateWithSections();
                            break;
                        case 3:
                            a.DeleteRoom(b);
                            UpdateWithRooms();
                            break;
                        case 4:
                            a.DeleteSubject(b);
                            UpdateWithSubjects();
                            break;
                    }
                    MessageBox.Show("Successfully deleted " + b + " from data.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
