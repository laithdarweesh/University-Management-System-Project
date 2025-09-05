using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace AMS_DataAccess
{
    public class clsStudentDTO
    {
        public int StudentID {  get; set; }
        public int PersonID {  get; set; }
        public int RegistrationStudentAppID {  get; set; }
        public int RankID {  get; set; }
        public byte StudentStatus {  get; set; }
        public string Password { get; set; }
        public bool IsActive {  get; set; }
        public decimal CreditBalance {  get; set; }
        public decimal DebitBalance {  get; set; }
        public byte SpecializationHours {  get; set; }
        public byte PassedHours {  get; set; }
        public float CumulativeAverage {  get; set; }
        public int CreatedByAdminID {  get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastStatusDate { get; set; }
        public clsStudentDTO(int StudentID, int PersonID, int RegistrationStudentAppID, int RankID,
            byte StudentStatus, string Password, bool IsActive, decimal CreditBalance,
            decimal DebitBalance, byte SpecializationHours, byte PassedHours, float CumulativeAverage,
            int CreatedByAdminID, DateTime CreatedDate, DateTime LastStatusDate)
        {
            this.StudentID = StudentID;
            this.PersonID = PersonID;
            this.RegistrationStudentAppID = RegistrationStudentAppID;
            this.RankID = RankID;
            this.StudentStatus = StudentStatus;
            this.Password = Password;
            this.IsActive = IsActive;
            this.CreditBalance = CreditBalance;
            this.DebitBalance = DebitBalance;
            this.SpecializationHours = SpecializationHours;
            this.PassedHours = PassedHours;
            this.CumulativeAverage = CumulativeAverage;
            this.CreatedByAdminID = CreatedByAdminID;
            this.CreatedDate = CreatedDate;
            this.LastStatusDate = LastStatusDate;
        }
    }
    public class clsStudentData
    {
        public static int AddNewStudent(clsStudentDTO SDTO)
        {
            int StudentId = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewStudent", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PersonID", SDTO.PersonID);
                        command.Parameters.AddWithValue("@RegistrationStudentApplicationID", 
                                                            SDTO.RegistrationStudentAppID);

                        command.Parameters.AddWithValue("@RankID", SDTO.RankID);
                        command.Parameters.AddWithValue("@StudentStatus", SDTO.StudentStatus);
                        command.Parameters.AddWithValue("@Password", SDTO.Password);
                        command.Parameters.AddWithValue("@IsActive", SDTO.IsActive);
                        command.Parameters.AddWithValue("@CreditBalance", SDTO.CreditBalance);
                        command.Parameters.AddWithValue("@DebitBalance", SDTO.DebitBalance);
                        command.Parameters.AddWithValue("@SpecializationHours", SDTO.SpecializationHours);
                        command.Parameters.AddWithValue("@PassedHours", SDTO.PassedHours);
                        command.Parameters.AddWithValue("@CumulativeAverage", SDTO.CumulativeAverage);
                        command.Parameters.AddWithValue("@CreatedByAdminID", SDTO.CreatedByAdminID);
                        command.Parameters.AddWithValue("@CreatedDate", SDTO.CreatedDate);
                        command.Parameters.AddWithValue("@LastStatusDate", SDTO.LastStatusDate);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedId))
                        {
                            StudentId = InsertedId;
                        }
                    }
                }
                if (StudentId > 0)
                    clsGlobal.SaveToEventLog($"New Student added to System with ID: {StudentId}",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a new Student. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return StudentId;
        }
        public static bool UpdateStudent(clsStudentDTO SDTO)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateStudent", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@StudentID", SDTO.StudentID);
                        command.Parameters.AddWithValue("@PersonID", SDTO.PersonID);
                        command.Parameters.AddWithValue("@RegistrationStudentApplicationID", 
                                                            SDTO.RegistrationStudentAppID);

                        command.Parameters.AddWithValue("@RankID", SDTO.RankID);
                        command.Parameters.AddWithValue("@StudentStatus", SDTO.StudentStatus);
                        command.Parameters.AddWithValue("@IsActive", SDTO.IsActive);
                        command.Parameters.AddWithValue("@CreditBalance", SDTO.CreditBalance);
                        command.Parameters.AddWithValue("@DebitBalance", SDTO.DebitBalance);
                        command.Parameters.AddWithValue("@SpecializationHours", SDTO.SpecializationHours);
                        command.Parameters.AddWithValue("@PassedHours", SDTO.PassedHours);
                        command.Parameters.AddWithValue("@CumulativeAverage", SDTO.CumulativeAverage);
                        command.Parameters.AddWithValue("@CreatedByAdminID", SDTO.CreatedByAdminID);
                        command.Parameters.AddWithValue("@CreatedDate", SDTO.CreatedDate);
                        command.Parameters.AddWithValue("@LastStatusDate", SDTO.LastStatusDate);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"The record for Student ID: {SDTO.StudentID} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdateStudent method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeleteStudent(int StudentID)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeleteStudent", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StudentID", StudentID);
                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"Student with ID: {StudentID} deleted successfully",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeleteStudent method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static clsStudentDTO GetStudentInfoByStudentID(int StudentID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetStudentInfoByStudentID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StudentID", StudentID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsStudentDTO(
                                        reader.GetInt32(reader.GetOrdinal("StudentID")),
                                        reader.GetInt32(reader.GetOrdinal("PersonID")),
                                        reader.GetInt32(reader.GetOrdinal("RegistrationStudentApplicationID")),
                                        reader.GetInt32(reader.GetOrdinal("RankID")),
                                        reader.GetByte(reader.GetOrdinal("StudentStatus")),
                                        reader.GetString(reader.GetOrdinal("Password")),
                                        reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                        reader.GetDecimal(reader.GetOrdinal("CreditBalance")),
                                        reader.GetDecimal(reader.GetOrdinal("DebitBalance")),
                                        reader.GetByte(reader.GetOrdinal("SpecializationHours")),
                                        reader.GetByte(reader.GetOrdinal("PassedHours")),
                                        reader.GetFloat(reader.GetOrdinal("CumulativeAverage")),
                                        reader.GetInt32(reader.GetOrdinal("CreatedByAdminID")),
                                        reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                        reader.GetDateTime(reader.GetOrdinal("LastStatusDate"))
                                    );
                            }
                            else
                                return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetStudentInfoByStudentID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static clsStudentDTO GetStudentInfoByPersonID(int PersonID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetStudentInfoByPersonID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsStudentDTO(
                                            reader.GetInt32(reader.GetOrdinal("StudentID")),
                                            reader.GetInt32(reader.GetOrdinal("PersonID")),
                                            reader.GetInt32(reader.GetOrdinal("RegistrationStudentApplicationID")),
                                            reader.GetInt32(reader.GetOrdinal("RankID")),
                                            reader.GetByte(reader.GetOrdinal("StudentStatus")),
                                            reader.GetString(reader.GetOrdinal("Password")),
                                            reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                            reader.GetDecimal(reader.GetOrdinal("CreditBalance")),
                                            reader.GetDecimal(reader.GetOrdinal("DebitBalance")),
                                            reader.GetByte(reader.GetOrdinal("SpecializationHours")),
                                            reader.GetByte(reader.GetOrdinal("PassedHours")),
                                            reader.GetFloat(reader.GetOrdinal("CumulativeAverage")),
                                            reader.GetInt32(reader.GetOrdinal("CreatedByAdminID")),
                                            reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                            reader.GetDateTime(reader.GetOrdinal("LastStatusDate"))
                                        );
                            }
                            else
                                return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetStudentInfoByPersonID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static clsStudentDTO GetStudentInfoByRegistrationAppID(int RegistrationStudentAppID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetStudentInfoByRegistrationAppID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@RegistrationStudentApplicationID", RegistrationStudentAppID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsStudentDTO(
                                                reader.GetInt32(reader.GetOrdinal("StudentID")),
                                                reader.GetInt32(reader.GetOrdinal("PersonID")),
                                                reader.GetInt32(reader.GetOrdinal("RegistrationStudentApplicationID")),
                                                reader.GetInt32(reader.GetOrdinal("RankID")),
                                                reader.GetByte(reader.GetOrdinal("StudentStatus")),
                                                reader.GetString(reader.GetOrdinal("Password")),
                                                reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                                reader.GetDecimal(reader.GetOrdinal("CreditBalance")),
                                                reader.GetDecimal(reader.GetOrdinal("DebitBalance")),
                                                reader.GetByte(reader.GetOrdinal("SpecializationHours")),
                                                reader.GetByte(reader.GetOrdinal("PassedHours")),
                                                reader.GetFloat(reader.GetOrdinal("CumulativeAverage")),
                                                reader.GetInt32(reader.GetOrdinal("CreatedByAdminID")),
                                                reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                                reader.GetDateTime(reader.GetOrdinal("LastStatusDate"))
                                            );
                            }
                            else
                                return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetStudentInfoByRegistrationAppID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsStudentDTO> GetAllStudents()
        {
            var AllStudents = new List<clsStudentDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllStudents", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                AllStudents.Add(
                                    new clsStudentDTO(
                                                reader.GetInt32(reader.GetOrdinal("StudentID")),
                                                reader.GetInt32(reader.GetOrdinal("PersonID")),
                                                reader.GetInt32(reader.GetOrdinal("RegistrationStudentApplicationID")),
                                                reader.GetInt32(reader.GetOrdinal("RankID")),
                                                reader.GetByte(reader.GetOrdinal("StudentStatus")),
                                                reader.GetString(reader.GetOrdinal("Password")),
                                                reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                                reader.GetDecimal(reader.GetOrdinal("CreditBalance")),
                                                reader.GetDecimal(reader.GetOrdinal("DebitBalance")),
                                                reader.GetByte(reader.GetOrdinal("SpecializationHours")),
                                                reader.GetByte(reader.GetOrdinal("PassedHours")),
                                                reader.GetFloat(reader.GetOrdinal("CumulativeAverage")),
                                                reader.GetInt32(reader.GetOrdinal("CreatedByAdminID")),
                                                reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                                reader.GetDateTime(reader.GetOrdinal("LastStatusDate"))
                                            )
                                    );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllStudents method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return AllStudents;
        }
        public static bool IsStudentExistByStudentId(int StudentID)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_IsStudentExistByStudentID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StudentID", StudentID);
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
                clsGlobal.SaveToEventLog($"Error occurred in IsStudentExistByStudentId method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                IsFound = false;
            }

            return IsFound;
        }
        public static bool IsStudentExistByPersonId(int PersonID)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_IsStudentExistByPersonID", connection))
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
                clsGlobal.SaveToEventLog($"Error occurred in IsStudentExistByPersonId method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                IsFound = false;
            }

            return IsFound;
        }
        public static bool ChangeStudentPassword(int StudentId, string OldPassword, string NewPassword)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_ChangeStudentPassword", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StudentID", StudentId);
                        command.Parameters.AddWithValue("@OldPassword", OldPassword);
                        command.Parameters.AddWithValue("@NewPassword", NewPassword);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in ChangeStudentPassword method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeactivateStudentAccount(int StudentId, bool IsActive)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeactivateStudentAccount", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StudentID", StudentId);
                        command.Parameters.AddWithValue("@IsActive", IsActive);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeactivateStudentAccount method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
    }
}