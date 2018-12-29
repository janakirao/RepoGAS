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
using GASLibrary;

namespace WpfApp8
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GASDBEntities db = new GASDBEntities("metadata=res://*/BSASmodel.csdl|res://*/BSASmodel.ssdl|res://*/BSASmodel.msl;provider=System.Data.SqlClient;provider connection string='data source=192.168.178.48;initial catalog=BSASdatabase;user id=BSASuser;password=Tejith2015;pooling=False;MultipleActiveResultSets=True;App=EntityFramework'");

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            User validatedUser = new User();
            bool login = false;
            string currentUser = txtUserName.Text;
            string currentPassword = txtPasswordbox.Password;
            bool credentialsValidated = ValidateUserInput(currentUser, currentPassword);

            try
            {

                if (credentialsValidated)
                {
                    validatedUser = GetUserRecord(currentUser, currentPassword);

                    if (validatedUser.UserId > 0)
                    {
                        login = true;
                    }

                    else
                    {
                        MessageBox.Show("The credentials you entered does not exist in database, Please check  and try again", "User login", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                else
                {
                    MessageBox.Show("Error with your UserName or Password. Please check  and try again", "User login", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (login)
                {
                    CreateLogEntry("Login", "User logged in successfully", validatedUser.UserId, validatedUser.UserName);

                    if (validatedUser.LevelId == 3)
                    {
                        ManagerDashboard MD = new ManagerDashboard();
                        MD.user = validatedUser;
                        MD.ShowDialog();
                        this.Hide();

                    }

                    if (validatedUser.LevelId == 2)
                    {
                        StaffDashboard SD = new StaffDashboard();
                        SD.user = validatedUser;
                        SD.ShowDialog();
                        this.Hide();
                    }

                    if (validatedUser.LevelId == 1)
                    {
                        UserDashboard UD = new UserDashboard();
                        UD.user = validatedUser;
                        UD.ShowDialog();
                        this.Hide();
                    }
                }

                else
                {
                    CreateLogEntry("Login", "User did not Login", validatedUser.UserId, validatedUser.Username);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("exception", ex.Message);
                throw;
            }

        }

        private void CreateLogEntry(string category, string description, int userId, string userName)
        {
            string comment = $"{description} user credentials = {userName}";
            Log log = new Log();
            log.UserId = userId;
            log.Categery = category;
            log.Description = comment;
            log.DateTime = DateTime.Now;

        }

        private void SaveLog(Log log)
        {
            db.Entry(log).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();
        }

        /// <summary>
        /// validates user crentials against those in sql database
        /// </summary>
        /// <param name="username">
        /// Username entered by the user
        /// </param>
        /// <param name="password">
        /// password entered by user
        /// </param>
        /// <returns>
        /// validated user
        /// </returns>
        private bool ValidateUserInput(string username, string password)
        {
            bool validated = true;
            try
            {
                if (username.Length == 0 || username.Length > 30)
                {
                    validated = false;
                }

                // check each character in username string , this assumes numbers are not allowed in username string
                foreach (char ch in username)
                {
                    if (ch >= '0' || ch <= '9')
                    {
                        validated = false;
                    }
                }

                // password must exist and should not be longer than 30 characters
                if (password.Length == 0 || password.Length > 30)
                {
                    validated = false;
                }
            }

            catch (Exception)
            {
                validated = false;
            }
            return validated;

        }


        /// <summary>
        /// validates user credentials against those in sql database
        /// </summary>
        /// <param name="username">
        /// Username entered by the user
        /// </param>
        /// <param name="password">
        /// Password entered by user
        /// </param>
        /// <returns>
        /// validated user
        /// </returns>
        private User GetUserRecord(string username, string password)
        {
            // get username and password passed to the method from the Users table in SQL database

            User validatedUser = new User();
            try
            {
                foreach (var user in db.User.Where(t => t.Username == username && t.Password == password))
                {
                    validatedUser = user;
                }
            }

            catch (EntityException ex)
            {
                MessageBox.Show("Problem connecting to sql server, Application will now close. see exception" + ex.InnerException, "Connect to database", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                Environment.Exit(0);
                throw;
            }
            return validatedUser;
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }

        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
