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
    public class clsPhoneDTO
    {
        public int PhoneId { get; set; }
        public string PhoneNumber { get; set; }
        public int PersonId { get; set; }
        public clsPhoneDTO(int PhoneId, string PhoneNumber, int PersonId)
        {
            this.PhoneId = PhoneId;
            this.PhoneNumber = PhoneNumber;
            this.PersonId = PersonId;
        }
    }
    public class clsPhoneData
    {
        public static int AddNewPhoneNumber(clsPhoneDTO PhoneDTO)
        {
            int PhoneID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewPhoneNumber", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@PhoneNumber", PhoneDTO.PhoneNumber);
                        command.Parameters.AddWithValue("@PersonID", PhoneDTO.PersonId);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedId))
                        {
                            PhoneID = InsertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a new Phone Number. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return PhoneID;
        }
        public static bool UpdatePhoneNumber(clsPhoneDTO PhoneDTO)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdatePhoneNumber", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@PhoneID", PhoneDTO.PhoneId);
                        command.Parameters.AddWithValue("@PhoneNumber", PhoneDTO.PhoneNumber);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"The record for Phone ID: {PhoneDTO.PhoneId} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdatePhoneNumber method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static clsPhoneDTO GetPhoneInfoByID(int PhoneId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetPhoneInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PhoneID", PhoneId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsPhoneDTO(
                                    reader.GetInt32(reader.GetOrdinal("PhoneID")),
                                    reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    reader.GetInt32(reader.GetOrdinal("PersonID"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetPhoneInfoByID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static clsPhoneDTO GetPhoneInfoByPhoneNumber(string PhoneNumber)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetPhoneInfoByPhoneNumber", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsPhoneDTO(
                                    reader.GetInt32(reader.GetOrdinal("PhoneID")),
                                    reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    reader.GetInt32(reader.GetOrdinal("PersonID"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetPhoneInfoByPhoneNumber method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsPhoneDTO> GetAllPhones()
        {
            var Phones = new List<clsPhoneDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllPhones", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                Phones.Add(
                                    new clsPhoneDTO(
                                        reader.GetInt32(reader.GetOrdinal("PhoneID")),
                                        reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                        reader.GetInt32(reader.GetOrdinal("PersonID"))
                                        )
                                    );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllPhones method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return Phones;
        }
        public static List<clsPhoneDTO> GetAllPhonesByPerson(int PersonId)
        {
            var PhonesByPerson = new List<clsPhoneDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllPhonesByPerson", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PersonID", PersonId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PhonesByPerson.Add(
                                        new clsPhoneDTO(
                                            reader.GetInt32(reader.GetOrdinal("PhoneID")),
                                            reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                            reader.GetInt32(reader.GetOrdinal("PersonID"))
                                    ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllPhonesByPerson method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return PhonesByPerson;
        }
    }
}