using SheldueLogic;
using System.Windows;

namespace MainDesktop.UserProfilePage
{
    /// <summary>
    /// Interaction logic for UserProfile.xaml
    /// </summary>
    public partial class UserProfile : Window
    {
        private Sheldue sheldue;


        public UserProfile(Sheldue sheldue)
        {
            this.sheldue = sheldue;

            InitializeComponent();

            initContent();
        }

        /// <summary>
        /// (BAD FUNCTION КОСТЫЛЬ)
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
