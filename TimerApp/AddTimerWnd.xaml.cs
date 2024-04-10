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
using System.Windows.Shapes;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace zad1
{
    /// <summary>
    /// Логика взаимодействия для AddTimerWnd.xaml
    /// </summary>
    public partial class AddTimerWnd : Window
    {
        public AddTimerWnd()
        {
            InitializeComponent();
            HourWPF.Text = DateTime.Now.Hour.ToString();
            MinuteWPF.Text = DateTime.Now.AddMinutes(1).Minute.ToString();
            CalendarWPF.DisplayDateStart = DateTime.Now;
            CalendarWPF.SelectedDate = DateTime.Now;
        }
        public void AddTimer_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        public void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = false;
        }
    }
}
