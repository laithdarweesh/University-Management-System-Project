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
    public class clsDepartmentDTO
    {
        public int DepartmentID {  get; set; }
        public string DepartmentName {  get; set; }
        public int CollegeID {  get; set; }
        public byte? CreditHours {  get; set; }
        public clsDepartmentDTO(int DepartmentID, string DepartmentName, int CollegeID, byte? CreditHours)
        {
            this.DepartmentID = DepartmentID;
            this.DepartmentName = DepartmentName;
            this.CollegeID = CollegeID;
            this.CreditHours = CreditHours;
        }
    }
    public class clsDepartmentData
    {
        public static int AddNewDepartment(clsDepartmentDTO DptDTO)
        {
            int DepartmentID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewDepartment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DepartmentName", DptDTO.DepartmentName);
                        command.Parameters.AddWithValue("@CollegeID", DptDTO.CollegeID);
                        command.Parameters.AddWithValue("@CreditHours", clsGlobal.ToDbNull(DptDTO.CreditHours));

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedId))
                        {
                            DepartmentID = InsertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a new Department. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return DepartmentID;
        }
        public static bool UpdateDepartment(clsDepartmentDTO DptDTO)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateDepartment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@DepartmentID", DptDTO.DepartmentID);
                        command.Parameters.AddWithValue("@DepartmentName", DptDTO.DepartmentName);
                        command.Parameters.AddWithValue("@CreditHours", clsGlobal.ToDbNull(DptDTO.CreditHours));

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"The record for Department ID: {DptDTO.DepartmentID} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdateDepartment method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeleteDepartment(int DepartmentId)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeleteDepartment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DepartmentID", DepartmentId);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"Department with ID: {DepartmentId} deleted successfully",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeleteDepartment method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static clsDepartmentDTO GetDepartmentInfoByID(int DepartmentId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetDepartmentInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DepartmentID", DepartmentId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsDepartmentDTO(
                                    reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                                    reader.GetString(reader.GetOrdinal("DepartmentName")),
                                    reader.GetInt32(reader.GetOrdinal("CollegeID")),

                                    reader.IsDBNull(reader.GetOrdinal("CreditHours")) ? (byte?) null : 
                                    reader.GetByte(reader.GetOrdinal("CreditHours"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetDepartmentInfoByID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsDepartmentDTO> GetAllDepartments()
        {
            var Departments = new List<clsDepartmentDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllDepartments", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Departments.Add(new clsDepartmentDTO(
                                        reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                                        reader.GetString(reader.GetOrdinal("DepartmentName")),
                                        reader.GetInt32(reader.GetOrdinal("CollegeID")),

                                        reader.IsDBNull(reader.GetOrdinal("CreditHours")) ? (byte?)null :
                                        reader.GetByte(reader.GetOrdinal("CreditHours"))
                                    ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllDepartments method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return Departments;
        }
    }
}