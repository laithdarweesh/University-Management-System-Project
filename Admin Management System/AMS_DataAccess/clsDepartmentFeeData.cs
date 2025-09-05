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
    public class clsDepartmentFeeDTO
    {
        public int DepartmentFeesID { get; set; }
        public int DepartmentID { get; set; }
        public int ProgramTypeID { get; set; }
        public decimal Fee { get; set; }
        public clsDepartmentFeeDTO(int DepartmentFeesID, int DepartmentID, int ProgramTypeID, decimal Fee)
        {
            this.DepartmentFeesID = DepartmentFeesID;
            this.DepartmentID = DepartmentID;
            this.ProgramTypeID = ProgramTypeID;
            this.Fee = Fee;
        }
    }

    public class clsDepartmentFeeData
    {
        public static int AddNewDepartmentFee(clsDepartmentFeeDTO DptFeeDTO)
        {
            int DepartmentFeeId = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewDepartmentFee", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DepartmentID", DptFeeDTO.DepartmentID);
                        command.Parameters.AddWithValue("@ProgramTypeID", DptFeeDTO.ProgramTypeID);
                        command.Parameters.AddWithValue("@Fee", DptFeeDTO.Fee);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedId))
                        {
                            DepartmentFeeId = InsertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a new Department Fee. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return DepartmentFeeId;
        }
        public static bool UpdateDepartmentFee(clsDepartmentFeeDTO DptFeeDTO)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateDepartmentFee", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@DepartmentFeesID", DptFeeDTO.DepartmentFeesID);
                        command.Parameters.AddWithValue("@Fee", DptFeeDTO.Fee);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"The record for Department Fee ID: {DptFeeDTO.DepartmentFeesID} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdateDepartmentFee method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeleteDepartmentFee(int DepartmentFeeId)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeleteDepartmentFee", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DepartmentFeesID", DepartmentFeeId);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"Department Fee with ID: {DepartmentFeeId} deleted successfully",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeleteDepartmentFee method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static clsDepartmentFeeDTO GetDepartmentFeeInfoByID(int DepartmentFeeId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetDepartmentFeeInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DepartmentFeesID", DepartmentFeeId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsDepartmentFeeDTO(
                                    reader.GetInt32(reader.GetOrdinal("DepartmentFeesID")),
                                    reader.GetInt32(reader.GetOrdinal("DepartmentID")),
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
                clsGlobal.SaveToEventLog($"Error occurred in GetDepartmentFeeInfoByID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsDepartmentFeeDTO> GetAllDepartmentFees()
        {
            var DepartmentFees = new List<clsDepartmentFeeDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllDepartmentFees", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DepartmentFees.Add(new clsDepartmentFeeDTO(
                                        reader.GetInt32(reader.GetOrdinal("DepartmentFeesID")),
                                        reader.GetInt32(reader.GetOrdinal("DepartmentID")),
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
                clsGlobal.SaveToEventLog($"Error occurred in GetAllDepartmentFees method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return DepartmentFees;
        }
    }
}