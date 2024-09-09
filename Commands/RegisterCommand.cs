using GigTracker.Services;
using GigTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GigTracker.Commands
{
    /// <summary>
    /// Command to allow for users to register an account to the database.
    /// </summary>
    public class RegisterCommand : CommandBase
    {
        private SupabaseConnection supabaseConnection;
        private LoginViewModel _loginViewModel;
        private string username;
        private string password;
        private string user_salt;
        public RegisterCommand(LoginViewModel homeViewModel)
        {
            _loginViewModel = homeViewModel;

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            try
            {
                supabaseConnection = new SupabaseConnection();
                await supabaseConnection.InitializeClient();
            }
            catch (Exception ex)
            {
                // Handle exceptions or log them
                MessageBox.Show($"Error initializing Supabase connection: {ex.Message}");
            }
        }

        // Create account with hashed & salted password
        public override async void Execute(object? parameter)
        {
            username = _loginViewModel.Username;
            password = _loginViewModel.Password;
            user_salt = PasswordService.GenerateSaltBase64();

            // Adding unique salt to password
            password += user_salt;

            byte[] saltBytes = Convert.FromBase64String(user_salt);

            // Hash the password with the salt
            var hashedPass = PasswordService.HashPassword(password, saltBytes);


            await supabaseConnection.CreateAccount(username, hashedPass, user_salt);
        }
    }
}
