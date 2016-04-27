using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GryphonSecurity_v2_2.Domain;
using GryphonSecurity_v2_2.Domain.Entity;
using GryphonSecurity_v2_2.Resources;

namespace GryphonSecurity_v2_2
{
    public partial class RegisterLayout : PhoneApplicationPage
    {
        Controller controller = Controller.Instance;
        User user;
        long userId;
        String userFirstname;
        String userLastname;
        public RegisterLayout()
        {
            InitializeComponent();
            textBoxUserFirstname.Visibility = Visibility.Collapsed;
            textBoxUserLastname.Visibility = Visibility.Collapsed;
        }

        private void RegistrerBrugerButton_Click(object sender, RoutedEventArgs e)
        {
            if (!controller.checkNetworkConnection())
            {
                if (!checkUser())
                {
                    MessageBox.Show(AppResources.UserFillSpaces);
                }
                else
                {
                    userId = Convert.ToInt64(textBoxUserId.Text);
                    userFirstname = textBoxUserFirstname.Text;
                    userLastname = textBoxUserLastname.Text;
                    createUserLocalStorage(userId, userFirstname, userLastname);
                }
            }
            else
            {
                createUserLocalStorage(user.Id, user.Firstname, user.Lastname);
            }

        }

        public Boolean checkUser()
        {
            Boolean check = true;
            if (textBoxUserId.Equals(""))
            {
                check = false;
            }
            if (textBoxUserFirstname.Text.Equals(""))
            {
                check = false;
            }
            if (textBoxUserLastname.Text.Equals(""))
            {
                check = false;
            }
            return check;
        }

        public void createUserLocalStorage(long userId, String userFirstname, String userLastname)
        {
            try
            {
                User localUser = new User(userId, userFirstname, userLastname);
                if (controller.createUser(localUser))
                {
                    MessageBox.Show(AppResources.UserCreated);
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show(AppResources.UserNotCreated);
                }

            }
            catch (Exception)
            {
                MessageBox.Show(AppResources.UserRegistrationError);
            }
        }

        private async void SearchForUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (controller.checkNetworkConnection())
            {
                long id = Convert.ToInt64(textBoxUserId.Text);
                user = await controller.getUser(id);
                if (!object.ReferenceEquals(user, null))
                {
                    Firstname.Text = AppResources.UserFirstname + " " + user.Firstname;
                    Lastname.Text = AppResources.UserLastname + " " + user.Lastname;
                }
                else
                {
                    MessageBox.Show(AppResources.UserNotFound);
                }
            }
            else
            {
                MessageBox.Show(AppResources.NoNetWorkConnection);
                textBoxUserFirstname.Visibility = Visibility.Visible;
                textBoxUserLastname.Visibility = Visibility.Visible;
            }

        }
    }
}