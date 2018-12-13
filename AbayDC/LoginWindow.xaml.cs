using DedicatedClient;
using DedicatedClient.UserServiceReference;
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

namespace DedicatedCliend
{
    public partial class LoginWindow : Window
    { 

        public UserServiceClient userService;

        public LoginWindow()
        {
            InitializeComponent();
            userService = new UserServiceClient("NetTcpBinding_IUserService");
        }
        private void Login()
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            //TODO: login
            string token = userService.Login(username, password);
            if (token.Length < 1)
            {
                MessageBox.Show("Username or password is incorrect.");
                return;
            }

            MainWindow mainWindow = new MainWindow(token);
            mainWindow.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }
        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Login();
            }
        }
    }
}
