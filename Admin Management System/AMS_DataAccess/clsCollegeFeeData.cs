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
    public class clsCollegeFeeDTO
    {
        public int CollegeFeeID {  get; set; }
        public int CollegeID {  get; set; }
        public int ProgramTypeID {  get; set; }
        public decimal Fee {  get; set; }
        public clsCollegeFeeDTO(int CollegeFeeID, int CollegeID, int ProgramTypeID, decimal Fee)
        {
            this.CollegeFeeID = CollegeFeeID;
            this.CollegeID = CollegeID;
            this.ProgramTypeID = ProgramTypeID;
            this.Fee = Fee;
        }

    }
    public class clsCollegeFeeData
    {
        public static int AddNewCollegeFee(clsCollegeFeeDTO ClgFDTO)
        {
            int CollegeFeeID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewCollegeFee", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CollegeID", ClgFDTO.CollegeID);
                        command.Parameters.AddWithValue("@ProgramTypeID", ClgFDTO.ProgramTypeID);
                        command.Parameters.AddWithValue("@Fees", ClgFDTO.Fee);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedId))
                        {
                            CollegeFeeID = InsertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a new College Fee. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return CollegeFeeID;
        }
        public static bool UpdateCollegeFee(clsCollegeFeeDTO ClgFDTO)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateCollegeFee", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@CollegeFeesID", ClgFDTO.CollegeFeeID);
                        command.Parameters.AddWithValue("@Fees", ClgFDTO.Fee);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"The record for College Fee ID: {ClgFDTO.CollegeFeeID} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdateCollegeFee method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeleteCollegeFee(int CollegeFeeId)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeleteCollegeFee", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CollegeFeesID", CollegeFeeId);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"College Fee with ID: {CollegeFeeId} deleted successfully",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeleteCollegeFee method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static clsCollegeFeeDTO GetCollegeFeeInfoByID(int CollegeFeeId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetCollegeFeeInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CollegeFeeID", CollegeFeeId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsCollegeFeeDTO(
                                    reader.GetInt32(reader.GetOrdinal("CollegeFeesID")),
                                    reader.GetInt32(reader.GetOrdinal("CollegeID")),
                                    reader.GetInt32(reader.GetOrdinal("ProgramTypeID")),
                                    reader.GetDecimal(reader.GetOrdinal("Fees"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetCollegeFeeInfoByID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsCollegeFeeDTO> GetAllCollegeFees()
        {
            var CollegeFees = new List<clsCollegeFeeDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllCollegeFees", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CollegeFees.Add(new clsCollegeFeeDTO(
                                        reader.GetInt32(reader.GetOrdinal("CollegeFeesID")),
                                        reader.GetInt32(reader.GetOrdinal("CollegeID")),
                                        reader.GetInt32(reader.GetOrdinal("ProgramTypeID")),
                                        reader.GetDecimal(reader.GetOrdinal("Fees"))
                                    ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllCollegeFees method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return CollegeFees;
        }
    }
}