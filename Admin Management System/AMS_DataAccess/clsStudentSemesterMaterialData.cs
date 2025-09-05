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
    public class clsStudentSemesterMaterialDTO
    { 
        public int StudentSemesterMaterialID { get; set; }
        public int SemesterMaterialID { get; set; }
        public int StudentID { get; set; }
        public DateTime RegistrationDate { get; set; }
        public byte Status { get; set; }
        public float? FinalMark { get; set; }
        public clsStudentSemesterMaterialDTO(int StudentSemesterMaterialID, int SemesterMaterialID,
            int StudentID, DateTime RegistrationDate, byte Status, float? FinalMark)
        {
            this.StudentSemesterMaterialID = StudentSemesterMaterialID;
            this.SemesterMaterialID = SemesterMaterialID;
            this.StudentID = StudentID;
            this.RegistrationDate = RegistrationDate;
            this.Status = Status;
            this.FinalMark = FinalMark;
        }
    }
    public class clsStudentSemesterMaterialData
    {
        public static int AddNewStudentSemesterMaterial(clsStudentSemesterMaterialDTO StdSemesterMaterialDTO)
        {
            int StudentSemesterMaterialId = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewStudentSemesterMaterial", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@SemesterMaterialID", StdSemesterMaterialDTO.SemesterMaterialID);
                        command.Parameters.AddWithValue("@StudentID", StdSemesterMaterialDTO.StudentID);
                        command.Parameters.AddWithValue("@RegistrationDate", StdSemesterMaterialDTO.RegistrationDate);
                        command.Parameters.AddWithValue("@Status", StdSemesterMaterialDTO.Status);
                        command.Parameters.AddWithValue("@FinalMark", clsGlobal.ToDbNull(StdSemesterMaterialDTO.FinalMark));

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedId))
                        {
                            StudentSemesterMaterialId = InsertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a New Student Semester Material. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return StudentSemesterMaterialId;
        }
        public static bool UpdateStudentSemesterMaterial(clsStudentSemesterMaterialDTO StdSemesterMaterialDTO)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateStudentSemesterMaterial", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@StudentSemesterMaterialID", StdSemesterMaterialDTO.StudentSemesterMaterialID);
                        command.Parameters.AddWithValue("@SemesterMaterialID", StdSemesterMaterialDTO.SemesterMaterialID);
                        command.Parameters.AddWithValue("@StudentID", StdSemesterMaterialDTO.StudentID);
                        command.Parameters.AddWithValue("@RegistrationDate", StdSemesterMaterialDTO.RegistrationDate);
                        command.Parameters.AddWithValue("@Status", StdSemesterMaterialDTO.Status);
                        command.Parameters.AddWithValue("@FinalMark", clsGlobal.ToDbNull(StdSemesterMaterialDTO.FinalMark));

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"The record for Student Semester Material ID: {StdSemesterMaterialDTO.StudentSemesterMaterialID} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdateStudentSemesterMaterial method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeleteStudentSemesterMaterial(int StudentSemesterMaterialId)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeleteStudentSemesterMaterial", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StudentSemesterMaterialID", StudentSemesterMaterialId);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"Student Semester Material with ID: {StudentSemesterMaterialId} deleted successfully",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeleteStudentSemesterMaterial method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static clsStudentSemesterMaterialDTO GetStudentSemesterMaterialInfoByID(int StudentSemesterMaterialId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetStudentSemesterMaterialInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StudentSemesterMaterialID", StudentSemesterMaterialId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsStudentSemesterMaterialDTO(
                                    reader.GetInt32(reader.GetOrdinal("StudentSemesterMaterialID")),
                                    reader.GetInt32(reader.GetOrdinal("SemesterMaterialID")),
                                    reader.GetInt32(reader.GetOrdinal("StudentID")),
                                    reader.GetDateTime(reader.GetOrdinal("RegistrationDate")),
                                    reader.GetByte(reader.GetOrdinal("Status")),
                                    
                                    reader.IsDBNull(reader.GetOrdinal("FinalMark")) ?
                                    (float?)null : reader.GetFloat(reader.GetOrdinal("FinalMark"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetStudentSemesterMaterialInfoByID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsStudentSemesterMaterialDTO> GetAllStudentSemesterMaterials()
        {
            var StudentSemesterMaterials = new List<clsStudentSemesterMaterialDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllStudentSemesterMaterials", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                StudentSemesterMaterials.Add(new clsStudentSemesterMaterialDTO(
                                    reader.GetInt32(reader.GetOrdinal("StudentSemesterMaterialID")),
                                    reader.GetInt32(reader.GetOrdinal("SemesterMaterialID")),
                                    reader.GetInt32(reader.GetOrdinal("StudentID")),
                                    reader.GetDateTime(reader.GetOrdinal("RegistrationDate")),
                                    reader.GetByte(reader.GetOrdinal("Status")),

                                    reader.IsDBNull(reader.GetOrdinal("FinalMark")) ?
                                    (float?)null : reader.GetFloat(reader.GetOrdinal("FinalMark"))
                                    ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllStudentSemesterMaterials method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return StudentSemesterMaterials;
        }
    }
}