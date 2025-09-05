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
    public class clsSemesterMaterialDTO
    { 
        public int SemesterMaterialID { get; set; }
        public int CourseID { get; set; }
        public int DoctorID { get; set; }
        public int BrancheID { get; set; }
        public int RoomID { get; set; }
        public int AnnualSemesterID { get; set; }
        public short Year { get; set; }
        public byte CourseStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByDoctorID { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int ModifiedByDoctorID { get; set; }
        public clsSemesterMaterialDTO(int SemesterMaterialID, int CourseID, int DoctorID, int BrancheID,
            int RoomID, int AnnualSemesterID, short Year, byte CourseStatus, DateTime CreatedDate,
            int CreatedByDoctorID, DateTime? LastModifiedDate, int ModifiedByDoctorID)
        {
            this.SemesterMaterialID = SemesterMaterialID;
            this.CourseID = CourseID;
            this.DoctorID = DoctorID;
            this.BrancheID = BrancheID;
            this.RoomID = RoomID;
            this.AnnualSemesterID = AnnualSemesterID;
            this.Year = Year;
            this.CourseStatus = CourseStatus;
            this.CreatedDate = CreatedDate;
            this.CreatedByDoctorID = CreatedByDoctorID;
            this.LastModifiedDate = LastModifiedDate;
            this.ModifiedByDoctorID = ModifiedByDoctorID;
        }
    }
    public class clsSemesterMaterialData
    {
        public static int AddNewSemesterMaterial(clsSemesterMaterialDTO SemesterMaterialDTO)
        {
            int SemesterMaterialId = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewSemesterMaterial", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@CourseID", SemesterMaterialDTO.CourseID);
                        command.Parameters.AddWithValue("@DoctorID", SemesterMaterialDTO.DoctorID);
                        command.Parameters.AddWithValue("@BrancheID", SemesterMaterialDTO.BrancheID);
                        command.Parameters.AddWithValue("@RoomID", SemesterMaterialDTO.RoomID);
                        command.Parameters.AddWithValue("@AnnualSemesterID", SemesterMaterialDTO.AnnualSemesterID);
                        command.Parameters.AddWithValue("@Year", SemesterMaterialDTO.Year);
                        command.Parameters.AddWithValue("@CourseStatus", SemesterMaterialDTO.CourseStatus);
                        command.Parameters.AddWithValue("@CreatedDate", SemesterMaterialDTO.CreatedDate);
                        command.Parameters.AddWithValue("@CreatedByDoctorID", SemesterMaterialDTO.CreatedByDoctorID);
                        command.Parameters.AddWithValue("@LastModifiedDate", clsGlobal.ToDbNull(SemesterMaterialDTO.LastModifiedDate));                        
                        command.Parameters.AddWithValue("@ModifiedByDoctorID", SemesterMaterialDTO.ModifiedByDoctorID);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedId))
                        {
                            SemesterMaterialId = InsertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a New Semester Material. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return SemesterMaterialId;
        }
        public static bool UpdateSemesterMaterial(clsSemesterMaterialDTO SemesterMaterialDTO)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateSemesterMaterial", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@SemesterMaterialID", SemesterMaterialDTO.SemesterMaterialID);
                        command.Parameters.AddWithValue("@CourseID", SemesterMaterialDTO.CourseID);
                        command.Parameters.AddWithValue("@DoctorID", SemesterMaterialDTO.DoctorID);
                        command.Parameters.AddWithValue("@BrancheID", SemesterMaterialDTO.BrancheID);
                        command.Parameters.AddWithValue("@RoomID", SemesterMaterialDTO.RoomID);
                        command.Parameters.AddWithValue("@AnnualSemesterID", SemesterMaterialDTO.AnnualSemesterID);
                        command.Parameters.AddWithValue("@Year", SemesterMaterialDTO.Year);
                        command.Parameters.AddWithValue("@CourseStatus", SemesterMaterialDTO.CourseStatus);
                        command.Parameters.AddWithValue("@LastModifiedDate", clsGlobal.ToDbNull(SemesterMaterialDTO.LastModifiedDate));
                        command.Parameters.AddWithValue("@ModifiedByDoctorID", SemesterMaterialDTO.ModifiedByDoctorID);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"The record for Semester Material ID: {SemesterMaterialDTO.SemesterMaterialID} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdateSemesterMaterial method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeleteSemesterMaterial(int SemesterMaterialId)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeleteSemesterMaterial", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SemesterMaterialID", SemesterMaterialId);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"Semester Material with ID: {SemesterMaterialId} deleted successfully",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeleteSemesterMaterial method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static clsSemesterMaterialDTO GetSemesterMaterialInfoByID(int SemesterMaterialId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetSemesterMaterialInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SemesterMaterialID", SemesterMaterialId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsSemesterMaterialDTO(
                                    reader.GetInt32(reader.GetOrdinal("SemesterMaterialID")),
                                    reader.GetInt32(reader.GetOrdinal("CourseID")),
                                    reader.GetInt32(reader.GetOrdinal("DoctorID")),
                                    reader.GetInt32(reader.GetOrdinal("BrancheID")),
                                    reader.GetInt32(reader.GetOrdinal("RoomID")),
                                    reader.GetInt32(reader.GetOrdinal("AnnualSemesterID")),
                                    reader.GetInt16(reader.GetOrdinal("Year")),
                                    reader.GetByte(reader.GetOrdinal("CourseStatus")),
                                    reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                    reader.GetInt32(reader.GetOrdinal("CreatedByDoctorID")),

                                    reader.IsDBNull(reader.GetOrdinal("LastModifiedDate")) ?
                                    (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("LastModifiedDate")),

                                    reader.GetInt32(reader.GetOrdinal("ModifiedByDoctorID")

                                    ));
                            }
                            else
                                return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetSemesterMaterialInfoByID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsSemesterMaterialDTO> GetAllSemesterMaterials()
        {
            var SemesterMaterials = new List<clsSemesterMaterialDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllSemesterMaterials", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SemesterMaterials.Add(new clsSemesterMaterialDTO(
                                    reader.GetInt32(reader.GetOrdinal("SemesterMaterialID")),
                                    reader.GetInt32(reader.GetOrdinal("CourseID")),
                                    reader.GetInt32(reader.GetOrdinal("DoctorID")),
                                    reader.GetInt32(reader.GetOrdinal("BrancheID")),
                                    reader.GetInt32(reader.GetOrdinal("RoomID")),
                                    reader.GetInt32(reader.GetOrdinal("AnnualSemesterID")),
                                    reader.GetInt16(reader.GetOrdinal("Year")),
                                    reader.GetByte(reader.GetOrdinal("CourseStatus")),
                                    reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                    reader.GetInt32(reader.GetOrdinal("CreatedByDoctorID")),

                                    reader.IsDBNull(reader.GetOrdinal("LastModifiedDate")) ?
                                    (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("LastModifiedDate")),

                                    reader.GetInt32(reader.GetOrdinal("ModifiedByDoctorID"))
                                    ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllSemesterMaterials method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return SemesterMaterials;
        }
    }
}