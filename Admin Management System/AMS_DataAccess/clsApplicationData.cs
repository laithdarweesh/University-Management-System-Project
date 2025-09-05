using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;

namespace AMS_DataAccess
{
    public class clsApplicationDTO
    {
        public int ApplicationID { get; set; }
        public int ApplicantPersonID { get; set; }
        public int ApplicationTypeID { get; set; }
        public byte ApplicationStatus { get; set; }
        public DateTime ApplicationDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByAdminID { get; set; }
        public DateTime LastStatusDate { get; set; }
        public clsApplicationDTO(int ApplicationID, int ApplicantPersonID, int ApplicationTypeID,
                                 byte ApplicationStatus, DateTime ApplicationDate, decimal PaidFees,
                                 int CreatedByAdminID, DateTime LastStatusDate)
        {
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationStatus = ApplicationStatus;
            this.ApplicationDate = ApplicationDate;
            this.PaidFees = PaidFees;
            this.CreatedByAdminID = CreatedByAdminID;
            this.LastStatusDate = LastStatusDate;
        }
    }
    public class clsApplicationWithDetailsDTO
    {
        public clsApplicationDTO ApplicationDTO { get; set; }
        public string FullApplicantName { get; set; }
        public string ApplicationTypeTitle { get; set; }
        public string CreatedByAdminName { get; set; }
        public clsApplicationWithDetailsDTO(clsApplicationDTO AppDTO, string FullApplicantName, 
                                            string ApplicationTypeTitle, string CreatedByAdminName)
        {
            this.ApplicationDTO = AppDTO;
            this.FullApplicantName = FullApplicantName;
            this.ApplicationTypeTitle = ApplicationTypeTitle;
            this.CreatedByAdminName = CreatedByAdminName;
        }
    }
    public class clsApplicationData
    {
        public static int AddNewApplication(clsApplicationDTO AppDTO)
        {
            int ApplicationId = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewApplication", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ApplicantPersonID", AppDTO.ApplicantPersonID);
                        command.Parameters.AddWithValue("@AppTypeID", AppDTO.ApplicationTypeID);
                        command.Parameters.AddWithValue("@AppStatus", AppDTO.ApplicationStatus);
                        command.Parameters.AddWithValue("@AppDate", AppDTO.ApplicationDate);
                        command.Parameters.AddWithValue("@PaidFees", AppDTO.PaidFees);
                        command.Parameters.AddWithValue("@CreatedByAdminID", AppDTO.CreatedByAdminID);
                        command.Parameters.AddWithValue("@LastStatusDate", AppDTO.LastStatusDate);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedId))
                        {
                            ApplicationId = InsertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a New Application. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return ApplicationId;
        }
        public static bool UpdateApplication(clsApplicationDTO AppDTO)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateApplication", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ApplicationID", AppDTO.ApplicationID);
                        command.Parameters.AddWithValue("@AppTypeID", AppDTO.ApplicationTypeID);
                        command.Parameters.AddWithValue("@AppStatus", AppDTO.ApplicationStatus);
                        command.Parameters.AddWithValue("@PaidFees", AppDTO.PaidFees);
                        command.Parameters.AddWithValue("@LastStatusDate", AppDTO.LastStatusDate);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"The record for Application ID: {AppDTO.ApplicationID} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdateApplication method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeleteApplication(int ApplicationId)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeleteApplication", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationId);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"Application with ID: {ApplicationId} deleted successfully",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeleteApplication method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool IsApplicationExist(int ApplicationId)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_IsApplicationExist", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AppId", ApplicationId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            IsFound = reader.Read();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in IsApplicationExist method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return IsFound;
            }

            return IsFound;
        }
        public static bool DoesPersonHaveActiveApplication(int PersonId)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DoesPersonHaveActiveApplication", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PersonId", PersonId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            IsFound = reader.Read();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DoesPersonHaveActiveApplication method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return IsFound;
            }

            return IsFound;
        }
        public static clsApplicationDTO GetApplicationInfoByID(int ApplicationId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAppInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsApplicationDTO(
                                    reader.GetInt32(reader.GetOrdinal("ApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicantPersonID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicationTypeID")),
                                    reader.GetByte(reader.GetOrdinal("ApplicationStatus")),
                                    reader.GetDateTime(reader.GetOrdinal("ApplicationDate")),
                                    reader.GetDecimal(reader.GetOrdinal("PaidFees")),
                                    reader.GetInt32(reader.GetOrdinal("CreatedByAdminID")),
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
                clsGlobal.SaveToEventLog($"Error occurred in GetApplicationInfoByID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static clsApplicationDTO GetApplicationInfoByPersonId(int ApplicantPersonID, int AppTypeId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAppInfoByPersonID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                        command.Parameters.AddWithValue("@ApplicationTypeID", AppTypeId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsApplicationDTO(
                                    reader.GetInt32(reader.GetOrdinal("ApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicantPersonID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicationTypeID")),
                                    reader.GetByte(reader.GetOrdinal("ApplicationStatus")),
                                    reader.GetDateTime(reader.GetOrdinal("ApplicationDate")),
                                    reader.GetDecimal(reader.GetOrdinal("PaidFees")),
                                    reader.GetInt32(reader.GetOrdinal("CreatedByAdminID")),
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
                clsGlobal.SaveToEventLog($"Error occurred in GetApplicationInfoByPersonId method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsApplicationDTO> GetAllApplications()
        {
            var Applications = new List<clsApplicationDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllApplications", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Applications.Add(new clsApplicationDTO(
                                    reader.GetInt32(reader.GetOrdinal("ApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicantPersonID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicationTypeID")),
                                    reader.GetByte(reader.GetOrdinal("ApplicationStatus")),
                                    reader.GetDateTime(reader.GetOrdinal("ApplicationDate")),
                                    reader.GetDecimal(reader.GetOrdinal("PaidFees")),
                                    reader.GetInt32(reader.GetOrdinal("CreatedByAdminID")),
                                    reader.GetDateTime(reader.GetOrdinal("LastStatusDate"))
                                    ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllApplications method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return Applications;
        }
        public static List<clsApplicationDTO> GetAllApplicationsByAppType(int ApplicationTypeId)
        {
            var Applications = new List<clsApplicationDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllApplicationsByAppType", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Applications.Add(new clsApplicationDTO(
                                    reader.GetInt32(reader.GetOrdinal("ApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicantPersonID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicationTypeID")),
                                    reader.GetByte(reader.GetOrdinal("ApplicationStatus")),
                                    reader.GetDateTime(reader.GetOrdinal("ApplicationDate")),
                                    reader.GetDecimal(reader.GetOrdinal("PaidFees")),
                                    reader.GetInt32(reader.GetOrdinal("CreatedByAdminID")),
                                    reader.GetDateTime(reader.GetOrdinal("LastStatusDate"))
                                    ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllApplicationsByAppType method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return Applications;
        }
        public static List<clsApplicationWithDetailsDTO> GetAllApplicationsWithDetails()
        {
            var ApplicationsDetails = new List<clsApplicationWithDetailsDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("Select * From ApplicationDetails_View", connection))
                    {
                        command.CommandType = CommandType.Text;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var AppDTO = new clsApplicationDTO(
                                    reader.GetInt32(reader.GetOrdinal("ApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicantPersonID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicationTypeID")),
                                    reader.GetByte(reader.GetOrdinal("ApplicationStatus")),
                                    reader.GetDateTime(reader.GetOrdinal("ApplicationDate")),
                                    reader.GetDecimal(reader.GetOrdinal("PaidFees")),
                                    reader.GetInt32(reader.GetOrdinal("CreatedByAdminID")),
                                    reader.GetDateTime(reader.GetOrdinal("LastStatusDate"))
                                    );

                                string FullName = reader.GetString(reader.GetOrdinal("FullApplicantName"));
                                string AppTypeTitle = reader.GetString(reader.GetOrdinal("ApplicationTypeTitle"));
                                string CreatedByAdminName = reader.GetString(reader.GetOrdinal("CreatedByAdminName"));

                                ApplicationsDetails.Add(new clsApplicationWithDetailsDTO(AppDTO, FullName, 
                                                                    AppTypeTitle, CreatedByAdminName));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllApplicationsWithDetails method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return ApplicationsDetails;
        }
        public static clsApplicationWithDetailsDTO GetApplicationWithDetailsByAppId(int ApplicationId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("Select * From ApplicationDetails_View Where ApplicationID = @ApplicationId", connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@ApplicationId", ApplicationId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var AppDTO = new clsApplicationDTO(
                                    reader.GetInt32(reader.GetOrdinal("ApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicantPersonID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicationTypeID")),
                                    reader.GetByte(reader.GetOrdinal("ApplicationStatus")),
                                    reader.GetDateTime(reader.GetOrdinal("ApplicationDate")),
                                    reader.GetDecimal(reader.GetOrdinal("PaidFees")),
                                    reader.GetInt32(reader.GetOrdinal("CreatedByAdminID")),
                                    reader.GetDateTime(reader.GetOrdinal("LastStatusDate"))
                                    );

                                string FullName = reader.GetString(reader.GetOrdinal("FullApplicantName"));
                                string AppTypeTitle = reader.GetString(reader.GetOrdinal("ApplicationTypeTitle"));
                                string CreatedByAdminName = reader.GetString(reader.GetOrdinal("CreatedByAdminName"));

                                return new clsApplicationWithDetailsDTO(AppDTO,FullName,AppTypeTitle,
                                                                        CreatedByAdminName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetApplicationsWithDetailsByAppId method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return null;
        }
        public static List<clsApplicationWithDetailsDTO> GetApplicationsWithDetailsByPersonId(int PersonId)
        {
            var AppsByPerson = new List<clsApplicationWithDetailsDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("Select * From ApplicationDetails_View Where ApplicantPersonID = @PersonId", connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@PersonId", PersonId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var AppDTO = new clsApplicationDTO(
                                    reader.GetInt32(reader.GetOrdinal("ApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicantPersonID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicationTypeID")),
                                    reader.GetByte(reader.GetOrdinal("ApplicationStatus")),
                                    reader.GetDateTime(reader.GetOrdinal("ApplicationDate")),
                                    reader.GetDecimal(reader.GetOrdinal("PaidFees")),
                                    reader.GetInt32(reader.GetOrdinal("CreatedByAdminID")),
                                    reader.GetDateTime(reader.GetOrdinal("LastStatusDate"))
                                    );

                                string FullName = reader.GetString(reader.GetOrdinal("FullApplicantName"));
                                string AppTypeTitle = reader.GetString(reader.GetOrdinal("ApplicationTypeTitle"));
                                string CreatedByAdminName = reader.GetString(reader.GetOrdinal("CreatedByAdminName"));

                                AppsByPerson.Add(new clsApplicationWithDetailsDTO(AppDTO, FullName, 
                                                 AppTypeTitle, CreatedByAdminName));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetApplicationsWithDetailsByPersonId method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return AppsByPerson;
        }
        public static List<clsApplicationWithDetailsDTO> GetApplicationsWithDetailsByAppTypeId(int AppTypeId)
        {
            var AppsByAppType = new List<clsApplicationWithDetailsDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("Select * From ApplicationDetails_View Where ApplicationTypeID = @AppTypeId", connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@AppTypeId", AppTypeId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var AppDTO = new clsApplicationDTO(
                                    reader.GetInt32(reader.GetOrdinal("ApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicantPersonID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicationTypeID")),
                                    reader.GetByte(reader.GetOrdinal("ApplicationStatus")),
                                    reader.GetDateTime(reader.GetOrdinal("ApplicationDate")),
                                    reader.GetDecimal(reader.GetOrdinal("PaidFees")),
                                    reader.GetInt32(reader.GetOrdinal("CreatedByAdminID")),
                                    reader.GetDateTime(reader.GetOrdinal("LastStatusDate"))
                                    );

                                string FullName = reader.GetString(reader.GetOrdinal("FullApplicantName"));
                                string AppTypeTitle = reader.GetString(reader.GetOrdinal("ApplicationTypeTitle"));
                                string CreatedByAdminName = reader.GetString(reader.GetOrdinal("CreatedByAdminName"));

                                AppsByAppType.Add(new clsApplicationWithDetailsDTO(AppDTO, FullName,
                                                 AppTypeTitle, CreatedByAdminName));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetApplicationsWithDetailsByAppTypeId method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return AppsByAppType;
        }
        public static List<clsApplicationWithDetailsDTO> GetApplicationsWithDetailsByAppStatus(byte ApplicationStatus)
        {
            var AppsByAppStatus = new List<clsApplicationWithDetailsDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("Select * From ApplicationDetails_View Where ApplicationStatus = @AppStatus", connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@AppStatus", ApplicationStatus);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var AppDTO = new clsApplicationDTO(
                                    reader.GetInt32(reader.GetOrdinal("ApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicantPersonID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicationTypeID")),
                                    reader.GetByte(reader.GetOrdinal("ApplicationStatus")),
                                    reader.GetDateTime(reader.GetOrdinal("ApplicationDate")),
                                    reader.GetDecimal(reader.GetOrdinal("PaidFees")),
                                    reader.GetInt32(reader.GetOrdinal("CreatedByAdminID")),
                                    reader.GetDateTime(reader.GetOrdinal("LastStatusDate"))
                                    );

                                string FullName = reader.GetString(reader.GetOrdinal("FullApplicantName"));
                                string AppTypeTitle = reader.GetString(reader.GetOrdinal("ApplicationTypeTitle"));
                                string CreatedByAdminName = reader.GetString(reader.GetOrdinal("CreatedByAdminName"));

                                AppsByAppStatus.Add(new clsApplicationWithDetailsDTO(AppDTO, FullName,
                                                 AppTypeTitle, CreatedByAdminName));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetApplicationsWithDetailsByAppStatus method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return AppsByAppStatus;
        }
        public static List<clsApplicationWithDetailsDTO> GetActiveApplicationsForPerson(int PersonId, int AppTypeId)
        {
            var ActiveAppsForPerson = new List<clsApplicationWithDetailsDTO>();

            try
            {
                string query = @"Select * From ApplicationDetails_View Where ApplicantPersonID = @PersonId 
                                      And ApplicationTypeID = @AppTypeId And  ApplicationStatus = @Status";

                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@PersonId", PersonId);
                        command.Parameters.AddWithValue("@AppTypeId", AppTypeId);
                        command.Parameters.AddWithValue("@Status", 1);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var AppDTO = new clsApplicationDTO(
                                    reader.GetInt32(reader.GetOrdinal("ApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicantPersonID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicationTypeID")),
                                    reader.GetByte(reader.GetOrdinal("ApplicationStatus")),
                                    reader.GetDateTime(reader.GetOrdinal("ApplicationDate")),
                                    reader.GetDecimal(reader.GetOrdinal("PaidFees")),
                                    reader.GetInt32(reader.GetOrdinal("CreatedByAdminID")),
                                    reader.GetDateTime(reader.GetOrdinal("LastStatusDate"))
                                    );

                                string FullName = reader.GetString(reader.GetOrdinal("FullApplicantName"));
                                string AppTypeTitle = reader.GetString(reader.GetOrdinal("ApplicationTypeTitle"));
                                string CreatedByAdminName = reader.GetString(reader.GetOrdinal("CreatedByAdminName"));

                                ActiveAppsForPerson.Add(new clsApplicationWithDetailsDTO(AppDTO, FullName,
                                                 AppTypeTitle, CreatedByAdminName));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetActiveApplicationsForPerson method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return ActiveAppsForPerson;
        }
    }
}