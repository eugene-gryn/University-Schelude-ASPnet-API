using System;
using System.Collections.Generic;
using System.Collections;
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
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.AvalonDock;
using Notification.Wpf;
using Microsoft.Win32;
using System.Text.RegularExpressions;

using SheldueLogic;
using SheldueLogic.SheldueObj;
using System.ComponentModel;

namespace MainDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Brush LectionBrush = Brushes.LightGray;
        Brush PraticeBrush = Brushes.White;


        public static void PlayDefaultSound()
        {
            bool found = false;
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"AppEvents\Schemes\Apps\.Default\Notification.Default\.Current"))
                {
                    if (key != null)
                    {
                        Object o = key.GetValue(null); // pass null to get (Default)
                        if (o != null)
                        {
                            System.Media.SoundPlayer theSound = new System.Media.SoundPlayer((String)o);
                            theSound.Play();
                            found = true;
                        }
                    }
                }
            }
            catch
            { }
            if (!found) System.Media.SystemSounds.Beep.Play(); // consolation prize
        }


        public static void Error(string msg)
        {
            System.Windows.MessageBox.Show(msg, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public static void Notification(string message, string title, NotificationType type, int duration)
        {
            NotificationManager notification = new NotificationManager();
            var content = new NotificationContent();

            //Init content of notification
            content.Message = message;
            content.Title = title;
            content.Type = type;

            notification.Show(content, null, new TimeSpan(0, 0, duration));
        }


        bool IsTodayCouple() => NearCouple.subject.Name == (string)SubjectLable.Content;

        private void InitCoupleTimer()
        {
            var nowTime = DateTime.Now.TimeOfDay;

            NearCouple = sheldue.GetNearCouple(DateTime.Today.DayOfWeek, nowTime);
            if (String.IsNullOrEmpty(NearCouple.subject.Name))
            {
                NearCouple = sheldue.GetNearCouple(DateTime.Today.AddDays(1).DayOfWeek, new TimeSpan(0, 0, 0));
                SubjectLable.Content = DateTime.Today.AddDays(1).DayOfWeek.ToString() + " | " + NearCouple.subject.Name;
            }
            else
            {
                SubjectLable.Content = NearCouple.subject.Name;
            }

            SheldueLabel.Content = sheldue.Sheldues[sheldue.CurrentWeek].WeekName;

            CurrentCoupleBeginTimeLable.Content = NearCouple.begin.ToString(@"hh\:mm\:ss");
            CurrentCoupleEndTimeLable.Content = NearCouple.end.ToString(@"hh\:mm\:ss");
            CurrentCoupleCurrentTimeLable.Content = nowTime.ToString(@"hh\:mm\:ss");

            if (!NearCouple.subject.isPractice)
            {
                SubjectLable.Background = LectionBrush;
            }
            else
            {
                SubjectLable.Background = PraticeBrush;
            }

            if (IsTodayCouple() && NearCouple.begin < nowTime && nowTime < NearCouple.end)
            {
                double now = (double)(NearCouple.end.Ticks - nowTime.Ticks);
                double time = (double)(NearCouple.end.Ticks - NearCouple.begin.Ticks);
                double aaa = now / time;
                CoupleProgressBar.Value = 100.0 * (1.0 - aaa);
            }
            else
            {
                CoupleProgressBar.Value = 0;
            }
        }

        private void CoupleViewUpdate()
        {
            if (sheldue.Logged && sheldue.Sheldues.Count > 0)
            {
                List<CouplesView> list = new List<CouplesView>();
                var day = Sheldue.ConvertDaysOfWeek(PickSheldueDate.SelectedDate.Value.DayOfWeek);

                foreach (var couple in sheldue.Sheldues[sheldue.PlanWeek(PickSheldueDate.SelectedDate.Value)].days[(int)day].Couples)
                {
                    if (!String.IsNullOrEmpty(couple.subject.Name)) list.Add(new CouplesView(couple));
                }

                list.Sort((CouplesView a, CouplesView b) =>
                {
                    if (a.couple.begin == b.couple.begin) return 0;
                    else if (a.couple.begin > b.couple.begin) return 1;
                    else return -1;
                });

                AllCouplesListView.ItemsSource = list;
            }
        }

        private void CoupleNotification()
        {
            if (NearCouple.subject.Name != "")
            {
                if (NearCouple.isTimeBeforeCouple(DateTime.Now.TimeOfDay,
                    ref sheldue.NotificatedBeforeCouple,
                    ref sheldue.NotificatedAboutCouple,
                    ref sheldue.NotificatedHomeworkCouple
                    ))
                {
                    // Init notification
                    var notificationManager = new NotificationManager(Dispatcher);
                    var content = new NotificationContent();

                    //Init content of notification
                    content.Message = "Join couple";
                    content.Title = NearCouple.subject.Name;
                    content.Type = NotificationType.Information;

                    //Play default notification of windows
                    PlayDefaultSound();

                    // Button to open link in google meet
                    content.LeftButtonContent = "Open meet link";
                    if (NearCouple.subject.GoogleMeetUrl != null) {
                        string link = NearCouple.subject.GoogleMeetUrl;
                        content.LeftButtonAction = new Action(() =>
                        {
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = link,
                                UseShellExecute = true
                            });
                        });
                    }

                    // Show the notification
                    notificationManager.Show(content, null, new TimeSpan(0, 5, 0));
                }
                else if (NearCouple.isTimeAboutCouple(DateTime.Now.TimeOfDay,
                    ref sheldue.NotificatedBeforeCouple,
                    ref sheldue.NotificatedAboutCouple,
                    ref sheldue.NotificatedHomeworkCouple
                    ))
                {
                    // Init notification
                    var notificationManager = new NotificationManager(Dispatcher);
                    var content = new NotificationContent();

                    //Init content of notification
                    content.Message = "Join couple";
                    content.Title = NearCouple.subject.Name;
                    content.Type = NotificationType.Information;

                    //Play default notification of windows
                    PlayDefaultSound();

                    // Button to open link in google meet
                    content.LeftButtonContent = "Open meet link";
                    if (NearCouple.subject.GoogleMeetUrl != null)
                    {
                        string link = NearCouple.subject.GoogleMeetUrl;
                        content.LeftButtonAction = new Action(() =>
                        {
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = link,
                                UseShellExecute = true
                            });
                        });
                    }

                    // Show the notification
                    notificationManager.Show(content, null, TimeSpan.MaxValue);
                }
                else if (NearCouple.isTimeHomework(DateTime.Now.TimeOfDay,
                    ref sheldue.NotificatedBeforeCouple,
                    ref sheldue.NotificatedAboutCouple,
                    ref sheldue.NotificatedHomeworkCouple
                    ))
                {
                    // Add notification about add homework COMING SOON....
                }
            }
        }

        private void EraseEmptyCouples()
        {
            foreach (var sheldue in sheldue.Sheldues)
            {
                foreach (var day in sheldue.days)
                {
                    foreach (var couple in day.Couples)
                    {
                        if (couple.isEmpty()) day.Couples.Remove(couple);
                    } 
                } 
            }
            foreach (var shelduee in sheldue.Sheldues)
            {
                foreach (var day in shelduee.days)
                {
                    foreach (var couple in day.Couples)
                    {
                        if (couple.isEmpty()) day.Couples.Remove(couple);
                    } 
                } 
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                var loadedSheldue = json.LoadObj(saveFileName);
                if (loadedSheldue != null)
                {
                    sheldue = loadedSheldue;
                }
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }


            // Label Contex menu
            ContextMenu ContexMenuLabelName = new ContextMenu();
            MenuItem xMenuItem = new MenuItem();
            // init
            xMenuItem.Header = "Paste google-meet url";
            xMenuItem.Click += XMenuItem_Click;
            // add
            ContexMenuLabelName.Items.Add(xMenuItem);
            SubjectLable.ContextMenu = ContexMenuLabelName;


            // Date time picker sheldue view list
            PickSheldueDate.SelectedDate = DateTime.Today;
            AllCouplesListView.SelectionMode = SelectionMode.Single;

            everySecondTimer.Tick += EverySecondTimer_Tick;
            everySecondTimer.Interval = new TimeSpan(0, 0, 1);
            everySecondTimer.Start();
        }

        Sheldue sheldue = new Sheldue();
        Couple NearCouple = new Couple();
        bool RealClose = false;

        JsonSaveLoader json = new JsonSaveLoader();
        string saveFileName = "settings.ini";

        System.Windows.Threading.DispatcherTimer everySecondTimer = new System.Windows.Threading.DispatcherTimer();


        /// <summary>
        /// Function where code runs every second
        /// </summary>
        private void EverySecondTimer_Tick(object sender, EventArgs e)
        {
            if (sheldue.Logged && sheldue.Sheldues.Count > 0)
            {
                EraseEmptyCouples();

                // Init timer based on couple
                InitCoupleTimer();
                // Update table view couple
                var index = AllCouplesListView.SelectedIndex;
                CoupleViewUpdate();
                AllCouplesListView.SelectedIndex = index;
                // Notification Checker
                CoupleNotification();
                // Init of couple add button
                AddCoupleButton.IsEnabled = true;
            }
            else
            {
                AddCoupleButton.IsEnabled = false;
            }
        }


        private void M_MenuExit_Click(object sender, RoutedEventArgs e)
        {
            RealClose = true;
            this.Close();
        }

        private void M_MenuUserProfile_Click(object sender, RoutedEventArgs e)
        {
            UserProfilePage.UserProfile profile = new UserProfilePage.UserProfile(sheldue);
            profile.ShowInTaskbar = false;
            profile.Owner = Application.Current.MainWindow;
            profile.Show();
            this.IsEnabled = false;
            profile.Closed += Profile_Closed;
        }

        private void Profile_Closed(object sender, EventArgs e)
        {
            this.IsEnabled = true;
        }

        private void M_MenuUserLogout_Click(object sender, RoutedEventArgs e)
        {
            sheldue = new Sheldue();
        }

        private void PickShelduePastButton_Click(object sender, RoutedEventArgs e)
        {
            PickSheldueDate.SelectedDate = PickSheldueDate.SelectedDate.Value.AddDays(-1);
            CoupleViewUpdate();
        }

        private void PickSheldueFutureButton_Click(object sender, RoutedEventArgs e)
        {
            PickSheldueDate.SelectedDate = PickSheldueDate.SelectedDate.Value.AddDays(1);
            CoupleViewUpdate();
        }

        private void PickSheldueDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CoupleViewUpdate();
        }

        private void AddCoupleButton_Click(object sender, RoutedEventArgs e)
        {
            if (sheldue.Logged && sheldue.Sheldues.Count > 0)
            {
                var day = Sheldue.ConvertDaysOfWeek(PickSheldueDate.SelectedDate.Value.DayOfWeek);
                ref var CouplesList = ref sheldue.Sheldues[sheldue.CurrentWeek].days[(int)day].Couples;

                if (BeginTimePicker != null &&
                    EndTimePicker != null &&
                    BeginTimePicker.Value < EndTimePicker.Value &&
                    !String.IsNullOrEmpty(SubjectNameTextBox.Text) &&
                    !String.IsNullOrWhiteSpace(SubjectNameTextBox.Text) &&
                    isPracticeCheckBox.IsChecked != null)
                {
                    CouplesList.Add(new Couple(
                        BeginTimePicker.Value.Value.TimeOfDay,
                        EndTimePicker.Value.Value.TimeOfDay,
                        new Subject(SubjectNameTextBox.Text, (bool)isPracticeCheckBox.IsChecked)));
                }
                else
                {
                    MainWindow.Error("Wrong field!");
                }
            }
        }

        private void M_MenuBot_Click(object sender, RoutedEventArgs e)
        {
        }

        // Open tray
        private void TrayIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            this.Show();
            TrayIcon.Visibility = Visibility.Hidden;
        }

        // Close handling option
        protected override void OnClosing(CancelEventArgs e)
        {
            if (!RealClose)
            {
                e.Cancel = true;
            }

            this.Hide();
            TrayIcon.Visibility = Visibility.Visible;

            try
            {
                json.SaverObj(saveFileName, sheldue);
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }

            base.OnClosing(e);
        }

        private void CloseTrayItem_Click(object sender, RoutedEventArgs e)
        {
            RealClose = true;
            this.Close();
        }

        private void ShowTrayItem_Click(object sender, RoutedEventArgs e)
        {
            this.Show();
            TrayIcon.Visibility = Visibility.Hidden;
        }

        private void GoogleMeetMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AllCouplesListView.SelectedItems.Count > 0)
                {
                    // Gets current day of week
                    var day = Sheldue.ConvertDaysOfWeek(PickSheldueDate.SelectedDate.Value.DayOfWeek);
                    // Gets Couple list of today
                    ref var CouplesList = ref sheldue.Sheldues[sheldue.PlanWeek(PickSheldueDate.SelectedDate.Value)].days[(int)day].Couples;
                    // Gets index of selected subject in today sheldue
                    var index = CouplesList.IndexOf(((CouplesView)AllCouplesListView.SelectedItem).couple);

                    // Open dialog to change google meet url
                    Regex link = new Regex(@"(https://meet.google.com/)(\w+)");
                    string linkStr = Clipboard.GetText();
                    if (link.IsMatch(linkStr))
                    {
                        CouplesList[index].subject.GoogleMeetUrl = linkStr;
                        MainWindow.Notification("Added link: " + linkStr, "Link Added", NotificationType.Success, 5);
                    }
                    else
                    {
                        MainWindow.Notification("Wrong link: " + linkStr, "Link Wrong!", NotificationType.Error, 5);
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                Error("Element doesn't exitst anymore!");
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }

        }

        private void DeleteSubjectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AllCouplesListView.SelectedItems.Count > 0)
                {
                    // Gets current day of week
                    var day = Sheldue.ConvertDaysOfWeek(PickSheldueDate.SelectedDate.Value.DayOfWeek);
                    // Gets Couple list of today
                    ref var CouplesList = ref sheldue.Sheldues[sheldue.PlanWeek(PickSheldueDate.SelectedDate.Value)].days[(int)day].Couples;
                    // Gets index of selected subject in today sheldue
                    var index = CouplesList.IndexOf(((CouplesView)AllCouplesListView.SelectedItem).couple);

                    CouplesList.RemoveAt(index);
                    MainWindow.Notification("Removed!", "Remove", NotificationType.Success, 5);

                }
            }
            catch (IndexOutOfRangeException)
            {
                Error("Element doesn't exitst anymore!");
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        private void XMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty((string)SubjectLable.Content))
                {

                    // Gets current day of week
                    var day = Sheldue.ConvertDaysOfWeek(PickSheldueDate.SelectedDate.Value.DayOfWeek);
                    // Gets Couple list of today
                    ref var CouplesList = ref sheldue.Sheldues[sheldue.PlanWeek(PickSheldueDate.SelectedDate.Value)].days[(int)day].Couples;
                    // Gets index of selected subject in today sheldue
                    var index = CouplesList.FindIndex(0, (Couple a) =>
                    {
                        if ((string)SubjectLable.Content == a.subject.Name)
                        {
                            if (SubjectLable.Background == LectionBrush && !a.subject.isPractice) return true;
                            else if (SubjectLable.Background != LectionBrush && a.subject.isPractice) return true;
                            else return false;
                        }
                        else return false;

                    });

                    if (index > -1)
                    {
                        // Open dialog to change google meet url
                        Regex link = new Regex(@"(https://meet.google.com/)(\w+)");
                        string linkStr = Clipboard.GetText();
                        if (link.IsMatch(linkStr))
                        {
                            CouplesList[index].subject.GoogleMeetUrl = linkStr;
                            MainWindow.Notification("Added link: " + linkStr, "Link Added", NotificationType.Success, 5);
                        }
                        else
                        {
                            MainWindow.Notification("Wrong link: " + linkStr, "Link Wrong!", NotificationType.Error, 5);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }
    }
}
