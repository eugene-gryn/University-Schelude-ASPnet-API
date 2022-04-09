using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ScheduleLogic;

namespace MainDesktop.UserProfilePage
{
    /// <summary>
    ///     Interaction logic for Page1.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private readonly UserProfile profile;
        private readonly ScheduleEngine sheldue;

        public LoginPage(ScheduleEngine sheldue, UserProfile profile)
        {
            InitializeComponent();

            this.sheldue = sheldue;
            this.profile = profile;
        }

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            if (!sheldue.Login(LoginTextBox.Text, PasswordTextBox.Password))
                MessageBox.Show("Error!", "Wrong password or login");
            else
                MessageBox.Show("Valid password!", "Successfully login", MessageBoxButton.OK,
                    MessageBoxImage.Information);

            profile.initContent();
        }

        private void TextBoxKeyEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                ButtonEnter_Click(sender, e);
            }
        }
    }
}