using System;
using Microsoft.Win32;
using DVLD_Buisness;
using System.Windows.Forms;

namespace DVLD.Classes
{
    internal static class clsGlobal
    {
        public static clsUser CurrentUser;

        private const string RegistryPath = @"SOFTWARE\DVLDApp\Login";

        public static bool RememberUsernameAndPassword(string Username, string Password)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(RegistryPath);

                if (string.IsNullOrEmpty(Username))
                {
                    Registry.CurrentUser.DeleteSubKeyTree(RegistryPath, false);
                    return true;
                }

                key.SetValue("Username", Username);
                key.SetValue("Password", Password);
                key.Close();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public static bool GetStoredCredential(ref string Username, ref string Password)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath);

                if (key != null)
                {
                    Username = key.GetValue("Username")?.ToString() ?? "";
                    Password = key.GetValue("Password")?.ToString() ?? "";
                    key.Close();
                    return true;
                }
                else
                {
                    Username = "";
                    Password = "";
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
        }
    }
}
