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
    public class clsStudentSemesterCaseDTO
    {
        public int StudentSemesterCaseID { get; set; }
        public int StudentID { get; set; }
        public byte? SemesterHours { get; set; } 
        public byte? PassedHours { get; set; } 
        public float? SemesterAverage { get; set; } 
        public int SemesterRankID { get; set; } 
        public int AnnualSemesterID { get; set; } 
        public short? Year { get; set; } 
        public decimal? SemesterHoursFees { get; set; } 
        public decimal? PaidSemesterHoursFees { get; set; } 
        public bool? PaySemesterHoursFees { get; set; } 
        public decimal? LateFineFees { get; set; } 
        public byte Status { get; set; }
        public DateTime LastStatusDate { get; set; }
        public clsStudentSemesterCaseDTO(int StudentSemesterCaseID, int StudentID, byte? SemesterHours,
                                         byte? PassedHours, float? SemesterAverage, int SemesterRankID,
                                         int AnnualSemesterID, short? Year, decimal? SemesterHoursFees,
                                         decimal? PaidSemesterHoursFees, bool? PaySemesterHoursFees,
                                         decimal? LateFineFees, byte Status, DateTime LastStatusDate)
        {
            this.StudentSemesterCaseID = StudentSemesterCaseID;
            this.StudentID = StudentID;
            this.SemesterHours = SemesterHours;
            this.PassedHours = PassedHours;
            this.SemesterAverage = SemesterAverage;
            this.SemesterRankID = SemesterRankID;
            this.AnnualSemesterID = AnnualSemesterID;
            this.Year = Year;
            this.SemesterHoursFees = SemesterHoursFees;
            this.PaidSemesterHoursFees = PaidSemesterHoursFees;
            this.PaySemesterHoursFees = PaySemesterHoursFees;
            this.LateFineFees = LateFineFees;
            this.Status = Status;
            this.LastStatusDate = LastStatusDate;
        }
    }
    public class clsStudentSemesterCaseData
    {
        public static int AddNewStudentSemesterCase(clsStudentSemesterCaseDTO StdSemesterCaseDTO)
        {
            int StudentSemesterCaseId = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewStudentSemesterCase", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@StudentID", StdSemesterCaseDTO.StudentID);
                        command.Parameters.AddWithValue("@SemesterHours", clsGlobal.ToDbNull(StdSemesterCaseDTO.SemesterHours));
                        command.Parameters.AddWithValue("@PassedHours", clsGlobal.ToDbNull(StdSemesterCaseDTO.PassedHours));
                        command.Parameters.AddWithValue("@SemesterAverage", clsGlobal.ToDbNull(StdSemesterCaseDTO.SemesterAverage));
                        command.Parameters.AddWithValue("@SemesterRankID", StdSemesterCaseDTO.SemesterRankID);
                        command.Parameters.AddWithValue("@AnnualSemesterID", StdSemesterCaseDTO.AnnualSemesterID);
                        command.Parameters.AddWithValue("@Year", clsGlobal.ToDbNull(StdSemesterCaseDTO.Year));
                        command.Parameters.AddWithValue("@SemesterHoursFees", clsGlobal.ToDbNull(StdSemesterCaseDTO.SemesterHoursFees));
                        command.Parameters.AddWithValue("@PaidSemesterHoursFees", clsGlobal.ToDbNull(StdSemesterCaseDTO.PaidSemesterHoursFees));
                        command.Parameters.AddWithValue("@PaySemesterHoursFees", clsGlobal.ToDbNull(StdSemesterCaseDTO.PaySemesterHoursFees));
                        command.Parameters.AddWithValue("@LateFineFees", clsGlobal.ToDbNull(StdSemesterCaseDTO.LateFineFees));
                        command.Parameters.AddWithValue("@Status", StdSemesterCaseDTO.Status);
                        command.Parameters.AddWithValue("@LastStatusDate", StdSemesterCaseDTO.LastStatusDate);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedId))
                        {
                            StudentSemesterCaseId = InsertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a New Student Semester Case. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return StudentSemesterCaseId;
        }
        public static bool UpdateStudentSemesterCase(clsStudentSemesterCaseDTO StdSemesterCaseDTO)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateStudentSemesterCase", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@StudentSemesterCaseID", StdSemesterCaseDTO.StudentSemesterCaseID);
                        command.Parameters.AddWithValue("@StudentID", StdSemesterCaseDTO.StudentID);
                        command.Parameters.AddWithValue("@SemesterHours", clsGlobal.ToDbNull(StdSemesterCaseDTO.SemesterHours));
                        command.Parameters.AddWithValue("@PassedHours", clsGlobal.ToDbNull(StdSemesterCaseDTO.PassedHours));
                        command.Parameters.AddWithValue("@SemesterAverage", clsGlobal.ToDbNull(StdSemesterCaseDTO.SemesterAverage));
                        command.Parameters.AddWithValue("@SemesterRankID", StdSemesterCaseDTO.SemesterRankID);
                        command.Parameters.AddWithValue("@AnnualSemesterID", StdSemesterCaseDTO.AnnualSemesterID);
                        command.Parameters.AddWithValue("@Year", clsGlobal.ToDbNull(StdSemesterCaseDTO.Year));
                        command.Parameters.AddWithValue("@SemesterHoursFees", clsGlobal.ToDbNull(StdSemesterCaseDTO.SemesterHoursFees));
                        command.Parameters.AddWithValue("@PaidSemesterHoursFees", clsGlobal.ToDbNull(StdSemesterCaseDTO.PaidSemesterHoursFees));
                        command.Parameters.AddWithValue("@PaySemesterHoursFees", clsGlobal.ToDbNull(StdSemesterCaseDTO.PaySemesterHoursFees));
                        command.Parameters.AddWithValue("@LateFineFees", clsGlobal.ToDbNull(StdSemesterCaseDTO.LateFineFees));
                        command.Parameters.AddWithValue("@Status", StdSemesterCaseDTO.Status);
                        command.Parameters.AddWithValue("@LastStatusDate", StdSemesterCaseDTO.LastStatusDate);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"The record for Graduate Student ID: {StdSemesterCaseDTO.StudentSemesterCaseID} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdateStudentSemesterCase method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeleteStudentSemesterCase(int StudentSemesterCaseId)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeleteStudentSemesterCase", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StudentSemesterCaseID", StudentSemesterCaseId);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"Student Semester Case with ID: {StudentSemesterCaseId} deleted successfully",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeleteStudentSemesterCase method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static clsStudentSemesterCaseDTO GetStudentSemesterCaseInfoByID(int StudentSemesterCaseId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetStudentSemesterCaseInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StudentSemesterCaseID", StudentSemesterCaseId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsStudentSemesterCaseDTO(
                                    reader.GetInt32(reader.GetOrdinal("StudentSemesterCaseID")),
                                    reader.GetInt32(reader.GetOrdinal("StudentID")),

                                    reader.IsDBNull(reader.GetOrdinal("SemesterHours")) ?
                                    (byte?)null : reader.GetByte(reader.GetOrdinal("SemesterHours")),

                                    reader.IsDBNull(reader.GetOrdinal("PassedHours")) ?
                                    (byte?)null : reader.GetByte(reader.GetOrdinal("PassedHours")),

                                    reader.IsDBNull(reader.GetOrdinal("SemesterAverage")) ?
                                    (float?)null : reader.GetFloat(reader.GetOrdinal("SemesterAverage")),

                                    reader.GetInt32(reader.GetOrdinal("SemesterRankID")),
                                    reader.GetInt32(reader.GetOrdinal("AnnualSemesterID")),

                                    reader.IsDBNull(reader.GetOrdinal("Year")) ?
                                    (short?)null : reader.GetInt16(reader.GetOrdinal("Year")),

                                    reader.IsDBNull(reader.GetOrdinal("SemesterHoursFees")) ?
                                    (decimal?)null : reader.GetDecimal(reader.GetOrdinal("SemesterHoursFees")),

                                    reader.IsDBNull(reader.GetOrdinal("PaidSemesterHoursFees")) ?
                                    (decimal?)null : reader.GetDecimal(reader.GetOrdinal("PaidSemesterHoursFees")),

                                    reader.IsDBNull(reader.GetOrdinal("PaySemesterHoursFees")) ?
                                    (Boolean?)null : reader.GetBoolean(reader.GetOrdinal("PaySemesterHoursFees")),

                                    reader.IsDBNull(reader.GetOrdinal("LateFineFees")) ?
                                    (decimal?)null : reader.GetDecimal(reader.GetOrdinal("LateFineFees")),

                                    reader.GetByte(reader.GetOrdinal("Status")),
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
                clsGlobal.SaveToEventLog($"Error occurred in GetStudentSemesterCaseInfoByID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsStudentSemesterCaseDTO> GetAllStudentSemesterCases()
        {
            var StudentSemesterCases = new List<clsStudentSemesterCaseDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllStudentSemesterCases", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                StudentSemesterCases.Add(new clsStudentSemesterCaseDTO(
                                    reader.GetInt32(reader.GetOrdinal("StudentSemesterCaseID")),
                                    reader.GetInt32(reader.GetOrdinal("StudentID")),

                                    reader.IsDBNull(reader.GetOrdinal("SemesterHours")) ?
                                    (byte?)null : reader.GetByte(reader.GetOrdinal("SemesterHours")),

                                    reader.IsDBNull(reader.GetOrdinal("PassedHours")) ?
                                    (byte?)null : reader.GetByte(reader.GetOrdinal("PassedHours")),

                                    reader.IsDBNull(reader.GetOrdinal("SemesterAverage")) ?
                                    (float?)null : reader.GetFloat(reader.GetOrdinal("SemesterAverage")),

                                    reader.GetInt32(reader.GetOrdinal("SemesterRankID")),
                                    reader.GetInt32(reader.GetOrdinal("AnnualSemesterID")),

                                    reader.IsDBNull(reader.GetOrdinal("Year")) ?
                                    (short?)null : reader.GetInt16(reader.GetOrdinal("Year")),

                                    reader.IsDBNull(reader.GetOrdinal("SemesterHoursFees")) ?
                                    (decimal?)null : reader.GetDecimal(reader.GetOrdinal("SemesterHoursFees")),

                                    reader.IsDBNull(reader.GetOrdinal("PaidSemesterHoursFees")) ?
                                    (decimal?)null : reader.GetDecimal(reader.GetOrdinal("PaidSemesterHoursFees")),

                                    reader.IsDBNull(reader.GetOrdinal("PaySemesterHoursFees")) ?
                                    (Boolean?)null : reader.GetBoolean(reader.GetOrdinal("PaySemesterHoursFees")),

                                    reader.IsDBNull(reader.GetOrdinal("LateFineFees")) ?
                                    (decimal?)null : reader.GetDecimal(reader.GetOrdinal("LateFineFees")),

                                    reader.GetByte(reader.GetOrdinal("Status")),
                                    reader.GetDateTime(reader.GetOrdinal("LastStatusDate"))
                                    ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllStudentSemesterCases method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return StudentSemesterCases;
        }
    }
}