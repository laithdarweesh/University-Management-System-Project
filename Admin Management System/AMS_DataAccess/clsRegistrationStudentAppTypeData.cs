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
    public class clsRegistrationStudentAppTypeDTO
    {
        public int RegistrationStudentAppTypeID { get; set; }
        public string RegistrationStudentAppTypeTitle { get; set; }
        public clsRegistrationStudentAppTypeDTO(int RegistrationStudentAppTypeID, string RegistrationStudentAppTypeTitle)
        {
            this.RegistrationStudentAppTypeID = RegistrationStudentAppTypeID;
            this.RegistrationStudentAppTypeTitle = RegistrationStudentAppTypeTitle;
        }
    }
    public class clsRegistrationStudentAppTypeData
    {
        public static int AddNewRegistrationStudentAppType(clsRegistrationStudentAppTypeDTO RegistrationAppTypeDTO)
        {
            int RegistrationStudentAppTypeID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewRegistrationStudentAppType", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@RegistrationStudentAppTypeTitle", RegistrationAppTypeDTO.RegistrationStudentAppTypeTitle);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedId))
                        {
                            RegistrationStudentAppTypeID = InsertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a New Registration Student App Type. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return RegistrationStudentAppTypeID;
        }
        public static bool UpdateRegistrationStudentAppType(clsRegistrationStudentAppTypeDTO RegistrationAppTypeDTO)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateRegistrationStudentAppType", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@RegistrationStudentApplicationTypeID", RegistrationAppTypeDTO.RegistrationStudentAppTypeID);
                        command.Parameters.AddWithValue("@RegistrationStudentAppTypeTitle", RegistrationAppTypeDTO.RegistrationStudentAppTypeTitle);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"The record for Registration Student App Type ID: {RegistrationAppTypeDTO.RegistrationStudentAppTypeID} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdateRegistrationStudentAppType method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeleteRegistrationStudentAppType(int RegistrationStudentAppTypeID)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeleteRegistrationStudentAppType", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@RegistrationStudentApplicationTypeID", RegistrationStudentAppTypeID);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"Registration Student App Type with ID: {RegistrationStudentAppTypeID} deleted successfully",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeleteRegistrationStudentAppType method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static clsRegistrationStudentAppTypeDTO GetRegistrationStudentAppTypeByID(int RegistrationStudentAppTypeID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetRegistrationStudentAppTypeByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@RegistrationStudentApplicationTypeID", RegistrationStudentAppTypeID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsRegistrationStudentAppTypeDTO(
                                    reader.GetInt32(reader.GetOrdinal("RegistrationStudentApplicationTypeID")),
                                    reader.GetString(reader.GetOrdinal("RegistrationStudentApplicationTypeTitle"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetRegistrationStudentAppTypeByID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsRegistrationStudentAppTypeDTO> GetAllRegistrationStudentAppTypes()
        {
            var RegistrationStudentAppTypes = new List<clsRegistrationStudentAppTypeDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllRegistrationStudentAppTypes", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                RegistrationStudentAppTypes.Add(new clsRegistrationStudentAppTypeDTO(
                                    reader.GetInt32(reader.GetOrdinal("RegistrationStudentApplicationTypeID")),
                                    reader.GetString(reader.GetOrdinal("RegistrationStudentApplicationTypeTitle"))
                                    ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllRegistrationStudentAppTypes method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return RegistrationStudentAppTypes;
        }
    }
}