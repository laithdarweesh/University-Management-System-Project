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
    public class clsApplicationTypeDTO
    { 
        public int ApplicationTypeID {  get; set; }
        public string ApplicationTypeTitle {  get; set; }
        public decimal ApplicationFees {  get; set; }
        public clsApplicationTypeDTO(int AppTypeID, string AppTypeTitle, decimal AppFees)
        {
            this.ApplicationTypeID = AppTypeID;
            this.ApplicationTypeTitle = AppTypeTitle;
            this.ApplicationFees = AppFees;
        }
    }
    public class clsApplicationTypeData
    {
        public static int AddNewApplicationType(clsApplicationTypeDTO AppTypeDTO)
        {
            int AppTypeId = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewApplicationType", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AppTypeTitle", AppTypeDTO.ApplicationTypeTitle);
                        command.Parameters.AddWithValue("@AppFees", AppTypeDTO.ApplicationFees);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedId))
                        {
                            AppTypeId = InsertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a new application type. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return AppTypeId;
        }
        public static bool UpdateApplicationType(clsApplicationTypeDTO AppTypeDTO)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateApplicationType", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ApplicationTypeID", AppTypeDTO.ApplicationTypeID);
                        command.Parameters.AddWithValue("@AppTypeTitle", AppTypeDTO.ApplicationTypeTitle);
                        command.Parameters.AddWithValue("@AppFees", AppTypeDTO.ApplicationFees);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"The record for App Type ID: {AppTypeDTO.ApplicationTypeID} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdateApplicationType method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeleteApplicationType(int AppTypeId)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeleteApplicationType", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ApplicationTypeID", AppTypeId);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"App Type with ID: {AppTypeId} deleted successfully",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeleteApplicationType method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static clsApplicationTypeDTO GetApplicationTypeInfoByID(int AppTypeId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetApplicationTypeInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ApplicationTypeID", AppTypeId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsApplicationTypeDTO(
                                    reader.GetInt32(reader.GetOrdinal("ApplicationTypeID")),
                                    reader.GetString(reader.GetOrdinal("ApplicationTypeTitle")),
                                    reader.GetDecimal(reader.GetOrdinal("ApplicationFees"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetApplicationTypeInfoByID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsApplicationTypeDTO> GetAllApplicationTypes()
        {
            var AppTypes = new List<clsApplicationTypeDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllApplicationTypes", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AppTypes.Add(new clsApplicationTypeDTO(
                                        reader.GetInt32(reader.GetOrdinal("ApplicationTypeID")),
                                        reader.GetString(reader.GetOrdinal("ApplicationTypeTitle")),
                                        reader.GetDecimal(reader.GetOrdinal("ApplicationFees"))
                                        )
                                    );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllApplicationTypes method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return AppTypes;
        }
    }
}