using System;
using System.Collections.Generic;
using System.Data;
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
        public int ControlInt = 0; // 1 = SY, 2 = Sec, 3 = R, 4 = Subj, 5 = Stud

        public maintenance()
        {
            InitializeComponent();
            StudentVisuals(Visibility.Hidden);
        }

        private void NonStudentVisuals(Visibility v)
        {
            Delete.Visibility = v;
            AddEntry.Visibility = v;
            Entry.Visibility = v;
        }

        private void StudentVisuals(Visibility v)
        {
            AddStudButton.Visibility = v;
            gnEntry.Visibility = v;
            gnLabel.Visibility = v;
            lnEntry.Visibility = v;
            lnLabel.Visibility = v;
            miLabel.Visibility = v;
            miEntry.Visibility = v;
            snEntry.Visibility = v;
            snLabel.Visibility = v;
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

        private void UpdateWithStudents()
        {
            Results.Items.Clear();

            var a = new Maintenance.MaintenanceSoapClient();

            using (a)
            {
                foreach (Maintenance.Student b in a.GetStudents())
                {
                    Results.Items.Add(string.Format("{0}    {1}",b.studNum,b.fullName));
                }
            }
        }

        private void SchoolYear_Click(object sender, RoutedEventArgs e)
        {
            UpdateWithSchoolYears();
            ControlInt = 1;
            NonStudentVisuals(Visibility.Visible);
            StudentVisuals(Visibility.Hidden);
        }

        private void Section_Click(object sender, RoutedEventArgs e)
        {
            UpdateWithSections();
            ControlInt = 2;
            NonStudentVisuals(Visibility.Visible);
            StudentVisuals(Visibility.Hidden);
        }

        private void Room_Click(object sender, RoutedEventArgs e)
        {
            UpdateWithRooms();
            ControlInt = 3;
            NonStudentVisuals(Visibility.Visible);
            StudentVisuals(Visibility.Hidden);
        }

        private void Subject_Click(object sender, RoutedEventArgs e)
        {
            UpdateWithSubjects();
            ControlInt = 4;
            NonStudentVisuals(Visibility.Visible);
            StudentVisuals(Visibility.Hidden);
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

        private void Students_Click(object sender, RoutedEventArgs e)
        {
            UpdateWithStudents();
            ControlInt = 5;
            NonStudentVisuals(Visibility.Hidden);
            StudentVisuals(Visibility.Visible);
        }

        private void AddStudButton_Click(object sender, RoutedEventArgs e)
        {
            if ((gnEntry.Text == "" && lnEntry.Text == "" && miEntry.Text == "") || snEntry.Text == "")
            {
                MessageBox.Show("At least one name field and the Student Number field should be populated.", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (snEntry.Text.Length != 10)
                {
                    MessageBox.Show("Student Number should have 10 characters.", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var a = new Maintenance.MaintenanceSoapClient();

                    a.InsertStudent(
                        lnEntry.Text.ToString(),
                        gnEntry.Text.ToString(),
                        miEntry.Text.ToString(),
                        snEntry.Text.ToString()
                        );

                    MessageBox.Show("New student added.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateWithStudents();
                    gnEntry.Text = "";
                    lnEntry.Text = "";
                    miEntry.Text = "";
                    snEntry.Text = "";
                }
            }
        }
    }
}
