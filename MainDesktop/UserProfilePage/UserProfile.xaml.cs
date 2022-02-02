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
using SheldueLogic;

namespace MainDesktop.UserProfilePage
{
    /// <summary>
    /// Interaction logic for UserProfile.xaml
    /// </summary>
    public partial class UserProfile : Window
    {
        public UserProfile(Sheldue sheldue)
        {
            this.sheldue = sheldue;

            InitializeComponent();

            initContent();
        }

        public void initContent()
        {
            if (sheldue.Logged)
            {
                this.Height = 300;
                this.Width = 300;
                WindowMain.Content = new ProfilePage(sheldue);
            }
            else
            {
                WindowMain.Content = new LoginPage(sheldue, this);
            }
        }

        Sheldue sheldue;

    }
}
