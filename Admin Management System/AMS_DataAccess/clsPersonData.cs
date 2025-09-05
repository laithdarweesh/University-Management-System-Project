using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data;

namespace AMS_DataAccess
{

    #nullable enable
    public class clsPersonDTO
    {
        public int PersonID { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? ThirdName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte Gendor { get; set; }
        public string? Email { get; set; }
        public int NationalityCountryID { get; set; }
        public string? ImagePath { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastStatusDate { get; set; }
        public int CreatedByAdminID { get; set; }
        public clsPersonDTO(int PersonID, string NationalNo, string FirstName, string? SecondName,
            string? ThirdName, string LastName, DateTime DateOfBirth, byte Gendor, string? Email,
            int NationalityCountryID, string? ImagePath, DateTime CreatedDate, DateTime LastStatusDate,
            int CreatedByAdminID)
        {
            this.PersonID = PersonID;
            this.NationalNo = NationalNo;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.DateOfBirth = DateOfBirth;
            this.Gendor = Gendor;
            this.Email = Email;
            this.NationalityCountryID = NationalityCountryID;
            this.ImagePath = ImagePath;
            this.CreatedDate = CreatedDate;
            this.LastStatusDate = LastStatusDate;
            this.CreatedByAdminID = CreatedByAdminID;
        }
    }
    public class clsPersonData
    {
        public static int AddNewPerson(clsPersonDTO PDTO)
        {
            int PersonId = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewPerson", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@NationalNo", PDTO.NationalNo);
                        command.Parameters.AddWithValue("@FirstName", PDTO.FirstName);

                        command.Parameters.AddWithValue("@SecondName", clsGlobal.ToDbNull(PDTO.SecondName));
                        command.Parameters.AddWithValue("@ThirdName", clsGlobal.ToDbNull(PDTO.ThirdName));

                        command.Parameters.AddWithValue("@LastName", PDTO.LastName);
                        command.Parameters.AddWithValue("@DateOfBirth", PDTO.DateOfBirth);
                        command.Parameters.AddWithValue("@Gendor", PDTO.Gendor);

                        command.Parameters.AddWithValue("@Email", clsGlobal.ToDbNull(PDTO.Email));
                        command.Parameters.AddWithValue("@NationalityCountryID", PDTO.NationalityCountryID);
                        command.Parameters.AddWithValue("@ImagePath", clsGlobal.ToDbNull(PDTO.ImagePath));

                        command.Parameters.AddWithValue("@CreatedDate", PDTO.CreatedDate);
                        command.Parameters.AddWithValue("@LastStatusDate", PDTO.LastStatusDate);
                        command.Parameters.AddWithValue("@CreatedByAdminID", PDTO.CreatedByAdminID);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InserteId))
                        {
                            PersonId = InserteId;

                            switch (PersonId)
                            {
                                case -2:
                                    throw new Exception("National Number already exists.");

                                case -3:
                                    throw new Exception("Invalid DateOfBirth.");

                                default:
                                    if (PersonId <= 0)
                                        throw new Exception(@"Unexpected negative return value from 
                                                                  the stored procedure.");
                                    break;
                            }
                        }
                        else
                        {
                            throw new Exception(@"Unexpected null or invalid return value from the 
                                                      stored procedure.");
                        }
                    }
                }

                if (PersonId > 0)
                    clsGlobal.SaveToEventLog($"New Person added to System with ID: {PersonId}",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a new person. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return PersonId;
        }
        public static bool UpdatePerson(clsPersonDTO PDTO)
        {
            int Result = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdatePerson", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@PersonID", PDTO.PersonID);
                        command.Parameters.AddWithValue("@NationalNo", PDTO.NationalNo);
                        command.Parameters.AddWithValue("@FirstName", PDTO.FirstName);

                        command.Parameters.AddWithValue("@SecondName", clsGlobal.ToDbNull(PDTO.SecondName));
                        command.Parameters.AddWithValue("@ThirdName", clsGlobal.ToDbNull(PDTO.ThirdName));

                        command.Parameters.AddWithValue("@LastName", PDTO.LastName);
                        command.Parameters.AddWithValue("@DateOfBirth", PDTO.DateOfBirth);
                        command.Parameters.AddWithValue("@Gendor", PDTO.Gendor);

                        command.Parameters.AddWithValue("@Email", clsGlobal.ToDbNull(PDTO.Email));
                        command.Parameters.AddWithValue("@NationalityCountryID", PDTO.NationalityCountryID);

                        command.Parameters.AddWithValue("@ImagePath", clsGlobal.ToDbNull(PDTO.ImagePath));

                        command.Parameters.AddWithValue("@CreatedDate", PDTO.CreatedDate);
                        command.Parameters.AddWithValue("@LastStatusDate", PDTO.LastStatusDate);
                        command.Parameters.AddWithValue("@CreatedByAdminID", PDTO.CreatedByAdminID);

                        connection.Open();
                        Result = (int)command.ExecuteScalar();
                    }
                }

                if (Result == 1)
                {
                    clsGlobal.SaveToEventLog($"The record for Person ID: {PDTO.PersonID} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);

                    return true;
                }

                if (Result == 0)
                    throw new Exception("PersonID not found.");

                else if (Result == -1)
                    throw new Exception("An error occurred while updating the person info.");

                else
                    throw new Exception($"Unknown result from the stored procedure.{Result}");
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdatePerson method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }
        }
        public static bool DeletePerson(int PersonId)
        {
            int RowsAffected = 0, result = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeletePerson", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PersonID", PersonId);

                        SqlParameter returnvalue = new SqlParameter
                        {
                            Direction = ParameterDirection.ReturnValue
                        };

                        command.Parameters.Add(returnvalue);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                        result = (int)returnvalue.Value;
                    }
                }

                if (result == -1)
                    throw new Exception($"Person with ID {PersonId} does not exist.");

                else if (result == 0)
                    throw new Exception($"An error occurred while deleting Person with ID: {PersonId}.");

                else
                    throw new Exception($"Unknown result from the stored procedure.{result}");
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeletePerson method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static clsPersonDTO GetPersonInfoByID(int PersonId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetPersonInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PersonId", PersonId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsPersonDTO(
                                    reader.GetInt32(reader.GetOrdinal("PersonID")),
                                    reader.GetString(reader.GetOrdinal("NationalNo")),
                                    reader.GetString(reader.GetOrdinal("FirstName")),

                                    reader.IsDBNull(reader.GetOrdinal("SecondName")) ? (string?) null :
                                    reader.GetString(reader.GetOrdinal("SecondName")),

                                    reader.IsDBNull(reader.GetOrdinal("ThirdName")) ? (string?)null :
                                    reader.GetString(reader.GetOrdinal("ThirdName")),

                                    reader.GetString(reader.GetOrdinal("LastName")),
                                    reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                    reader.GetByte(reader.GetOrdinal("Gendor")),

                                    reader.IsDBNull(reader.GetOrdinal("Email")) ? (string?)null :
                                    reader.GetString(reader.GetOrdinal("Email")),

                                    reader.GetInt32(reader.GetOrdinal("NationalityCountryID")),

                                    reader.IsDBNull(reader.GetOrdinal("ImagePath")) ? (string?)null :
                                    reader.GetString(reader.GetOrdinal("ImagePath")),

                                    reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                    reader.GetDateTime(reader.GetOrdinal("LastStatusDate")),
                                    reader.GetInt32(reader.GetOrdinal("CreatedByAdminID"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetPersonInfoByID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static clsPersonDTO GetPersonInfoByNationalNo(string NationalNo)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetPersonByNationalNo", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@NationalNo", NationalNo);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsPersonDTO(
                                        reader.GetInt32(reader.GetOrdinal("PersonID")),
                                        reader.GetString(reader.GetOrdinal("NationalNo")),
                                        reader.GetString(reader.GetOrdinal("FirstName")),

                                        reader.IsDBNull(reader.GetOrdinal("SecondName")) ? (string?)null :
                                        reader.GetString(reader.GetOrdinal("SecondName")),

                                        reader.IsDBNull(reader.GetOrdinal("ThirdName")) ? (string?)null :
                                        reader.GetString(reader.GetOrdinal("ThirdName")),

                                        reader.GetString(reader.GetOrdinal("LastName")),
                                        reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                        reader.GetByte(reader.GetOrdinal("Gendor")),

                                        reader.IsDBNull(reader.GetOrdinal("Email")) ? (string?)null :
                                        reader.GetString(reader.GetOrdinal("Email")),

                                        reader.GetInt32(reader.GetOrdinal("NationalityCountryID")),

                                        reader.IsDBNull(reader.GetOrdinal("ImagePath")) ? (string?)null :
                                        reader.GetString(reader.GetOrdinal("ImagePath")),

                                        reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                        reader.GetDateTime(reader.GetOrdinal("LastStatusDate")),
                                        reader.GetInt32(reader.GetOrdinal("CreatedByAdminID"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetPersonInfoByNationalNo method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsPersonDTO> GetAllPeople()
        {
            var AllPeople = new List<clsPersonDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllPeople", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AllPeople.Add(
                                    new clsPersonDTO(
                                    reader.GetInt32(reader.GetOrdinal("PersonID")),
                                    reader.GetString(reader.GetOrdinal("NationalNo")),
                                    reader.GetString(reader.GetOrdinal("FirstName")),

                                    reader.IsDBNull(reader.GetOrdinal("SecondName")) ? (string?) null :
                                    reader.GetString(reader.GetOrdinal("SecondName")),

                                    reader.IsDBNull(reader.GetOrdinal("ThirdName")) ? (string?)null :
                                    reader.GetString(reader.GetOrdinal("ThirdName")),

                                    reader.GetString(reader.GetOrdinal("LastName")),
                                    reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                    reader.GetByte(reader.GetOrdinal("Gendor")),

                                    reader.IsDBNull(reader.GetOrdinal("Email")) ? (string?)null :
                                    reader.GetString(reader.GetOrdinal("Email")),

                                    reader.GetInt32(reader.GetOrdinal("NationalityCountryID")),

                                    reader.IsDBNull(reader.GetOrdinal("ImagePath")) ? (string?)null :
                                    reader.GetString(reader.GetOrdinal("ImagePath")),

                                    reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                    reader.GetDateTime(reader.GetOrdinal("LastStatusDate")),
                                    reader.GetInt32(reader.GetOrdinal("CreatedByAdminID"))
                                        )
                                    );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllPeople method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return AllPeople;
        }
        public static bool IsPersonExist(int PersonId)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_IsPersonExistByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PersonID", PersonId);
                        connection.Open();

                        int result = (int)command.ExecuteScalar();
                        IsFound = (result == 1);
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in IsPersonExist method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                IsFound = false;
            }

            return IsFound;
        }
        public static bool IsPersonExist(string NationalNo)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_IsPersonExistByNationalNo", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@NationalNo", NationalNo);
                        connection.Open();

                        int result = (int)command.ExecuteScalar();
                        IsFound = (result == 1);
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in IsPersonExist method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                IsFound = false;
            }

            return IsFound;
        }
    }
}