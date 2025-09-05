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
    public class clsHourPriceDTO
    {
        public int HourPriceID { get; set; }
        public int DepartmentID { get; set; }
        public int ProgramTypeID { get; set; }
        public decimal Price { get; set; }
        public clsHourPriceDTO(int HourPriceID, int DepartmentID, int ProgramTypeID, decimal Price) 
        {
            this.HourPriceID = HourPriceID; 
            this.DepartmentID = DepartmentID;
            this.ProgramTypeID = ProgramTypeID;
            this.Price = Price;
        }
    }
    public class clsHourPriceData
    {
        public static int AddNewHourPrice(clsHourPriceDTO HourPriceDTO)
        {
            int HourPriceId = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewHourPrice", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DepartmentID", HourPriceDTO.DepartmentID);
                        command.Parameters.AddWithValue("@ProgramTypeID", HourPriceDTO.ProgramTypeID);
                        command.Parameters.AddWithValue("@Price", HourPriceDTO.Price);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedId))
                        {
                            HourPriceId = InsertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a new Hour Price Fee. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return HourPriceId;
        }
        public static bool UpdateHourPrice(clsHourPriceDTO HourPriceDTO)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateHourPrice", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@HourPriceID", HourPriceDTO.HourPriceID);
                        command.Parameters.AddWithValue("@Price", HourPriceDTO.Price);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"The record for Hour Price ID: {HourPriceDTO.HourPriceID} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdateHourPrice method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeleteHourPrice(int HourPriceId)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeleteHourPrice", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@HourPriceID", HourPriceId);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"Hour Price with ID: {HourPriceId} deleted successfully",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeleteHourPrice method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static clsHourPriceDTO GetHourPriceInfoByID(int HourPriceId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetHourPriceInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@HourPriceID", HourPriceId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsHourPriceDTO(
                                    reader.GetInt32(reader.GetOrdinal("HourPriceID")),
                                    reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                                    reader.GetInt32(reader.GetOrdinal("ProgramTypeID")),
                                    reader.GetDecimal(reader.GetOrdinal("Price"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetHourPriceInfoByID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsHourPriceDTO> GetAllHourPrices()
        {
            var HourPrices = new List<clsHourPriceDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllHourPrices", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                HourPrices.Add(new clsHourPriceDTO(
                                        reader.GetInt32(reader.GetOrdinal("HourPriceID")),
                                        reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                                        reader.GetInt32(reader.GetOrdinal("ProgramTypeID")),
                                        reader.GetDecimal(reader.GetOrdinal("Price"))
                                    ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllHourPrices method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return HourPrices;
        }
    }
}