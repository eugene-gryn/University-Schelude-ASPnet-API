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

namespace MainDesktop.UserProfilePage
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage(SheldueLogic.Sheldue sheldue, UserProfile profile)
        {
            InitializeComponent();

            this.sheldue = sheldue;
            this.profile = profile;
        }

        SheldueLogic.Sheldue sheldue;

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            if (!sheldue.Login(LoginTextBox.Text, PasswordTextBox.Password)) MessageBox.Show("Error!", "Wrong password or login");
            else MessageBox.Show("Valid password!", "Successfully login", MessageBoxButton.OK, MessageBoxImage.Information);
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


        UserProfile profile;
    }
}
