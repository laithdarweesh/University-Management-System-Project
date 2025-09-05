using AMS_DataAccess;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS_BusinessLayer
{
    public class clsGlobalAdmin
    {
        public static bool SaveUserNameAndPasswordToRegistry(string UserName, string Password)
        {
            string KeyPath = @"HKEY_CURRENT_USER\SOFTWARE\UMS_Project\Admins";

            string KeyUserName = "UserName";
            string KeyPassword = "Password";

            string KeyUserData = UserName;
            string KeyPasswordData = Password;

            if (KeyUserData == "" || KeyPasswordData == "")
            {
                RemoveStoredCredential(KeyPath, KeyUserName);
                RemoveStoredCredential(KeyPath, KeyPassword);

                return false;
            }

            try
            {
                //Save UserName
                Registry.SetValue(KeyPath, KeyUserName, KeyUserData, RegistryValueKind.String);

                //Save Password
                Registry.SetValue(KeyPath, KeyPassword, KeyPasswordData, RegistryValueKind.String);

                return true;
            }
            catch (Exception ex)
            {
                // Log an Event to Event Log
                SharedLibrary.clsGlobal.SaveToEventLog(ex.Message, clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
                return false;
            }
        }
        public static bool GetStoredCredential(ref string UserName, ref string Password)
        {
            string KeyPath = @"HKEY_CURRENT_USER\SOFTWARE\UMS_Project\Admins";

            string KeyUserName = "UserName";
            string KeyPassword = "Password";

            try
            {
                UserName = Registry.GetValue(KeyPath, KeyUserName, null) as string;
                Password = Registry.GetValue(KeyPath, KeyPassword, null) as string;

                if (UserName == null || Password == null)
                {
                    UserName = "";
                    Password = "";

                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log an Event to Event Log
                SharedLibrary.clsGlobal.SaveToEventLog(ex.Message, clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                return false;
            }
        }
        private static bool RemoveStoredCredential(string KeyPath, string KeyValueName)
        {
            try
            {
                // Open the registry key in read/write mode with explicit registry view
                using (RegistryKey BaseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                {
                    using (RegistryKey Key = BaseKey.OpenSubKey(KeyPath, true))
                    {
                        if (Key != null)
                        {
                            // Delete the specified value
                            Key.DeleteValue(KeyValueName);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                // Log an Event to Event Log
                SharedLibrary.clsGlobal.SaveToEventLog(ex.Message, clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
                return false;
            }
            catch (Exception ex)
            {
                // Log an Event to Event Log
                SharedLibrary.clsGlobal.SaveToEventLog(ex.Message, clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
                return false;
            }
        }
    }
}
