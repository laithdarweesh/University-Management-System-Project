using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS_DataAccess
{
    public class clsDoctorDTO
    {
        public int DoctorID { get; set; }
        public int PersonID { get; set; }
        public int CollegeID { get; set; }
        public int DepartmentID { get; set; }
        public byte DoctorStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByAdminID { get; set; }
        public DateTime LastStatusDate { get; set; }
        public byte PermissionLevel { get; set; }
        public bool IsActive { get; set; }
        public clsDoctorDTO(int DoctorID, int PersonID, int CollegeID, int DepartmentID,
            byte DoctorStatus, DateTime CreatedDate, int CreatedByAdminID,
            DateTime LastStatusDate, byte PermissionLevel, bool IsActive)
        {
            this.DoctorID = DoctorID;
            this.PersonID = PersonID;
            this.CollegeID = CollegeID;
            this.DepartmentID = DepartmentID;
            this.DoctorStatus = DoctorStatus;
            this.CreatedDate = CreatedDate;
            this.CreatedByAdminID = CreatedByAdminID;
            this.LastStatusDate = LastStatusDate;
            this.PermissionLevel = PermissionLevel;
            this.IsActive = IsActive;
        }
    }
    public class clsDoctorWithDetailsDTO
    {
        public clsDoctorDTO DoctorDTO { get; set; }
        public string FullName {  get; set; }
        public string College {  get; set; }
        public string Department {  get; set; }
        public string CreatedBy {  get; set; }
        public clsDoctorWithDetailsDTO(clsDoctorDTO DoctorDTO, string FullName, string College,
                                       string Department, string CreatedBy)
        {
            this.DoctorDTO = DoctorDTO;
            this.FullName = FullName;
            this.College = College;
            this.Department = Department;
            this.CreatedBy = CreatedBy;
        }
    }
    public class clsDoctorCredentialsDTO
    { 
        public int DoctorID {  get; set; }
        public string Password {  get; set; }
        public clsDoctorCredentialsDTO(int DoctorID, string Password)
        {
            this.DoctorID = DoctorID;
            this.Password = Password;
        }
    }
    public class clsDoctorSalaryDTO
    {
        public int DoctorID { get; set; }
        public decimal Salary { get; set; }
        public clsDoctorSalaryDTO(int DoctorID, decimal Salary)
        {
            this.DoctorID=DoctorID;
            this.Salary = Salary;
        }
    }
    public class clsDoctorData
    {
        public static int AddNewDoctor(clsDoctorDTO DoctorDTO, clsDoctorCredentialsDTO CredentialsDTO, 
                                       clsDoctorSalaryDTO SalaryDTO)
        {
            int DoctorId = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewDoctor", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@PersonID", DoctorDTO.PersonID);
                        command.Parameters.AddWithValue("@CollegeID", DoctorDTO.CollegeID);
                        command.Parameters.AddWithValue("@DepartmentID", DoctorDTO.DepartmentID);
                        command.Parameters.AddWithValue("@DoctorStatus", DoctorDTO.DoctorStatus);
                        command.Parameters.AddWithValue("@Salary", SalaryDTO.Salary);
                        command.Parameters.AddWithValue("@CreatedDate", DoctorDTO.CreatedDate);
                        command.Parameters.AddWithValue("@CreatedByAdminID", DoctorDTO.CreatedByAdminID);
                        command.Parameters.AddWithValue("@LastStatusDate", DoctorDTO.LastStatusDate);
                        command.Parameters.AddWithValue("@PermissionLevel", DoctorDTO.PermissionLevel);
                        command.Parameters.AddWithValue("@IsActive", DoctorDTO.IsActive);
                        command.Parameters.AddWithValue("@Password", CredentialsDTO.Password);


                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedId))
                        {
                            DoctorId = InsertedId;
                        }
                    }
                }

                if (DoctorId > 0)
                    clsGlobal.SaveToEventLog($"New Doctor added to System with ID: {DoctorId}",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a new doctor. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return DoctorId;
        }
        public static bool UpdateDoctor(clsDoctorDTO DoctorDTO)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateDoctor", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@DoctorID", DoctorDTO.DoctorID);
                        command.Parameters.AddWithValue("@PersonID", DoctorDTO.PersonID);
                        command.Parameters.AddWithValue("@CollegeID", DoctorDTO.CollegeID);
                        command.Parameters.AddWithValue("@DepartmentID", DoctorDTO.DepartmentID);
                        command.Parameters.AddWithValue("@DoctorStatus", DoctorDTO.DoctorStatus);
                        command.Parameters.AddWithValue("@CreatedDate", DoctorDTO.CreatedDate);
                        command.Parameters.AddWithValue("@CreatedByAdminID", DoctorDTO.CreatedByAdminID);
                        command.Parameters.AddWithValue("@LastStatusDate", DoctorDTO.LastStatusDate);
                        command.Parameters.AddWithValue("@PermissionLevel", DoctorDTO.PermissionLevel);
                        command.Parameters.AddWithValue("@IsActive", DoctorDTO.IsActive);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"The record for Doctor ID: {DoctorDTO.DoctorID} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdateDoctor method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeleteDoctor(int DoctorID)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeleteDoctor", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DoctorID", DoctorID);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"Doctor with ID: {DoctorID} deleted successfully",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeleteDoctor method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static clsDoctorWithDetailsDTO GetDoctorInfoByDoctorId(int DoctorID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetDoctorInfoByDoctorID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DoctorID", DoctorID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // IsActive is returned from the View as a string (e.g., "Yes"/"No"),
                                // unlike the main table where it is stored as a bool (true/false).

                                bool IsActive = reader.GetString(reader.GetOrdinal("IsActive")) == "Yes";

                                var DoctorDTO = new clsDoctorDTO(
                                        reader.GetInt32(reader.GetOrdinal("DoctorID")),
                                        reader.GetInt32(reader.GetOrdinal("PersonID")),
                                        reader.GetInt32(reader.GetOrdinal("CollegeID")),
                                        reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                                        reader.GetByte(reader.GetOrdinal("Status")),
                                        reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                        reader.GetInt32(reader.GetOrdinal("CreatedByAdminID")),
                                        reader.GetDateTime(reader.GetOrdinal("LastStatusDate")),
                                        reader.GetByte(reader.GetOrdinal("PermissionLevel")),
                                        IsActive
                                    );

                                string FullName = reader.GetString(reader.GetOrdinal("FullName"));
                                string College = reader.GetString(reader.GetOrdinal("College"));
                                string Department = reader.GetString(reader.GetOrdinal("Department"));
                                string CreatedBy = reader.GetString(reader.GetOrdinal("CreatedBy"));

                                return new clsDoctorWithDetailsDTO(DoctorDTO,FullName,College,Department,CreatedBy);
                            }
                            else
                                return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetDoctorInfoByDoctorId method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static clsDoctorWithDetailsDTO GetDoctorInfoByPersonId(int PersonID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetDoctorInfoByPersonID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // IsActive is returned from the View as a string (e.g., "Yes"/"No"),
                                // unlike the main table where it is stored as a bool (true/false).

                                bool IsActive = reader.GetString(reader.GetOrdinal("IsActive")) == "Yes";

                                var DoctorDTO = new clsDoctorDTO(
                                        reader.GetInt32(reader.GetOrdinal("DoctorID")),
                                        reader.GetInt32(reader.GetOrdinal("PersonID")),
                                        reader.GetInt32(reader.GetOrdinal("CollegeID")),
                                        reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                                        reader.GetByte(reader.GetOrdinal("Status")),
                                        reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                        reader.GetInt32(reader.GetOrdinal("CreatedByAdminID")),
                                        reader.GetDateTime(reader.GetOrdinal("LastStatusDate")),
                                        reader.GetByte(reader.GetOrdinal("PermissionLevel")),
                                        IsActive
                                    );

                                string FullName = reader.GetString(reader.GetOrdinal("FullName"));
                                string College = reader.GetString(reader.GetOrdinal("College"));
                                string Department = reader.GetString(reader.GetOrdinal("Department"));
                                string CreatedBy = reader.GetString(reader.GetOrdinal("CreatedBy"));

                                return new clsDoctorWithDetailsDTO(DoctorDTO, FullName, College, Department, CreatedBy);
                            }
                            else
                                return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetDoctorInfoByPersonId method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsDoctorWithDetailsDTO> GetAllDoctors()
        {
            var Doctors = new List<clsDoctorWithDetailsDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllDoctors", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // IsActive is returned from the View as a string (e.g., "Yes"/"No"),
                                // unlike the main table where it is stored as a bool (true/false).

                                bool IsActive = reader.GetString(reader.GetOrdinal("IsActive")) == "Yes";

                                var DoctorDTO = new clsDoctorDTO(
                                        reader.GetInt32(reader.GetOrdinal("DoctorID")),
                                        reader.GetInt32(reader.GetOrdinal("PersonID")),
                                        reader.GetInt32(reader.GetOrdinal("CollegeID")),
                                        reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                                        reader.GetByte(reader.GetOrdinal("DoctorStatus")),
                                        reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                        reader.GetInt32(reader.GetOrdinal("CreatedByAdminID")),
                                        reader.GetDateTime(reader.GetOrdinal("LastStatusDate")),
                                        reader.GetByte(reader.GetOrdinal("PermissionLevel")),
                                        IsActive
                                    );

                                string FullName = reader.GetString(reader.GetOrdinal("FullName"));
                                string College = reader.GetString(reader.GetOrdinal("College"));
                                string Department = reader.GetString(reader.GetOrdinal("Department"));
                                string CreatedBy = reader.GetString(reader.GetOrdinal("CreatedBy"));

                                Doctors.Add(new clsDoctorWithDetailsDTO(DoctorDTO, FullName, College,
                                            Department, CreatedBy));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllDoctors method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return Doctors;
        }
        public static bool IsDoctorExistByDoctorID(int DoctorID)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_IsDoctorExistByDoctorID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DoctorID", DoctorID);
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
                clsGlobal.SaveToEventLog($"Error occurred in IsDoctorExistByDoctorID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                IsFound = false;
            }

            return IsFound;
        }
        public static bool IsDoctorExistByPersonID(int PersonID)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_IsDoctorExistByPersonID", connection))
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
                clsGlobal.SaveToEventLog($"Error occurred in IsDoctorExistByPersonID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                IsFound = false;
            }

            return IsFound;
        }
        public static bool SetDoctorPermissionLevel(int DoctorID, byte PermissionLevel)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_SetDoctorPermissionLevel", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DoctorID", DoctorID);
                        command.Parameters.AddWithValue("@PermissionLevel", PermissionLevel);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in SetDoctorPermissionLevel method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool ChangeDoctorPassword(int DoctorID, string OldPassword, string NewPassword)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_ChangeDoctorPassword", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DoctorID", DoctorID);
                        command.Parameters.AddWithValue("@OldPassword", OldPassword);
                        command.Parameters.AddWithValue("@NewPassword", NewPassword);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in ChangeDoctorPassword method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeactivateDoctorAccount(int DoctorID, bool IsActive)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeactivateDoctorAccount", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DoctorID", DoctorID);
                        command.Parameters.AddWithValue("@IsActive", IsActive);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeactivateDoctorAccount method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
    }
}