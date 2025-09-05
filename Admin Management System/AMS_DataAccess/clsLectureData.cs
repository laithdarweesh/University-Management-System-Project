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
    public class clsLectureDTO
    {
        public int LectureID { get; set; }
        public int StudentSemesterMaterialID { get; set; }
        public bool IsStudentAttended { get; set; }
        public DateTime? LectureDateTime { get; set; }
        public clsLectureDTO(int LectureID, int StudentSemesterMaterialID, bool IsStudentAttended,
                             DateTime? LectureDateTime)
        {
            this.LectureID = LectureID;
            this.StudentSemesterMaterialID = StudentSemesterMaterialID;
            this.IsStudentAttended = IsStudentAttended;
            this.LectureDateTime = LectureDateTime;
        }
    }
    public class clsLectureData
    {
        public static int AddNewLecture(clsLectureDTO LectureDTO)
        {
            int LectureId = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewLecture", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@StudentSemesterMaterialID", LectureDTO.StudentSemesterMaterialID);
                        command.Parameters.AddWithValue("@IsStudentAttended", LectureDTO.IsStudentAttended);
                        command.Parameters.AddWithValue("@LectureDateTime", clsGlobal.ToDbNull(LectureDTO.LectureDateTime));

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedId))
                        {
                            LectureId = InsertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a New Lecture. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return LectureId;
        }
        public static bool UpdateLecture(clsLectureDTO LectureDTO)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateLecture", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@LectureID", LectureDTO.LectureID);
                        command.Parameters.AddWithValue("@IsStudentAttended", LectureDTO.IsStudentAttended);
                        command.Parameters.AddWithValue("@LectureDateTime", clsGlobal.ToDbNull(LectureDTO.LectureDateTime));

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"The record for Lecture ID: {LectureDTO.LectureID} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdateLecture method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeleteLecture(int LectureId)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeleteLecture", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@LectureID", LectureId);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"Lecture with ID: {LectureId} deleted successfully",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeleteLecture method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static clsLectureDTO GetLectureInfoByID(int LectureId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetLectureInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@LectureID", LectureId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsLectureDTO(
                                    reader.GetInt32(reader.GetOrdinal("LectureID")),
                                    reader.GetInt32(reader.GetOrdinal("StudentSemesterMaterialID")),
                                    reader.GetBoolean(reader.GetOrdinal("IsStudentAttended")),

                                    reader.IsDBNull(reader.GetOrdinal("LectureDateTime")) ?
                                    (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("LectureDateTime"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetLectureInfoByID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsLectureDTO> GetAllLectures()
        {
            var Lectures = new List<clsLectureDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllLectures", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Lectures.Add(new clsLectureDTO(
                                    reader.GetInt32(reader.GetOrdinal("LectureID")),
                                    reader.GetInt32(reader.GetOrdinal("StudentSemesterMaterialID")),
                                    reader.GetBoolean(reader.GetOrdinal("IsStudentAttended")),

                                    reader.IsDBNull(reader.GetOrdinal("LectureDateTime")) ?
                                    (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("LectureDateTime"))
                                    ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllLectures method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return Lectures;
        }
    }
}