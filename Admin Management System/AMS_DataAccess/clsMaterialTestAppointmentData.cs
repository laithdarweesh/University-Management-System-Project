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
    public class clsMaterialTestAppointmentDTO
    {
        public int TestAppointmentID { get; set; }
        public int TestTypeID { get; set; }
        public int SemesterMaterialID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int RoomID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedByDoctorID { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int ModifiedByDoctorID { get; set; }
        public clsMaterialTestAppointmentDTO(int TestAppointmentID, int TestTypeID, int SemesterMaterialID, 
            DateTime AppointmentDate, int RoomID, DateTime? CreatedDate, int CreatedByDoctorID, 
            DateTime? LastModifiedDate, int ModifiedByDoctorID)
        {
            this.TestAppointmentID = TestAppointmentID;
            this.TestTypeID = TestTypeID;
            this.SemesterMaterialID = SemesterMaterialID;
            this.AppointmentDate = AppointmentDate;
            this.RoomID = RoomID;
            this.CreatedDate = CreatedDate;
            this.CreatedByDoctorID = CreatedByDoctorID;
            this.LastModifiedDate = LastModifiedDate;
            this.ModifiedByDoctorID = ModifiedByDoctorID;
        }
    }
    public class clsMaterialTestAppointmentData
    {
        public static int AddNewMaterialTestAppointment(clsMaterialTestAppointmentDTO MaterialTestAppointmentDTO)
        {
            int MaterialTestAppointmentId = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewMaterialTestAppointment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@TestTypeID", MaterialTestAppointmentDTO.TestTypeID);
                        command.Parameters.AddWithValue("@SemesterMaterialID", MaterialTestAppointmentDTO.SemesterMaterialID);
                        command.Parameters.AddWithValue("@AppointmentDate", MaterialTestAppointmentDTO.AppointmentDate);
                        command.Parameters.AddWithValue("@RoomID", MaterialTestAppointmentDTO.RoomID);

                        command.Parameters.AddWithValue("@CreatedDate", clsGlobal.ToDbNull(MaterialTestAppointmentDTO.CreatedDate));
                        command.Parameters.AddWithValue("@CreatedByDoctorID", MaterialTestAppointmentDTO.CreatedByDoctorID);
                        command.Parameters.AddWithValue("@LastModifiedDate", clsGlobal.ToDbNull(MaterialTestAppointmentDTO.LastModifiedDate));
                        
                        command.Parameters.AddWithValue("@ModifiedByDoctorID", MaterialTestAppointmentDTO.ModifiedByDoctorID);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedId))
                        {
                            MaterialTestAppointmentId = InsertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a New Material Test Appointment. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return MaterialTestAppointmentId;
        }
        public static bool UpdateMaterialTestAppointment(clsMaterialTestAppointmentDTO MaterialTestAppointmentDTO)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateMaterialTestAppointment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@TestAppointmentID", MaterialTestAppointmentDTO.TestAppointmentID);
                        command.Parameters.AddWithValue("@TestTypeID", MaterialTestAppointmentDTO.TestTypeID);
                        command.Parameters.AddWithValue("@SemesterMaterialID", MaterialTestAppointmentDTO.SemesterMaterialID);
                        command.Parameters.AddWithValue("@AppointmentDate", MaterialTestAppointmentDTO.AppointmentDate);
                        command.Parameters.AddWithValue("@RoomID", MaterialTestAppointmentDTO.RoomID);

                        command.Parameters.AddWithValue("@LastModifiedDate", clsGlobal.ToDbNull(MaterialTestAppointmentDTO.LastModifiedDate));
                        command.Parameters.AddWithValue("@ModifiedByDoctorID", MaterialTestAppointmentDTO.ModifiedByDoctorID);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"The record for Update Material Test Appointment ID: {MaterialTestAppointmentDTO.TestAppointmentID} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdateMaterialTestAppointment method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeleteMaterialTestAppointment(int MaterialTestAppointmentId)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeleteMaterialTestAppointment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@TestAppointmentID", MaterialTestAppointmentId);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"Material Test Appointment with ID: {MaterialTestAppointmentId} deleted successfully",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeleteMaterialTestAppointment method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static clsMaterialTestAppointmentDTO GetMaterialTestAppointmentInfoByID(int MaterialTestAppointmentId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetMaterialTestAppointmentInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@TestAppointmentID", MaterialTestAppointmentId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsMaterialTestAppointmentDTO(
                                    reader.GetInt32(reader.GetOrdinal("TestAppointmentID")),
                                    reader.GetInt32(reader.GetOrdinal("TestTypeID")),
                                    reader.GetInt32(reader.GetOrdinal("SemesterMaterialID")),
                                    reader.GetDateTime(reader.GetOrdinal("AppointmentDate")),
                                    reader.GetInt32(reader.GetOrdinal("RoomID")),

                                    reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ?
                                    (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CreatedDate")),

                                    reader.GetInt32(reader.GetOrdinal("CreatedByDoctorID")),

                                    reader.IsDBNull(reader.GetOrdinal("LastModifiedDate")) ?
                                    (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("LastModifiedDate")),

                                    reader.GetInt32(reader.GetOrdinal("ModifiedByDoctorID"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetMaterialTestAppointmentByID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static clsMaterialTestAppointmentDTO GetMaterialTestAppointmentInfoBySemesterMaterialID(int SemesterMaterialID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetMaterialTestAppointmentInfoBySemesterMaterialID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SemesterMaterialID", SemesterMaterialID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsMaterialTestAppointmentDTO(
                                    reader.GetInt32(reader.GetOrdinal("TestAppointmentID")),
                                    reader.GetInt32(reader.GetOrdinal("TestTypeID")),
                                    reader.GetInt32(reader.GetOrdinal("SemesterMaterialID")),
                                    reader.GetDateTime(reader.GetOrdinal("AppointmentDate")),
                                    reader.GetInt32(reader.GetOrdinal("RoomID")),

                                    reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ?
                                    (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CreatedDate")),

                                    reader.GetInt32(reader.GetOrdinal("CreatedByDoctorID")),

                                    reader.IsDBNull(reader.GetOrdinal("LastModifiedDate")) ?
                                    (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("LastModifiedDate")),

                                    reader.GetInt32(reader.GetOrdinal("ModifiedByDoctorID"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetMaterialTestAppointmentInfoBySemesterMaterialID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static clsMaterialTestAppointmentDTO GetMaterialTestAppointmentInfoByRoomID(int RoomID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetMaterialTestAppointmentInfoByRoomID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@RoomID", RoomID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsMaterialTestAppointmentDTO(
                                    reader.GetInt32(reader.GetOrdinal("TestAppointmentID")),
                                    reader.GetInt32(reader.GetOrdinal("TestTypeID")),
                                    reader.GetInt32(reader.GetOrdinal("SemesterMaterialID")),
                                    reader.GetDateTime(reader.GetOrdinal("AppointmentDate")),
                                    reader.GetInt32(reader.GetOrdinal("RoomID")),

                                    reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ?
                                    (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CreatedDate")),

                                    reader.GetInt32(reader.GetOrdinal("CreatedByDoctorID")),

                                    reader.IsDBNull(reader.GetOrdinal("LastModifiedDate")) ?
                                    (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("LastModifiedDate")),

                                    reader.GetInt32(reader.GetOrdinal("ModifiedByDoctorID"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetMaterialTestAppointmentInfoByRoomID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static clsMaterialTestAppointmentDTO GetMaterialTestAppointmentInfoByTestTypeID(int TestTypeID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetMaterialTestAppointmentInfoByTestTypeID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsMaterialTestAppointmentDTO(
                                    reader.GetInt32(reader.GetOrdinal("TestAppointmentID")),
                                    reader.GetInt32(reader.GetOrdinal("TestTypeID")),
                                    reader.GetInt32(reader.GetOrdinal("SemesterMaterialID")),
                                    reader.GetDateTime(reader.GetOrdinal("AppointmentDate")),
                                    reader.GetInt32(reader.GetOrdinal("RoomID")),

                                    reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ?
                                    (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CreatedDate")),

                                    reader.GetInt32(reader.GetOrdinal("CreatedByDoctorID")),

                                    reader.IsDBNull(reader.GetOrdinal("LastModifiedDate")) ?
                                    (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("LastModifiedDate")),

                                    reader.GetInt32(reader.GetOrdinal("ModifiedByDoctorID"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetMaterialTestAppointmentInfoByTestTypeID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsMaterialTestAppointmentDTO> GetAllMaterialTestAppointments()
        {
            var MaterialTestAppointments = new List<clsMaterialTestAppointmentDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllMaterialTestAppointments", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MaterialTestAppointments.Add(new clsMaterialTestAppointmentDTO(
                                    reader.GetInt32(reader.GetOrdinal("TestAppointmentID")),
                                    reader.GetInt32(reader.GetOrdinal("TestTypeID")),
                                    reader.GetInt32(reader.GetOrdinal("SemesterMaterialID")),
                                    reader.GetDateTime(reader.GetOrdinal("AppointmentDate")),
                                    reader.GetInt32(reader.GetOrdinal("RoomID")),

                                    reader.IsDBNull(reader.GetOrdinal("CreatedDate")) ?
                                    (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CreatedDate")),

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
                clsGlobal.SaveToEventLog($"Error occurred in GetAllMaterialTestAppointments method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return MaterialTestAppointments;
        }
    }
}