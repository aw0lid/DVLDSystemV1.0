using System;
using System.Data;
using DVLD_DataAccess;
using DVLDBackend;

namespace DVLD_Buisness
{
    public class clsUser
    {
        public enum enMode { AddNew, Update }
        public enMode Mode { get; private set; } = enMode.AddNew;

        public int UserID { get; private set; } = -1;
        public int PersonID { get; set; } = -1;
        public clsPerson PersonInfo { get; private set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        private string _originalPassword = string.Empty;

        public clsUser() { Mode = enMode.AddNew; }

        private clsUser(int userID, int personID, string userName, string password, bool isActive)
        {
            UserID = userID;
            PersonID = personID;
            PersonInfo = clsPerson.Find(personID);
            UserName = userName;
            Password = password;
            _originalPassword = password;
            IsActive = isActive;
            Mode = enMode.Update;
        }

        private bool _AddNewUser()
        {
            Password = clsCryptographyLibrary.ComputeSHA256Hash(Password);

            UserID = clsUserData.AddNewUser(PersonID, UserName, Password, IsActive);

            return (UserID != -1);
        }

        private bool _UpdateUser()
        {
            if (Password != _originalPassword)
            {
                Password = clsCryptographyLibrary.ComputeSHA256Hash(Password);
            }

            return clsUserData.UpdateUser(UserID, PersonID, UserName, Password, IsActive);
        }

        public static clsUser FindByUserID(int userID)
        {
            int personID = -1;
            string userName = "", password = "";
            bool isActive = false;

            bool isFound = clsUserData.GetUserInfoByUserID(userID, ref personID, ref userName, ref password, ref isActive);

            return isFound ? new clsUser(userID, personID, userName, password, isActive) : null;
        }

        public static clsUser FindByPersonID(int personID)
        {
            int userID = -1;
            string userName = "", password = "";
            bool isActive = false;

            bool isFound = clsUserData.GetUserInfoByPersonID(personID, ref userID, ref userName, ref password, ref isActive);

            return isFound ? new clsUser(userID, personID, userName, password, isActive) : null;
        }

        public static clsUser FindByUsernameAndPassword(string userName, string password)
        {
            int userID = -1, personID = -1;
            bool isActive = false;

            string hashedPassword = clsCryptographyLibrary.ComputeSHA256Hash(password);

            bool isFound = clsUserData.GetUserInfoByUsernameAndPassword(userName, hashedPassword,
                                                                        ref userID, ref personID, ref isActive);

            return isFound ? new clsUser(userID, personID, userName, hashedPassword, isActive) : null;
        }

        public bool Save()
        {
            if (Mode == enMode.AddNew)
            {
                if (_AddNewUser())
                {
                    Mode = enMode.Update;
                    _originalPassword = Password;
                    return true;
                }
                return false;
            }
            else
            {
                bool result = _UpdateUser();
                _originalPassword = Password;
                return result;
            }
        }

        public static DataTable GetAllUsers() => clsUserData.GetAllUsers();

        public static bool DeleteUser(int userID) => clsUserData.DeleteUser(userID);

        public static bool IsUserExist(int userID) => clsUserData.IsUserExist(userID);
        public static bool IsUserExist(string userName) => clsUserData.IsUserExist(userName);
        public static bool IsUserExistForPersonID(int personID) => clsUserData.IsUserExistForPersonID(personID);
    }
}
