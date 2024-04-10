using System;
using System.Collections.Generic;
using System.IO;
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
using static zad1.AddTimerWnd;

namespace zad1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Store for clocks
        Dictionary<string, DateTime> Timers = new Dictionary<string, DateTime>();

        //stopwatch
        System.Windows.Threading.DispatcherTimer Stopwatch;

        //Tray
        System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();

        public MainWindow()
        {
            InitializeComponent();

            //Stopwatch
            Stopwatch = new System.Windows.Threading.DispatcherTimer();
            //назначение обработчика события Тик
            Stopwatch.Tick += new EventHandler(dispatcherTimer_Tick);
            //TimeSpan – переменная для хранения времени в формате часы/минуты/секунды
            Stopwatch.Interval = new TimeSpan(0, 0, 1);


            //загрузка картинки, которая будет отображаться в области уведомлений
            ni.Icon = System.Drawing.Icon.ExtractAssociatedIcon(@"Timer.ico");
            ni.Visible = true;
            //обработчик события "двойной клик" по иконке в области уведомлений
            ni.DoubleClick +=
            delegate (object sender, EventArgs args)
            {
                //показать окно
                this.Show();
                this.WindowState = WindowState.Normal;
            };
        }
        //перезапись метода обработки события сворачивания окна
        protected override void OnStateChanged(EventArgs e)
        {
            //вместо того что бы сворачивать окно, обработчик события будет его скрывать
            if (WindowState == System.Windows.WindowState.Minimized)
                this.Hide();
            base.OnStateChanged(e);
        }

        bool timerOn = false;//Переменная для проверка включен ли таймер. По умолчанию выключен
        bool anytimers = false;//Переменная для проверки

        private void AddTimerMainWPF_Click(object sender, RoutedEventArgs e)
        {
            //создание нового окна (название класса – то, что было указано при добавлении окна)
            AddTimerWnd addTimer = new AddTimerWnd();

            if (addTimer.ShowDialog() == true)
            {
                DateTime time = new DateTime(
                    addTimer.CalendarWPF.SelectedDate.Value.Year,
                    addTimer.CalendarWPF.SelectedDate.Value.Month,
                    addTimer.CalendarWPF.SelectedDate.Value.Day,

                    int.Parse(addTimer.HourWPF.Text),
                    int.Parse(addTimer.MinuteWPF.Text),
                    int.Parse(addTimer.SecondWPF.Text));

                if (Timers.ContainsKey(addTimer.TimerNameWPF.Text) == false)
                {
                    Timers.Add(addTimer.TimerNameWPF.Text, time);

                    ListBoxTimers.Items.Add(addTimer.TimerNameWPF.Text);

                    anytimers = true;
                }
                else
                {
                    MessageBox.Show("Такое имя уже есть");
                }
            }
        }

        private void ListBoxTimers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxTimers.SelectedIndex > -1)
            {
                anytimers = true;
                DateTime SelectedTime = Timers[ListBoxTimers.SelectedItem.ToString()];
                UntilTo.Content = "Отчитывать до: " + SelectedTime.ToString(@"dd\:HH\:mm\:ss");
            }
        }


        //Four buttons
        DateTime TimeInTimer = new DateTime();
        string NameCurentTimer;
        private void TimerOn_Click(object sender, RoutedEventArgs e)
        {
            if (anytimers == true)
            {
                Stopwatch.Start();
                timerOn = true;
                TimeInTimer = Timers[ListBoxTimers.SelectedItem.ToString()];
                NameCurentTimer = ListBoxTimers.SelectedItem.ToString();
            }
        }
        private void TimerOff_Click(object sender, RoutedEventArgs e)
        {
            if (anytimers == true)
            {
                if (timerOn == true)
                {
                    Stopwatch.Stop();
                    timerOn = false;
                    DateTime SelectedTime = Timers[ListBoxTimers.SelectedItem.ToString()];
                    UntilTo.Content = "Отчитывать до: " + SelectedTime.ToString(@"dd\:HH\:mm\:ss");
                    TimeLeft.Content = "";
                }
            }
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if(anytimers == true && timerOn == false) {
            AddTimerWnd addTimer = new AddTimerWnd();

                if (addTimer.ShowDialog() == true)
                {
                    Timers.Remove(ListBoxTimers.SelectedItem.ToString());
                    ListBoxTimers.Items.Remove(ListBoxTimers.SelectedItem.ToString());
                    DateTime time = new DateTime(
                        addTimer.CalendarWPF.SelectedDate.Value.Year,
                        addTimer.CalendarWPF.SelectedDate.Value.Month,
                        addTimer.CalendarWPF.SelectedDate.Value.Day,

                        int.Parse(addTimer.HourWPF.Text),
                        int.Parse(addTimer.MinuteWPF.Text),
                        int.Parse(addTimer.SecondWPF.Text));

                    if (Timers.ContainsKey(addTimer.TimerNameWPF.Text) == false)
                    {
                        Timers.Add(addTimer.TimerNameWPF.Text, time);

                        ListBoxTimers.Items.Add(addTimer.TimerNameWPF.Text);
                    }
                    else
                    {
                        MessageBox.Show("Такое имя уже есть");
                    }
                }
            }

        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxTimers.SelectedIndex > -1)
            {
                Timers.Remove(ListBoxTimers.SelectedItem.ToString());
                ListBoxTimers.Items.Remove(ListBoxTimers.SelectedItem.ToString());
                TimeLeft.Content = "";
                UntilTo.Content = "";
            }
        }

        
        //For Stopwatch
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {

            DateTime SelectedTime = TimeInTimer;
            //Вычитаем выбранное время от текущего и получаем разницу
            TimeSpan LeftTime = SelectedTime.Subtract(DateTime.Now);

            string LabelLeftTime = Timers[ListBoxTimers.SelectedItem.ToString()].ToString(@"dd\:HH\:mm\:ss");
            UntilTo.Content = "Отчитывать до: " + LabelLeftTime;


            //Обработчики для средних форматов
            /*if ((string)Format.SelectionBoxItem == @"hh\:mm\:ss")
            {
                int H = LeftTime.Days * 24 + LeftTime.Hours;
                TimeLeft.Content = "Выбран таймер: " + NameCurentTimer
                    + "\n1Осталось: " + H + ":" + LeftTime.Minutes.ToString("") + ":" + LeftTime.Seconds;
            }

            if ((string)Format.SelectionBoxItem == @"mm\:ss")
            {
                int H = LeftTime.Days * 24 + LeftTime.Hours;
                int M = H * 60 + LeftTime.Minutes;
                TimeLeft.Content = "Выбран таймер: " + NameCurentTimer
                    + "\n2Осталось: " + M + ":" + LeftTime.Seconds;
            }*/

            //Обработчики для крайних форматов
            if ((string)Format.SelectionBoxItem == @"total ss")
                TimeLeft.Content = "Выбран таймер: " + NameCurentTimer
                + "\nTОсталось: " + LeftTime.TotalSeconds.ToString(".") + " сек.";

            else TimeLeft.Content = "Выбран таймер: " + NameCurentTimer
                    + "\n4Осталось: " + LeftTime.ToString(@"dd\:hh\:mm\:ss");

            if (LeftTime <= TimeSpan.Zero)
            {
                Stopwatch.Stop();

                //Попытка вывести уведомление по верх всех окон
                Topmost = true;

                MessageBox.Show("Время настало!", "Get Up!");
                timerOn = false;
                UntilTo.Content = "Отчитывать до: " + LabelLeftTime;
            }

            //Выключаем по верх всех окон
            Topmost = false;
        }

        //For text files
        private string LinkToFile()
        {
            //создание диалога
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            //настройка параметров диалога
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            //вызов диалога
            dlg.ShowDialog();

            return dlg.FileName;
        }
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Timers.Clear();
            ListBoxTimers.Items.Clear();
            //открытие файла для чтения
            StreamReader file = new StreamReader(@LinkToFile());

            //Чтение справки в начале файла
            for (int i = 0; i < 5; i++)
            {
                file.ReadLine();
            }
            //построчное чтение файла
            while (file.ReadLine() != null)
            {
                string firststr = file.ReadLine();
                DateTime secondstr = DateTime.Parse(file.ReadLine());
                Timers.Add(firststr, secondstr);
                ListBoxTimers.Items.Add(firststr);
            }

            //закрытие файла
            file.Close();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if(anytimers == true) 
            {
                //открытие файла в который запишем строки
                using (StreamWriter outputFile = new StreamWriter(@LinkToFile()))
                {
                    int count = ListBoxTimers.Items.Count;

                    /*for (int i = 0; i < count;)*/

                    outputFile.WriteLine(
                        "Данную справку удалять НЕЛЬЗЯ!\n" +
                        "Ниже приведена строгая типизация файла.\n" +
                        "Обязательно сохранять пустые строки между будильниками. Также изменения производить стоит путем\n" +
                        "замены одних чисел на нужные, с учетом того что максимальное число в первых трех (с право на лево)\n" +
                        "парах нулей = 59. Строки с именами можно менять произвольно, не оставляя их пустыми." + "\n");

                    for (int i = 0; i < count; i++)
                    {
                        var name = ListBoxTimers.Items[i];
                        outputFile.WriteLine(name.ToString());
                        outputFile.WriteLine(Timers[name.ToString()]);
                    }
                }
            }
        }
        
    }
}
