using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text;
using System.IO;

namespace WellApi
{
    public class DB
    {
        static SqlConnection sqlConnection = null;

        public static void ConnectToDb()
        {
            string connectionString = "Server=tcp:wellhtw.database.windows.net,1433;Initial Catalog=well;Persist Security Info=False;User ID=htw;Password=maph2019!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
        }
        
        public static void DisconnectFromDb()
        {
            if( sqlConnection != null)
            {
                sqlConnection.Close();
                sqlConnection = null;
            }
        }

        public static SmallWell[] GetSmallWells()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT w.Id, w.Name, w.Status, l.Longitude, l.Latitude ");
            sb.Append("FROM [well].[dbo].[Well] w ");
            sb.Append("JOIN [well].[dbo].[Location] l ");
            sb.Append("ON w.LocationId = l.id;");
            String sql = sb.ToString();

            List<SmallWell> smallWells = new List<SmallWell>();
            using (SqlCommand command = new SqlCommand(sql, sqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SmallWell smallWell = new SmallWell
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Status = reader.GetString(2)
                        };
                        smallWell.Location.Longitude = reader.GetDouble(3);
                        smallWell.Location.Latitude = reader.GetDouble(4);
                        smallWells.Add(smallWell);
                    }
                }
            }
            return smallWells.ToArray();
        }
    
        public static Well GetWell(int Id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT w.Id, w.Name, w.Image, w.Status, l.id, l.Longitude, l.Latitude, f.id, f.Organisation, f.OpeningDate, f.Price, t.id, t.Name, t.Particularity, t.Depth ");
            sb.Append("FROM [well].[dbo].[Well] w ");
            sb.Append("JOIN [well].[dbo].[Location] l ");
            sb.Append("ON w.LocationId = l.id");
            sb.Append("JOIN [well].[dbo].[FundingInfo] f ");
            sb.Append("ON w.FundingInfoId = f.id");
            sb.Append("JOIN [well].[dbo].[WellType] t ");
            sb.Append("ON w.WellTypeId = t.id");
            sb.Append($"WHERE w.id = {Id};");
            String sql = sb.ToString();

            Well well = new Well();
            using (SqlCommand command = new SqlCommand(sql, sqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        well.Id = reader.GetInt32(0);
                        well.Name = reader.GetString(1);
                        well.Image = null;
                        Stream real_Image = reader.GetStream(2);
                        well.Status = reader.GetString(3);
                        well.Location.Id = reader.GetInt32(4);
                        well.Location.Longitude = reader.GetDouble(5);
                        well.Location.Latitude = reader.GetDouble(6);
                        well.FundingInfo.Id = reader.GetInt32(7);
                        well.FundingInfo.Organisation = reader.GetString(8);
                        well.FundingInfo.OpeningDate = reader.GetDateTime(9);
                        well.FundingInfo.Price = reader.GetDouble(10);
                        well.WellType.Id = reader.GetInt32(11);
                        well.WellType.Name = reader.GetString(12);
                        well.WellType.Particularity = reader.GetString(13);
                        well.WellType.Depth = reader.GetDouble(14);
                    }
                }
            }
            if( well.Id != 0)
            {
                StringBuilder sb2 = new StringBuilder();
                sb2.Append("SELECT Id, Description, Works, Confirmed, StatusChangedDate");
                sb2.Append("FROM [well].[dbo].[StatusHistory]");
                sb2.Append($"WHERE Id = {well.Id};");
                String sql2 = sb2.ToString();
                List<WellStatus> wellStatuses = new List<WellStatus>();
                using (SqlCommand command = new SqlCommand(sql2, sqlConnection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            WellStatus wellStatus = new WellStatus
                            {
                                Id = reader.GetInt32(0),
                                Description = reader.GetString(1),
                                Works = reader.GetBoolean(2),
                                Confirmed = reader.GetBoolean(3),
                                StatusChangedDate = reader.GetDateTime(4)
                            };
                            wellStatuses.Add(wellStatus);
                        }
                    }
                }
                well.StatusHistory = wellStatuses.ToArray();
            }
            if(well.WellType.Id != 0)
            {
                StringBuilder sb2 = new StringBuilder();
                sb2.Append("SELECT p.Id, p.Name, p.Description");
                sb2.Append("FROM [well].[dbo].[Part] p");
                sb2.Append("JOIN [well].[dbo].[WellParts] w");
                sb2.Append("On w.PartId = p.Id");
                sb2.Append($"WHERE w.WellTypeId = {well.WellType.Id};");
                String sql2 = sb2.ToString();
                List<Part> parts = new List<Part>();
                using (SqlCommand command = new SqlCommand(sql2, sqlConnection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Part part = new Part
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2)
                            };
                            parts.Add(part);
                        }
                    }
                }
                well.WellType.Parts = parts.ToArray();
            }
            return well;
        }
    
        public static void CreateNewWell(Well newWell)
        {

            foreach (var part in newWell.WellType.Parts)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("INSERT INTO [well].[dbo].[Part] (Name, Description)");
                sb.Append($"VALUES ('{part.Name}','{part.Description}');");
                String sql = sb.ToString();

                Well well = new Well();
                using (SqlCommand command = new SqlCommand(sql, sqlConnection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                }
            }
            
        }
    
        public static void UpdateWell(Well newWell)
        {

        }

        public static void DeleteWell(int Id)
        {

        }
    }
}
