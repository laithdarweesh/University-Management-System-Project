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

    #nullable enable
    public class clsRoomDTO
    {
        public int RoomID { get; set; }
        public string? RoomName { get; set; }
        public int CollegeID { get; set; }
        public byte NumberOfSeats { get; set; }
        public clsRoomDTO(int RoomID, string? RoomName, int CollegeID, byte NumberOfSeats)
        {
            this.RoomID = RoomID;
            this.RoomName = RoomName;
            this.CollegeID = CollegeID;
            this.NumberOfSeats = NumberOfSeats;
        }
    }
    public class clsRoomData
    {
        public static int AddNewRoom(clsRoomDTO RoomDTO)
        {
            int RoomId = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_AddNewRoom", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@RoomName", clsGlobal.ToDbNull(RoomDTO.RoomName));
                        command.Parameters.AddWithValue("@CollegeID", RoomDTO.CollegeID);
                        command.Parameters.AddWithValue("@NumberOfSeats", RoomDTO.NumberOfSeats);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int InsertedId))
                        {
                            RoomId = InsertedId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred while adding a new Room. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return RoomId;
        }
        public static bool UpdateRoom(clsRoomDTO RoomDTO)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_UpdateRoom", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@RoomID", RoomDTO.RoomID);
                        command.Parameters.AddWithValue("@RoomName", clsGlobal.ToDbNull(RoomDTO.RoomName));
                        command.Parameters.AddWithValue("@CollegeID", RoomDTO.CollegeID);
                        command.Parameters.AddWithValue("@NumberOfSeats", RoomDTO.NumberOfSeats);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"The record for Room ID: {RoomDTO.RoomID} was updated successfully.",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in UpdateRoom method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static bool DeleteRoom(int RoomId)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_DeleteRoom", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@RoomID", RoomId);

                        connection.Open();
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

                if (RowsAffected > 0)
                {
                    clsGlobal.SaveToEventLog($"Room with ID: {RoomId} deleted successfully",
                        clsSettingsData.EventLogSourceName, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in DeleteRoom method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return false;
            }

            return (RowsAffected > 0);
        }
        public static clsRoomDTO GetRoomInfoByID(int RoomId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetRoomInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@RoomID", RoomId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new clsRoomDTO(
                                    reader.GetInt32(reader.GetOrdinal("RoomID")),

                                    reader.IsDBNull(reader.GetOrdinal("RoomName")) ?
                                    (string?) null : reader.GetString(reader.GetOrdinal("RoomName")),

                                    reader.GetInt32(reader.GetOrdinal("CollegeID")),
                                    reader.GetByte(reader.GetOrdinal("NumberOfSeats"))
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
                clsGlobal.SaveToEventLog($"Error occurred in GetRoomInfoByID method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);

                return null;
            }
        }
        public static List<clsRoomDTO> GetAllRooms()
        {
            var Rooms = new List<clsRoomDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllRooms", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Rooms.Add(new clsRoomDTO(
                                        reader.GetInt32(reader.GetOrdinal("RoomID")),

                                        reader.IsDBNull(reader.GetOrdinal("RoomName")) ?
                                        (string?)null : reader.GetString(reader.GetOrdinal("RoomName")),

                                        reader.GetInt32(reader.GetOrdinal("CollegeID")),
                                        reader.GetByte(reader.GetOrdinal("NumberOfSeats"))
                                    ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsGlobal.SaveToEventLog($"Error occurred in GetAllRooms method. Details: {ex.Message} | StackTrace: {ex.StackTrace}",
                    clsSettingsData.EventLogSourceName, EventLogEntryType.Error);
            }

            return Rooms;
        }
    }
}