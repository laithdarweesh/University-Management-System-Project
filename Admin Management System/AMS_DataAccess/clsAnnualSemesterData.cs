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
    public class clsAnnualSemesterDTO
    { 
        public int AnnualSemesterID { get; set; }
        public string SemesterName { get; set; }
        public clsAnnualSemesterDTO(int AnnualSemesterID, string SemesterName) 
        {
            this.AnnualSemesterID = AnnualSemesterID;
            this.SemesterName = SemesterName;
        }
    }
    public class clsAnnualSemesterData
    {
        public static int AddNewAnnualSemester(clsAnnualSemesterDTO AnnualSmtDTO)
        {
            int AnnualSemesterId = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewAnnualSemester", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@SemesterName", AnnualSmtDTO.SemesterName);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedId))
                        {
                            AnnualSemesterId = InsertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a new Annual Semester. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return AnnualSemesterId;
        }
        public static bool UpdateAnnualSemester(clsAnnualSemesterDTO AnnualSmtDTO)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateAnnualSemester", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@AnnualSemesterID", AnnualSmtDTO.AnnualSemesterID);
                        command.Parameters.AddWithValue("@SemesterName", AnnualSmtDTO.SemesterName);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"The record for Annual Semester ID: {AnnualSmtDTO.AnnualSemesterID} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdateAnnualSemester method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static clsAnnualSemesterDTO GetAnnualSemesterInfoByID(int AnnualSemesterId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAnnualSemesterInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AnnualSemesterID", AnnualSemesterId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsAnnualSemesterDTO(
                                    reader.GetInt32(reader.GetOrdinal("AnnualSemesterID")),
                                    reader.GetString(reader.GetOrdinal("SemesterName"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetAnnualSemesterInfoByID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsAnnualSemesterDTO> GetAllAnnualSemesters()
        {
            var AnnualSemesters = new List<clsAnnualSemesterDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllAnnualSemesters", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AnnualSemesters.Add(new clsAnnualSemesterDTO(
                                        reader.GetInt32(reader.GetOrdinal("AnnualSemesterID")),
                                        reader.GetString(reader.GetOrdinal("SemesterName"))
                                    ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllAnnualSemesters method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return AnnualSemesters;
        }
    }
}