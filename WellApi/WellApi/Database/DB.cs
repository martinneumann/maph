using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Device.Location;
using WellApi.Models;
using System.Text;

namespace WellApi
{
    public class DB
    {
        // Connection

        static SqlConnection sqlConnection = null;
        static string conn = "";
        public static void ConnectToDb(string connectionString)
        {
            conn = connectionString;
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
        }
        public static void ReconnectToDb()
        {
            sqlConnection.Close();
            sqlConnection = new SqlConnection(conn);
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

        // Well

        // Mutiple SQL Querries
        public static Well GetCompleteWell(int wellId)
        {
            Well well = ExecuteSelectWell(wellId);
            if (well == null || well.Id == 0)
                return null;
            well.MaintenanceLogs = ExecuteSelectMaintenanceLogs(wellId);
            if (well.WellType.Id == 0)
                return well;
            well.WellType.Parts = ExecuteSelectWellParts(well.WellType.Id);
            return well;
        }
        public static int AddCompleteWell(NewWell newWell)
        {
            if (newWell == null || newWell.WellTypeId == 0)
                return 0;
            int wellId = ExecuteInsertWell(newWell);
            if (wellId == 0)
                return 0;
            List<MaintenanceLog> maintenanceLogs = new List<MaintenanceLog>();
            MaintenanceLog maintenanceLog = new MaintenanceLog
            {
                Works = true,
                Confirmed = true,
                Description = "new Well created"
            };
            maintenanceLogs.Add(maintenanceLog);
            ExecuteInsertMaintenanceLogs(maintenanceLogs.ToArray(), wellId);
            return wellId;
        }
        public static int UpdateCompleteWell(ChangedWell changedWell)
        {
            if (changedWell == null || changedWell.Id == 0 || changedWell.WellTypeId == 0)
                return 0;
            int affected = ExecuteUpdateWell(changedWell);
            return affected;
        }

        // Single SQL Querry

        public static SmallWell[] ExecuteSelectNearbySmallWells(LocationForSearch locationForSearch)
        {
            if (locationForSearch == null && locationForSearch.SearchRadius == 0 && locationForSearch.Location == null)
                return null;
            GeoCoordinate position = new GeoCoordinate(locationForSearch.Location.Latitude, locationForSearch.Location.Longitude);
            SmallWell[] smallWells = ExecuteSelectSmallWells();
            if (smallWells == null)
                return null;
            List<SmallWell> nearbySmallWells = new List<SmallWell>();
            foreach (SmallWell well in smallWells)
            {
                GeoCoordinate coordinate = new GeoCoordinate(well.Location.Latitude, well.Location.Longitude);
                if (position.GetDistanceTo(coordinate) <= locationForSearch.SearchRadius)
                {
                    nearbySmallWells.Add(well);
                }
            }
            return nearbySmallWells.ToArray();
        }
        public static SmallWell[] ExecuteSelectSmallWells()
        {
            using (SqlCommand sqlCommand = SqlQuerry.SelectSmallWells())
            {
                if (sqlCommand == null)
                    return null;
                sqlCommand.Connection = sqlConnection;
                List<SmallWell> smallWells = new List<SmallWell>();
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SmallWell smallWell = new SmallWell();
                        if (!reader.IsDBNull(0))
                            smallWell.Id = reader.GetInt32(0);
                        if (!reader.IsDBNull(1))
                            smallWell.Name = reader.GetString(1);
                        if (!reader.IsDBNull(2))
                            smallWell.Status = reader.GetString(2);
                        smallWell.Location = new Location();
                        if (!reader.IsDBNull(3))
                            smallWell.Location.Longitude = reader.GetDouble(3);
                        if (!reader.IsDBNull(4))
                            smallWell.Location.Latitude = reader.GetDouble(4);
                        smallWells.Add(smallWell);
                    }
                }
                return smallWells.ToArray();
            }
        }
        public static WellTypeNoParts[] ExecuteSelectWellTypes()
        {
            SqlCommand sqlCommand = SqlQuerry.SelectWellTypes();
            if (sqlCommand == null)
                return null;
            sqlCommand.Connection = sqlConnection;
            List<WellTypeNoParts> wellTypes = new List<WellTypeNoParts>();
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    WellTypeNoParts wellType = new WellTypeNoParts();
                    if (!reader.IsDBNull(0))
                        wellType.Id = reader.GetInt32(0);
                    if (!reader.IsDBNull(1))
                        wellType.Name = reader.GetString(1);
                    wellTypes.Add(wellType);
                }
            }
            return wellTypes.ToArray();
        }
        public static Well ExecuteSelectWell(int wellId)
        {
            string sqlSelectWell = SqlQuerry.SelectWell(wellId);
            if (sqlSelectWell == null)
                return null;
            using (SqlCommand command = new SqlCommand(sqlSelectWell, sqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Well well = new Well();
                        if (!reader.IsDBNull(0))
                            well.Id = reader.GetInt32(0);
                        if (!reader.IsDBNull(1))
                            well.Name = reader.GetString(1);
                        if (!reader.IsDBNull(2))
                            well.Status = reader.GetString(2);
                        well.Location = new Location();
                        if (!reader.IsDBNull(3))
                            well.Location.Latitude = reader.GetDouble(3);
                        if (!reader.IsDBNull(4))
                            well.Location.Longitude = reader.GetDouble(4);
                        well.FundingInfo = new FundingInfo();
                        if (!reader.IsDBNull(5))
                            well.FundingInfo.Organisation = reader.GetString(5);
                        if (!reader.IsDBNull(6))
                            well.FundingInfo.OpeningDate = reader.GetDateTime(6);
                        if (!reader.IsDBNull(7))
                            well.FundingInfo.Price = reader.GetDouble(7);
                        well.WellType = new WellType();
                        if (!reader.IsDBNull(8))
                            well.WellType.Id = reader.GetInt32(8);
                        if (!reader.IsDBNull(9))
                            well.WellType.Name = reader.GetString(9);
                        if (!reader.IsDBNull(10))
                            well.WellType.Particularity = reader.GetString(10);
                        if (!reader.IsDBNull(11))
                            well.WellType.Depth = reader.GetDouble(11);
                        return well;
                    }
                }
            }
            return null;
        }     
        public static MaintenanceLog[] ExecuteSelectMaintenanceLogs(int wellId)
        {
            string sqlSelectMaintenanceLog = SqlQuerry.SelectMaintenanceLog(wellId);
            if (sqlSelectMaintenanceLog == null)
                return null;
            List<MaintenanceLog> maintenanceLogs = new List<MaintenanceLog>();
            using (SqlCommand command = new SqlCommand(sqlSelectMaintenanceLog, sqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MaintenanceLog maintenanceLog = new MaintenanceLog();

                        if (!reader.IsDBNull(0))
                            maintenanceLog.Id = reader.GetInt32(0);
                        if (!reader.IsDBNull(1))
                            maintenanceLog.Description = reader.GetString(1);
                        if (!reader.IsDBNull(2))
                            maintenanceLog.Works = reader.GetBoolean(2);
                        if (!reader.IsDBNull(3))
                            maintenanceLog.Confirmed = reader.GetBoolean(3);
                        if (!reader.IsDBNull(4))
                            maintenanceLog.StatusChangedDate = reader.GetDateTime(4);
                        maintenanceLogs.Add(maintenanceLog);
                    }
                }
            }
            return maintenanceLogs.ToArray();
        }
        public static Part[] ExecuteSelectWellParts(int wellTypeId)
        {
            string sqlSelectWellParts = SqlQuerry.SelectWellParts(wellTypeId);
            if (sqlSelectWellParts == null)
                return null;
            List<Part> parts = new List<Part>();
            using (SqlCommand command = new SqlCommand(sqlSelectWellParts, sqlConnection))
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
            return parts.ToArray();
        }
        public static int[] ExecuteInsertParts(Part[] parts)
        {
            string sqlParts = SqlQuerry.InsertParts(parts);
            if (sqlParts == null)
                return null;
            List<int> partsId = new List<int>();
            using (SqlCommand command = new SqlCommand(sqlParts, sqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        partsId.Add(reader.GetInt32(0));
                    }
                }
            }
            return partsId.ToArray();
        }
        public static int ExecuteInsertWellType(WellType wellType)
        {
            string sqlWellType = SqlQuerry.InsertWellType(wellType);
            if (sqlWellType == null)
                return 0;
            using (SqlCommand command = new SqlCommand(sqlWellType, sqlConnection))
            {
                return (int)command.ExecuteScalar();
            }
        }
        public static void ExecuteInsertWellParts(int wellTypeId, int[] partId)
        {
            string sqlWellParts = SqlQuerry.InsertWellParts(wellTypeId, partId);
            if (sqlWellParts == null)
                return;
            using (SqlCommand command = new SqlCommand(sqlWellParts, sqlConnection))
            {
                command.ExecuteNonQuery();
            }
        }
        public static int ExecuteInsertWell(NewWell newWell)
        {
            SqlCommand command = SqlQuerry.InsertWell(newWell);
            if (command == null)
                return 0;
            command.Connection = sqlConnection;
            return (int)command.ExecuteScalar();
        }
        public static void ExecuteInsertMaintenanceLogs(MaintenanceLog[] maintenanceLogs, int wellId)
        {
            string sqlMaintenanceLogs = SqlQuerry.InsertMaintenanceLogs(maintenanceLogs, wellId);
            if (sqlMaintenanceLogs == null)
                return;
            using (SqlCommand command = new SqlCommand(sqlMaintenanceLogs, sqlConnection))
            {
                command.ExecuteNonQuery();
            }
        }
        public static int ExecuteDeleteWell(int wellId)
        {
            string sqlDelete = SqlQuerry.DeleteWell(wellId);
            if (sqlDelete == null)
                return 0;
            using (SqlCommand command = new SqlCommand(sqlDelete, sqlConnection))
                return command.ExecuteNonQuery();
        }
        public static int ExecuteUpdatePart(Part part)
        {
            string sqlPart = SqlQuerry.UpdatePart(part);
            if (sqlPart == null)
                return 0;
            using (SqlCommand command = new SqlCommand(sqlPart, sqlConnection))
            {
                return command.ExecuteNonQuery();
            }
        }
        public static int ExecuteUpdateWellType(WellType wellType)
        {
            string sqlWellType = SqlQuerry.UpdateWellType(wellType);
            if (sqlWellType == null)
                return 0;
            using (SqlCommand command = new SqlCommand(sqlWellType, sqlConnection))
            {
                return command.ExecuteNonQuery();
            }
        }   
        public static int ExecuteUpdateFundingInfo(FundingInfo fundingInfo, int wellId)
        {
            SqlCommand sqlCommand = SqlQuerry.UpdateFundingInfo(fundingInfo, wellId);
            if (sqlCommand == null)
                return 0;
            sqlCommand.Connection = sqlConnection;
            return sqlCommand.ExecuteNonQuery();
        }
        public static int ExecuteUpdateLocation(Location location, int wellId)
        {
            string sqlLocation = SqlQuerry.UpdateLocation(location, wellId);
            if (sqlLocation == null)
                return 0;
            using (SqlCommand command = new SqlCommand(sqlLocation, sqlConnection))
            {
                return command.ExecuteNonQuery();
            }
        }
        public static int ExecuteUpdateWell(ChangedWell changedWell)
        {
            SqlCommand sqlCommand = SqlQuerry.UpdateWell(changedWell);
            if (sqlCommand == null)
                return 0;
            sqlCommand.Connection = sqlConnection;
            return sqlCommand.ExecuteNonQuery();
        }
        public static int ExecuteUpdateMaintenanceLog(MaintenanceLog maintenanceLog, int wellId)
        {
            string sqlMaintenanceLog = SqlQuerry.UpdateMaintenanceLog(maintenanceLog, wellId);
            if (sqlMaintenanceLog == null)
                return 0;
            using (SqlCommand command = new SqlCommand(sqlMaintenanceLog, sqlConnection))
            {
                return command.ExecuteNonQuery();
            }
        }
        
        
        // Issue

        // Mutiple SQL Querries

        public static Issue GetCompleteIssue(int issueId)
        {
            Issue issue = ExecuteSelectIssue(issueId);
            if (issue != null || issue.Id != 0)
                issue.BrokenParts = ExecuteSelectBrokenParts(issue.Id);
            return issue;
        }
        public static bool AddCompleteNewIssue(NewIssue newIssue)
        {
            int issueId = ExecuteInsertIssue(newIssue);
            if (issueId == 0)

                return false;
            ExecuteInsertBrokenParts(newIssue.BrokenPartIds, issueId);

            string status = ExecuteSelectWellStatus(newIssue.WellId);
            if (status != null && status == "green")
            {
                if (newIssue.ConfirmedBy == null || newIssue.ConfirmedBy == "")
                    ExecuteUpdateWellStatus(newIssue.WellId, "yellow");
                else if (newIssue.Works == false && status != "red")
                {
                    ExecuteUpdateWellStatus(newIssue.WellId, "red");
                }
            }
            MaintenanceLog maintenanceLog = new MaintenanceLog
            {
                Description = $"Issue #{issueId} Create",
                Works = newIssue.Works,
                Confirmed = false
            };
            if (newIssue.ConfirmedBy != null && newIssue.ConfirmedBy.Length > 0)
                maintenanceLog.Confirmed = true;
            MaintenanceLog[] maintenanceLogs = new MaintenanceLog[1];
            maintenanceLogs[0] = maintenanceLog;
            ExecuteInsertMaintenanceLogs(maintenanceLogs, issueId);
            return true;
        }
        public static int UpdateCompleteIssue(UpdateIssue updateIssue)
        {
            //Update Broken Parts is missing
            if (updateIssue.Id == 0)
                return 0;
            ExecuteUpdateIssue(updateIssue);
            //Update well
            if (!updateIssue.Open)
            {
                SmallIssue[] smallIssues = ExecuteSelectSmallIssues();
                bool allClosed = true;
                foreach (SmallIssue smallIssue in smallIssues)
                {
                    Issue otherIssue = ExecuteSelectIssue(smallIssue.Id);
                    if (otherIssue.Open)
                    {
                        allClosed = false;
                        break;
                    }
                }
                if (allClosed)
                    ExecuteUpdateWellStatus(updateIssue.WellId, "green");
            }
            else if (updateIssue.ConfirmedBy == null || updateIssue.ConfirmedBy == "")
            {
                SmallIssue[] smallIssues = ExecuteSelectSmallIssues();
                bool NoConfirmedIssues = true;
                foreach (SmallIssue smallIssue in smallIssues)
                {
                    Issue otherIssue = ExecuteSelectIssue(smallIssue.Id);
                    if (otherIssue.Works == false && otherIssue.ConfirmedBy != null && otherIssue.ConfirmedBy.Length > 0)
                    { 
                        NoConfirmedIssues = false;
                        break;
                    }
                }
                if (NoConfirmedIssues)
                    ExecuteUpdateWellStatus(updateIssue.WellId, "yellow");
            }
            else if (updateIssue.Works == false)
            {
                ExecuteUpdateWellStatus(updateIssue.WellId, "red");
            }

            MaintenanceLog maintenanceLog = new MaintenanceLog
            {
                Description = $"Issue #{updateIssue.Id} Updated",
                Works = updateIssue.Works,
                Confirmed = false
            };
            if (updateIssue.ConfirmedBy != null &&
                updateIssue.ConfirmedBy.Length > 0)
                maintenanceLog.Confirmed = true;
            MaintenanceLog[] maintenanceLogs = new MaintenanceLog[1];
            maintenanceLogs[0] = maintenanceLog;
            ExecuteInsertMaintenanceLogs(maintenanceLogs, updateIssue.Id);
            return 1;
        }

        // Single SQL Querry

        public static SmallIssue[] ExecuteSelectSmallIssues()
        {
            string sqlSmallIssue = SqlQuerry.SelectSmallIssue();
            if (sqlSmallIssue == null)
                return null;
            List<SmallIssue> smallIssues = new List<SmallIssue>();
            using (SqlCommand command = new SqlCommand(sqlSmallIssue, sqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SmallIssue smallIssue = new SmallIssue
                        {
                            Id = reader.GetInt32(0)
                        };
                        if (!reader.IsDBNull(1))
                            smallIssue.CreationDate = reader.GetDateTime(1);
                        if (!reader.IsDBNull(2))
                            smallIssue.WellId = reader.GetInt32(2);
                        smallIssues.Add(smallIssue);
                    }
                }
            }
            return smallIssues.ToArray();
        }
        public static Issue ExecuteSelectIssue(int issueId)
        {
            Issue issue = new Issue();
            string sqlIssue = SqlQuerry.SelectIssue(issueId);
            if (sqlIssue == null)
                return null;
            using (SqlCommand command = new SqlCommand(sqlIssue, sqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        issue.Id = reader.GetInt32(0);
                        if (!reader.IsDBNull(1))
                            issue.WellId = reader.GetInt32(1);
                        if(!reader.IsDBNull(2))
                            issue.Description = reader.GetString(2);
                        if (!reader.IsDBNull(3))
                            issue.CreationDate = reader.GetDateTime(3);
                        if (!reader.IsDBNull(4))
                            issue.Status = reader.GetString(4);
                        if (!reader.IsDBNull(5))
                            issue.Open = reader.GetBoolean(5);
                        if (!reader.IsDBNull(6))
                            issue.ConfirmedBy = reader.GetString(6);
                        if (!reader.IsDBNull(7))
                            issue.SolvedDate = reader.GetDateTime(7);
                        if (!reader.IsDBNull(8))
                            issue.RepairedBy = reader.GetString(8);
                        if (!reader.IsDBNull(9))
                            issue.Works = reader.GetBoolean(9);
                    }
                }
            }
            return issue;
        }
        public static Part[] ExecuteSelectBrokenParts(int issueId)
        {
            string sqlBrokenParts = SqlQuerry.SelectBrokenParts(issueId);
            if (sqlBrokenParts == null)
                return null;
            List<Part> parts = new List<Part>();
            using (SqlCommand command = new SqlCommand(sqlBrokenParts, sqlConnection))
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
            return parts.ToArray();
        }      
        public static int ExecuteInsertIssue(NewIssue newIssue)
        {
            string sqlInsertIssue = SqlQuerry.InsertIssue(newIssue);
            if (sqlInsertIssue == null)
                return 0;
            using (SqlCommand command = new SqlCommand(sqlInsertIssue, sqlConnection))
            {
                try
                {
                    return (int)command.ExecuteScalar();
                }
                catch
                {
                    return 0;
                }
            }
        }
        public static void ExecuteInsertBrokenParts(int[] partIds, int issueId)
        {
            string sqlBrokenParts = SqlQuerry.InsertBrokenParts(partIds, issueId);
            if (sqlBrokenParts == null)
                return;
            using (SqlCommand command = new SqlCommand(sqlBrokenParts, sqlConnection))
            {
                command.ExecuteNonQuery();
            }
        }
        public static void ExecuteUpdateIssue(UpdateIssue updateIssue)
        {
            string sqlUpdateIssue = SqlQuerry.UpdateIssue(updateIssue);
            if (sqlUpdateIssue == null)
                return;
            using (SqlCommand command = new SqlCommand(sqlUpdateIssue, sqlConnection))
            {
                command.ExecuteNonQuery();
            }
        }
        public static int ExecuteDeleteIssue(int issueId)
        {
            string sqlDeleteIssue = SqlQuerry.DeleteIssue(issueId);
            if (sqlDeleteIssue == null)
                return 0;
            using (SqlCommand command = new SqlCommand(sqlDeleteIssue, sqlConnection))
                return command.ExecuteNonQuery();
        }
        public static string ExecuteSelectWellStatus(int wellId)
        {
            string sqlSelectWellStatus = SqlQuerry.SelectWellStatus(wellId);
            if (sqlSelectWellStatus == null)
                return null;
            using (SqlCommand command = new SqlCommand(sqlSelectWellStatus, sqlConnection))
            {
                return (string)command.ExecuteScalar();
            }
        }
        public static int ExecuteUpdateWellStatus(int wellId, string status)
        {
            string sqlUpdateWellStatus = SqlQuerry.UpdateWellStatus(wellId, status);
            if (sqlUpdateWellStatus == null)
                return 0;
            using (SqlCommand command = new SqlCommand(sqlUpdateWellStatus, sqlConnection))
            {
                return command.ExecuteNonQuery();
            }
        }
    }
}
