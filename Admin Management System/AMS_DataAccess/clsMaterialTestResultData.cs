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
    public class clsMaterialTestResultDTO
    {
        public int TestResultID { get; set; }
        public int TestAppointmentID { get; set; }
        public int StudentID { get; set; }
        public byte Result { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByDoctorID { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int ModifiedByDoctorID { get; set; }
        public bool? IsAccredited { get; set; }
        public clsMaterialTestResultDTO(int TestResultID, int TestAppointmentID, int StudentID, byte Result, DateTime CreatedDate,
            int CreatedByDoctorID, DateTime LastModifiedDate, int ModifiedByDoctorID, bool? IsAccredited)
        {
            this.TestResultID = TestResultID;
            this.TestAppointmentID = TestAppointmentID;
            this.StudentID = StudentID;
            this.Result = Result;
            this.CreatedDate = CreatedDate;
            this.CreatedByDoctorID = CreatedByDoctorID;
            this.LastModifiedDate = LastModifiedDate;
            this.ModifiedByDoctorID = ModifiedByDoctorID;
            this.IsAccredited = IsAccredited;
        }
    }
    public class clsMaterialTestResultData
    {
        public static int AddNewMaterialTestResult(clsMaterialTestResultDTO MaterialResultDTO)
        {
            int TestResultID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewMaterialTestResult", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@TestAppointmentID", MaterialResultDTO.TestAppointmentID);
                        command.Parameters.AddWithValue("@StudentID", MaterialResultDTO.StudentID);
                        command.Parameters.AddWithValue("@Result", MaterialResultDTO.Result);
                        command.Parameters.AddWithValue("@CreatedDate", MaterialResultDTO.CreatedDate);
                        command.Parameters.AddWithValue("@CreatedByDoctorID", MaterialResultDTO.CreatedByDoctorID);
                        command.Parameters.AddWithValue("@LastModifiedDate", MaterialResultDTO.LastModifiedDate);
                        command.Parameters.AddWithValue("@ModifiedByDoctorID", MaterialResultDTO.ModifiedByDoctorID);

                        command.Parameters.AddWithValue("@IsAccredited", clsGlobal.ToDbNull(MaterialResultDTO.IsAccredited));

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedId))
                        {
                            TestResultID = InsertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a New Material Test Result. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return TestResultID;
        }
        public static bool UpdateMaterialTestResult(clsMaterialTestResultDTO MaterialResultDTO)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateMaterialTestResult", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@TestResultID", MaterialResultDTO.TestResultID);
                        command.Parameters.AddWithValue("@Result", MaterialResultDTO.Result);
                        command.Parameters.AddWithValue("@LastModifiedDate", MaterialResultDTO.LastModifiedDate);
                        command.Parameters.AddWithValue("@ModifiedByDoctorID", MaterialResultDTO.ModifiedByDoctorID);

                        command.Parameters.AddWithValue("@IsAccredited", clsGlobal.ToDbNull(MaterialResultDTO.IsAccredited));

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"The record for Update Material Test Result ID: {MaterialResultDTO.TestResultID} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdateMaterialTestResult method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeleteMaterialTestResult(int TestResultID)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeleteMaterialTestResult", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@TestResultID", TestResultID);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"Test Result with ID: {TestResultID} deleted successfully",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeleteMaterialTestResult method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static clsMaterialTestResultDTO GetMaterialTestResultInfoByID(int TestResultID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetMaterialTestResultInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@TestResultID", TestResultID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsMaterialTestResultDTO(
                                    reader.GetInt32(reader.GetOrdinal("TestResultID")),
                                    reader.GetInt32(reader.GetOrdinal("TestAppointmentID")),
                                    reader.GetInt32(reader.GetOrdinal("StudentID")),
                                    reader.GetByte(reader.GetOrdinal("Result")),
                                    reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                    reader.GetInt32(reader.GetOrdinal("CreatedByDoctorID")),
                                    reader.GetDateTime(reader.GetOrdinal("LastModifiedDate")),
                                    reader.GetInt32(reader.GetOrdinal("ModifiedByDoctorID")),

                                    reader.IsDBNull(reader.GetOrdinal("IsAccredited")) ?
                                    (bool?)null : reader.GetBoolean(reader.GetOrdinal("IsAccredited"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetMaterialTestResultInfoByID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsMaterialTestResultDTO> GetAllMaterialTestResults()
        {
            var MaterialTestResults = new List<clsMaterialTestResultDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllMaterialTestResults", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MaterialTestResults.Add(new clsMaterialTestResultDTO(
                                    reader.GetInt32(reader.GetOrdinal("TestResultID")),
                                    reader.GetInt32(reader.GetOrdinal("TestAppointmentID")),
                                    reader.GetInt32(reader.GetOrdinal("StudentID")),
                                    reader.GetByte(reader.GetOrdinal("Result")),
                                    reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                    reader.GetInt32(reader.GetOrdinal("CreatedByDoctorID")),
                                    reader.GetDateTime(reader.GetOrdinal("LastModifiedDate")),
                                    reader.GetInt32(reader.GetOrdinal("ModifiedByDoctorID")),

                                    reader.IsDBNull(reader.GetOrdinal("IsAccredited")) ?
                                    (bool?)null : reader.GetBoolean(reader.GetOrdinal("IsAccredited"))
                                    ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllMaterialTestResults method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return MaterialTestResults;
        }
    }
}