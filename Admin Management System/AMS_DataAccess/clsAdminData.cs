using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS_DataAccess
{
    public class clsAdminDTO
    {
        public int AdminID { get; set; }
        public int PersonID { get; set; }
        public string AdminName { get; set; }
        public byte PermissionLevel { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByAdminID { get; set; }
        public DateTime LastStatusDate { get; set; }

        public clsAdminDTO(int AdminID, int PersonID, string AdminName, byte PermissionLevel, bool IsActive, 
                           DateTime CreatedDate, int CreatedByAdminID, DateTime LastStatusDate)
        {
            this.AdminID = AdminID;
            this.PersonID = PersonID;
            this.AdminName = AdminName;
            this.PermissionLevel = PermissionLevel;
            this.IsActive = IsActive;
            this.CreatedDate = CreatedDate;
            this.CreatedByAdminID = CreatedByAdminID;
            this.LastStatusDate = LastStatusDate;
        }
    }
    public class clsAdminCredentialsDTO
    {
        public int AdminID { get; set; }
        public string AdminName { get; set; }
        public string Password { get; set; }
        public clsAdminCredentialsDTO(int AdminID, string AdminName, string Password)
        {
            this.AdminID = AdminID;
            this.AdminName = AdminName;
            this.Password = Password;
        }
    }
    public class clsAdminWithDetailsDTO
    {
        public clsAdminDTO AdminDTO { get; set; }
        public string NationalNo { get; set; }
        public string FullName { get; set; }
        public string CreatedByAdmin {  get; set; }
        public clsAdminWithDetailsDTO(clsAdminDTO AdminDTO, string NationalNo, string FullName,
                                      string CreatedByAdmin)
        {
            this.AdminDTO = AdminDTO;
            this.NationalNo = NationalNo;
            this.FullName = FullName;
            this.CreatedByAdmin = CreatedByAdmin;
        }
    }
    public class clsAdminData
    {
        public static int AddNewAdmin(clsAdminDTO ADTO, clsAdminCredentialsDTO CredentialsDTO)
        {
            int AdminId = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewAdmin", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@PersonID", ADTO.PersonID);
                        command.Parameters.AddWithValue("@AdminName", ADTO.AdminName);

                        //use CredentialsDTO to handle Password
                        command.Parameters.AddWithValue("@Password", CredentialsDTO.Password);

                        command.Parameters.AddWithValue("@PermissionLevel", ADTO.PermissionLevel);
                        command.Parameters.AddWithValue("@IsActive", ADTO.IsActive);
                        command.Parameters.AddWithValue("@CreatedDate", ADTO.CreatedDate);
                        command.Parameters.AddWithValue("@CreatedByAdminID", ADTO.CreatedByAdminID);
                        command.Parameters.AddWithValue("@LastStatusDate", ADTO.LastStatusDate);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedId))
                        {
                            AdminId = InsertedId;
                        }
                    }
                }
               
                if (AdminId > 0)
                    clsGlobal.SaveToEventLog($"New Admin added to System with ID: {AdminId}",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a new admin. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return AdminId;
        }
        public static bool UpdateAdmin(clsAdminDTO ADTO)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateAdmin", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@AdminID", ADTO.AdminID);
                        command.Parameters.AddWithValue("@PersonID", ADTO.PersonID);
                        command.Parameters.AddWithValue("@AdminName", ADTO.AdminName);
                        command.Parameters.AddWithValue("@PermissionLevel", ADTO.PermissionLevel);
                        command.Parameters.AddWithValue("@IsActive", ADTO.IsActive);
                        command.Parameters.AddWithValue("@CreatedDate", ADTO.CreatedDate);
                        command.Parameters.AddWithValue("@CreatedByAdminID", ADTO.CreatedByAdminID);
                        command.Parameters.AddWithValue("@LastStatusDate", ADTO.LastStatusDate);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"The record for Admin ID: {ADTO.AdminID} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdateAdmin method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeleteAdmin(int AdminID)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeleteAdmin", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AdminID", AdminID);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0) 
                {
                    clsGlobal.SaveToEventLog($"Admin with ID: {AdminID} deleted successfully",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeleteAdmin method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static clsAdminWithDetailsDTO GetAdminInfoByAdminId(int AdminID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAdminInfoByAdminID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AdminID", AdminID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var AdminDTO = new clsAdminDTO(
                                    reader.GetInt32(reader.GetOrdinal("AdminID")),
                                    reader.GetInt32(reader.GetOrdinal("PersonID")),
                                    reader.GetString(reader.GetOrdinal("AdminName")),
                                    reader.GetByte(reader.GetOrdinal("PermissionLevel")),
                                    reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                    reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                    reader.GetInt32(reader.GetOrdinal("CreatedByAdminID")),
                                    reader.GetDateTime(reader.GetOrdinal("LastStatusDate"))
                                    );

                                string NationalNo = reader.GetString(reader.GetOrdinal("NationalNo"));
                                string FullName = reader.GetString(reader.GetOrdinal("FullName"));
                                string CreatedByAdmin = reader.GetString(reader.GetOrdinal("CreatedByAdmin"));

                                return new clsAdminWithDetailsDTO(AdminDTO, NationalNo, FullName, CreatedByAdmin);

                            }
                            else
                                return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAdminInfoByAdminId method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static clsAdminWithDetailsDTO GetAdminInfoByAdminName(string AdminName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAdminInfoByAdminName", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AdminName", AdminName);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var AdminDTO = new clsAdminDTO(
                                    reader.GetInt32(reader.GetOrdinal("AdminID")),
                                    reader.GetInt32(reader.GetOrdinal("PersonID")),
                                    reader.GetString(reader.GetOrdinal("AdminName")),
                                    reader.GetByte(reader.GetOrdinal("PermissionLevel")),
                                    reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                    reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                    reader.GetInt32(reader.GetOrdinal("CreatedByAdminID")),
                                    reader.GetDateTime(reader.GetOrdinal("LastStatusDate"))
                                    );

                                string NationalNo = reader.GetString(reader.GetOrdinal("NationalNo"));
                                string FullName = reader.GetString(reader.GetOrdinal("FullName"));
                                string CreatedByAdmin = reader.GetString(reader.GetOrdinal("CreatedByAdmin"));

                                return new clsAdminWithDetailsDTO(AdminDTO, NationalNo, FullName, CreatedByAdmin);

                            }
                            else
                                return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAdminInfoByAdminName method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static clsAdminWithDetailsDTO GetAdminInfoByPersonId(int PersonID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAdminInfoByPersonID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var AdminDTO = new clsAdminDTO(
                                    reader.GetInt32(reader.GetOrdinal("AdminID")),
                                    reader.GetInt32(reader.GetOrdinal("PersonID")),
                                    reader.GetString(reader.GetOrdinal("AdminName")),
                                    reader.GetByte(reader.GetOrdinal("PermissionLevel")),
                                    reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                    reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                    reader.GetInt32(reader.GetOrdinal("CreatedByAdminID")),
                                    reader.GetDateTime(reader.GetOrdinal("LastStatusDate"))
                                    );

                                string NationalNo = reader.GetString(reader.GetOrdinal("NationalNo"));
                                string FullName = reader.GetString(reader.GetOrdinal("FullName"));
                                string CreatedByAdmin = reader.GetString(reader.GetOrdinal("CreatedByAdmin"));

                                return new clsAdminWithDetailsDTO(AdminDTO, NationalNo, FullName, CreatedByAdmin);
                            }
                            else
                                return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAdminInfoByPersonId method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsAdminWithDetailsDTO> GetAllAdminsWithDetails()
        {
            var AllAdmins = new List<clsAdminWithDetailsDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllAdmins", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                var AdminDTO = new clsAdminDTO(
                                            reader.GetInt32(reader.GetOrdinal("AdminID")),
                                            reader.GetInt32(reader.GetOrdinal("PersonID")),
                                            reader.GetString(reader.GetOrdinal("AdminName")),
                                            reader.GetByte(reader.GetOrdinal("PermissionLevel")),
                                            reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                            reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                            reader.GetInt32(reader.GetOrdinal("CreatedByAdminID")),
                                            reader.GetDateTime(reader.GetOrdinal("LastStatusDate"))
                                    );
                                
                                string NationalNo = reader.GetString(reader.GetOrdinal("NationalNo"));
                                string FullName = reader.GetString(reader.GetOrdinal("FullName"));
                                string CreatedByAdmin = reader.GetString(reader.GetOrdinal("CreatedByAdmin"));
                                
                                AllAdmins.Add(new clsAdminWithDetailsDTO(AdminDTO, NationalNo, FullName, 
                                                                        CreatedByAdmin));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllAdmins method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return AllAdmins;
        }
        public static bool IsAdminExistByAdminId(int AdminID)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_IsAdminExistByAdminID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AdminID", AdminID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            IsFound = reader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in IsAdminExistByAdminId method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                IsFound = false;
            }

            return IsFound;
        }
        public static bool IsAdminExistByPersonId(int PersonID)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_IsAdminExistByPersonID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            IsFound = reader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in IsAdminExistByPersonId method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                IsFound = false;
            }

            return IsFound;
        }
        public static bool IsAdminExistByAdminName(string AdminName)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_IsAdminExistByAdminName", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AdminName", AdminName);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            IsFound = reader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in IsAdminExistByAdminName method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                IsFound = false;
            }

            return IsFound;
        }
        public static bool IsAdminExistByAdminNameAndPassword(string AdminName, string Password)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_IsAdminExistByAdminNameAndPassword", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AdminName", AdminName);
                        command.Parameters.AddWithValue("@Password", Password);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            IsFound = reader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in IsAdminExistByAdminNameAndPassword method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                IsFound = false;
            }

            return IsFound;
        }
        public static bool SetAdminPermissionLevel(int AdminId, short PermissionLevel)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_SetAdminPermissionLevel", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AdminID", AdminId);
                        command.Parameters.AddWithValue("@PermissionLevel", PermissionLevel);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) 
            {
                clsGlobal.SaveToEventLog($"Error occurred in SetAdminPermissionLevel method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool ChangeAdminPassword(int AdminID, string OldPassword, string NewPassword)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_ChangeAdminPassword", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AdminID", AdminID);
                        command.Parameters.AddWithValue("@OldPassword", OldPassword);
                        command.Parameters.AddWithValue("@NewPassword", NewPassword);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in ChangeAdminPassword method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeactivateAdminAccount(int AdminId, bool IsActive)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeactivateAdminAccount", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AdminID", AdminId);
                        command.Parameters.AddWithValue("@IsActive", IsActive);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeactivateAdminAccount method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
    }
}