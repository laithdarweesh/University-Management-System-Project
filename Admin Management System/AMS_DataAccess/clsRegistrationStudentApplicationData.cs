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
    public class clsRegistrationStudentAppDTO
    {
        public int RegistrationStudentApplicationID { get; set; }
        public int ApplicationID { get; set; }
        public int CollegeID { get; set; }
        public int DepartmentID { get; set; }
        public int ProgramTypeID { get; set; }
        public bool? PayMainFees { get; set; }
        public bool? PayCollegeFees { get; set; }
        public bool? PayDepartmentFees { get; set; }
        public bool? PayApplicationFees { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int RegistrationStudentAppTypeID { get; set; }
        public clsRegistrationStudentAppDTO(int RegistrationAppID, int AppID, int CollegeID, int DepartmentID,
            int ProgramTypeID, bool? PayMainFees, bool? PayCollegeFees, bool? PayDepartmentFees, 
            bool? PayApplicationFees, DateTime RegistrationDate, int RegistrationAppTypeID)
        {
            this.RegistrationStudentApplicationID = RegistrationAppID;
            this.ApplicationID = AppID;
            this.CollegeID = CollegeID;
            this.DepartmentID = DepartmentID;
            this.ProgramTypeID = ProgramTypeID;
            this.PayMainFees = PayMainFees; 
            this.PayCollegeFees = PayCollegeFees;
            this.PayDepartmentFees = PayDepartmentFees;
            this.PayApplicationFees = PayApplicationFees;
            this.RegistrationDate = RegistrationDate;
            this.RegistrationStudentAppTypeID = RegistrationAppTypeID;
        }
    }
    public class clsRegistrationStudentApplicationData
    {
        public static int AddNewRegistrationStudentApplication(clsRegistrationStudentAppDTO RegistrationStudentAppDTO)
        {
            int RegistrationStudentAppID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewRegistrationStudentApplication", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ApplicationID", RegistrationStudentAppDTO.ApplicationID);
                        command.Parameters.AddWithValue("@CollegeID", RegistrationStudentAppDTO.CollegeID);
                        command.Parameters.AddWithValue("@DepartmentID", RegistrationStudentAppDTO.DepartmentID);
                        command.Parameters.AddWithValue("@ProgramTypeID", RegistrationStudentAppDTO.ProgramTypeID);

                        command.Parameters.AddWithValue("@PayMainFees", clsGlobal.ToDbNull(RegistrationStudentAppDTO.PayMainFees));
                        command.Parameters.AddWithValue("@PayCollegeFees", clsGlobal.ToDbNull(RegistrationStudentAppDTO.PayCollegeFees));
                        command.Parameters.AddWithValue("@PayDepartmentFees", clsGlobal.ToDbNull(RegistrationStudentAppDTO.PayDepartmentFees));
                        command.Parameters.AddWithValue("@PayApplicationFees", clsGlobal.ToDbNull(RegistrationStudentAppDTO.PayApplicationFees));

                        command.Parameters.AddWithValue("@RegistrationDate", RegistrationStudentAppDTO.RegistrationDate);
                        command.Parameters.AddWithValue("@RegistrationStudentAppTypeID", RegistrationStudentAppDTO.RegistrationStudentAppTypeID);
                        
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedId))
                        {
                            RegistrationStudentAppID = InsertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a New Registration Student Application. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return RegistrationStudentAppID;
        }
        public static bool UpdateRegistrationStudentApplication(clsRegistrationStudentAppDTO RegistrationStudentAppDTO)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateRegistrationStudentApplication", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@RegistrationStudentApplicationID", RegistrationStudentAppDTO.RegistrationStudentApplicationID);
                        command.Parameters.AddWithValue("@CollegeID", RegistrationStudentAppDTO.CollegeID);
                        command.Parameters.AddWithValue("@DepartmentID", RegistrationStudentAppDTO.DepartmentID);
                        command.Parameters.AddWithValue("@ProgramTypeID", RegistrationStudentAppDTO.ProgramTypeID);

                        command.Parameters.AddWithValue("@PayMainFees", clsGlobal.ToDbNull(RegistrationStudentAppDTO.PayMainFees));
                        command.Parameters.AddWithValue("@PayCollegeFees", clsGlobal.ToDbNull(RegistrationStudentAppDTO.PayCollegeFees));
                        command.Parameters.AddWithValue("@PayDepartmentFees", clsGlobal.ToDbNull(RegistrationStudentAppDTO.PayDepartmentFees));
                        command.Parameters.AddWithValue("@PayApplicationFees", clsGlobal.ToDbNull(RegistrationStudentAppDTO.PayApplicationFees));

                        command.Parameters.AddWithValue("@RegistrationDate", RegistrationStudentAppDTO.RegistrationDate);
                        command.Parameters.AddWithValue("@RegistrationStudentAppTypeID", RegistrationStudentAppDTO.RegistrationStudentAppTypeID);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"The record for Update Registration Student App ID: {RegistrationStudentAppDTO.RegistrationStudentApplicationID} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdateRegistrationStudentApplication method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeleteRegistrationStudentApplication(int RegistrationStudentAppID)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeleteRegistrationStudentApplication", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@RegistrationStudentApplicationID", RegistrationStudentAppID);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"Registration Student Application with ID: {RegistrationStudentAppID} deleted successfully",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeleteRegistrationStudentApplication method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static clsRegistrationStudentAppDTO GetRegistrationStudentAppByID(int RegistrationStudentAppId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetRegistrationStudentAppByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@RegistrationStudentApplicationID", RegistrationStudentAppId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsRegistrationStudentAppDTO(
                                    reader.GetInt32(reader.GetOrdinal("RegistrationStudentApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("CollegeID")),
                                    reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                                    reader.GetInt32(reader.GetOrdinal("ProgramTypeID")),

                                    reader.IsDBNull(reader.GetOrdinal("PayMainFees")) ?
                                    (bool?)null : reader.GetBoolean(reader.GetOrdinal("PayMainFees")),

                                    reader.IsDBNull(reader.GetOrdinal("PayCollegeFees")) ?
                                    (bool?) null : reader.GetBoolean(reader.GetOrdinal("PayCollegeFees")),

                                    reader.IsDBNull(reader.GetOrdinal("PayDepartmentFees")) ?
                                    (bool?) null : reader.GetBoolean(reader.GetOrdinal("PayDepartmentFees")),

                                    reader.IsDBNull(reader.GetOrdinal("PayApplicationFees")) ?
                                    (bool?) null : reader.GetBoolean(reader.GetOrdinal("PayApplicationFees")),

                                    reader.GetDateTime(reader.GetOrdinal("RegistrationDate")),
                                    reader.GetInt32(reader.GetOrdinal("RegistrationStudentAppTypeID"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetRegistrationStudentAppByID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static clsRegistrationStudentAppDTO GetRegistrationStudentAppByApplicationID(int AppId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetRegistrationStudentAppByAppID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ApplicationID", AppId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsRegistrationStudentAppDTO(
                                    reader.GetInt32(reader.GetOrdinal("RegistrationStudentApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("CollegeID")),
                                    reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                                    reader.GetInt32(reader.GetOrdinal("ProgramTypeID")),

                                    reader.IsDBNull(reader.GetOrdinal("PayMainFees")) ?
                                    (bool?)null : reader.GetBoolean(reader.GetOrdinal("PayMainFees")),

                                    reader.IsDBNull(reader.GetOrdinal("PayCollegeFees")) ?
                                    (bool?)null : reader.GetBoolean(reader.GetOrdinal("PayCollegeFees")),

                                    reader.IsDBNull(reader.GetOrdinal("PayDepartmentFees")) ?
                                    (bool?)null : reader.GetBoolean(reader.GetOrdinal("PayDepartmentFees")),

                                    reader.IsDBNull(reader.GetOrdinal("PayApplicationFees")) ?
                                    (bool?)null : reader.GetBoolean(reader.GetOrdinal("PayApplicationFees")),

                                    reader.GetDateTime(reader.GetOrdinal("RegistrationDate")),
                                    reader.GetInt32(reader.GetOrdinal("RegistrationStudentAppTypeID"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetRegistrationStudentAppByApplicationID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static clsRegistrationStudentAppDTO GetRegistrationStudentAppByCollegeID(int CollegeId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetRegistrationStudentAppByCollegeID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CollegeID", CollegeId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsRegistrationStudentAppDTO(
                                    reader.GetInt32(reader.GetOrdinal("RegistrationStudentApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("CollegeID")),
                                    reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                                    reader.GetInt32(reader.GetOrdinal("ProgramTypeID")),

                                    reader.IsDBNull(reader.GetOrdinal("PayMainFees")) ?
                                    (bool?)null : reader.GetBoolean(reader.GetOrdinal("PayMainFees")),

                                    reader.IsDBNull(reader.GetOrdinal("PayCollegeFees")) ?
                                    (bool?)null : reader.GetBoolean(reader.GetOrdinal("PayCollegeFees")),

                                    reader.IsDBNull(reader.GetOrdinal("PayDepartmentFees")) ?
                                    (bool?)null : reader.GetBoolean(reader.GetOrdinal("PayDepartmentFees")),

                                    reader.IsDBNull(reader.GetOrdinal("PayApplicationFees")) ?
                                    (bool?)null : reader.GetBoolean(reader.GetOrdinal("PayApplicationFees")),

                                    reader.GetDateTime(reader.GetOrdinal("RegistrationDate")),
                                    reader.GetInt32(reader.GetOrdinal("RegistrationStudentAppTypeID"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetRegistrationStudentAppByCollegeID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static clsRegistrationStudentAppDTO GetRegistrationStudentAppByDepartmentID(int DepartmentId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetRegistrationStudentAppByDepartmentID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DepartmentID", DepartmentId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsRegistrationStudentAppDTO(
                                    reader.GetInt32(reader.GetOrdinal("RegistrationStudentApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("CollegeID")),
                                    reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                                    reader.GetInt32(reader.GetOrdinal("ProgramTypeID")),

                                    reader.IsDBNull(reader.GetOrdinal("PayMainFees")) ?
                                    (bool?)null : reader.GetBoolean(reader.GetOrdinal("PayMainFees")),

                                    reader.IsDBNull(reader.GetOrdinal("PayCollegeFees")) ?
                                    (bool?)null : reader.GetBoolean(reader.GetOrdinal("PayCollegeFees")),

                                    reader.IsDBNull(reader.GetOrdinal("PayDepartmentFees")) ?
                                    (bool?)null : reader.GetBoolean(reader.GetOrdinal("PayDepartmentFees")),

                                    reader.IsDBNull(reader.GetOrdinal("PayApplicationFees")) ?
                                    (bool?)null : reader.GetBoolean(reader.GetOrdinal("PayApplicationFees")),

                                    reader.GetDateTime(reader.GetOrdinal("RegistrationDate")),
                                    reader.GetInt32(reader.GetOrdinal("RegistrationStudentAppTypeID"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetRegistrationStudentAppByDepartmentID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static clsRegistrationStudentAppDTO GetRegistrationStudentAppByProgramTypeID(int ProgramTypeId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetRegistrationStudentAppByProgramTypeID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProgramTypeID", ProgramTypeId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsRegistrationStudentAppDTO(
                                    reader.GetInt32(reader.GetOrdinal("RegistrationStudentApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("CollegeID")),
                                    reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                                    reader.GetInt32(reader.GetOrdinal("ProgramTypeID")),

                                    reader.IsDBNull(reader.GetOrdinal("PayMainFees")) ?
                                    (bool?)null : reader.GetBoolean(reader.GetOrdinal("PayMainFees")),

                                    reader.IsDBNull(reader.GetOrdinal("PayCollegeFees")) ?
                                    (bool?)null : reader.GetBoolean(reader.GetOrdinal("PayCollegeFees")),

                                    reader.IsDBNull(reader.GetOrdinal("PayDepartmentFees")) ?
                                    (bool?)null : reader.GetBoolean(reader.GetOrdinal("PayDepartmentFees")),

                                    reader.IsDBNull(reader.GetOrdinal("PayApplicationFees")) ?
                                    (bool?)null : reader.GetBoolean(reader.GetOrdinal("PayApplicationFees")),

                                    reader.GetDateTime(reader.GetOrdinal("RegistrationDate")),
                                    reader.GetInt32(reader.GetOrdinal("RegistrationStudentAppTypeID"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetRegistrationStudentAppByProgramTypeID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsRegistrationStudentAppDTO> GetAllRegistrationStudentApplications()
        {
            var RegistrationStudentApps = new List<clsRegistrationStudentAppDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllRegistrationStudentApplications", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                RegistrationStudentApps.Add(new clsRegistrationStudentAppDTO(
                                    reader.GetInt32(reader.GetOrdinal("RegistrationStudentApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("CollegeID")),
                                    reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                                    reader.GetInt32(reader.GetOrdinal("ProgramTypeID")),

                                    reader.IsDBNull(reader.GetOrdinal("PayMainFees")) ?
                                    (bool?)null : reader.GetBoolean(reader.GetOrdinal("PayMainFees")),

                                    reader.IsDBNull(reader.GetOrdinal("PayCollegeFees")) ?
                                    (bool?)null : reader.GetBoolean(reader.GetOrdinal("PayCollegeFees")),

                                    reader.IsDBNull(reader.GetOrdinal("PayDepartmentFees")) ?
                                    (bool?)null : reader.GetBoolean(reader.GetOrdinal("PayDepartmentFees")),

                                    reader.IsDBNull(reader.GetOrdinal("PayApplicationFees")) ?
                                    (bool?)null : reader.GetBoolean(reader.GetOrdinal("PayApplicationFees")),

                                    reader.GetDateTime(reader.GetOrdinal("RegistrationDate")),
                                    reader.GetInt32(reader.GetOrdinal("RegistrationStudentAppTypeID"))
                                    ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllRegistrationStudentApplications method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return RegistrationStudentApps;
        }
    }
}