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
    public class clsCollegesDTO
    { 
        public int CollegeID {  get; set; }
        public string CollegeName {  get; set; }
        public clsCollegesDTO(int CollegeID, string CollegeName)
        {
            this.CollegeID = CollegeID;
            this.CollegeName = CollegeName;
        }
    }
    public class clsCollegeData
    {
        public static int AddNewCollege(clsCollegesDTO ClgDTO)
        {
            int CollegeID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewCollege", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CollegeName", ClgDTO.CollegeName);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedId))
                        {
                            CollegeID = InsertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a new College. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return CollegeID;
        }
        public static bool UpdateCollege(clsCollegesDTO ClgDTO)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateCollege", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CollegeID", ClgDTO.CollegeID);
                        command.Parameters.AddWithValue("@CollegeName", ClgDTO.CollegeName);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"The record for College ID: {ClgDTO.CollegeID} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdateCollege method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeleteCollege(int CollegeId)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeleteCollege", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CollegeID", CollegeId);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"College with ID: {CollegeId} deleted successfully",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeleteCollege method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static clsCollegesDTO GetCollegeInfoByID(int CollegeId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetCollegeInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CollegeID", CollegeId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsCollegesDTO(
                                    reader.GetInt32(reader.GetOrdinal("CollegeID")),
                                    reader.GetString(reader.GetOrdinal("CollegeName"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetCollegeInfoByID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsCollegesDTO> GetAllColleges()
        {
            var Colleges = new List<clsCollegesDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllColleges", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Colleges.Add(new clsCollegesDTO(
                                        reader.GetInt32(reader.GetOrdinal("CollegeID")),
                                        reader.GetString(reader.GetOrdinal("CollegeName"))
                                    ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllColleges method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return Colleges;
        }
    }
}