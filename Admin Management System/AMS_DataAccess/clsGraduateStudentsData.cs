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
    public class clsGraduateStudentsDTO
    {
        public int GraduateStudentID { get; set; }
        public int StudentID { get; set; }
        public float FinalCumulativeAverage { get; set; }
        public int RankID { get; set; }
        public short GraduationYear { get; set; }
        public int GraduationAnnualSemesterID { get; set; }
        public clsGraduateStudentsDTO(int GraduateStudentID, int StudentID, float FinalCumulativeAverage,
                                      int RankID, short GraduationYear, int GraduationAnnualSemesterID)
        {
            this.GraduateStudentID = GraduateStudentID;
            this.StudentID = StudentID;
            this.FinalCumulativeAverage = FinalCumulativeAverage;
            this.RankID = RankID;
            this.GraduationYear = GraduationYear;
            this.GraduationAnnualSemesterID = GraduationAnnualSemesterID;
        }
    }
    public class clsGraduateStudentsData
    {
        public static int AddNewGraduateStudent(clsGraduateStudentsDTO GradStdDTO)
        {
            int GraduateStudentId = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewGraduateStudent", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@StudentID", GradStdDTO.StudentID);
                        command.Parameters.AddWithValue("@FinalCumulativeAverage", GradStdDTO.FinalCumulativeAverage);
                        command.Parameters.AddWithValue("@RankID", GradStdDTO.RankID);
                        command.Parameters.AddWithValue("@GraduationYear", GradStdDTO.GraduationYear);
                        command.Parameters.AddWithValue("@GraduationAnnualSemesterID", GradStdDTO.GraduationAnnualSemesterID);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedId))
                        {
                            GraduateStudentId = InsertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a New Graduate Student. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return GraduateStudentId;
        }
        public static bool UpdateGraduateStudent(clsGraduateStudentsDTO GradStdDTO)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateGraduateStudent", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@GraduateStudentID", GradStdDTO.GraduateStudentID);
                        command.Parameters.AddWithValue("@StudentID", GradStdDTO.StudentID);
                        command.Parameters.AddWithValue("@FinalCumulativeAverage", GradStdDTO.FinalCumulativeAverage);
                        command.Parameters.AddWithValue("@RankID", GradStdDTO.RankID);
                        command.Parameters.AddWithValue("@GraduationYear", GradStdDTO.GraduationYear);
                        command.Parameters.AddWithValue("@GraduationAnnualSemesterID", GradStdDTO.GraduationAnnualSemesterID);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"The record for Graduate Student ID: {GradStdDTO.GraduateStudentID} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdateGraduateStudent method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeleteGraduateStudent(int GraduateStudentId)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeleteGraduateStudent", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@GraduateStudentID", GraduateStudentId);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"Graduate Student with ID: {GraduateStudentId} deleted successfully",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeleteGraduateStudent method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static clsGraduateStudentsDTO GetGraduateStudentInfoByID(int GraduateStudentId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetGraduateStudentInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@GraduateStudentID", GraduateStudentId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsGraduateStudentsDTO(
                                    reader.GetInt32(reader.GetOrdinal("GraduateStudentID")),
                                    reader.GetInt32(reader.GetOrdinal("StudentID")),
                                    reader.GetFloat(reader.GetOrdinal("FinalCumulativeAverage")),
                                    reader.GetInt32(reader.GetOrdinal("RankID")),
                                    reader.GetInt16(reader.GetOrdinal("GraduationYear")),
                                    reader.GetInt32(reader.GetOrdinal("GraduationAnnualSemesterID"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetGraduateStudentInfoByID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsGraduateStudentsDTO> GetAllGraduateStudents()
        {
            var GraduateStudents = new List<clsGraduateStudentsDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllGraduateStudents", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                GraduateStudents.Add(new clsGraduateStudentsDTO(
                                        reader.GetInt32(reader.GetOrdinal("GraduateStudentID")),
                                        reader.GetInt32(reader.GetOrdinal("StudentID")),
                                        reader.GetFloat(reader.GetOrdinal("FinalCumulativeAverage")),
                                        reader.GetInt32(reader.GetOrdinal("RankID")),
                                        reader.GetInt16(reader.GetOrdinal("GraduationYear")),
                                        reader.GetInt32(reader.GetOrdinal("GraduationAnnualSemesterID"))
                                    ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllGraduateStudents method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return GraduateStudents;
        }
    }
}