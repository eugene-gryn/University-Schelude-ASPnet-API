using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Win32;
using Notification.Wpf;
using ScheduleLogic;
using ScheduleLogic.Subject;
using UserProfile = MainDesktop.UserProfilePage.UserProfile;

namespace MainDesktop
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer everySecondTimer = new DispatcherTimer();
        private JsonSaveLoader json = new JsonSaveLoader();
        private Brush LectionBrush = Brushes.LightGray;
        private Couple NearCouple = new Couple();
        private Brush PraticeBrush = Brushes.White;
        private bool RealClose = false;
        private string saveFileName = "settings.ini";

        private Schedule sheldue = new Schedule();

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                Schedule loadedSheldue = json.LoadObj(saveFileName);
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
            MenuItem xMenuItem = new MenuItem
            {
                // init
                Header = "Paste google-meet url"
            };
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


        public static void PlayDefaultSound()
        {
            bool found = false;
            try
            {
                using (RegistryKey key =
                       Registry.CurrentUser.OpenSubKey(
                           @"AppEvents\Schemes\Apps\.Default\Notification.Default\.Current"))
                {
                    if (key != null)
                    {
                        object o = key.GetValue(null); // pass null to get (Default)
                        if (o != null)
                        {
                            SoundPlayer theSound = new SoundPlayer((string) o);
                            theSound.Play();
                            found = true;
                        }
                    }
                }
            }
            catch
            {
            }

            if (!found)
            {
                SystemSounds.Beep.Play(); // consolation prize
            }
        }

        public static void Error(string msg)
        {
            MessageBox.Show(msg, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Notification(string message, string title, NotificationType type, int duration)
        {
            NotificationManager notification = new NotificationManager();
            NotificationContent content = new NotificationContent
            {
                //Init content of notification
                Message = message,
                Title = title,
                Type = type
            };

            notification.Show(content, null, new TimeSpan(0, 0, duration));
        }

        private bool IsTodayCouple()
        {
            return NearCouple.subject.Name == (string) SubjectLable.Content;
        }

        /// <summary>
        ///     Method which update timer (НЕ ПРАВИЛЬНО, ПОТОМУ ЧТО НАДО ЭТО ВСЕ ПЕРЕНЕСТИ КАК-ТО В БИЗНЕС ЛОГИКУ)
        /// </summary>
        private void InitCoupleTimer()
        {
            TimeSpan nowTime = DateTime.Now.TimeOfDay;

            NearCouple = sheldue.GetNearCouple(DateTime.Today.DayOfWeek, nowTime);
            if (string.IsNullOrEmpty(NearCouple.subject.Name))
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
                double now = NearCouple.end.Ticks - nowTime.Ticks;
                double time = NearCouple.end.Ticks - NearCouple.begin.Ticks;
                double aaa = now / time;
                CoupleProgressBar.Value = 100.0 * (1.0 - aaa);
            }
            else
            {
                CoupleProgressBar.Value = 0;
            }
        }

        /// <summary>
        ///     Update couple list view ()
        /// </summary>
        private void CoupleViewUpdate()
        {
            if (sheldue.Logged && sheldue.Sheldues.Count > 0)
            {
                List<CouplesView> list = new List<CouplesView>();
                DaysOfWeek day = Schedule.ConvertDaysOfWeek(PickSheldueDate.SelectedDate.Value.DayOfWeek);

                foreach (Couple couple in sheldue.Sheldues[sheldue.PlanWeek(PickSheldueDate.SelectedDate.Value)]
                             .days[(int) day].Couples)
                {
                    if (!string.IsNullOrEmpty(couple.subject.Name))
                    {
                        list.Add(new CouplesView(couple));
                    }
                }

                list.Sort((CouplesView a, CouplesView b) =>
                {
                    if (a.couple.begin == b.couple.begin)
                    {
                        return 0;
                    }
                    else if (a.couple.begin > b.couple.begin)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                });

                AllCouplesListView.ItemsSource = list;
            }
        }

        /// <summary>
        ///     Notyfi about couple (ЭТО НЕ ПРАВИЛЬНО НУЖНО ЧЕРЕЗ БИЗНЕС ЛОГИКУ ЭТО ВСЕ ОБРАБАТЫВАТЬ)
        /// </summary>
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
                    NotificationManager notificationManager = new NotificationManager(Dispatcher);
                    NotificationContent content = new NotificationContent
                    {
                        //Init content of notification
                        Message = "Join couple",
                        Title = NearCouple.subject.Name,
                        Type = NotificationType.Information
                    };

                    //Play default notification of windows
                    PlayDefaultSound();

                    // Button to open link in google meet
                    content.LeftButtonContent = "Open meet link";
                    if (NearCouple.subject.GoogleMeetUrl != null)
                    {
                        string link = NearCouple.subject.GoogleMeetUrl;
                        content.LeftButtonAction = new Action(() =>
                        {
                            Process.Start(new ProcessStartInfo
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
                    NotificationManager notificationManager = new NotificationManager(Dispatcher);
                    NotificationContent content = new NotificationContent
                    {
                        //Init content of notification
                        Message = "Join couple",
                        Title = NearCouple.subject.Name,
                        Type = NotificationType.Information
                    };

                    //Play default notification of windows
                    PlayDefaultSound();

                    // Button to open link in google meet
                    content.LeftButtonContent = "Open meet link";
                    if (NearCouple.subject.GoogleMeetUrl != null)
                    {
                        string link = NearCouple.subject.GoogleMeetUrl;
                        content.LeftButtonAction = new Action(() =>
                        {
                            Process.Start(new ProcessStartInfo
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

        /// <summary>
        ///     Erase empty couples (ЭТО КОСТЫЛЬ, НУЖНО УЗНАТЬ ГДЕ МАССИВ ЗАСОРЯЕТСЯ)
        /// </summary>
        private void EraseEmptyCouples()
        {
            for (int i = 0; i < sheldue.Sheldues.Count; i++)
            {
                SubjectWeek shelduee = sheldue.Sheldues[i];
                for (int i1 = 0; i1 < shelduee.days.Length; i1++)
                {
                    Day day = shelduee.days[i1];
                    day.Couples.RemoveAll((Couple couple) => couple.isEmpty());
                }
            }
        }

        /// <summary>
        ///     Function where code runs every second
        /// </summary>
        private void EverySecondTimer_Tick(object sender, EventArgs e)
        {
            if (sheldue.Logged && sheldue.Sheldues.Count > 0)
            {
                EraseEmptyCouples();

                // Init timer based on couple
                InitCoupleTimer();
                // Update table view couple
                int index = AllCouplesListView.SelectedIndex;
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
            Close();
        }

        private void M_MenuUserProfile_Click(object sender, RoutedEventArgs e)
        {
            UserProfile profile = new UserProfile(sheldue)
            {
                ShowInTaskbar = false,
                Owner = Application.Current.MainWindow
            };
            profile.Show();
            IsEnabled = false;
            profile.Closed += Profile_Closed;
        }

        private void Profile_Closed(object sender, EventArgs e)
        {
            IsEnabled = true;
        }

        private void M_MenuUserLogout_Click(object sender, RoutedEventArgs e)
        {
            sheldue = new Schedule();
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
                DaysOfWeek day = Schedule.ConvertDaysOfWeek(PickSheldueDate.SelectedDate.Value.DayOfWeek);
                ref List<Couple> CouplesList = ref sheldue.Sheldues[sheldue.CurrentWeek].days[(int) day].Couples;

                if (BeginTimePicker != null &&
                    EndTimePicker != null &&
                    BeginTimePicker.Value < EndTimePicker.Value &&
                    !string.IsNullOrEmpty(SubjectNameTextBox.Text) &&
                    !string.IsNullOrWhiteSpace(SubjectNameTextBox.Text) &&
                    isPracticeCheckBox.IsChecked != null)
                {
                    CouplesList.Add(new Couple(
                        BeginTimePicker.Value.Value.TimeOfDay,
                        EndTimePicker.Value.Value.TimeOfDay,
                        new Subject(SubjectNameTextBox.Text, (bool) isPracticeCheckBox.IsChecked)));
                }
                else
                {
                    Error("Wrong field!");
                }
            }
        }

        private void M_MenuBot_Click(object sender, RoutedEventArgs e)
        {
        }

        // Open tray
        private void TrayIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            Show();
            TrayIcon.Visibility = Visibility.Hidden;
        }

        // Close handling option
        protected override void OnClosing(CancelEventArgs e)
        {
            if (!RealClose)
            {
                e.Cancel = true;
            }

            Hide();
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
            Close();
        }

        private void ShowTrayItem_Click(object sender, RoutedEventArgs e)
        {
            Show();
            TrayIcon.Visibility = Visibility.Hidden;
        }

        private void GoogleMeetMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AllCouplesListView.SelectedItems.Count > 0)
                {
                    // Gets current day of week
                    DaysOfWeek day = Schedule.ConvertDaysOfWeek(PickSheldueDate.SelectedDate.Value.DayOfWeek);
                    // Gets Couple list of today
                    ref List<Couple> CouplesList = ref sheldue
                        .Sheldues[sheldue.PlanWeek(PickSheldueDate.SelectedDate.Value)].days[(int) day].Couples;
                    // Gets index of selected subject in today sheldue
                    int index = CouplesList.IndexOf(((CouplesView) AllCouplesListView.SelectedItem).couple);

                    // Open dialog to change google meet url
                    Regex link = new Regex(@"(https://meet.google.com/)(\w+)");
                    string linkStr = Clipboard.GetText();
                    if (link.IsMatch(linkStr))
                    {
                        CouplesList[index].subject.GoogleMeetUrl = linkStr;
                        Notification("Added link: " + linkStr, "Link Added", NotificationType.Success, 5);
                    }
                    else
                    {
                        Notification("Wrong link: " + linkStr, "Link Wrong!", NotificationType.Error, 5);
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
                    DaysOfWeek day = Schedule.ConvertDaysOfWeek(PickSheldueDate.SelectedDate.Value.DayOfWeek);
                    // Gets Couple list of today
                    ref List<Couple> CouplesList = ref sheldue
                        .Sheldues[sheldue.PlanWeek(PickSheldueDate.SelectedDate.Value)].days[(int) day].Couples;
                    // Gets index of selected subject in today sheldue
                    int index = CouplesList.IndexOf(((CouplesView) AllCouplesListView.SelectedItem).couple);

                    CouplesList.RemoveAt(index);
                    Notification("Removed!", "Remove", NotificationType.Success, 5);
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
                if (!string.IsNullOrEmpty((string) SubjectLable.Content))
                {
                    // Gets current day of week
                    DaysOfWeek day = Schedule.ConvertDaysOfWeek(PickSheldueDate.SelectedDate.Value.DayOfWeek);
                    // Gets Couple list of today
                    ref List<Couple> CouplesList = ref sheldue
                        .Sheldues[sheldue.PlanWeek(PickSheldueDate.SelectedDate.Value)].days[(int) day].Couples;
                    // Gets index of selected subject in today sheldue
                    int index = CouplesList.FindIndex(0, (Couple a) =>
                    {
                        if ((string) SubjectLable.Content == a.subject.Name)
                        {
                            if (SubjectLable.Background == LectionBrush && !a.subject.isPractice)
                            {
                                return true;
                            }
                            else if (SubjectLable.Background != LectionBrush && a.subject.isPractice)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    });

                    if (index > -1)
                    {
                        // Open dialog to change google meet url
                        Regex link = new Regex(@"(https://meet.google.com/)(\w+)");
                        string linkStr = Clipboard.GetText();
                        if (link.IsMatch(linkStr))
                        {
                            CouplesList[index].subject.GoogleMeetUrl = linkStr;
                            Notification("Added link: " + linkStr, "Link Added", NotificationType.Success, 5);
                        }
                        else
                        {
                            Notification("Wrong link: " + linkStr, "Link Wrong!", NotificationType.Error, 5);
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