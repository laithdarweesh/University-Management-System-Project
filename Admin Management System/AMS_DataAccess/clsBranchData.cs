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
    public class clsBranchDTO
    {
        public int BranchID { get; set; }
        public string BranchNumber { get; set; }
        public int DepartmentID { get; set; }
        public byte? DaysOfWeek { get; set; }
        public TimeSpan? Time { get; set; }
        public byte? Capacity { get; set; }
        public clsBranchDTO(int BranchID, string BranchNumber, int DepartmentID, byte? DaysOfWeek,
                            TimeSpan? Time, byte? Capacity)
        {
            this.BranchID = BranchID;
            this.BranchNumber = BranchNumber;
            this.DepartmentID = DepartmentID;
            this.DaysOfWeek = DaysOfWeek;
            this.Time = Time;
            this.Capacity = Capacity;
        }
    }
    public class clsBranchData
    {
        public static int AddNewBranch(clsBranchDTO BranchDTO)
        {
            int BranchId = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewBranch", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@BranchNumber", BranchDTO.BranchNumber);
                        command.Parameters.AddWithValue("@DepartmentID", BranchDTO.DepartmentID);
                            
                        command.Parameters.AddWithValue("@DaysOfWeek", clsGlobal.ToDbNull(BranchDTO.DaysOfWeek));
                        command.Parameters.AddWithValue("@Time", clsGlobal.ToDbNull(BranchDTO.Time));
                        command.Parameters.AddWithValue("@Capacity", clsGlobal.ToDbNull(BranchDTO.Capacity));

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedId))
                        {
                            BranchId = InsertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a new Branch. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return BranchId;
        }
        public static bool UpdateBranch(clsBranchDTO BranchDTO)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateBranch", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@BranchID", BranchDTO.BranchID);
                        command.Parameters.AddWithValue("@BranchNumber", BranchDTO.BranchNumber);
                        command.Parameters.AddWithValue("@DepartmentID", BranchDTO.DepartmentID);

                        command.Parameters.AddWithValue("@DaysOfWeek", clsGlobal.ToDbNull(BranchDTO.DaysOfWeek));
                        command.Parameters.AddWithValue("@Time", clsGlobal.ToDbNull(BranchDTO.Time));
                        command.Parameters.AddWithValue("@Capacity", clsGlobal.ToDbNull(BranchDTO.Capacity));

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"The record for Branch ID: {BranchDTO.BranchID} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdateBranch method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeleteBranch(int BranchId)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeleteBranch", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@BranchID", BranchId);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"Branch with ID: {BranchId} deleted successfully",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeleteBranch method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static clsBranchDTO GetBranchInfoByID(int BranchId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetBranchInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@BranchID", BranchId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsBranchDTO(
                                    reader.GetInt32(reader.GetOrdinal("BranchID")),
                                    reader.GetString(reader.GetOrdinal("BranchNumber")),
                                    reader.GetInt32(reader.GetOrdinal("DepartmentID")),

                                    reader.IsDBNull(reader.GetOrdinal("DaysOfWeek")) ? (byte?) null :
                                    reader.GetByte(reader.GetOrdinal("DaysOfWeek")),

                                    reader.IsDBNull(reader.GetOrdinal("Time")) ? (TimeSpan?) null :
                                    reader.GetTimeSpan(reader.GetOrdinal("Time")),

                                    reader.IsDBNull(reader.GetOrdinal("Capacity")) ? (byte?) null :
                                    reader.GetByte(reader.GetOrdinal("Capacity"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetBranchInfoByID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsBranchDTO> GetAllBranches()
        {
            var Branches = new List<clsBranchDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllBranches", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Branches.Add(new clsBranchDTO(
                                        reader.GetInt32(reader.GetOrdinal("BranchID")),
                                        reader.GetString(reader.GetOrdinal("BranchNumber")),
                                        reader.GetInt32(reader.GetOrdinal("DepartmentID")),

                                        reader.IsDBNull(reader.GetOrdinal("DaysOfWeek")) ? (byte?)null :
                                        reader.GetByte(reader.GetOrdinal("DaysOfWeek")),

                                        reader.IsDBNull(reader.GetOrdinal("Time")) ? (TimeSpan?)null :
                                        reader.GetTimeSpan(reader.GetOrdinal("Time")),

                                        reader.IsDBNull(reader.GetOrdinal("Capacity")) ? (byte?)null :
                                        reader.GetByte(reader.GetOrdinal("Capacity"))
                                    ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllBranches method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return Branches;
        }
    }
}