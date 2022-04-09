using System.Windows;
using ScheduleLogic;

namespace MainDesktop.UserProfilePage
{
    /// <summary>
    ///     Interaction logic for UserProfile.xaml
    /// </summary>
    public partial class UserProfile : Window
    {
        private readonly ScheduleEngine sheldue;


        public UserProfile(ScheduleEngine sheldue)
        {
            this.sheldue = sheldue;

            InitializeComponent();

            initContent();
        }

        /// <summary>
        ///     (BAD FUNCTION КОСТЫЛЬ)
        /// </summary>
        public void initContent()
        {
            if (sheldue.Logged)
            {
                Height = 300;
                Width = 300;
                WindowMain.Content = new ProfilePage(sheldue);
            }
            else
            {
                WindowMain.Content = new LoginPage(sheldue, this);
            }
        }
    }
}