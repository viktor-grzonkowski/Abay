using DedicatedCliend.UserServiceReference;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace DedicatedClient
{
    public partial class LoginWindow : Window
    {
        private UserServiceClient userService;

        public LoginWindow()
        {
            InitializeComponent();
            userService = new UserServiceClient("NetTcpBinding_IUserService");
        }

        private void Login()
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;
            User user = null;

            try
            {
                user = userService.Login(username, password);
            }
            catch (System.Exception)
            {
                MessageBox.Show("No connenction to the Host. Try again later!");
                return;
            }
            
            if (user == null)
            {
                MessageBox.Show("Username or password is incorrect.");
                return;
            }

            if (!user.Admin)
            {
                MessageBox.Show("Not sufficient rights.");
                return;
            }

            MainWindow mainWindow = new MainWindow(user);
            mainWindow.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }
        private void txtField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Login();
            }
        }
    }
}
