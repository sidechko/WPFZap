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

namespace WPFkalendarZpisnoy
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int selectedYear = 0;
        int selectedMount = 0;
        Button[] days = new Button[31];
        DateTime selectedDateTime;
        WorkWithFile wwf = new WorkWithFile("def");
        int selectedNoteId = -1;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += loadMain;
        }

        private void loadMain(object sender, RoutedEventArgs e)
        {
            setCurrentDate();
            generateCalendar();
            updateSelectedDate();
            updateSelectedDayInfo(DateTime.Now.Day);
        }

        private void generateCalendar()
        {
            int i = 0;
            while (i < 31)
            {
                Button date = new Button();
                date.Content = i + 1;
                date.Name = "day" + i;
                date.Click += clickDate;
                Grid.SetRow(date, i / 7);
                Grid.SetColumn(date, i % 7);
                daysGrid.Children.Add(date);
                date.Style = (Style)this.Resources["dayButton"];
                days[i] = date;
                i++;
            }
        }
        
        private void updateDayCountInMount()
        {
            int dayInMonth = DateTime.DaysInMonth(selectedYear, selectedMount);
            for(int i = 27; i<31; i++)
            {
                days[i].Visibility = i < dayInMonth ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private void updateWeekend()
        {
            int dayInMount = DateTime.DaysInMonth(selectedYear, selectedMount);
            bool isThisMountAndYear = DateTime.Now.Year == selectedYear && DateTime.Now.Month == selectedMount;
            int currentDay = DateTime.Now.Day;
            List<int> _days = wwf.findNumNotedDayAtMY(selectedYear, selectedMount).ToList();
            for (int i = 0; i< 31; i++)
            {
                if (dayInMount < i+1) continue;
                if(isThisMountAndYear && currentDay == i + 1)
                {
                    days[i].Tag = "T";
                    continue;
                }
                DateTime dt = new DateTime(selectedYear, selectedMount, i+1);
                if (dt.DayOfWeek.Equals(DayOfWeek.Saturday) || dt.DayOfWeek.Equals(DayOfWeek.Sunday))
                    days[i].Tag = "W";
                else days[i].Tag = "O";
            }
            foreach(int day in _days)
            {
                days[day-1].Tag = "N";
            }
        }

        private void setCurrentDate()
        {
            DateTime now = DateTime.Now;
            this.selectedYear = now.Year;
            this.selectedMount = now.Month;
        }

        private void updateSelectedDate()
        {
            SelectedYearLabel.Content = selectedYear;
            SelectedMountLabel.Content = selectedMount;
            updateDayCountInMount();
            updateWeekend();
        }

        private void clickDate(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button)) return;
            if (!(((Button)sender).Content is int)) return;
            updateSelectedDayInfo((int)((Button)sender).Content);
            updateSelectedDayNotes((int)((Button)sender).Content);
            selectedNoteId = -1;
        }

        private void updateSelectedDayNotes(int day)
        {
            selectedDayNotes.ItemsSource = wwf.findNotesAtDateAsNote(selectedYear, selectedMount, day).OrderBy(n=>n.id).Select(d=>d.name);
        }

        private void updateSelectedDayInfo(int day)
        {
            selectedDateTime = new DateTime(selectedYear, selectedMount, day);
            selectedDate.Content = selectedDateTime.ToString("dd.MM.yyyy (ddd)");
        }

        private void moveDate(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button)) return;
            Button button = (Button)sender;
            if (!(button.Tag is string)) return;
            string tag = (string)button.Tag;
            if (!(button.Content is string)) return;
            string content = (string)button.Content;
            if (content != "+" && content != "-") return;
            bool typeOfMove = content == "+";
            if (tag == "Y") moveYear(typeOfMove);
            else if (tag == "M") moveMount(typeOfMove);
            updateSelectedDate();
        }

        private void moveMount(bool type)
        {
            if (type)
            {
                if (selectedMount == 12) { selectedMount = 1; moveYear(true); }
                else selectedMount++;
            }
            else
            {
                if (selectedMount == 1) { selectedMount = 12; moveYear(false); }
                else selectedMount--;
            }
        }

        private void moveYear(bool type)
        {
            selectedYear = type ? selectedYear + 1 : selectedYear - 1;
        }

        private void addNoteInDay(int day)
        {
            wwf.appendNewNote(selectedYear,selectedMount,day,noteName.Text, noteTextBody.Text);
            updateSelectedDayNotes(day);
        }

        private void addNote_Click(object sender, RoutedEventArgs e)
        {
            addNoteInDay(selectedDateTime.Day);
            updateWeekend();
            updateSelectedDayNotes(selectedDateTime.Day);
        }

        private void changeNote_Click(object sender, RoutedEventArgs e)
        {
            wwf.changeNote(selectedYear,selectedMount,selectedDateTime.Day, selectedNoteId, noteName.Text, noteTextBody.Text);
            updateWeekend();
            updateSelectedDayNotes(selectedDateTime.Day);

        }

        private void removeNote_Click(object sender, RoutedEventArgs e)
        {
            wwf.removeNote(selectedYear, selectedMount, selectedDateTime.Day, selectedNoteId);
            updateWeekend();
            updateSelectedDayNotes(selectedDateTime.Day);
        }

        private void selectedDayNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedNoteId = selectedDayNotes.SelectedIndex;
            var n = wwf.getNoteAtDateByIdAsNote(selectedYear, selectedMount, selectedDateTime.Day, selectedNoteId);
            if (n == null) return;
            noteName.Text = n.name;
            noteTextBody.Text = n.note;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var n in wwf.getAllNotes())
            {
                sb.Append(n.ToString());
                sb.Append("\n");
            }
            MessageBox.Show(sb.ToString());
        }
    }
}
