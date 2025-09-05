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
    public class clsPostponementStudyAppDTO
    {
        public int PostponementAppID { get; set; }
        public int ApplicationID { get; set; }
        public int StudentID { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }
        public string Reason { get; set; }
        public clsPostponementStudyAppDTO(int PostponementAppID, int ApplicationID, int StudentID,
                                          DateTime StartingDate, DateTime EndingDate, string Reason)
        {
            this.PostponementAppID = PostponementAppID;
            this.ApplicationID = ApplicationID;
            this.StudentID = StudentID;
            this.StartingDate = StartingDate;
            this.EndingDate = EndingDate;
            this.Reason = Reason;
        }
    }
    public class clsPostponementStudyAppData
    {
        public static int AddNewPostponementStudyApp(clsPostponementStudyAppDTO PostponementAppDTO)
        {
            int PostponementStudyAppId = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewPostponementStudyApp", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ApplicationID", PostponementAppDTO.ApplicationID);
                        command.Parameters.AddWithValue("@StudentID", PostponementAppDTO.StudentID);
                        command.Parameters.AddWithValue("@StartingDate", PostponementAppDTO.StartingDate);
                        command.Parameters.AddWithValue("@EndingDate", PostponementAppDTO.EndingDate);
                        command.Parameters.AddWithValue("@Reason", PostponementAppDTO.Reason);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedId))
                        {
                            PostponementStudyAppId = InsertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a New Postponement Study App. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return PostponementStudyAppId;
        }
        public static bool UpdatePostponementStudyApp(clsPostponementStudyAppDTO PostponementAppDTO)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdatePostponementStudyApp", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@PostponementApplicationID", PostponementAppDTO.PostponementAppID);
                        command.Parameters.AddWithValue("@StartingDate", PostponementAppDTO.StartingDate);
                        command.Parameters.AddWithValue("@EndingDate", PostponementAppDTO.EndingDate);
                        command.Parameters.AddWithValue("@Reason", PostponementAppDTO.Reason);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"The record for Postponement Study App ID: {PostponementAppDTO.PostponementAppID} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdatePostponementStudyApp method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeletePostponementStudyApp(int PostponementStudyAppId)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeletePostponementStudyApp", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PostponementApplicationID", PostponementStudyAppId);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"Postponement Study with ID: {PostponementStudyAppId} deleted successfully",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeletePostponementStudyApp method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static clsPostponementStudyAppDTO GetPostponementStudyAppInfoByID(int PostponementStudyAppId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetPostponementStudyAppInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PostponementApplicationID", PostponementStudyAppId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsPostponementStudyAppDTO(
                                    reader.GetInt32(reader.GetOrdinal("PostponementApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("StudentID")),
                                    reader.GetDateTime(reader.GetOrdinal("StartingDate")),
                                    reader.GetDateTime(reader.GetOrdinal("EndingDate")),
                                    reader.GetString(reader.GetOrdinal("Reason"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetPostponementStudyAppInfoByID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsPostponementStudyAppDTO> GetAllPostponementStudyApplications()
        {
            var PostponementStudyApplications = new List<clsPostponementStudyAppDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllPostponementStudyApplications", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PostponementStudyApplications.Add(new clsPostponementStudyAppDTO(
                                    reader.GetInt32(reader.GetOrdinal("PostponementApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("ApplicationID")),
                                    reader.GetInt32(reader.GetOrdinal("StudentID")),
                                    reader.GetDateTime(reader.GetOrdinal("StartingDate")),
                                    reader.GetDateTime(reader.GetOrdinal("EndingDate")),
                                    reader.GetString(reader.GetOrdinal("Reason"))
                                    ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllPostponementStudyApplications method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return PostponementStudyApplications;
        }
    }
}