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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Main.NavigationService.Navigate(new DataGrid());
            main_button.IsEnabled = false;
        }


        private void main_Click(object sender, RoutedEventArgs e)
        {
            Main.NavigationService.Navigate(new DataGrid());
            main_button.IsEnabled = false;

            remove_button.IsEnabled = true;
            edit_button.IsEnabled = true;
        }

        private void remove_click(object sender, RoutedEventArgs e)
        {
            Main.NavigationService.Navigate(new studentSearch());
            main_button.IsEnabled = true;

            remove_button.IsEnabled = false;
            edit_button.IsEnabled = true;
        }

        private void edit_click(object sender, RoutedEventArgs e)
        {
            Main.NavigationService.Navigate(new attendanceCheck());
            main_button.IsEnabled = true;
            remove_button.IsEnabled = true;
            edit_button.IsEnabled = false;
        }
    }
}
